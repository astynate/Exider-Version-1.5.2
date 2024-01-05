class ForwardMessage extends MessageWindow {

    constructor(header, messageId) {

        super(header);

        this.messageId = messageId;

    }

    SendRequest() {

        if (this.currenUser.id != null && this.messageId != null) {

            var tokenValue = getCookie
                ('.AspNetCore.Mvc.CookieTempDataProvider');

            this.Delete();

            connection.invoke("SendForwardMessage", tokenValue, this.currenUser.id,
                this.messageId.toString())

                .catch(function (err) {

                    return console.error(err.toString());

                });

        }

    }

    SelectUser(event) {

        if (event.target.classList.contains('Pop-Up-Window-Friend') && event.target != null) {

            if (event.target != this.currenUser && this.currenUser == null) {

                this.currenUser = event.target;

                event.target.querySelector
                    ('.Pop-Up-Window-Friend-Submit').id = 'active';

            }

            else if (event.target == this.currenUser) {

                this.currenUser = null;
                event.target.querySelector
                    ('.Pop-Up-Window-Friend-Submit').id = 'passive';

            }

            else if (event.target != this.currenUser && this.currenUser != null) {

                this.currenUser.querySelector
                    ('.Pop-Up-Window-Friend-Submit').id = 'passive';

                this.currenUser = event.target;

                event.target.querySelector
                    ('.Pop-Up-Window-Friend-Submit').id = 'active';

            }

        }

    }

    GetUsersByPrefix(prefix) {

        // To Do:
        // Search for user chat

    }

    async GetFriends() {

        const xhr = new XMLHttpRequest();

        xhr.open('GET', '/chats/ChatList');
        xhr.onload = async function () {

            if (xhr.status === 200) {

                let chats = JSON.parse(xhr.response);
                let list = document.querySelector('.pop-up-window-list');
                let tokenValue = getCookie('.AspNetCore.Mvc.CookieTempDataProvider');
                let userId = UserHubCOR.GetUserIdByJwt(tokenValue);

                for (let i = 0; i < chats.length; i++) {

                    let $chat = document.createElement('div');
                    let dialogUser = await ClientDatabase.GetEntityFromTable('users',
                        ((chats[i].user_id != userId) ? chats[i].user_id : chats[i].admin_id));

                    $chat.className = 'Pop-Up-Window-Friend';
                    $chat.id = chats[i].chat_id;
                    $chat.innerHTML =

                        `<img src="data:image/png;base64,${dialogUser.avatar}">
                        <div class="Pop-Up-Window-Friend-Description">
                            <h1>${dialogUser.name} ${dialogUser.surname}</h1>
                        </div>
                        <div class="Pop-Up-Window-Friend-Submit">
                            <img src="Images/Chats/submit.png" id="passive">
                        </div>`;

                    list.append($chat);

                }

            }

        };

        xhr.send();

    }

}