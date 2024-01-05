class MyMessageContextMenu extends MessageContentMenu {

    constructor(message, x, y) {

        const buttons = {

            Reply: 'Images/Chats/reply.png',  
            Edit: 'Images/Chats/edit.png',
            Copy: 'Images/Chats/copy.png',
            Pin: 'Images/Chats/pin.png',
            Forward: 'Images/Chats/forward.png',
            Delete: 'Images/Chats/delete.png'
        
        };
        
        const properties = {
        
            height: 280, 
            width: 180
        
        };

        super(buttons, properties, message, x, y);

    }

    static GetInstance(message, x, y) {

        if (!MyMessageContextMenu.instance) {

            MyMessageContextMenu.instance =
                new MyMessageContextMenu(message, x, y);

        } else {

            MyMessageContextMenu.instance
                .SetCoordinates(x, y);

            MyMessageContextMenu.instance
                .messageJson = message;

        }

        return MyMessageContextMenu.instance;

    }

}