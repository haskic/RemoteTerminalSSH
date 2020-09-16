var UserName = "";

//var socketInviterString = "";
var AllUsers = [];
var firstWordStatus = true; // it should be update to initial state when time-out or game session is over
var AjaxPort = 5001;
let myId;
let hubConnectionTerminal;
let lastCommandResult;

function getTerminalName() {
    let terminalName = window.location.href.match("[a-zA-Z0-9]*$");
    console.log("Terminal Name = " + terminalName);
    return terminalName;
}



async function startConnection() {
    hubConnectionTerminal = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5002/terminal")
        .build();

    hubConnectionTerminal.on("GetResponse", function (textResponse, response = null) {
        console.log("RESPONSE FROM SSH SERVER = " + textResponse);
        console.log("RESPONSE FROM TERMINAL= " + response);
    });

    hubConnectionTerminal.on("GetResponseCommand", function (textResponse, responseCmd) {
        console.log("RESPONSE FROM TERMINAL= " + responseCmd);
        lastCommandResult = responseCmd;

        addResponseToConsole(responseCmd);
    });

    hubConnectionTerminal.on("GetNameTerminal", function (textResponse, name) {
        console.log("RESPONSE FROM SSHSERVER = " + textResponse);
        UserName = name.substring(0, name.length - 1);
        document.getElementsByClassName("static-console")[0].innerHTML = `${UserName}@Remote-Linux-Machine:~$`;
    });




    await hubConnectionTerminal.start();
    console.log("HUBID = " + JSON.stringify(hubConnectionTerminal))
    console.log("USERID = " + hubConnectionTerminal.connectionId)
    myId = await getMyId();
    let terminalName = getTerminalName();
    document.getElementById("terminal-window-title").innerText = terminalName;

    await hubConnectionTerminal.invoke("CreateSSH", hubConnectionTerminal.connectionId, myId, terminalName.toString());
    await hubConnectionTerminal.invoke("GetUserNameSSHTerminal", hubConnectionTerminal.connectionId, myId);


}



//connection.connectionMethods.onConnected = () => {
//    console.log("ZHAKAR connected");
//    connection.invoke("SendMessage", connection.connectionId,"Alexander");
//    connection.invoke("ConnectSSH", connection.connectionId, "1488");
//    connection.invoke("GetUserNameSSHTerminal", connection.connectionId, "1488");
    
//}

//connection.connectionMethods.onDisconnected = () => {

//}

//connection.clientMethods["get_message"] = (socketID,wordText) => {

//    console.log("TEXT FROM SERVER= " + wordText);
//}
//connection.clientMethods["GetResponse"] = (textResponse, response = null) => {

//    console.log("RESPONSE FROM SSH SERVER = " + textResponse);
//    console.log("RESPONSE FROM TERMINAL= " + response);

//}

//connection.clientMethods["GetResponseCommand"] = (textResponse,responseCmd) => {

//    console.log("RESPONSE FROM TERMINAL= " + responseCmd);
//    addResponseToConsole(responseCmd);

//}


//connection.clientMethods["GetNameTerminal"] = (textResponse, name) => {
//    console.log("RESPONSE FROM SSHSERVER = " + textResponse);
//    UserName = name.substring(0, name.length - 1);
//    document.getElementsByClassName("static-console")[0].innerHTML = `${UserName}@Remote-Linux-Machine:~$`;
//}


//connection.start();


var inputLine = document.getElementsByClassName('editable-command')[0];
var el2 = document.getElementsByClassName('container')[0];
var consoleBox = document.getElementsByClassName("console-box")[0];



console.log(el2);
consoleBox.addEventListener('keydown', logKey);

//console.log("A" + connection.connectionId);


function logKey(e) {


    if (e.code == 'Enter') {
        e.preventDefault();
        let cmd = document.getElementsByClassName('editable-command')[document.getElementsByClassName('editable-command').length - 1].innerText;
        cmd = cmd.split(' ');
        let args = cmd.slice(1);
        cmd = cmd[0];


        console.log("CMD = " + cmd);

        commnadHandler(cmd, args);
        console.log("______________________________________s");
    }


}

function addResponseToConsole(response) {
    let newResponseLine = document.createElement('div');
    newResponseLine.innerText = response;
    newResponseLine.className = 'response-line';
    consoleBox.appendChild(newResponseLine);
    newLine();

}

function newLine() {
    let newStaticLine = document.createElement('div');
    newStaticLine.className = 'static-console';
    newStaticLine.innerText = `${UserName}@Remote-Linux-Machine:~$`;
    let newCommandLine = document.createElement('div');
    newCommandLine.className = 'editable-command';
    newCommandLine.contentEditable = true;
    newCommandLine.spellcheck = false;
    let newConsoleLine = document.createElement('div');
    newConsoleLine.className = 'console-line';
    newConsoleLine.appendChild(newStaticLine);
    newConsoleLine.appendChild(newCommandLine);
    consoleBox.appendChild(newConsoleLine);
    newCommandLine.focus();
}

function commnadHandler(cmd, args) {
    if (commands.hasOwnProperty(cmd)) {
        commands[cmd](consoleBox, cmd, args);
        newLine();

    }
    else {
        sendCommandToServer(cmd + ' ' + args.join(' '));
        console.log("SENT COMMAND = " + cmd + ' ' + args.join(' ') + '|');
    }
}


function sendCommandToServer(commandString) {
    //connection.invoke("CommandSSH", connection.connectionId, "1488", commandString);
    console.log("COMMAND = " + commandString);
    hubConnectionTerminal.invoke("CommandSSH", hubConnectionTerminal.connectionId, myId, commandString);
}

var commands = {
    'clear': function (terminalBox) {
        terminalBox.innerHTML = '';
    },
    'connect': function (terminalBox, cmd, args) {
        console.log("ARGS = " + args);
        let ip = args[0];
        console.log("IP ADDRESS = " + ip);
    }

}


async function getMyId() {
    let myUserId;
    await fetch('/user/getId', {
        method: 'GET',
    })
        .then(response => response.json())
        .then(commits => {
            myUserId = commits.userId;
            console.log("MY ID = " ,commits);

        });
    return myUserId;

}

startConnection();