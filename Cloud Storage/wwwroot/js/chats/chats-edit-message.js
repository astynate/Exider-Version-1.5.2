class Edit {

    constructor() {

        this.$wrapper = document
            .querySelector('.Reply');

        this.$wrapper.addEventListener('click',
            () => this.CloseReplyView());

    }

    OpenEditWindow(message, id) {

        this.$wrapper.querySelector('span')
            .innerHTML = `<span class="sender-name">Edit message</span>`;

        this.$wrapper.querySelector('p')
            .innerHTML = message;

        this.$wrapper.id = "active";

        editMessageId = id;

        document.querySelector('#message-text')
            .innerHTML = message;

        document.getElementById('chat')
            .scrollTop = element.scrollHeight;

        FocusAtLineEnd();

    }

    CloseReplyView() {

        this.$wrapper.id = 'passive';

        messageField.innerHTML = null;
        editMessageId = null;

    }

    static GetInstance() {

        if (!Edit.instance) {

            Edit.instance = new Edit();

        } 

        return Edit.instance;

    }

}