
let element = document.createElement("div");
element.className = "search-result-container";
let image = document.createElement("img");
image.id = "header-photo";
document.body.appendChild(image);
if (window.QUnit && window.QUnit.config) {
    window.QUnit.config.autostart = false;
}


QUnit.test("Animation Success test", function (assert) {  
    element = document.createElement("div");
    element.className = "profile-menu";
    document.body.appendChild(element);

    assert.ok(profileClick() === false, "Menu closed! pass!");
    assert.ok(element.style.opacity == 0, "Opacity == 0 pass!");

    assert.ok(profileClick() === true, "Menu is open! pass!");
    assert.ok(element.style.opacity == 0, "Opacity == 1 pass!");

    assert.ok(profileClick() === false, "Menu is closed! pass!");
    
});


QUnit.test("Search response", function (assert) {
    let done = assert.async();

hubConnection.invoke("GetSearchResult", "a");

    setTimeout(() => {
        let res = lastSearchResult[0][0].NickName
        console.log("RESULT = " + res);
        assert.ok(res == "aliastr", "PASS");

        assert.ok(true);
        done();

    }, 100);



});

QUnit.test("Search response 2 ", function (assert) {
    let done = assert.async();
   
    hubConnection.invoke("GetSearchResult", "u");
    
    setTimeout(() => {
        let res = lastSearchResult[0][0].NickName
        console.log("RESULT = " + res);
        assert.ok(res == "unichtogitel", "PASS");

        assert.ok(true);

        done();

    }, 100);

});

QUnit.test("Search response 3 ", function (assert) {
    let done = assert.async();

    hubConnection.invoke("GetSearchResult", "k");

    setTimeout(() => {
        let res = lastSearchResult[0][0].NickName
        console.log("RESULT = " + res);
        assert.ok(res == "kektor228", "PASS");

        assert.ok(true);

        done();

    }, 100);

});
QUnit.test("Search response 4 ", function (assert) {
    let done = assert.async();

    hubConnection.invoke("GetSearchResult", "z");

    setTimeout(() => {
        let res = lastSearchResult[0][0].NickName
        console.log("RESULT = " + res);
        assert.ok(res == "zhakar1", "PASS");

        assert.ok(true,"PASS");

        done();

    }, 100);

});


QUnit.test("Terminal response ", function (assert) {
    let done = assert.async();
    hubConnectionTerminal.invoke("CommandSSH", hubConnectionTerminal.connectionId, "a87867d6-701d-40c7-9ab6-19ec51baac57", "ls");

    setTimeout(() => {
        let res = lastCommandResult;
        console.log("RESPONSE CONSOLE = " + res);
        assert.ok(res == "alexanderfolder\ntexts\n", "PASS");

        assert.ok(true, "PASS");

        done();

    }, 100);

});