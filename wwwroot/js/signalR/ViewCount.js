//create connection
var connectionViewCount = new signalR.HubConnectionBuilder().withUrl("/hubs/viewCounterHub?sessionId=@rabbitObj.SessionId").build();

//connect to methods that hub invokes aka receive notifications from hub
connectionViewCount.on("updateCurrentViews", (value) => {
    var newCountSpan = document.getElementById("currentViewsCount");
    newCountSpan.innerText = value.toString();
})

connectionViewCount.on("updateTotalViews", (value) => {
    var newCountSpan = document.getElementById("totalViewsCount");
    newCountSpan.innerText = value.toString();
})

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