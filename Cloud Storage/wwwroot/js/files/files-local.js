//let button = document.getElementById("create_button");
//let formObject = document.querySelector(".create_form");

//button.onclick = () => {

//    formObject.id = (formObject.id == 'active') ? 'passive': 'active';

//}

var imageFormats = ['img', 'png', 'gif', 'jpg', 'jpeg'];
var currentFileId = null;

function SetPreview(response) {

    let fileJson = JSON.parse(response);
    let $title = document.querySelector('.top-panel-center-info');

    currentFileId = fileJson.id;

    $title.querySelector('h1').innerHTML = fileJson.full_name;
    $title.querySelector('p').innerHTML = fileJson.time;

    document.querySelector('.right-content')
        .innerHTML = null;

    if (imageFormats.includes(fileJson.type)) {

        let img = document.createElement('img');

        img.className = 'right-image';
        img.src = 'data:image/png;base64,' + fileJson.file;

        document.querySelector('.right-content')
            .append(img);

    }

    else if (fileJson.type == 'txt') {

        const reader = new FileReader();

        let textEdit = document.createElement('div');

        textEdit.className = 'edit-txt';
        textEdit.contentEditable = 'false';

        reader.onload = function (event) {

            const fileContents = event.target.result;

            textEdit.innerHTML = fileContents;

            document.querySelector('.right-content')
                .append(textEdit);

        };

        const binaryString = atob(fileJson.file);
        const uint8Array = new Uint8Array(binaryString.length);

        for (let i = 0; i < binaryString.length; i++) {

            uint8Array[i] = binaryString.charCodeAt(i);

        }

        const decoder = new TextDecoder('utf-8');
        const decodedString = decoder.decode(uint8Array);
        const blob = new Blob([decodedString]);

        reader.readAsText(blob);

        SetEditButton();

    }

}


window.addEventListener('click', (event) => {

    let isItem = event.target.className == 'item'
        || event.target.closest('.item');

    if (isItem) {

        let item = event.target.closest('.item');
        let xhr = new XMLHttpRequest();

        xhr.open('GET', '/files/file?id=' + item.getAttribute('data-file-id').toString());

        xhr.onload = function () {

            if (xhr.status == 200) {

                SetPreview(this.responseText);

            }

        }

        xhr.send();

    }

});

function SetEditButton() {

    if (document.querySelector('.top-panel-button[id="save"] img')) {

        try {

            document.querySelector('.top-panel-button[id="save"] img')
                .src = 'Images/Files/edit.png';

            document.querySelector('.top-panel-button[id="save"]')
                .id = 'edit';

            document.querySelector('.top-panel-button[id="edit"] img')
                .id = 'edit';

            document.querySelector('.edit-txt')
                .contentEditable = 'false';

        } catch { }

    }

}