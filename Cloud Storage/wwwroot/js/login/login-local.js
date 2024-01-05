// ------------------------------------------------------------------------ //

main_form.querySelectorAll('form .login-entry-field').forEach(element => {

    var input = element.querySelector('input');
    
    input.addEventListener('focus', () => SelectBox(element,
        Validate(element)));

    input.addEventListener('focusout', () => RemoveSelection(element,
        Validate(element)));
    
});

// ------------------------------------------------------------------------ //

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


// ------------------------------------------------------------------------ //