const TERMINAL_LIST = [];
const CURRENT_USER = {};

const EDIT_MENU = {
    prelogin: {},
    isActive: false,
    currentTerminal: {
        Name: null,
        UserName: null,
        Ip: null,
        Password: null
    }
};
const ADD_MENU = {
    prelogin: {}
};
window.onload = async function () {
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _GETTING USER'S TERMINALS FROM SERVER(DB) _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    await this.loadTerminals();
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _PRELOGIN CHECKBOX CONTROLLER IN ADDMENU CONTAINER _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    ADD_MENU.prelogin.checkbox = document.getElementById("prelogin-id");
    ADD_MENU.prelogin.checkbox.onclick = function () {
        ADD_MENU.prelogin.element = document.getElementsByClassName("pre-login-form")[0];
        if (ADD_MENU.prelogin.checkbox.checked == true) {
            ADD_MENU.prelogin.element.style.opacity = "1";
        }
        else {
            ADD_MENU.prelogin.element.style.opacity = "0";
        }
    }
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _PRELOGIN CHECKBOX CONTROLLER IN EDITMENU CONTAINER _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    EDIT_MENU.prelogin.checkbox = document.getElementById("prelogin-editbox");
    EDIT_MENU.prelogin.checkbox.onclick = function () {
        EDIT_MENU.prelogin.element = document.getElementsByClassName("pre-login-form")[1];
        if (EDIT_MENU.prelogin.checkbox.checked == true) {
            EDIT_MENU.prelogin.element.style.opacity = "1";
        }
        else {
            EDIT_MENU.prelogin.element.style.opacity = "0";
        }
    }
}

async function loadTerminals() {
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _GETTING USER'S TERMINALS FROM SERVER(DB) _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    TERMINAL_LIST.length = 0;
    let terminals = await getTerminalsFromServer();
    CURRENT_USER.Id = await getCurrentUserId();
    this.console.log("Terminals = ", terminals)
    this.console.log("CURRENT USER ID = ", CURRENT_USER.Id);
    terminals.forEach(terminal => {
        addTerminal(terminal);
        TERMINAL_LIST.push(terminal);
    });
    return true;
}
function addTerminal(terminalObj) {
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _CREATING TERMINAL-CONTAINER _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    let containerElement = document.getElementsByClassName("container")[0];
    let newTerminalContainer = document.createElement('div');
    let icoElement = document.createElement('div');
    let imageElement = document.createElement('img');
    let terminalTitle = document.createElement('div');
    let name = document.getElementById('terminal-name-input');
    let editContainer = document.createElement('div');
    let imageGearElement = document.createElement('img');
    newTerminalContainer.className = "terminal-link-container";
    icoElement.className = "terminal-link-ico";
    imageElement.src = '/icons/desktop-ico.png';
    icoElement.appendChild(imageElement);
    newTerminalContainer.appendChild(icoElement);
    terminalTitle.className = 'terminal-link-title';
    newTerminalContainer.onclick = () => {
        if (!EDIT_MENU.isActive) {
            location.href = "/user/terminal/" + CURRENT_USER.Id + "/" + terminalObj.Name 
        }
    }
    editContainer.className = 'terminal-link-edit-button';
    imageGearElement.src = '/icons/gear-ico.png';
    editContainer.appendChild(imageGearElement);
    editContainer.dataset.name = terminalObj.Name;
    editContainer.onclick = (event) => {
        event.preventDefault();
        let terminal = TERMINAL_LIST.find((element) => {
            return element.Name == event.target.dataset.name;
        });
        openEditMenu(terminal);
    }
    terminalTitle.innerText = terminalObj.Name;
    newTerminalContainer.appendChild(terminalTitle);
    newTerminalContainer.appendChild(editContainer);
    containerElement.appendChild(newTerminalContainer);
}


function openAddMenu(event) {
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _OPENING ADDING NEW TERMINAL MENU _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    let addConsoleTableElement = document.getElementsByClassName("add-console-table")[0];
    let addConsoleButtonContainerElement = document.getElementsByClassName("add-connection-button-container")[0];
    addConsoleTableElement.style.display = "flex";
    addConsoleTableElement.style.opacity = "1";
    addConsoleButtonContainerElement.style.opacity = "0";
}

