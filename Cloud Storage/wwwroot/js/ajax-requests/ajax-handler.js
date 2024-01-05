class AjaxHandler {

    constructor(controller, action, method, data) {

        this.path = `/${controller}/${action}`;
        this.method = method;
        this.data = data;

        this.SendAjaxRequest();

    }

    SendAjaxRequest() {

        var xhr = new XMLHttpRequest();

        xhr.open(this.method, this.path, true);

        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 302) {
                    let redirectUrl = xhr.getResponseHeader('Location');
                    window.location.href = redirectUrl;
                }
            }
        };

        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.send(JSON.stringify(this.data));

    }

}