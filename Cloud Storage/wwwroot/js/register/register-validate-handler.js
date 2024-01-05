// ------------------------------------------------------------------------ //

function Validate(object) {

    if (object != null) {

        if (Number(object.id) < 3)
        {

            return ValidateName(object);

        }

        if (Number(object.id) == 3) {

            return ValidateEmail(object);

        }

        else
        {

            return ValidatePassword(object);

        }
        
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