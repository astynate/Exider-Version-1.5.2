class Reply {

    constructor() {

        this.$wrapper = document
            .querySelector('.Reply');

        this.$wrapper.addEventListener('click',
            () => this.CloseReplyView());

    }

    SetReplyMessage(name, surname, message, id) {

        this.$wrapper.querySelector('span')
            .innerHTML = `Reply to <span class="sender-name">${name} ${surname}</span>`;

        this.$wrapper.querySelector('p')
            .innerHTML = message;

        this.$wrapper.id = "active";

        replyMessageId = id;

        document.getElementById('chat')
            .scrollTop = element.scrollHeight;

        FocusAtLineEnd();

    }

    CloseReplyView() {

        this.$wrapper.id = 'passive';
        replyMessageId = null;

    }

    static GetInstance() {

        if (!Reply.instance) {

            Reply.instance = new Reply();

        } 

        return Reply.instance;

    }

}