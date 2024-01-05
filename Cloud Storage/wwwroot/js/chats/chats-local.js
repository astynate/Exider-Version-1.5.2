var chatUrlParametrs = new URLSearchParams(window.location.search);
var chatId = chatUrlParametrs.get('id');
var Chats = document.getElementsByClassName('Main-Content-Account');
var replyMessageId = null;
var editMessageId = null;
var isGroup = false;

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

const userHub = new UserHub(connection);

const links = document
    .querySelectorAll('#chat-link');

links.forEach(link => {

    link.addEventListener('click', async function (event) {

        event.preventDefault();
        chatId = link.getAttribute('data-chat-id');

        Reply.GetInstance().CloseReplyView();
        Edit.GetInstance().CloseReplyView();

        if (link.getAttribute('data-chat-id').split('-')[0] == 'D') SetDialogHeader(this);
        else SetChatHeader(this);

        history.pushState({}, '', `chats?id=${link.getAttribute('data-chat-id')}`);

        if (document.querySelector('.Main-Content-Account[id="active"]')) {

            document.querySelector
                ('.Main-Content-Account[id="active"]').id = 'passive'

        }

        link.querySelector('.Main-Content-Account')
            .id = 'active';

        try {

            document.querySelector('.chat-loader')
                .id = 'active';

        } catch { }

        var xhr = new XMLHttpRequest();

        xhr.addEventListener("load", async function () {

            try {

                document.querySelector('.chat-loader')
                    .id = 'passive';

            } catch { }

            if (xhr.readyState === 4 && xhr.status === 200) {

                let messagesJson = JSON
                    .parse(xhr.responseText);

                document.getElementById('all-messages')
                    .innerHTML = null;

                for (let i = messagesJson.length - 1; i >= 0; i--) {

                    let type = null

                    let isFirst = i > 0 && ((i < messagesJson.length - 1 &&
                        messagesJson[i - 1].user_id == messagesJson[i].user_id &&
                        messagesJson[i + 1].user_id != messagesJson[i].user_id) ||
                        (i == messagesJson.length - 2 &&
                        messagesJson[i - 1].user_id == messagesJson[i].user_id));

                    let isMiddle = i > 0 && (i < messagesJson.length - 2 &&
                        messagesJson[i - 1].user_id == messagesJson[i].user_id &&
                        messagesJson[i + 1].user_id == messagesJson[i].user_id);

                    let isLast = i < messagesJson.length - 2 && ((i > 0 &&
                        messagesJson[i - 1].user_id != messagesJson[i].user_id &&
                        messagesJson[i + 1].user_id == messagesJson[i].user_id) ||
                        (i == 0 && messagesJson[i + 1].user_id == messagesJson[i].user_id));

                    if (isFirst)
                        type = 'first';
                    if (isMiddle)
                        type = 'middle';
                    if (isLast)
                        type = 'last';

                    await UserHubCOR.AppendMessageReverse
                        (messagesJson[i], type);

                }

            } else {

                window.location.reload();

            }

        });

        xhr.open("GET", `/chats/chat?id=${link.getAttribute('data-chat-id')}`);
        xhr.send();

    });

});

window.addEventListener('keypress', (event) => {

    if (!event.shiftKey && event.key === 'Enter') {

        if (messageField.innerHTML != null) {

            if (editMessageId != null) {

                connection.invoke('EditMessage', messageField.innerHTML.toString(), editMessageId.toString())
                    .catch(function (err) {

                        return console.error(err.toString());

                    });

            }

            else {

                UserHub.SendRequest
                    (connection, messageField.innerHTML, replyMessageId);

            }

        }

        messageField.innerHTML = null;
        replyMessageId = null;
        editMessageId = null;
        Edit.GetInstance().CloseReplyView();

        event.preventDefault();

    }

});

function SetDialogHeader(link) {

    try {

        var request = indexedDB.open('Messenger', 1);

        request.onsuccess = function (requestSeccess) {

            let db = requestSeccess.target.result;
            let transaction = db.transaction(['users'], 'readwrite');
            let store = transaction.objectStore('users');
            let request = store.get(link.getAttribute('data-user-id'));

            request.onsuccess = function (requestSeccess) {

                chatObject = requestSeccess.target.result;

                document.querySelector('.Chat-Header-MinDesc #chat-avatar')
                    .src = `data:image/png;base64,${chatObject.avatar}`;

                document.querySelector('.Chat-Header-MinDesc h1')
                    .innerHTML = `${chatObject.name} ${chatObject.surname}`;

            }

        };

        document.querySelector('.Chat-Header')
            .id = 'active';

        document.querySelector('.Chat-Input')
            .id = 'active';

        if (document.querySelector('.chat-not-select')) {

            document.querySelector('.chat-not-select')
                .id = 'passive';

            setTimeout(function () {

                document.querySelector('.chat-not-select')
                    .remove();

            }, 300);

        }

        FocusAtLineEnd();

    }

    catch (exception) {

        console.warn('Indexed DB');
        console.error(exception);

    }

}

function SetChatHeader(link) {

    try {

        var request = indexedDB.open('Messenger', 1);

        request.onsuccess = function (requestSeccess) {

            let db = requestSeccess.target.result;
            let transaction = db.transaction(['chats'], 'readwrite');
            let store = transaction.objectStore('chats');
            let request = store.get(link.getAttribute('data-chat-id'));

            request.onsuccess = function (requestSeccess) {

                chatObject = requestSeccess.target.result;

                document.querySelector('.Chat-Header-MinDesc img')
                    .src = `data:image/png;base64,${chatObject.avatar}`;

                document.querySelector('.Chat-Header-MinDesc h1')
                    .innerHTML = `${chatObject.name}`;

            }

        };

        document.querySelector('.Chat-Header')
            .id = 'active';

        document.querySelector('.Chat-Input')
            .id = 'active';

        if (document.querySelector('.chat-not-select')) {

            document.querySelector('.chat-not-select')
                .id = 'passive';

            setTimeout(function () {

                document.querySelector('.chat-not-select')
                    .remove();

            }, 300);

        }

        FocusAtLineEnd();

    }

    catch (exception) {

        console.warn('Indexed DB');
        console.error(exception);

    }

}