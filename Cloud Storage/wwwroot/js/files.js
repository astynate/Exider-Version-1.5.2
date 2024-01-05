// Body Glocal Function

document.body.addEventListener('contextmenu', function (event) {

    event.preventDefault();

});

document.body.onclick = () => {

    contextMenu.id = 'passive';

}

// Global variables

var button = document.getElementById("create_button");
var form = document.getElementById("create_form");

// Button click Handler

button.onclick = () => {

    if (form.clientHeight == "0") {

        form.style.height = "300px";

    }

    else {

        form.style.height = "0px";

    }

}

// Global variables

var contextMenu = document.getElementsByClassName('File-Context-Menu')[0];
var Files = document.getElementsByClassName('File');
var CurrentFile = document.getElementById('current_file');
var CurrentPath = "/";

// Context Menu Handler

for (var i = 0; i < Files.length; i++) {

    (function (index) {

        Files[index].addEventListener('contextmenu', function (event) {

            event.preventDefault();

            contextMenu.style.top = `${event.clientY}px`;
            contextMenu.style.left = `${event.clientX}px`;

            CurrentFile.value = Files[index].id;
            document.getElementById("new_file_name").value = Files[index].querySelector("h1").id;

            if (contextMenu.id == 'active') {

                contextMenu.id = 'passive';

                setTimeout(function () {

                    contextMenu.id = 'active';

                }, 400);

            }

            else {

                contextMenu.id = 'active';

            }

        });

        Files[index].onclick = () => {

            if (Files[index].classList.contains('selected')) {

                Files[index].classList.remove('selected');

            }

            else {

                Files[index].classList.add('selected');

            }

        }

    })(i);

}

// Edit Name

var Rename = document.getElementById('rename');
var CloseEditName = document.getElementById('edit-name-close');
var EditNamePanel = document.getElementsByClassName('edit-name')[0];
var submit_button = EditNamePanel.querySelector('input[type="submit"]');

Rename.onclick = () => {

    EditNamePanel.id = 'active';
    document.getElementById('old_file_name').value = CurrentFile.value;

    EditNamePanel.querySelector('h2').innerHTML = 'Edit name';
    submit_button.value = 'Save changes';

}

CloseEditName.onclick = () => {

    EditNamePanel.id = 'passive';

}

// Create Folder

document.getElementById('create-folder').onclick = () => {

    EditNamePanel.id = 'active';
    document.getElementById('new_file_name').value = "";
    document.getElementById('new_file_name').placeholder = "Folder Name";
    document.getElementById('old_file_name').value = CurrentFile.value;

    EditNamePanel.querySelector('h2').innerHTML = 'Create folder';
    submit_button.value = 'Create folder';

}

// Create Document

document.getElementById('create-document').onclick = () => {

    EditNamePanel.id = 'active';
    document.getElementById('new_file_name').value = "";
    document.getElementById('new_file_name').placeholder = "Document name";
    document.getElementById('old_file_name').value = CurrentFile.value;

    EditNamePanel.querySelector('h2').innerHTML = 'Create folder';
    submit_button.value = 'Create document';

}