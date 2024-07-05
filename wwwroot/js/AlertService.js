export function triggerAlert(alertCase, message) {
    switch (alertCase) {
        case 'success':
            document.getElementById('alert-success').innerText = message;
            mdb.Alert.getInstance(document.getElementById('alert-success')).show();
            break;
        case 'danger':
            document.getElementById('alert-danger').innerText = message;
            mdb.Alert.getInstance(document.getElementById('alert-danger')).show();
            break;
    }
}