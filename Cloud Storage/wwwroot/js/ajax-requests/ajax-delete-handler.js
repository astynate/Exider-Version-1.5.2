class AjaxDeleteHandler extends AjaxHandler {

    SendAjaxRequest() {

        var xhr = new XMLHttpRequest();

        xhr.open(this.method, this.path, true);

        xhr.onload = function () {

            if (this.status == 200) {

                location.reload();

            }

        };

        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.send(JSON.stringify(this.data));

    }

}