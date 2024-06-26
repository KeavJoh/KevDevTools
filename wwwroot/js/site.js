// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const sidenav = document.getElementById('full-screen-example');
const sidenavInstance = Sidenav.getInstance(sidenav);

let innerWidth = null;

const setMode = (e) => {
    // Check necessary for Android devices
    if (window.innerWidth === innerWidth) {
        return;
    }

    innerWidth = window.innerWidth;

    if (window.innerWidth < 660) {
        sidenavInstance.changeMode('over');
        sidenavInstance.hide();
    } else {
        sidenavInstance.changeMode('side');
        sidenavInstance.show();
    }
};

setMode();

// Event listeners
window.addEventListener('resize', setMode);
