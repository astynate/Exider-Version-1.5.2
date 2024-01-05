class Form {

    constructor(args, route, decription, submit) {

        this.route = route;
        this.$pop_up_window = document.createElement('div');
        this.$pop_up_window.className = 'Pop-Up-Window';
        this.$pop_up_window.innerHTML =

            `<div class="Header">
                <span class="Header-Decription">${decription}</span>
                <img src="Images/Files/exit.png" class="Header-Close">
            </div>
            <form class="Content" name="data"></form>
            <div class="Footer-Submit">
                <span>${submit}</span>
            </div>`;


        Object.keys(args).forEach((key) => {

            let input = document.createElement('input');

            input.placeholder = args[key];
            input.name = key;
            input.maxLength = 35;

            this.$pop_up_window.querySelector('.Content').append(input);

        });

        document.body.append(this.$pop_up_window);

        this.$pop_up_window.querySelector('.Header-Close')
            .addEventListener('click', () => {
                document.body.removeChild(this.$pop_up_window);
                delete this;
            });

    }

}

class FolderForm extends Form {

    constructor() {

        super({ folder_name: 'Folder name' }, '/files/CreateFolder',
            'Create Folder', 'Create');

        this.$pop_up_window.querySelector('.Footer-Submit')
            .addEventListener('click', (event) => { this.SendFormData().bind(this); });

    }

    SendFormData() {

        const xhr = new XMLHttpRequest();
        const data = new FormData(document.forms.data);
        const parameters = new URLSearchParams(window.location.search);
        const folderId = parameters.get('id');

        data.append('id', folderId);
        xhr.open('post', this.route);

        xhr.onload = function () {

            if (this.status == 200) {

                location.reload();

            }

        };

        xhr.send(data);

        document.body.removeChild(this.$pop_up_window);
        delete this;

    }

}

class FileForm extends Form {

    constructor(type) {

        super({ folder_name: 'File name' }, '/files/CreateFile',
            'Create File', 'Create');

        this.$pop_up_window.querySelector('.Footer-Submit')
            .addEventListener('click', (event) => { this.SendFormData().bind(this); });

        this.type = type;

    }

    SendFormData() {

        const xhr = new XMLHttpRequest();
        const data = new FormData(document.forms.data);
        const parameters = new URLSearchParams(window.location.search);
        const folderId = parameters.get('id');

        data.append('id', folderId);
        data.append('type', this.type);

        xhr.open('post', this.route);

        xhr.onload = function () {

            if (this.status == 200) {

                location.reload();

            }

        };

        xhr.send(data);

        document.body.removeChild(this.$pop_up_window);
        delete this;

    }

}

class RenameForm extends Form {

    constructor() {

        super({ name: 'New name' }, '/files/rename', 'Rename', 'Save changes');

        this.$pop_up_window.querySelector('input').value = currentFile.querySelector('.item-name').innerHTML;
        this.$pop_up_window.querySelector('input').focus();

        this.$pop_up_window.querySelector('.Footer-Submit')

            .addEventListener('click', (event) => {

                const xhr = new XMLHttpRequest();
                const data = new FormData(document.forms.data);

                data.append('id', currentFile.getAttribute('data-file-id'));
                xhr.open('post', this.route);

                xhr.onload = function () {

                    if (this.status == 200) {

                        location.reload();

                    }

                };

                xhr.send(data);

                document.body.removeChild(this.$pop_up_window);
                delete this;

            });

    }

}

document.getElementById('create-folder')
    .addEventListener('click', function () {

        new FolderForm();

    });