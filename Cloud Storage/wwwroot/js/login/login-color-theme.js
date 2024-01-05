// ------------------------------------------------------------------------ //

function ChangeTheme() {

    if (localStorage.getItem('ColorMode') == 'ligth-color-mode')
    {
        SetColorMode('dark-color-mode');
    }

    else
    {
        SetColorMode('ligth-color-mode');
    }

}

// ------------------------------------------------------------------------ //

function SetColorMode(mode) {

    localStorage.setItem('ColorMode', mode);
    document.body.className = mode;

}

// ------------------------------------------------------------------------ //

SetColorMode(localStorage.getItem('ColorMode'));

// ------------------------------------------------------------------------ //