"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chitchat").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage",
    function(user, message) {
        var li = document.createElement("li");
        document.getElementById("messagesList").appendChild(li);
        // We can assign user-supplied strings to an element's textContent because it
        // is not interpreted as markup. If you're assigning in any other way, you 
        // should be aware of possible script injection concerns.
        var currentDate = new Date();
        var formattedDate = currentDate.toLocaleString();
        li.textContent = `[${formattedDate}] ${user}: ${message}`;
    });

connection.start().then(function() {
    document.getElementById("sendButton").disabled = false;
}).catch(function(err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click",
    function(event) {
        var message = document.getElementById("messageInput").value;
        connection.invoke("Broadcast", message).catch(function(err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });