


window.onload = () => {
    getTerminalsFromServer();
    getSharedTerminalsFromServer();
}
let myTerminals = [];


let editTableIsShow = false;
let preLoginCheckBox = document.getElementById("prelogin-id");

let selectedTerminalForEdit;


console.log(preLoginCheckBox);


preLoginCheckBox.onclick = function () {
    let preLoginForm = document.getElementsByClassName("pre-login-form")[0];

    if (preLoginCheckBox.checked == true) {
        preLoginForm.style.opacity = "1";
        console.log("ZHAKAR");
    }
    else {
        preLoginForm.style.opacity = "0";
    }
}

function changeCheckBoxEditBox(event) {
    let preLoginForm = document.getElementsByClassName("pre-login-form")[1];
    console.log(document.getElementsByClassName("pre-login-form"));
    console.log("LOL")

    if (event.target.checked == true) {
        preLoginForm.style.opacity = "1";
        console.log("ZHAKAR");
    }
    else {
        preLoginForm.style.opacity = "0";
    }
}
function addTerminalButton() {

    let containerElement = document.getElementsByClassName("container")[0];

    let newDesktopContainer = document.createElement('div');
    newDesktopContainer.className = "terminal-link-container";
   
    let icoElement = document.createElement('div');
    icoElement.className = "terminal-link-ico";
    let imageElement = document.createElement('img');
    imageElement.src = '/icons/desktop-ico.png';
    icoElement.appendChild(imageElement);
    newDesktopContainer.appendChild(icoElement);
    let terminalTitle = document.createElement('div');
    terminalTitle.className = 'terminal-link-title';
    let name = document.getElementById('terminal-name-input');
    newDesktopContainer.onclick = () => {
        if (!editTableIsShow) {
            location.href = "/user/terminal/" + name.value // bag fix this for zhakar
        }
    }
    let editContainer = document.createElement('div');
    editContainer.className = 'terminal-link-edit-button';
    let imageGearElement = document.createElement('img');
    imageGearElement.src = '/icons/gear-ico.png';
    editContainer.appendChild(imageGearElement);
    terminalTitle.innerText = name.value;
    newDesktopContainer.appendChild(terminalTitle);
    newDesktopContainer.appendChild(editContainer);
    containerElement.appendChild(newDesktopContainer);


    let desktopObj = {
        name: name.value,
        userName: document.getElementById("remote-desktop-username").value,
        password: document.getElementById("remote-desktop-password").value,
        ip: document.getElementById("remote-desktop-ip").value,
        sudo: ""
    }

    
    console.log("RESPONSE JSON = ");

    fetch('/user/terminal/add', {
        method: 'POST',
        body: JSON.stringify(desktopObj),
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(commits => console.log(commits));


    //fetch('/user/desktop/get')
    //    .then(response => response.json())
    //    .then(commits => console.log(commits));



    closeAddMenu();
    

}


function openAddMenu(event) {
    console.log("CLICK");
    let addConsoleTableElement = document.getElementsByClassName("add-console-table")[0];
    let addConsoleButtonContainerElement = document.getElementsByClassName("add-connection-button-container")[0];

    addConsoleTableElement.style.display = "flex";
    addConsoleButtonContainerElement.style.opacity = "0";

    setTimeout(() => {
        addConsoleTableElement.style.opacity = "1";

    }, 50);
    
}
function closeAddMenu(event) {
    let addConsoleTableElement = document.getElementsByClassName("add-console-table")[0];
    let addConsoleButtonContainerElement = document.getElementsByClassName("add-connection-button-container")[0];
 

    addConsoleTableElement.style.display = "flex";
    addConsoleTableElement.style.opacity = "0";
    addConsoleButtonContainerElement.style.display = "flex";
    setTimeout(() => {
        addConsoleButtonContainerElement.style.opacity = "1";


    }, 500);
    setTimeout(() => {
        addConsoleTableElement.style.display = "none";

        
    }, 400);
}


function closeEditMenu(event) {
    let addConsoleTableElement = document.getElementsByClassName("add-console-table")[1];
    editTableIsShow = false;

    addConsoleTableElement.style.display = "flex";
    addConsoleTableElement.style.opacity = "0";
   
    setTimeout(() => {
        addConsoleTableElement.style.display = "none";


    }, 400);
}

console.log("ZHAKAR");


function addTerminalToDesktop(terminalObj) {
    let containerElement = document.getElementsByClassName("container")[0];

    let newDesktopContainer = document.createElement('div');
    newDesktopContainer.className = "terminal-link-container";

    let icoElement = document.createElement('div');
    icoElement.className = "terminal-link-ico";
    let imageElement = document.createElement('img');
    imageElement.src = '/icons/desktop-ico.png';
    icoElement.appendChild(imageElement);
    newDesktopContainer.appendChild(icoElement);
    let terminalTitle = document.createElement('div');
    terminalTitle.className = 'terminal-link-title';
    let name = terminalObj.Name;
    newDesktopContainer.onclick = () => {
        if (!editTableIsShow) {
            location.href = "/user/terminal/" +terminalObj.UserId + "/" + name

        }
    }
    let editContainer = document.createElement('div');
    editContainer.className = 'terminal-link-edit-button';
    let imageGearElement = document.createElement('img');
    imageGearElement.src = '/icons/gear-ico.png';
    editContainer.appendChild(imageGearElement);
    editContainer.onclick = (event) => {

        let editTable = document.getElementById("edit-console-table");
        editTable.style.display = "flex";
        editTable.style.opacity = "1";
        editTableIsShow = true;
        let title;
        if (event.target.tagName == "IMG") {
            title = event.target.parentElement.parentElement.children[1].innerText;
        }
        else {
            title = event.target.parentElement.children[1].innerText;

        }
        let selectedTerminal = myTerminals.find((value) => {
            console.log("IN SEARCH " + value.name + "  " + title);
            return value.Name == title
        });
        selectedTerminalForEdit = {
            DesktopObj: {},
            OldName: ""
        };
        selectedTerminalForEdit.OldName = selectedTerminal.Name.slice(0);
        selectedTerminalForEdit.DesktopObj = Object.assign(selectedTerminalForEdit.DesktopObj, selectedTerminal);
        console.log("SELECTER OBJ 0  = ", selectedTerminalForEdit);

        console.log(selectedTerminal);
        document.querySelectorAll("#remote-desktop-username")[1].value = selectedTerminal.UserName;
        document.querySelectorAll("#remote-desktop-password")[1].value = terminalObj.Password;
        document.querySelectorAll("#terminal-name-input")[1].value = selectedTerminal.Name;
        document.querySelectorAll("#remote-desktop-ip")[1].value = selectedTerminal.Ip;


    }


    terminalTitle.innerText = name
    newDesktopContainer.appendChild(terminalTitle);
    newDesktopContainer.appendChild(editContainer);
    containerElement.appendChild(newDesktopContainer);



}


function addSharedTerminalToDesktop(terminalObj) {
    let containerElement = document.getElementsByClassName("container")[0];

    let newDesktopContainer = document.createElement('div');
    newDesktopContainer.style.background = "rgba(255,0,0,.4)";
    newDesktopContainer.className = "terminal-link-container";
    let providerTitle = document.createElement('div');
    providerTitle.className = "provider-title";
    providerTitle.innerText = "Provided";
    newDesktopContainer.appendChild(providerTitle);
    let icoElement = document.createElement('div');
    icoElement.className = "terminal-link-ico";
    let imageElement = document.createElement('img');
    imageElement.src = '/icons/desktop-ico.png';
    icoElement.appendChild(imageElement);
    newDesktopContainer.appendChild(icoElement);
    
    let terminalTitle = document.createElement('div');
    terminalTitle.className = 'terminal-link-title';
    let name = terminalObj.Name;
    newDesktopContainer.onclick = () => {
        if (!editTableIsShow) {
            location.href = "/user/terminal/" + terminalObj.UserId + "/" + name

        }
    }
    


    terminalTitle.innerText = name
    newDesktopContainer.appendChild(terminalTitle);
    
    containerElement.appendChild(newDesktopContainer);



}



function saveTerminalButton(event) {

    selectedTerminalForEdit.DesktopObj.UserName = document.querySelectorAll("#remote-desktop-username")[1].value;
    selectedTerminalForEdit.DesktopObj.Password = document.querySelectorAll("#remote-desktop-password")[1].value;
    selectedTerminalForEdit.DesktopObj.Ip = document.querySelectorAll("#remote-desktop-ip")[1].value;
    selectedTerminalForEdit.DesktopObj.Name = document.querySelectorAll("#terminal-name-input")[1].value;


    console.log("SELECTED");
    console.log(selectedTerminalForEdit);


    removeElementsByClass("terminal-link-container");

    fetch('/user/terminal/update', {
        method: 'POST',
        body: JSON.stringify(selectedTerminalForEdit),
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(commits => console.log("UPDATE RESPONSE: ", commits))
        .then(() => {
            let editTable = document.getElementById("edit-console-table");
            editTable.style.opacity = "0";
            editTableIsShow = false;
            editTable.style.display = "none";
            getTerminalsFromServer();
        });


    
   

}


function removeElementsByClass(className) {
    var elements = document.getElementsByClassName(className);
    while (elements.length > 0) {
        elements[0].parentNode.removeChild(elements[0]);
    }
}

function getTerminalsFromServer() {

    myTerminals = [];
    fetch('/user/terminal/get', {
        method: 'GET',
    })
        .then(response => response.json())
        .then(commits => {
            myTerminals = myTerminals.concat(JSON.parse(commits));
            console.log(JSON.parse(commits));

        }).then(() => {
            myTerminals.forEach(terminal => {
                addTerminalToDesktop(terminal);

            })
        });
   

}

function getSharedTerminalsFromServer() {

    let mySharedTerminals = [];
    fetch('/user/terminal/getshared', {
        method: 'GET',
    })
        .then(response => response.json())
        .then(commits => {
            myTerminals = myTerminals.concat(JSON.parse(commits));
            mySharedTerminals = mySharedTerminals.concat(JSON.parse(commits));

            console.log(JSON.parse(commits));

        }).then(() => {
            mySharedTerminals.forEach(terminal => {
                addSharedTerminalToDesktop(terminal);

            });
        });


}

//function getMyId(){
//    fetch('/user/getId', {
//        method: 'GET',
//    })
//        .then(response => response.json())
//        .then(commits => {
//            myUserId = commits.userId;
//            console.log("MY ID = " ,commits);

//        });
//}
