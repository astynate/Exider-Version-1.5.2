class AjaxDownloadHandler extends AjaxHandler {

    SendAjaxRequest() {

        var xhr = new XMLHttpRequest();

        xhr.open(this.method, this.path, true);
        xhr.responseType = 'blob';

        xhr.onload = function () {

            if (this.status == 200) {

                let downloadLink = document.createElement('a');
                downloadLink.href = URL.createObjectURL(xhr.response);

                let name = currentFile.querySelector('.item-name').innerHTML;
                let type = xhr.getResponseHeader('File-Type');

                downloadLink.download = `${name}.${type}`;
                document.body.appendChild(downloadLink);
                downloadLink.click();
                document.body.removeChild(downloadLink);

            }

        };

        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.send(JSON.stringify(this.data));

    }

}