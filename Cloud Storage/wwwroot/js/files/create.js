var createButton = document.querySelector('#add');

createButton.addEventListener('click', () => {

    document.querySelector('.create').id = document.querySelector('.create').id == 'active' ?
        'passive' : 'active';

});

window.addEventListener('click', (event) => {

    if (event.target.id != 'add') {

        document.querySelector('.create')
            .id = 'passive';

    }

});