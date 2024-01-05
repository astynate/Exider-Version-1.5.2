// ------------------------------------------------------------------------ //

function Validate(object) {

    if (object != null) {

        return (object.id == '0') ? ValidateName(object) :
            ValidatePassword(object);
        
    }

    else
    {

        return false;

    }

}

// ------------------------------------------------------------------------ //

document.addEventListener('keydown', (event) => {

    if (selectedField != null) {

        SelectBox(selectedField, Validate(selectedField));

    }

})

// ------------------------------------------------------------------------ //