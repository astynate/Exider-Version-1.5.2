var wrapper = document
    .querySelector('.content-wrapper');

function SetRightPanelState() {

    if (!localStorage.getItem('right-panel-display-state')) {

        localStorage.setItem('right-panel-display-state', 'passive');

    }

    wrapper.id = localStorage
        .getItem('right-panel-display-state');

}

window.addEventListener('click', (event) => {

    if (event.target.id == 'show') {

        localStorage.setItem('right-panel-display-state', 
            (wrapper.id == 'active') ? 'passive' : 'active');

    }

    else if (event.target.id == 'scale') {

        localStorage.setItem('right-panel-display-state',
            (wrapper.id == 'full-screen') ? 'active' : 'full-screen');

    }

    else if (event.target.id == 'save') {

        if (document.querySelector('.edit-txt') && currentFileId != null) {

            let xhr = new XMLHttpRequest();
            let data = new FormData();

            data.append('id', currentFileId);
            data.append('text', document.querySelector('.edit-txt').innerHTML);

            xhr.open('POST', 'files/save');
            xhr.send(data);

            SetEditButton();

        }

    }

    else if (event.target.id == 'edit') {

        if (document.querySelector('.edit-txt') && currentFileId != null) {

            document.querySelector('.top-panel-button[id="edit"] img')
                .src = 'Images/Files/save.png';

            document.querySelector('.top-panel-button[id="edit"]')
                .id = 'save';

            document.querySelector('.top-panel-button[id="save"] img')
                .id = 'save';

            document.querySelector('.edit-txt')
                .contentEditable = 'true';

        }

    }

    SetRightPanelState();

});

SetRightPanelState();