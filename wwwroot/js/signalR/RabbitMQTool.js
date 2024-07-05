import { triggerAlert } from '../Services/AlertService.js';

document.getElementById('rabbitConnectionForm').addEventListener('submit', createRabbitConnectionOnServer);
document.getElementById('btnDisconnect').addEventListener('click', deleteRabbitConnectionFromServer);
document.getElementById('btnSendMessage').addEventListener('click', sendMessage);
document.getElementById('selectSendingAndReceivingInSingleQueue').addEventListener('click', switchReceivingSettings)

window.onload = function () {
    switchReceivingSettings();
}

function switchReceivingSettings() {
    var receivingQueueName = document.getElementById('ReceivingQueueName');
    if (document.getElementById('selectSendingAndReceivingInSingleQueue').checked) {
        receivingQueueName.value = "";
        receivingQueueName.setAttribute("disabled", true);
    } else {
        receivingQueueName.removeAttribute("disabled");
    }
}

//create connection
var connectionRabbitMQService = new signalR.HubConnectionBuilder().withUrl("/hubs/rabbitMQToolHub").build();

var formularElemets = [
    'HostName', 'UserName', 'Password',
    'VirtualHost', 'Port', 'SendingQueueName',
    'Durable', 'Exclusive', 'AutoDelete', "ReceivingQueueName"
];

//connect to methods that hub invokes aka receive notifications from hub
connectionRabbitMQService.on("receiveMessage", (value) => {
    document.getElementById('messageList').innerHTML +=
        '<div class="row mt-4 justify-content-center">' +
        '<div class="col-6 d-flex justify-content-center">' +
        '<div class="form-outline" data-mdb-input-init>' +
        '<input type="text" class="form-control" value="' + value +'" disabled />' +
        '</div>' +
        '</div>' +
        '</div>';
})

//invoke hub methods aka send notification to hub
function sendMessage(event) {
    event.preventDefault();

    var message = document.getElementById('messageToSend').value;
    var sendingQueueName = document.getElementById('SendingQueueName').value;
    connectionRabbitMQService.invoke("SendMessageWithRabbitMQ", message, sendingQueueName).then(function (response) {
        if (response) {
            triggerAlert('success', 'Message sent successfully!');
        } else {
            triggerAlert('error', 'Message sending failed!');
        };
    });
}

function deleteRabbitConnectionFromServer(event) {
    event.preventDefault();
    const result = false;
    connectionRabbitMQService.invoke("DeleteRabbitConnection")
        .then(function (response) {
            if (response) {
                document.getElementById('connectionId').value = "";
                document.getElementById('bannerRabbitMQConnected').setAttribute("hidden", true);
                document.getElementById('bannerRabbitMQDisconnected').removeAttribute("hidden");
                document.getElementById('receiveAndSendingSection').setAttribute("hidden", true);
                document.getElementById('btnConnect').removeAttribute("hidden");
                document.getElementById('btnDisconnect').setAttribute("hidden", true);
                document.getElementById('selectSendingAndReceivingInSingleQueue').removeAttribute("disabled");

                formularElemets.forEach(function (id) {
                    document.getElementById(id).removeAttribute("disabled");
                });
                switchReceivingSettings();
                document.getElementById('messageList').innerHTML = "";
                triggerAlert('success', 'Connection deleted successfully!');
            } else {
                triggerAlert('error', 'Connection deletion failed!');
            }
        });
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
        SendingQueueName: formData.get('SendingQueueName'),
        ReceivingQueueName: formData.get('ReceivingQueueName'),
        Durable: formData.get('Durable') === "1",
        Exclusive: formData.get('Exclusive') === "1",
        AutoDelete: formData.get('AutoDelete') === "1",
        SessionId: document.querySelector('input[name="SessionId"]').value,
        ConnectionId: "",
        Connected: false
    };

    console.log('Sending connection data:', connectionData); // Log data being sent

    connectionRabbitMQService.invoke("CreateRabbitConnection", connectionData)
        .then(function (response) {
            console.log('ConnectionId:', response.connectionId);
            console.log('ConnectionStatus:', response.connected);

            if (response.connected) {
                document.getElementById('connectionId').value = response.connectionId;
                document.getElementById('bannerRabbitMQConnected').removeAttribute("hidden");
                document.getElementById('bannerRabbitMQDisconnected').setAttribute("hidden", true);
                formularElemets.forEach(function (id) {
                    document.getElementById(id).setAttribute("disabled", true);
                });
                document.getElementById('btnConnect').setAttribute("hidden", true);
                document.getElementById('btnDisconnect').removeAttribute("hidden");
                document.getElementById('receiveAndSendingSection').removeAttribute("hidden");
                document.getElementById('selectSendingAndReceivingInSingleQueue').setAttribute("disabled", true);
                triggerAlert('success', 'Connection created successfully!');
            } else {
                document.getElementById('bannerRabbitMQDisconnected').removeAttribute("hidden");
                document.getElementById('bannerRabbitMQConnected').setAttribute("hidden", true);
                triggerAlert('error', 'Connection failure! Please check parameters!');
            }
        })
        .catch(function (error) {
            document.getElementById('bannerRabbitMQDisconnected').removeAttribute("hidden");
            document.getElementById('bannerRabbitMQConnected').setAttribute("hidden", true);
            triggerAlert('error', error);
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

connectionRabbitMQService.start().then(fulfilled, rejected);