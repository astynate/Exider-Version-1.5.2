
// ------------------------------------------------------------------------ //

main_form.addEventListener('submit', (event) => {
  

    if (CheckFormValid() == false)
    {

        event.preventDefault();
        alert('The data you entered is in an incorrect format.');

    }

})

// ------------------------------------------------------------------------ //