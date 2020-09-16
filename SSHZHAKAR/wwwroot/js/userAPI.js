async function getCurrentUserId() {
    let currentUserId;
    await fetch('/user/getId', {
        method: 'GET',
    })
        .then(response => response.json())
        .then(response => {
            currentUserId = response.userId;
            console.log("Response Id = ", currentUserId);
        });
    return currentUserId;
}

function getSharedTerminalsFromServer() {
    let mySharedTerminals = [];
    fetch('/user/terminal/getshared', {
        method: 'GET',
    })
        .then(response => response.json())
        .then(response => {
            myTerminals = myTerminals.concat(JSON.parse(response));
            mySharedTerminals = mySharedTerminals.concat(JSON.parse(response));
            console.log(JSON.parse(response));
        })
    return mySharedTerminals;
}

async function getTerminalsFromServer() {
    let terminalList = [];
    await fetch('/user/terminal/get', {
        method: 'GET',
    })
        .then(response => response.json())
        .then(response => {
            terminalList = terminalList.concat(JSON.parse(response));
        })
    return terminalList;
}

async function addTerminalToServer(terminalObj) {
    await fetch('/user/terminal/add', {
        method: 'POST',
        body: JSON.stringify(terminalObj),
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(response => console.log("RESPONSE ADD TERMINAL", response));
    return true;

}

async function updateTerminalToServer(editedTerminalObj) {

    await fetch('/user/terminal/update', {
        method: 'POST',
        body: JSON.stringify(editedTerminalObj),
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(commits => console.log("UPDATE RESPONSE: ", commits))
    return true;
}
function removeElementsByClass(className) {
    var elements = document.getElementsByClassName(className);
    while (elements.length > 0) {
        elements[0].parentNode.removeChild(elements[0]);
    }
}