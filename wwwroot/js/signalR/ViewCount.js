//create connection
var connectionViewCount = new signalR.HubConnectionBuilder().withUrl("/hubs/viewCounterHub").build();

//connect to methods that hub invokes aka receive notifications from hub
connectionViewCount.on("updateCurrentViews", (value) => {
    var newCountSpan = document.getElementById("currentViewsCount");
    newCountSpan.innerText = value.toString();
})

connectionViewCount.on("updateTotalViews", (value) => {
    var newCountSpan = document.getElementById("totalViewsCount");
    newCountSpan.innerText = value.toString();
})

connectionViewCount.on("ReceiveMessage", (value) => {
    console.log(value);
    var newMessage = document.getElementById("messagesList");
    newMessage.innerHTML += `
        <div class="row mt-4">
            <div class="col-6">
                <div class="form-outline" data-mdb-input-init>
                    <input type="text" class="form-control" value="${value}" disabled />
                    <label class="form-label">Message</label>
                </div>
            </div>
        </div>
    `;
});

//invoke hub methods aka send notification to hub
function newWindowLoadedOnCLient() {
    connectionViewCount.send("NewWindowsLoaded");
}

//start connection
function fulfilled() {
    //do something on start
    console.log('connection started');
    newWindowLoadedOnCLient();
}
function rejected() {
    //do something on error
    console.log('connection rejected');
}

connectionViewCount.start().then(fulfilled, rejected);