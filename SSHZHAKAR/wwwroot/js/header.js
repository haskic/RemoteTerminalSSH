

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5002/search")
    .build();

const lastSearchResult = [];

hubConnection.on("Send", function (data) {


    console.log("DATA RECEIVE");
    console.log(data);


});

hubConnection.on("GetSearchResult", function (data) {


    console.log("DATA RECEIVE");
    console.log(data);
    if (lastSearchResult[0] != data) {
        let searchResultContainer = document.getElementsByClassName("search-result-container")[0];
        searchResultContainer.innerHTML = "";
        addItemResultToSearch(JSON.parse(data));

    }

    //console.log("DATA" + data);
    //console.log("LastDaTA" + lastSearchResult[0]);
    lastSearchResult[0] = data;


});


hubConnection.start();


function addItemResultToSearch(searchResultObj) {

    let searchResultContainer = document.getElementsByClassName("search-result-container")[0];
    searchResultObj.forEach(value => {
        let el = document.createElement("a");
        el.href = "/user/" + value.UserId;
        let subEl = document.createElement('div');
        subEl.className = "result-row";
        subEl.innerHTML = "<span>@</span>" + value.NickName;
        el.appendChild(subEl);
        searchResultContainer.appendChild(el);
    });
    return false;
}

let profileMenu = {
    isShow: false,
    element: null
};
function profileClick() {
    console.log("profile")
    profileMenu.element = document.getElementsByClassName('profile-menu')[0];
    if (profileMenu.isShow) {
        profileMenu.element.style.opacity = '0';
        setTimeout(() => {
            profileMenu.element.style.display = 'none';
        }, 300)
        profileMenu.isShow = false;
    }
    else if (!profileMenu.isShow) {
        profileMenu.element.style.display = 'flex';
        setTimeout(() => {
            profileMenu.element.style.opacity = '1';
        }, 50)
        profileMenu.isShow = true;
        console.log("ZHAKAR ONE")
    }
}
function getNameFromServer() {

    let element = document.getElementsByClassName("header-profile-username")[0];
    console.log(element);

    fetch('/user/getname', {
        method: 'GET',
    })
        .then(response => response.json())
        .then(commits => {
            console.log(commits);

            console.log(element);
            element.innerText = commits.name;

        });
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
    let image = document.getElementById("header-photo");

    let photoResult = await fetch(`/static/${userId}.png`, {
        method: 'GET',
    });
    console.log("IMAGE TO HEADER");
    if (photoResult.ok) {
        image.src = `/../static/${userId}.png`;
    }
    else {
        image.src = `/../icons/user-ico.png`;

    }

}

function searchChange(event) {
    if (event.target.value != "") {
        
        hubConnection.invoke("GetSearchResult", event.target.value);

    }
    
}
function searchFocusOff() {
    let searchResultContainer = document.getElementsByClassName("search-result-container")[0];
    lastSearchResult[0] = "";
    searchResultContainer.innerHTML = "";
}
getNameFromServer();
getPhotoInfo();
