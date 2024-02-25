"use scrict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notifications").build();

connection.start().then(function () {
    console.log("server on")
}).catch((error) => console.log(error.toString()))

connection.on("OnConnected", function () {
    OnConnected();
})

function OnConnected() {
    var username = $('hfUsername').val()
    connection.invoke('SaveUserConnection', username)
        .catch(function (error) {
            console.log(error.toString())
        });
}

connection.on("ReceivedNotifications", function (message) {
    DisplayGeneralNotification(message, "You have a new notification!");
})

connection.on("ReceivedClientNotification", function (message, username) {
    DisplayPersonalNotification(message, `${username}, you have a new notification!`);
})