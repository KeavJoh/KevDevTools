//create connection
var connectionViewCount = new signalR.HubConnectionBuilder().withUrl("/hubs/rabbitMQToolHub").build();

//connect to methods that hub invokes aka receive notifications from hub

//invoke hub methods aka send notification to hub
function createRabbitConnectionOnServer(event) {
    event.preventDefault();

    var form = document.getElementById('rabbitConnectionForm');
    var formData = new FormData(form);

    var connectionData = {
        HostName: formData.get('HostName'),
        UserName: formData.get('UserName'),
        Password: formData.get('Password'),
        VirtualHost: formData.get('VirtualHost'),
        Port: parseInt(formData.get('Port')),
        QueueName: formData.get('QueueName'),
        Durable: formData.get('Durable') === "1",
        Exclusive: formData.get('Exclusive') === "1",
        AutoDelete: formData.get('AutoDelete') === "1",
        SessionId: document.querySelector('input[name="SessionId"]').value,
        ConnectionId: "test",
        Connected: false
    };

    console.log('Sending connection data:', connectionData); // Log data being sent

    connectionViewCount.invoke("CreateRabbitConnection", connectionData)
        .then(function (response) {
            console.log('ConnectionId:', response.connectionId);
            console.log('ConnectionStatus:', response.connected);
        })
}

//start connection
function fulfilled() {
    //do something on start
    console.log('connection to rabbit tool started');
}
function rejected() {
    //do something on error
}

connectionViewCount.start().then(fulfilled, rejected);

document.getElementById('rabbitConnectionForm').addEventListener('submit', createRabbitConnectionOnServer);