// Copyright © 2023 Andreev S. Minsk, Belarus

// Color Mode

var colorMode = 'Light';
var selectedField = null;

// Box Operations

function RemoveSelection(object, valid) {

    if (valid) {

        object.style.border = '3px solid rgba(0, 150, 255, 0.25)';

    }
    
    else {

        object.style.border = '3px solid rgba(255, 0, 0, 0.25)';

    }

}

// Box Operations

function SelectBox(object, valid) {

    if (valid) {

        object.style.border = '3px solid rgba(0, 150, 255, 1)';

    }

    else {

        object.style.border = '3px solid rgba(255, 0, 0, 1)';

    }

}

function ChangeColor(object, valid) {

    if (valid) {

        object.style.borderColor = 'rgba(0, 150, 255)';

    }

    else {

        object.style.borderColor = 'rgba(255, 0, 0)';

    }

}

function Validate(object) {

    if (Number(object.id) != null) {

        if (Number(object.id) == 3) {

            return ValidateEmail(object);

        }

        else if (Number(object.id) == 4) {

            return ValidatePassword(object);

        }

    }

    return ValidateName(object);

}

// Change Theme

function SetColorMode(mode) {

    localStorage.setItem('ColorMode', mode);
    document.body.className = mode;

}

SetColorMode(localStorage.getItem('ColorMode'));

function ChangeTheme() {

    if (localStorage.getItem('ColorMode') == 'ligth-color-mode') {

        SetColorMode('dark-color-mode');

    }

    else {

        SetColorMode('ligth-color-mode');

    }

}

// Invoke Functions

var main_form = document.getElementById('main-form');

main_form.querySelectorAll('form .login-entry-field').forEach(element => {
    
    element.querySelector('input').addEventListener('focus', (event) => {

        SelectBox(element, Validate(element));
    
    })

    element.querySelector('input').addEventListener('focusout', (event) => {

        RemoveSelection(element, Validate(element));
    
    })
    
});

function CheckFormValid() {

    var elements = main_form.querySelectorAll('form .login-entry-field');
    var isValid = true;

    for (var i = 0; i < elements.length; i++) {

        (function (index) {

            if (Validate(elements[index]) == false) {
    
                isValid = false;
        
            }

        })(i);

    }

    return isValid;

}

// Invoke Functions

main_form.addEventListener('submit', (event) => {

    event.preventDefault();

    if (CheckFormValid()) {

        alert('Ok');

    }

    else {

        alert('No');

    }

})


// Invoke Functions

document.addEventListener('keydown', (event) => {

    ChangeColor(selectedField, Validate(selectedField));

})