"use strict";

class Message {
    constructor(user, messageTxt) {
        this.user = user;
        this.messageText = messageTxt;
        this.dateTimeMsg = new Date().toLocaleString();
    }
}

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    var msg = message.messageText.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
    li.innerHTML = '<strong>' + message.user + '</strong>:&nbsp;&nbsp;' + msg;
    var list = document.getElementById("messagesList");
    list.appendChild(li);
    if (list.childElementCount >50) {
        list.removeChild(list.childNodes[0]);
    }
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var objMessage = new Message(user, message);
    //connection.invoke("SendMessage", user, message).catch(function (err) {
    connection.invoke("SendMessage", objMessage).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
    if (message.substring(0, 1) == "/") {
        $.ajax({
            url: "Home/Listen",
            data: { user: user, messageTxt: message },
            success: function (data) {
                
            },
            error: function (err, ajaxOptions, thrownError) {
                console.error(err.toString());
            }
        });
    }
    
});

