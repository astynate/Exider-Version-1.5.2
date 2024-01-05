
var Rename = document.getElementById('edit-profile');
var CloseEditName = document.getElementById('edit-name-close');
var EditNamePanel = document.getElementsByClassName('edit-name')[0];
var submit_button = EditNamePanel.querySelector('input[type="submit"]');

Rename.onclick = () => {

    EditNamePanel.id = 'active';
    document.getElementById('old_file_name').value = CurrentFile.value;

    submit_button.value = 'Save changes';

}

CloseEditName.onclick = () => {

    EditNamePanel.id = 'passive';

}