var notificationButton = document.querySelector('#notification');

notificationButton.addEventListener('click', () => {

    document.querySelector('.notifications-menu').id = document.querySelector('.notifications-menu').id == 'active' ?
        'passive' : 'active';

});

window.addEventListener('click', (event) => {

    if (event.target.id != 'notification') {

        document.querySelector('.notifications-menu')
            .id = 'passive';

    }

});