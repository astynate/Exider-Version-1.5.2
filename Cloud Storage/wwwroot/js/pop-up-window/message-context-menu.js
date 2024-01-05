class MessageContextMenu extends ContextMenu  {

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

        super(buttons, properties, x, y);
        this.messageJson = message;

    }

    async Reply() {

        try {

            const replyWindow = Reply.GetInstance();
            Edit.GetInstance().CloseReplyView();

            if (this.messageJson != null) {

                let text = this.messageJson.text
                let id = this.messageJson.id

                let user = await ClientDatabase
                    .GetEntityFromTable('users', this.messageJson.user_id);

                replyWindow.SetReplyMessage(user.name,
                    user.surname, text, id);

            }

        }

        catch (exception) {

            console.error(exception);

        }

    }

    Edit() {

        if (this.messageJson != null) {

            try {

                Reply.GetInstance().CloseReplyView();

                Edit.GetInstance()
                    .OpenEditWindow(this.messageJson.text, this.messageJson.id);

            }

            catch (exception) {

                console.warn('An exception occurred when trying to open the message editing window');
                console.error(exception);

            }

        }

        FocusAtLineEnd();

    }

    Copy() {

        navigator.clipboard.writeText(this.messageJson.text).then().catch(function () {
            new ErrorMessage('The message text was not copied to the clipboard');
        });

        FocusAtLineEnd();

    }

    Pin() {

        new Error('Attention! This feature is not supported in this version. (Exider Alpha V-0.0.1)');

    }

    Forward() {

        new ForwardMessage('Forward message',
            this.messageJson.id);

    }

    Delete() {

        OkCancel.GetInstance
        (
             'Delete message',
             'This message will be deleted for every one and you will be impossible to cancel this action',
             'Delete',
             'Cancel',
              this.messageJson.id
        );

    }

    static GetInstance(message, x, y) {

        if (!MessageContextMenu.instance) {

            MessageContextMenu.instance =
                new MessageContextMenu(message, x, y);

        } else {

            MessageContextMenu.instance
                .SetCoordinates(x, y);

            MessageContextMenu.instance
                .messageJson = message;

        }

        if (message != null) {

            var tokenValue = getCookie('.AspNetCore.Mvc.CookieTempDataProvider');
            var userId = UserHubCOR.GetUserIdByJwt(tokenValue);

            document.querySelector('.context-menu-button[id="Edit"]')
                .style.display = 'flex';

            document.querySelector('.context-menu-button[id="Delete"]')
                .style.display = 'flex';

            if (message.user_id != userId) {

                document.querySelector('.context-menu-button[id="Edit"]')
                    .style.display = 'none';

                document.querySelector('.context-menu-button[id="Delete"]')
                    .style.display = 'none';

            }

        }

        return MessageContextMenu.instance;

    }

}

window.addEventListener('click', function(event) {

    const isCreateButton = !!event.target.classList.contains('write-message') 
        || !!event.target.closest('.write-message');

    MessageContextMenu
        .GetInstance(null, event.clientX, event.clientY).Close();

    if (isCreateButton == false) {

        try {

            ChatCreationMenu
                .GetInstance().Close();

        }

        catch (exception) {

            console.log(exception);

        }

    }

});

document.addEventListener('contextmenu', async function(event) {

    event.preventDefault();

    const isTarget = event.target.classList.contains('message') 
        || event.target.closest('.message');

    try {

        if (isTarget) {

            var message = event.target.closest('.message');

            message.style.background = '#f5f5ff';

            setTimeout(() => {
                message.style.background = 'white';
            }, 2000);

            if (message.getAttribute('data-message-id') != null) {

                messageObject = await ClientDatabase.
                    GetMessageById(message.getAttribute('data-message-id'));

                MessageContextMenu.GetInstance(messageObject,
                    event.clientX, event.clientY).Open();

            }

        }

    }

    catch (exception) {

        console.warn('An error occurred while trying to create a context menu');
        console.error(exception);

    }

    event.preventDefault();

});