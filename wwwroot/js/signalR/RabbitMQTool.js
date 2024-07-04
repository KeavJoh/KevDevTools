//create connection
var connectionViewCount = new signalR.HubConnectionBuilder().withUrl("/hubs/rabbitMQToolHub").build();

var elementsToDisable = [
    'HostName', 'UserName', 'Password',
    'VirtualHost', 'Port', 'QueueName',
    'Durable', 'Exclusive', 'AutoDelete'
];

//connect to methods that hub invokes aka receive notifications from hub

//invoke hub methods aka send notification to hub
function deleteRabbitConnectionFromServer(event) {
    event.preventDefault();

    connectionViewCount.send("DeleteRabbitConnection");

    document.getElementById('connectionId').value = "";

    document.getElementById('bannerRabbitMQConnected').setAttribute("hidden", true);
    document.getElementById('bannerRabbitMQDisconnected').removeAttribute("hidden");
    document.getElementById('receiveAndSendingSection').setAttribute("hidden", true);

    elementsToDisable.forEach(function (id) {
        document.getElementById(id).removeAttribute("disabled");
        document.getElementById(id).value = "";
    });

    document.getElementById('btnConnect').removeAttribute("hidden");
    document.getElementById('btnDisconnect').setAttribute("hidden", true);

    console.log('Connection deleted');
}
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
        ConnectionId: "",
        Connected: false
    };

    console.log('Sending connection data:', connectionData); // Log data being sent

    connectionViewCount.invoke("CreateRabbitConnection", connectionData)
        .then(function (response) {
            console.log('ConnectionId:', response.connectionId);
            console.log('ConnectionStatus:', response.connected);

            if (response.connected) {
                document.getElementById('connectionId').value = response.connectionId;
                document.getElementById('bannerRabbitMQConnected').removeAttribute("hidden");
                document.getElementById('bannerRabbitMQDisconnected').setAttribute("hidden", true);
                elementsToDisable.forEach(function (id) {
                    document.getElementById(id).setAttribute("disabled", true);
                });
                document.getElementById('btnConnect').setAttribute("hidden", true);
                document.getElementById('btnDisconnect').removeAttribute("hidden");
                document.getElementById('receiveAndSendingSection').removeAttribute("hidden");
            } else {
                document.getElementById('bannerRabbitMQDisconnected').removeAttribute("hidden");
                document.getElementById('bannerRabbitMQConnected').setAttribute("hidden", true);
                document.getElementById('bannerRabbitMQDisconnected').innerText = "Connection failed";
            }
        })
        .catch(function (error) {
            document.getElementById('bannerRabbitMQDisconnected').removeAttribute("hidden");
            document.getElementById('bannerRabbitMQConnected').setAttribute("hidden", true);
            document.getElementById('bannerRabbitMQDisconnected').innerText = error;
        });
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
document.getElementById('btnDisconnect').addEventListener('click', deleteRabbitConnectionFromServer);