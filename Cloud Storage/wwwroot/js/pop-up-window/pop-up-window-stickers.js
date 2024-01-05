class EmojiWindow {

    static window = document
        .querySelector('.pop-up-window-stickers');

    static button = document
        .querySelector('.smile-button');

    static textField = document
        .querySelector('#message-text');

    static ChangeDisplay() {

        EmojiWindow.window.id = (EmojiWindow.window.id == 'active') ? 
            'passive' : 'active';

    }

    static Close(event) {

        let isTarget = !EmojiWindow.window.contains(event.target) &&
            !EmojiWindow.button.contains(event.target)

        if (isTarget) {

            EmojiWindow.window.id = 'passive';

        }

    }

    static AppendEmoji(emoji) {

        alert(emoji);

    }

}

window.addEventListener('click', (event) => {

    window.addEventListener('click', (event) => {

        EmojiWindow.Close(event);

        if (event.target.tagName.toLowerCase() !== 'input') {

            FocusAtLineEnd();

        }

    });

});

EmojiWindow.window.addEventListener('click', (event) => {

    if (event.target.className == 'smile') {

        EmojiWindow.window.id = 'passive';
        EmojiWindow.textField.innerHTML += event.target.innerHTML;
        FocusAtLineEnd();

    }

});