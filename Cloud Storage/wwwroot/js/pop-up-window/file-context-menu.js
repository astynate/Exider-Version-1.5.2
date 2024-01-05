class FileContextMenu extends ContextMenu {

    constructor(x, y) {

        const buttons = {

            Preview: 'Images/Files/preview.png',
            Rename: 'Images/Files/edit-black.png',
            Download: 'Images/Files/download.png',
            Delete: 'Images/Files/delete.png'

        };

        const properties = {

            height: 250,
            width: 180

        };

        super(buttons, properties, x, y);

    }

    Preview() {

        localStorage.setItem('right-panel-display-state', 'active');

        let xhr = new XMLHttpRequest();

        xhr.open('GET', '/files/file?id=' + currentFile.getAttribute('data-file-id').toString());

        xhr.onload = function () {

            if (xhr.status == 200) {

                SetPreview(this.responseText);

            }

        }

        xhr.send();

    }

    Download() {

        new AjaxDownloadHandler('files', 'download', 'POST',
            { id: currentFile.getAttribute('data-file-id') });

    }

    Rename() {

        new RenameForm();

    }

    Delete() {

        if (currentFile.classList.contains("folder")) {

            new AjaxDeleteHandler('files', 'delete', 'DELETE',
                { id: currentFile.getAttribute('data-file-id'), folder: true });

        } else {

            new AjaxDeleteHandler('files', 'delete', 'DELETE',
                { id: currentFile.getAttribute('data-file-id'), folder: false });

        }

    }

    static GetInstance(x, y) {

        if (!FileContextMenu.instance) {

            FileContextMenu.instance =
                new FileContextMenu(x, y);

        } else {

            FileContextMenu.instance
                .SetCoordinates(x, y);
        }

        return FileContextMenu.instance;

    }

}

window.addEventListener('click', function (event) {

    FileContextMenu
        .GetInstance(null, event.clientX, event.clientY).Close();

});

document.addEventListener('contextmenu', async function (event) {

    const isTarget = event.target.classList.contains('item')
        || event.target.closest('.item');

    try {

        if (isTarget) {

            currentFile = event.target
                    .closest('.item');

            FileContextMenu.GetInstance(event.clientX, event.clientY).Open();

        }

    }

    catch (exception) {

        console.warn('An error occurred while trying to create a context menu');
        console.error(exception);

    }

    event.preventDefault();

});

var currentFile = null;