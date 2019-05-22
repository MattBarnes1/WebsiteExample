// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function StartAlerts(User, Token) {
    setInterval(CheckEmail(User, Token), 5);
    setInterval(CheckLore(User, Token) , 5);
    setInterval(CheckPolls(User, Token), 5);
}


var myAjaxEmailRequests = new XMLHttpRequest();
var IntervalEmailBlink;
function CheckEmail(User, SecurityHash) {
    myAjaxEmailRequests.setRequestHeader("AJAX-TOKEN", SecurityHash)
    myAjaxEmailRequests.open("GET", "./EmailSystem/Alert", true, User, SecurityHash); //TODO:true?
    myAjaxEmailRequests.onreadystatechange = EmailCheckResults;
    myAjaxEmailRequests.send(NULL);

}


function EmailCheckResults()
{
    if (myAjaxEmailRequests.response == XMLHttpRequest.DONE) {
        if (myAjaxEmailRequests.status == 200) {
            IntervalEmailBlink = setInterval(blinkImage("emailalert", "emailImageActive", "emailImageInactive"), 2);
        } else {
            document.getElementById("emailalert").img = "emailImageError";
        }
    } else {
        clearInterval(IntervalEmailBlink);
    }
}


var IntervalLoreBlink;
function CheckLore(User, SecurityHash) {
    var myAjaxRequests = new XMLHttpRequest();
    myAjaxRequests.open("GET", "./LoreSystem/Alert", true, User, SecurityHash); //TODO:true?
    myAjaxRequests.onreadystatechange = LoreCheckResults;
    myAjaxRequests.send(NULL);
}

function LoreCheckResults(anXMLHttpRequest, Event)
{
    if (anXMLHttpRequest.response == XMLHttpRequest.DONE) {
        if (myAjaxEmailRequests.status == 200) {
            IntervalLoreBlink = setInterval(blinkImage("lorealert", "loreImageActive", "loreImageInactive"), 2);
        } else {
            document.getElementById("lorealert").img = "loreImageError";
        }
    } else {
        clearInterval(IntervalLoreBlink);
    }
}

function CheckPolls(User, SecurityHash) {
    var myAjaxRequests = new XMLHttpRequest();
    myAjaxRequests.open("GET", "./DisplayPollList?Handler=Alert", true, User, SecurityHash); //TODO:true?
    myAjaxRequests.onreadystatechange = PollsCheckResults;
    myAjaxRequests.send(NULL);
}

var IntervalPollBlink;
function PollsCheckResults()
{
    if (anXMLHttpRequest.response == XMLHttpRequest.DONE) {
        if (myAjaxEmailRequests.status == 200) {
            IntervalPollBlink = setInterval(blinkImage("pollalert", "pollImageActive", "pollImageInactive"), 2);
        } else {
            document.getElementById("pollalert").img = "pollImageError";
        }
    } else {
        clearInterval(IntervalPollBlink);
    }
}

function blinkImage(myElementIDString, ImageOn, ImageOff) {
    if (document.getElementById(myElementIDString).img == ImageOn) { document.getElementById(myElementIDString).img = ImageOff; }
    else { document.getElementById(myElementIDString).img == ImageOff}
}