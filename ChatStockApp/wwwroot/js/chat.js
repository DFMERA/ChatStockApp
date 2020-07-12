"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
    li.innerHTML = '<strong>' + user + '</strong>:&nbsp;&nbsp;' + msg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
    if (message.substring(0, 1) == "/") {
        $.ajax({
            url: "Home/Listen",
            data: { user: user, messageTxt: message },
            success: function (data) {
                //call is successfully completed and we got result in data
            },
            error: function (err, ajaxOptions, thrownError) {
                console.error(err.toString());
            }
        });
    }
    
});

