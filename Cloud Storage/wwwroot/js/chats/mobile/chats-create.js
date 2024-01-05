class ChatCreationMenu extends Menu {

    #isOpen = false;
    #trigger = document
        .querySelector('.write-message');

    constructor() {

        const buttons = {

            'Message': 'Images/Chats/message.png',  
            'Group': 'Images/Chats/group.png',
            'AI Chat': 'Images/Chats/ai.png'
        
        };
        
        const properties = {
        
            height: 150, 
            width: 200
        
        };

        super(buttons, properties);

        this.$window.style.transition = '0s ease';
        this.SetCoordinates();

    }

    Message() {

        new MessageWindow('New message');

    }

    Group() {

        new Error('Attention! This feature is not supported in this version. (Exider Alpha V-0.0.1)');

    }

    AI() {

        new Error('Attention! This feature is not supported in this version. (Exider Alpha V-0.0.1)');

    }

    Open() {

        this.$window.style
            .height = this._height + 'px';

        this.#isOpen = true;

    }

    Close() {

        this.$window.style
            .height = 0;

        this.#isOpen = false;

    }

    ChangeDisplay() {

        if (this.#isOpen) {

            this.Close()

        } else {

            this.Open();

        }

    }

    SetCoordinates() {

        const rect = this.#trigger
            .getBoundingClientRect();
            
        const y = rect.top + 42;

        this.$window
            .style.top = `${y}px`;

        this.$window
            .style.right = `${10}px`;

    }

    static GetInstance() {

        if (!ChatCreationMenu.instance) {

            ChatCreationMenu.instance = 
                new ChatCreationMenu();

        }

        return ChatCreationMenu
            .instance;

    }

}

const createButton = document
    .querySelector('.write-message');

createButton.addEventListener('click', () => {

    try {

        ChatCreationMenu
            .GetInstance().ChangeDisplay();

    }

    catch(exception) {

        console.error(exception);

    }

});