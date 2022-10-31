
function hideError() {
    for (i = 0; i < $(".text-danger").length; i++) {
        $(".text-danger")[i].style.visibility = "hidden";
    }
}
hideError()

function ValidateEmail(mail) {
    /*https://www.w3resource.com/javascript/form/email-validation.php*/
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
        return (true)
    }
    return (false)
}

function messageBack(response) {
    alert(response.text());
}

async function myFunction() {
    hideError()
    let button = document.getElementById("ContactSubmitButton");
    let theForum = document.getElementById("ContactForum");
    let LoadingPlaceHolder = document.getElementById("LoadingPlaceHolder");
    let nameBox = $("#nameBox");
    let emailBox = $("#emailBox");
    let subjectBox = $("#subjectBox");
    let messageBox = $("#messageBox");
    let name = $("#nameBox")[0].value;
    let email = $("#emailBox")[0].value;
    let subject = $("#subjectBox")[0].value;
    let message = $("#messageBox")[0].value;
    let code = grecaptcha.getResponse();
    let ready = true;
    if (typeof name === "string" && name.trim().length === 0) {
        $("#nameError")[0].style.visibility = "";
        ready = false;
    }
    if (!ValidateEmail(email)) {
        $("#emailError")[0].style.visibility = "";
        ready = false;
 
    }
    if (typeof subject === "string" && subject.trim().length === 0) {
        subject = " ";
    }
    if (typeof message === "string" && message.trim().length === 0) {
        $("#messageError")[0].style.visibility = "";
        ready = false;
    }
    if (typeof code === "string" && code.trim().length === 0) {
        code = "a";
    }
    if (ready) {
        let url = window.location.protocol + "//" + window.location.host+"/api/email/?name=" + encodeURIComponent(name) + "&email=" + encodeURIComponent(email) + "&subject=" + encodeURIComponent(subject) + "&messageBody=" + encodeURIComponent(message) + "&code=" + encodeURIComponent(code);

        theForum.style.visibility = "hidden";
        LoadingPlaceHolder.style.visibility = "visible";

        response = await fetch(url);
        if (response.ok) {
            let reply = await response.text();
            $("#output")[0].innerHTML = reply;
            LoadingPlaceHolder.style.visibility = "hidden";
        }
        else {
            $("#output")[0].innerHTML = "<h4>There was an error.</h4></br><h4>Please reload the page and try again.</h4>";
        }



    }

}

function stopreload(e)
{ 
    e.preventDefault();
    return false;
}

$("#ContactSubmitButton")[0].onclick = (function () { myFunction() });
$("form")[0].onsubmit = (function (e) { stopreload(e)});

