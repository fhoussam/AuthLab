var popup = null;

function openPopup(url, name, width, height) {
    var left = (window.screen.width - width) / 2;
    var top = (window.screen.height - height) / 2;
    var options = 'width=' + width + ',height=' + height + ',top=' + top + ',left=' + left;
    console.log(url);
    popup = window.open(url, name, options);
    setInterval(function () {
        const cookieValue = getCookie("idsrv.session");
        if (cookieValue) {
            popup.close();
            //location.reload();
            location.replace("/")
        }
        else {

            console.log("auth cookie not here");
        }
    }, 500);
}

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}