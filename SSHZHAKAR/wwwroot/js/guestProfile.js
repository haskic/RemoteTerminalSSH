




let guestTerminals = [];

let guestUserId;
let myUserId;
function getUserId() {
    guestUserId = window.location.href.match("(?<=user/)[^\>]*");
    console.log("GUEST USER ID = " + guestUserId);
    
}

async function startUp() {
    await getUserId();
    await getMyId();
    updateUserInfo();
    getPhotoInfo();

    //guestTerminals.push(
    //    {
    //        title: "Terminal-1",
    //        access: false
    //    }
    //);
}

async function getUserDataFromServer() {
    let userObj;
    await fetch('/user/profile/get/' + guestUserId, {
        method: 'GET',
    })
        .then(response => response.json())
        .then(jsonObj => {
            userObj = JSON.parse(jsonObj);
            console.log(JSON.parse(jsonObj));
        });
    console.log("GUEST USER INFO " + userObj)
    return userObj;

}
async function getPhotoInfo() {
    
    let image = document.getElementById("profile_photo");

    let photoResult = await fetch(`/static/${guestUserId}.png`, {
        method: 'GET',
    });
    if (photoResult.ok) {
        image.src = `/../static/${guestUserId}.png`;
    }
    else {
        image.src = `/../static/default_user.png`;

    }

}


async function updateUserInfo() {
    let userObj = await getUserDataFromServer();
    document.getElementById("name_field").innerText = userObj.Name;
    document.getElementById("lastname_field").innerText = userObj.LastName;
    document.getElementById("nickname_field").innerText = userObj.NickName;
    document.getElementById("email_field").innerText = userObj.Email;

}

function giveAccessButtonClick() {
    let terminalContainer = document.getElementsByClassName("give-access-container")[0];


    terminalContainer.style.display = "flex";

    setTimeout(() => {
        terminalContainer.style.opacity = "1";

    }, 50)


}


function terminalSelectAnimation(event, obj) {
    let element;

    if (event == null) {
        element = obj;
    }
    else {
        element = event.target;
    }
    console.log("CHILDRENS", element.children)
    var children = [].slice.call(element.children);
    children.forEach(value => {
        console.log("A");
        if (value.className == "terminal-title") {
            value.style.position = "absolute";
            value.style.bottom = "-30px";
            element.style.backgroundColor = "rgba(14, 247, 4,.7)";
            let image = getChildrenByClassName(element,"close-give-access-container-image");
            image.src = `../../icons/checked.png`;
            console.log("URL" + image.src);
        }
    });
}
function terminalUnSelectAnimation(event) {

    let element = event.target;
    console.log("CHILDRENS", element.children)
    var children = [].slice.call(element.children);
    children.forEach(value => {
        console.log("A");
        if (value.className == "terminal-title") {
            value.style.bottom = "17%";

            setTimeout(() => {

            }, 400);

            event.target.style.backgroundColor = "rgba(255,255,255,1)";
            let image = getChildrenByClassName(event.target, "close-give-access-container-image");

            image.src = `../../icons/desktop-ico.png`;
            console.log("URL" + image.src);
        }
    });
}

function acceptButtonClick() {

    let obj = {
        GuestId: guestUserId.toString(),
        Terminals: guestTerminals
    }
    console.log("SEND OBJ", JSON.stringify(obj));
    fetch('/user/terminal/share', {
        method: 'POST',
        body: JSON.stringify(obj),
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        }
    })
        .then(response => response.json())
        .then(commits => {
            console.log("UPDATE RESPONSE: ", commits);
            showAcceptButtonResponse();
        });
       



}

function terminalClick(event) {
    if (guestTerminals.Access) {
        terminalUnSelectAnimation(event);
    }
    else {
        terminalSelectAnimation(event);

    }
}

function addTerminalToTerminalsContainer(terminal) {
    let pushObj = {
        Name: terminal.Name,
        Access: false
    }

    let terminalContainer = document.getElementsByClassName("terminals-container")[0];
    let newContainerBox = document.createElement('div');
    let img = document.createElement('IMG');
    let terminalTitle = document.createElement('div');
    terminalTitle.className = "terminal-title";
    terminalTitle.innerText = terminal.Name;
    img.className = "close-give-access-container-image";
    newContainerBox.className = "terminal-box"
    img.src = "../../icons/desktop-ico.png";
    newContainerBox.appendChild(img);
    newContainerBox.appendChild(terminalTitle);
    if (terminal.Shared == guestUserId) {
        pushObj.Access = true;
        terminalSelectAnimation(null, newContainerBox);
    }
    
    newContainerBox.onclick = (event) => {
        let myTerminal;
        guestTerminals.forEach(value => {
            console.log("CHILD");
            console.log(getChildrenByClassName(event.target, "terminal-title"))
            if (value.Name == getChildrenByClassName(event.target, "terminal-title").innerText) {
                myTerminal = value;
                console.log("MYTERMINAL = ", myTerminal);
            }
        });
        if (myTerminal.Access) {
            terminalUnSelectAnimation(event);
            myTerminal.Access = false;
        }
        else {
            terminalSelectAnimation(event);
            myTerminal.Access = true;
        }
        console.log("CURRENT TERMINALS:", guestTerminals);
    }
    terminalContainer.appendChild(newContainerBox);
    guestTerminals.push(pushObj);

}

function getTerminalsFromServer() {
    myTerminals = [];

    fetch('/user/terminal/get/' + myUserId, {
        method: 'GET',
    })
        .then(response => response.json())
        .then(commits => {
            myTerminals = myTerminals.concat(JSON.parse(commits));
            console.log(JSON.parse(commits));

        }).then(() => {
            console.log("AZAZAZAZAZ");
            console.log(myTerminals);
            console.log("AZAZAZAZAZ");

            myTerminals.forEach(terminal => {
                addTerminalToTerminalsContainer(terminal);

            })
        });
}


function getChildrenByClassName(element, classname) {
    let result;
    let children = [].slice.call(element.children);
    children.forEach(value => {
        if (value.className == classname) {
            result = value;
        }
    });
    return result;
}

function closeTerminalContainerButton() {
    let terminalContainer = document.getElementsByClassName("give-access-container")[0];
    terminalContainer.style.opacity = "0";
    setTimeout(() => {
        terminalContainer.style.display = "none";
    },700)
}

function getMyId(){
    fetch('/user/getId', {
        method: 'GET',
    })
        .then(response => response.json())
        .then(commits => {
            myUserId = commits.userId;
            console.log("MY ID = " ,commits);
            getTerminalsFromServer();

        });
}

function showAcceptButtonResponse() {
    let element = document.getElementsByClassName("reponse-acceptButton")[0];
    element.style.opacity = 1;
    element.innerText = "Terminal(s) was shared";
    setTimeout(() => {
        element.style.opacity = 0;
    }, 2000);
}

startUp();
