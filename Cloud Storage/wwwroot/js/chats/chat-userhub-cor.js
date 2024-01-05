class UserHubCOR {

    static SaveMessage(messageJson) {

        var request = indexedDB.open('Messenger', 1);

        request.onupgradeneeded = function (event) {

            var db = event.target.result;
            var chats = db.createObjectStore('chats', { keyPath: 'chat_id' });
            var messages = db.createObjectStore('messages', { keyPath: 'id' });

        };

        request.onsuccess = function (event) {

            var db = event.target.result;
            var transaction = db.transaction(['messages'], 'readwrite');
            var store = transaction.objectStore('messages');

            store.add(messageJson);

            transaction.oncomplete = function () {
                db.close();
            };

        };

        request.onerror = function () {
            console.log('Ошибка при открытии/создании базы данных IndexedDB');
        };

    }

    static isUserMessageLast(userId) {

        let lastMessage = document
            .querySelector('#all-messages').lastElementChild;

        return lastMessage
            .getAttribute('data-user-id') == userId;

    }

    static RemoveAvatar(userId) {

        try {

            let lastMessage = document
                .querySelector('#all-messages').lastElementChild;

            let lastMessageAvatar = lastMessage
                .querySelector('.message-avatar');

            let reply = lastMessage.previousElementSibling;

            if (UserHubCOR.isUserMessageLast(userId)
                && lastMessageAvatar != null && reply.className != 'Reply-Message-Header') {

                lastMessage
                    .querySelector('.message-avatar').remove();

            }

        } catch { }

    }

    static AppendAvatar(userId, avatar) {

        let lastMessage = document
            .querySelector('#all-messages').lastElementChild;

        if (UserHubCOR.isUserMessageLast(userId)) {

            lastMessage.append(avatar);

            if (lastMessage.classList.contains('last') ||
                lastMessage.classList.contains('middle')) {

                lastMessage.classList.remove(lastMessage
                    .classList[lastMessage.classList.length - 1]);

                lastMessage.classList.add('last');

            }

            else {

                lastMessage.classList.remove(lastMessage
                    .classList[lastMessage.classList.length - 1]);

            }

        }

    }

    static async AppendMessageReverse(messageObject, type) {

        return new Promise(async (resolve, reject) => {

            var message_wrapper = document.getElementById('all-messages');
            var div = document.createElement('div');
            var tokenValue = getCookie('.AspNetCore.Mvc.CookieTempDataProvider');
            var userEmail = UserHubCOR.GetEmailByJwt(tokenValue);
            var userObject = await ClientDatabase.GetEntityFromTable('users', messageObject.user_id);
            var forward = null;
            var viewStatus = '';

            messageObject.text = UserHubCOR.ProcessMessageLinks(messageObject.text);

            if (messageObject.type == 'invite') {

                UserHubCOR.SendInviteMessage(messageObject, userObject);
                resolve(true);

            }

            else {

                try {

                    UserHubCOR.RemoveAvatar(userObject.root);
                    UserHubCOR.SaveMessage(messageObject);

                    if (messageObject.type == 'reply') {

                        message_wrapper.append
                            (await CreateReplyHeader(messageObject));

                    }

                } catch (exception) {

                    console.warn(exception);

                }

                div.className = 'message';

                if (type != null) {

                    div.classList.add(type);

                }

                if (messageObject.type == 'forward') {

                    forward = messageObject.reply;

                }

                if (userEmail == userObject.email) {

                    viewStatus = ((parseInt(messageObject.is_viewed) >= 1) ? '<img src="Images/Chats/double-check.png" class="dispatch-stage">' :
                        '<img src="Images/Chats/check.png" class="dispatch-stage">');
                }

                div.id = (userEmail == userObject.email) ? 'my' : 'None';
                div.setAttribute('data-message-id', messageObject.id);
                div.setAttribute('data-user-id', messageObject.user_id);
                div.innerHTML =

                    `<img src="data:image/png;base64,${userObject.avatar}" class="message-avatar">
                    <div class="message-content">` + ((forward != null) ? `<h1>Forwarded from: <span style="font-weight: 400;">${forward}</span></h1>` : '') +
                    `<p>${messageObject.text}</p>
                    </div>
                    <div class="message-information"">
                    <span>${messageObject.time.match(/\d{2}\.\d{2}\.\d{4} \d{1,2}:\d{2}/)[0].split(" ")[1]}</span>
                    ` + ((messageObject.is_edited == 1) ? '<span style="margin-left: 8px;" id="edited">edited</span>' : '') + viewStatus + `
                </div>`;

                message_wrapper.append(div);
                document.getElementById('chat').scrollTop = element.scrollHeight;

                resolve(true);

                if (userEmail != userObject.email) {

                    await UserHubCOR.ViewMessage(messageObject);

                }

            }

        });

    }

    static ChangeLastMessage(userId) {

        let lastMessage = document
            .querySelector('#all-messages').lastElementChild;

        if (UserHubCOR.isUserMessageLast(userId)) {

            if (lastMessage.classList.contains('last') ||
                lastMessage.classList.contains('middle')) {

                lastMessage.classList
                    .remove('last');

                lastMessage.classList
                    .add('middle');

            }

            else {

                lastMessage.classList
                    .add('first');

            }

        }

    }

    static MoveChatToTop(message) {

        let chat = document
            .querySelector(`[data-chat-id="${message.chat_id}"]`);

        chat.querySelector('p')
            .innerHTML = message.text;

        chat.querySelector('span')
            .innerHTML = message.time.match(/\d{2}\.\d{2}\.\d{4} \d{1,2}:\d{2}/)[0].split(" ")[1];

        const chatsWrapper = document
            .getElementById('all-chats');

        const allChats = Array
            .from(chatsWrapper.children);

        if (allChats[0].getAttribute('data-chat-id') != message.chat_id) {

            chat.remove();
            chatsWrapper.prepend(chat);

        }

    }

    static GetEmailByJwt(token) {

        var parts = token.split('.');
        var payload = parts[1];
        var decodedPayload = atob(payload);
        var payloadJson = JSON.parse(decodedPayload);

        return payloadJson["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];

    }

    static GetUserIdByJwt(token) {

        var parts = token.split('.');
        var payload = parts[1];
        var decodedPayload = atob(payload);
        var payloadJson = JSON.parse(decodedPayload);

        return payloadJson["sub"];

    }

    static ProcessMessageLinks(message) {

        const regex = /((http|https|ftp):\/\/[^\s]+)/g;
        const replacedMessage = message.replace(regex, '<a href="$1" target="_blank">$1</a>');

        return replacedMessage;

    }

    static SendInviteMessage(message, user) {

        var div = document.createElement('div');
        var message_wrapper = document.getElementById('all-messages');

        div.className = 'Invite-Message';
        div.id = 'invite';

        div.innerHTML = 

            `<div class="Inviter-Div">
                    <img src="data:image/png;base64,${user.avatar}" class="Inviter">
                </div>
                <h1>${user.name} ${user.surname}</h1>
                <p>${user.nickname} — ${message.time.split(' ')[0]}, ${message.time.match(/\d{2}\.\d{2}\.\d{4} \d{1,2}:\d{2}/)[0].split(" ")[1]}</p>`

        message_wrapper.append(div);

    }

    static async ViewMessage(message) {

        if (message.is_viewed < 1 || message.is_viewed == null) {

            connection.invoke('ChangeViewState', message.id.toString())
                .catch(function (err) {

                    return console.error(err.toString());

                });

        }

    }

}

async function CreateReplyHeader(message) {

    return new Promise((resolve, reject) => {

        try {

            var request = indexedDB.open('Messenger', 1);

            request.onsuccess = async function (requestSeccess) {

                var db = requestSeccess.target.result;
                var transaction = db.transaction(['users'], 'readwrite');
                var store = transaction.objectStore('users');
                var request = store.get(message.reply.split(' ')[1]);

                request.onsuccess = async function (requestSeccess) {

                    var userObject = requestSeccess.target.result;
                    var reply = document.createElement('div');
                    var replyMessage = await ClientDatabase.GetMessageById(message.reply.split(' ')[0]);

                    if (replyMessage) {

                        reply.className = "Reply-Message-Header";
                        reply.setAttribute('data-message-id', replyMessage.id);

                        reply.innerHTML =
                            `<div class="Reply-Message-Header-Left"></div>
                                <div class="Reply-Message-Header-Right">
                                <div class="Reply-Header-Message-Top">
                                    <div class="Reply-Message-Header-Sender">
                                    <span>Reply to:</span> ${userObject.name} ${userObject.surname}
                                    </div>
                                </div>
                                <div class="Reply-Message-Header-Message">${replyMessage.text}</div>
                            </div>`;

                        resolve(reply);

                    }

                    else {

                        reply.className = "Reply-Message-Header";

                        reply.innerHTML =
                            `<div class="Reply-Message-Header-Left"></div>
                                <div class="Reply-Message-Header-Right">
                                <div class="Reply-Header-Message-Top">
                                    <div class="Reply-Message-Header-Sender">
                                    ${userObject.name} ${userObject.surname}<img src="Images/Chats/go-to.png" class="GoTo"><span>click to go</span>
                                    </div>
                                </div>
                                <div class="Reply-Message-Header-Message">Not Found</div>
                            </div>`;

                        resolve(reply);

                        reject('Message not found');

                    }

                };

                request.onerror = async function (event) {
                    reject(event.target.error);
                };

            };

            request.onerror = async function (event) {
                reject(event.target.error);
            };

        } catch { }

    });

}