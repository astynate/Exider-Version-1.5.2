var avatar = document.querySelector('.main-content-header-avatar');

avatar.addEventListener('click', () => {

    document.querySelector('.account-menu').id = document.querySelector('.account-menu').id == 'active' ?
        'passive' : 'active';

});

window.addEventListener('click', (event) => {

    if (event.target.className != 'main-content-header-avatar') {

        document.querySelector('.account-menu')
            .id = 'passive';

    }

});