class UserHub {

    constructor(connection) {

        try {

            const _connection = connection;

            this.Start(_connection);

            _connection.onclose(async () => {
                await this.Start(_connection);

                window.location.reload();

            });

            _connection.on("MessageToGroup", (messageJson) =>
                { this.MessageToGroup(messageJson); });

            _connection.on("EditMessage", (messageJson) =>
                { this.EditMessage(messageJson); });

            _connection.on("DeleteMessage", (messageId) =>
                { this.DeleteMessage(messageId); });

            _connection.on("TypingNotification", (name) =>
                { this.TypingNotification(name); });

            _connection.on("ChangeViewState", (messageJson) =>
                { this.ChangeViewState(messageJson); });

        }

        catch (exception) {

            console.warn('Websocket protocol problem');
            console.log(exception);

            window.location.reload();

        }

    }

    async Start(_connection) {

        _connection.start().then(function () {

            if (_connection.state === signalR.HubConnectionState.Connected) {

                var openRequest = indexedDB.open('Messenger', 1);

                openRequest.onsuccess = function (event) {

                    var db = event.target.result;
                    var transaction = db.transaction(['chats'], 'readonly');
                    var store = transaction.objectStore('chats');

                    var cursorRequest = store.openCursor();

                    cursorRequest.onsuccess = function (event) {

                        var cursor = event.target.result;

                        if (cursor) {

                            _connection.invoke("JoinChat", cursor.value.chat_id).catch(function (err) {
                                return console.error(err.toString());
                            });

                            cursor.continue();

                        }

                    };

                };

                openRequest = indexedDB.open('Messenger', 1);

                openRequest.onsuccess = function (event) {

                    var db = event.target.result;
                    var transaction = db.transaction(['dialogues'], 'readonly');
                    var store = transaction.objectStore('dialogues');

                    var cursorRequest = store.openCursor();

                    cursorRequest.onsuccess = function (event) {

                        var cursor = event.target.result;

                        if (cursor) {

                            _connection.invoke("JoinChat", cursor.value.chat_id).catch(function (err) {
                                return console.error(err.toString());
                            });

                            cursor.continue();

                        }

                    };

                };

            }

            else {

                this.Start();

            }

        })
    }

    MessageToGroup(messageJson) {

        try {

            document.querySelector('.Chat-Wrapper').style
                .scrollBehavior = 'auto';

            var messageObject = JSON.parse(messageJson);
            var request = indexedDB.open('Messenger', 1);

            request.onsuccess = function (requestSeccess) {

                let db = requestSeccess.target.result;
                let transaction = db.transaction(['users'], 'readwrite');
                let store = transaction.objectStore('users');
                let request = store.get(messageObject.user_id);

                request.onsuccess = function (requestSeccess) {

                    let userObject = requestSeccess.target.result;

                    const urlParams = new URLSearchParams(window.location.search);
                    const chatId = urlParams.get('id');

                    UserHubCOR.MoveChatToTop(messageObject);

                    if (chatId == messageObject.chat_id) {

                        UserHubCOR.RemoveAvatar(userObject.root);
                        UserHubCOR.ChangeLastMessage(userObject.root);
                        UserHubCOR.SaveMessage(messageObject);

                        let type = null;

                        if (UserHubCOR.isUserMessageLast(userObject.root))
                            type = 'last';

                        UserHubCOR.AppendMessageReverse(messageObject, type);

                    }

                }

            }

        }

        catch (ex) {

            console.error(ex);

        }

    }

    DeleteMessage(messageId) {

        try {

            let message = document.querySelector(`[data-message-id="${messageId.toString()}"]`);
            let reply = message.previousElementSibling;
            let avatar = message.querySelector('.message-avatar');

            if (reply.className == 'Reply-Message-Header') {

                reply.remove();

            }

            message.remove();

            var request = indexedDB.open('Messenger', 1);

            request.onsuccess = function (requestSeccess) {

                let db = requestSeccess.target.result;
                let transaction = db.transaction(['messages'], 'readwrite');
                let store = transaction.objectStore('messages');
                let request = store.get(parseInt(message.getAttribute('data-message-id')));

                request.onsuccess = function (requestSeccess) {

                    messageObject = requestSeccess.target.result;

                    UserHubCOR.ChangeLastMessage(messageObject.user_id);

                    if (avatar != null)
                        UserHubCOR.AppendAvatar(messageObject.user_id, avatar);

                }

            };

        }

        catch (ex) {

            console.error(ex);

        }

    }

    TypingNotification(name) {

        TypingMessage
            .SendTypingMessageNotification(name);

    }

    EditMessage(messageJson) {

        let messageObject = JSON.parse(messageJson);
        let message = document.querySelector(`.message[data-message-id="${messageObject.id}"]`);
        let messageInformation = message.querySelector(`.message-information`)
        let span = document.createElement('span');

        span.style.marginLeft = '8px';
        span.innerHTML = 'edited';

        message.querySelector(`.message-content p`).innerHTML =
            messageObject.text;

        if (!messageInformation
            .querySelector('span[id="edited"]')) {

            messageInformation.append(span);

        }

        var request = indexedDB.open("Messenger");

        request.onsuccess = function (event) {

            var db = event.target.result;
            var transaction = db.transaction("messages", "readwrite");
            var objectStore = transaction.objectStore("messages");

            objectStore.put(messageObject);

        };

    }

    ChangeViewState(messageJson) {

        let messageObject = JSON.parse(messageJson);

        try {

            setTimeout(() => {

                let attribute = `[data-message-id="${messageObject.id}"]`;
                let message = document.querySelector(attribute);
                let messageInformation = message.querySelector(`.message-information`)

                if (messageInformation.querySelector('.dispatch-stage') != null) {

                    messageInformation
                        .querySelector('.dispatch-stage')
                        .src = 'Images/Chats/double-check.png';

                }

            }, 2000);

        } catch { }

        var request = indexedDB.open("Messenger");

        request.onsuccess = function (event) {

            var db = event.target.result;
            var transaction = db.transaction("messages", "readwrite");
            var objectStore = transaction.objectStore("messages");

            objectStore.put(messageObject);

        };

    }

    static SendRequest(connection, message) {

        var tokenValue =
            getCookie('.AspNetCore.Mvc.CookieTempDataProvider');

        if (chatId != null && message != null && message != '') {

            connection.invoke("SendMessage", tokenValue, chatId, message,
                (replyMessageId == null) ? null : replyMessageId.toString()).catch(function (err) {

                    return console.error(err.toString());

                });

        }

        let reply = Reply.GetInstance();
        reply.CloseReplyView();

    }

}