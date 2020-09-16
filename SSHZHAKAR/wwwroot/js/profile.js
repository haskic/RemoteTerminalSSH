function editButtonClick(event) {
    infoAnimation();
    putDataToInputs();
}
function infoAnimation() {

    let infoContainerElement = document.getElementsByClassName("info-col")[0];
    let infoTitleElement = document.getElementsByClassName("info-title")[0];
    let infoElement = document.getElementsByClassName("row-container")[0];
    let editContainer = document.getElementById("edit-container");

    infoTitleElement.style.transform = "rotateX(360deg)";
    infoTitleElement.innerText = "Edit Information";
    setTimeout(() => {
        infoElement.style.transform = "rotateX(360deg)";
        infoElement.style.opacity = "0";
    }, 0);
    setTimeout(() => {
        infoElement.style.display = "none";
        editContainer.style.display = "flex";
        infoElement.style.transform = "rotateX(0deg)";

        setTimeout(() => {
            editContainer.style.opacity = "1";

        }, 50);
    }, 500);
}

function infoAnimationDefault() {

    let infoTitleElement = document.getElementsByClassName("info-title")[0];
    let infoElement = document.getElementsByClassName("row-container")[0];
    let editContainer = document.getElementById("edit-container");

    infoTitleElement.style.transform = "rotateX(0deg)";
    infoTitleElement.innerText = "User Information";
    setTimeout(() => {
        editContainer.style.transform = "rotateX(360deg)";
        editContainer.style.opacity = "0";
    }, 0);
    setTimeout(() => {
        editContainer.style.display = "none";
        infoElement.style.display = "flex";
        editContainer.style.transform = "rotateX(0deg)";
        setTimeout(() => {
            infoElement.style.opacity = "1";
        }, 50);
    }, 500);
}

function getDataFromInputs() {
    let data = {
        Name: document.getElementById("name-input").value,
        LastName: document.getElementById("lastname-input").value,
        NickName: document.getElementById("nickname-input").value,
        Email: document.getElementById("email-input").value
    }
    return data;
}

async function putDataToInputs() {
    let data = await getUserDataFromServer();
    document.getElementById("name-input").value = data.Name;
    document.getElementById("lastname-input").value = data.LastName;
    document.getElementById("nickname-input").value = data.NickName;
    document.getElementById("email-input").value = data.Email;
}

async function getUserDataFromServer() {
    let userObj;
    await fetch('/user/profile/get', {
        method: 'GET',
    })
        .then(response => response.json())
        .then(jsonObj => {
            userObj = JSON.parse(jsonObj);
            console.log(JSON.parse(jsonObj));
        });
    return userObj;

}
async function getPhotoInfo() {
    let userId;
    await fetch('/user/profile/get', {
        method: 'GET',
    })
        .then(response => response.json())
        .then(jsonObj => {
            userObj = JSON.parse(jsonObj);
            console.log(JSON.parse(jsonObj));
            userId = userObj.UserId;
        });
    let image = document.getElementById("profile_photo");

    let photoResult = await fetch(`/static/${userId}.png`, {
        method: 'GET',
    });
    if (photoResult.ok) {
        image.src = `/../static/${userId}.png`;
    }
    else {
        image.src = `/../static/default_user.png`;

    }

}
async function sendUserDataToServer(UserObj) {
    await fetch('/user/profile/set', {
        method: 'POST',
        body: JSON.stringify(UserObj),
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(responseJson => {
            console.log(JSON.parse(responseJson));
        });
    infoAnimationDefault();
}


async function saveButtonClick() {
    let obj = getDataFromInputs();
    await sendUserDataToServer(obj);
    updateUserInfo();
}

async function updateUserInfo() {
    let userObj = await getUserDataFromServer();
    document.getElementById("name_field").innerText = userObj.Name;
    document.getElementById("lastname_field").innerText = userObj.LastName;
    document.getElementById("nickname_field").innerText = userObj.NickName;
    document.getElementById("email_field").innerText = userObj.Email;

}

async function fileUpload() {
    let fileInput = document.getElementById("file-upload");
    const formData = new FormData();
    console.log(fileInput.files[0]);
    formData.append("files", fileInput.files[0])
    let result = await fetch("/user/profile/photoUpload", {
        method: "POST",
        body: formData,
    });
    console.log("File was uploaded? " + result.ok);
    getPhotoInfo();
}

window.onload = () => {
    updateUserInfo();
    getPhotoInfo();
}