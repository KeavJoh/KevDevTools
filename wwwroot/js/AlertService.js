export function triggerAlert(alertCase, message) {
    switch (alertCase) {
        case 'success':
            document.getElementById('alert-success').innerText = message;
            mdb.Alert.getInstance(document.getElementById('alert-success')).show();
            break;
        case 'error':
            document.getElementById('alert-error').innerText = message;
            mdb.Alert.getInstance(document.getElementById('alert-error')).show();
            break;
    }
}