function closeAddMenu(event) {
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _CLOSING ADDING NEW TERMINAL MENU _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    let addConsoleTableElement = document.getElementsByClassName("add-console-table")[0];
    let addConsoleButtonContainerElement = document.getElementsByClassName("add-connection-button-container")[0];
    addConsoleTableElement.style.display = "flex";
    addConsoleTableElement.style.opacity = "0";
    addConsoleButtonContainerElement.style.display = "flex";
    addConsoleButtonContainerElement.style.opacity = "1";
    addConsoleTableElement.style.display = "none";
    ADD_MENU.prelogin.element.style.opacity = "0";
    ADD_MENU.prelogin.checkbox.checked = false;
}
function clearAddMenuFields() {
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ CLEAR FIELDS IN ADD TERMINAL MENU _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    document.querySelectorAll("#remote-desktop-username")[0].value = "";
    document.querySelectorAll("#remote-desktop-password")[0].value = "";
    document.querySelectorAll("#terminal-name-input")[0].value = "";
    document.querySelectorAll("#remote-desktop-ip")[0].value = "";
}

function openEditMenu(terminalObj) {
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _OPENING EDIT TERMINAL MENU _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    EDIT_MENU.isActive = true;
    let addConsoleTableElement = document.getElementsByClassName("add-console-table")[1];
    addConsoleTableElement.style.display = "flex";
    addConsoleTableElement.style.opacity = "1";
    console.log("TERMINAL = ", terminalObj);
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ FILL FIELDS IN EDITBOX_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    document.querySelectorAll("#remote-desktop-username")[1].value = terminalObj.UserName;
    document.querySelectorAll("#remote-desktop-password")[1].value = terminalObj.Password;
    document.querySelectorAll("#terminal-name-input")[1].value = terminalObj.Name;
    document.querySelectorAll("#remote-desktop-ip")[1].value = terminalObj.Ip;
    EDIT_MENU.currentTerminal.Name = terminalObj.Name;
    EDIT_MENU.currentTerminal.UserName = terminalObj.UserName;
    EDIT_MENU.currentTerminal.Ip = terminalObj.Ip;
    EDIT_MENU.currentTerminal.Password = terminalObj.Password;

}

function closeEditMenu(event) {
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _CLOSING EDIT TERMINAL MENU _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    EDIT_MENU.isActive = false;
    let addConsoleTableElement = document.getElementsByClassName("add-console-table")[1];
    addConsoleTableElement.style.display = "flex";
    addConsoleTableElement.style.opacity = "0";
    addConsoleTableElement.style.display = "none";
    EDIT_MENU.prelogin.element.style.opacity = "0";
    EDIT_MENU.prelogin.checkbox.checked = false;
}
function clearEditMenuFields() {
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ CLEAR FIELDS IN EDIT TERMINAL MENU _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    document.querySelectorAll("#remote-desktop-username")[1].value = "";
    document.querySelectorAll("#remote-desktop-password")[1].value = "";
    document.querySelectorAll("#terminal-name-input")[1].value = "";
    document.querySelectorAll("#remote-desktop-ip")[1].value = "";
}

async function addTerminalBUTTON(event) {
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ADD TERMINAL(in ADD BOX) BUTTON  CONTROLLER _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    let terminalObj = {};
    terminalObj.Username = document.querySelectorAll("#remote-desktop-username")[0].value;
    terminalObj.Password = document.querySelectorAll("#remote-desktop-password")[0].value;
    terminalObj.Name = document.querySelectorAll("#terminal-name-input")[0].value;
    terminalObj.Ip = document.querySelectorAll("#remote-desktop-ip")[0].value;
    await addTerminalToServer(terminalObj);
    removeElementsByClass("terminal-link-container");
    loadTerminals();
    clearAddMenuFields();
    closeAddMenu();
}
async function saveTerminalBUTTON(event) {
    //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _SAVE UPDATED TERMINAL(in EDIT BOX) BUTTON  CONTROLLER _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
    let terminalObj = {};
    terminalObj.Username = document.querySelectorAll("#remote-desktop-username")[1].value;
    terminalObj.Password = document.querySelectorAll("#remote-desktop-password")[1].value;
    terminalObj.Name = document.querySelectorAll("#terminal-name-input")[1].value;
    terminalObj.Ip = document.querySelectorAll("#remote-desktop-ip")[1].value;
    let editedTerminal = {};
    editedTerminal.terminalObj = terminalObj;
    editedTerminal.OldName = EDIT_MENU.currentTerminal.Name;
    await updateTerminalToServer(editedTerminal);
    removeElementsByClass("terminal-link-container");
    loadTerminals();
    clearEditMenuFields();
    closeEditMenu();
}