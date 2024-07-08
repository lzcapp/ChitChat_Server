"use strict";

// ReSharper disable once UndeclaredGlobalVariableUsing
var connection = new signalR.HubConnectionBuilder().withUrl("/chitchat?username=Server").build();

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
    }
);

connection.on("UserJoined",
    function(connectionID, user) {
        var li = document.createElement("li");
        document.getElementById("messagesList").appendChild(li);
        var currentDate = new Date();
        var formattedDate = currentDate.toLocaleString();
        li.textContent = `[${formattedDate}] [${user}] joined the server.`;
        
        var dropdown = document.getElementById("dropdown");
        var userItem = document.createElement("option");
        userItem.text = user;
        userItem.value = connectionID;
        dropdown.add(userItem);
    }
);

connection.on("UserLeft",
    function(connectionID, user) {
        var li = document.createElement("li");
        document.getElementById("messagesList").appendChild(li);
        var currentDate = new Date();
        var formattedDate = currentDate.toLocaleString();
        li.textContent = `[${formattedDate}] [${user}] left the server.`;

        var dropdown = document.getElementById("dropdown");
        for (var i = 0; i < dropdown.options.length; i++) {
            if (dropdown.options[i].value === connectionID) {
                dropdown.remove(i);
                break;
            }
        }
    }
);

connection.start().then(function() {
    document.getElementById("sendButton").disabled = false;
}).catch(function(err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click",
    function(event) {
        var dropdown = document.getElementById("dropdown");
        var connectionID = dropdown.options[dropdown.selectedIndex].value;
        var user = dropdown.options[dropdown.selectedIndex].text;
        var message = document.getElementById("messageInput").value;
        if (connectionID === "all") {
            connection.invoke("Broadcast", message).catch(function(err) {
                return console.error(err.toString());
            });
        } else {
            connection.invoke("SendPrivateMessage", connectionID, "Server", message).catch(function(err) {
                return console.error(err.toString());
            });
        }
        event.preventDefault();
    }
);