// ------------------------------------------------------------------------ //

function SelectBox(object, valid) {

    if (valid)
    {
        object.style.border = '3px solid rgba(0, 150, 255, 1)';
        selectedField = object;
    }

    else
    {
        object.style.border = '3px solid rgba(255, 0, 0, 1)';
        selectedField = object;
    }

}
// ------------------------------------------------------------------------ //

function RemoveSelection(object, valid) {

    if (valid)
    {
        object.style.border = '3px solid rgba(0, 150, 255, 0.25)';
        selectedField = null;
    }

    else
    {
        object.style.border = '3px solid rgba(255, 0, 0, 0.25)';
        selectedField = null;
    }

}

// ------------------------------------------------------------------------ //