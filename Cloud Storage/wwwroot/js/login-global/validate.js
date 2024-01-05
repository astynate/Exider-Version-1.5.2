
// ------------------------------------------------------------------------ //

function ValidateEmail(object) {

    var email = object.querySelector('input').value;
    var regex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

    var error_form = document.getElementById(`e${object.id}`);

    if (error_form != null)
    {

        if (email == null || email == '')
        {

            error_form.style.display = 'flex';
            error_form.querySelector('span').innerHTML = "This field must not be empty";

            return false;

        }

        if (email.length < 5)
        {

            error_form.style.display = 'flex';
            error_form.querySelector('span').innerHTML = "Your email address must not be less than 5 characters";

            return false;

        }

        if (!regex.test(email))
        {

            error_form.style.display = 'flex';
            error_form.querySelector('span').innerHTML = "The email address should look like this: zixe.company@gmail.com";

            return false;

        }

        else
        {

            error_form.style.display = 'none';

            return true;

        }

    }

    return false;


}

// ------------------------------------------------------------------------ //

function ValidatePassword(object) {

    var error_form = document.getElementById(`e${object.id}`);

    if (error_form != null)
    {

        if (object.querySelector('input').value.length < 7)
        {

            error_form.style.display = 'flex';
            error_form.querySelector('span').innerHTML = "This field must contain more than 7 characters";

            return false;

        }

        else
        {

            error_form.style.display = 'none';

            return true;

        }

    }

    return false;

}

// ------------------------------------------------------------------------ //

function ValidateName(object) {

    var error_form = document.getElementById(`e${object.id}`);
    var text = object.querySelector('input').value;

    if (text == null || text == '') {

        error_form.style.display = 'flex';
        error_form.querySelector('span').innerHTML = "This field must not be empty";

        return false;

    }

    else {

        error_form.style.display = 'none';
        return true;

    }

}

// ------------------------------------------------------------------------ //
