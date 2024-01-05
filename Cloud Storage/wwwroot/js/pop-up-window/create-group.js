class GroupWindow extends Window {

    #users = [];

    constructor(header) {

        const $content_html =

            `<div class="pop-up-window-content-header-seach">
                <div class="pop-up-window-content-header">
                    <img src="Images/Home/search.png">
                    <input type="text" placeholder="Search for accounts" maxlength="30">
                </div>
                <div class="pop-up-window-content-header-accouns"></div>
            </div>
            <div class="pop-up-window-list"></div>`;

        super(header, 'Submit', $content_html);

        this.$input = this.$content
            .querySelector('input');

        this.$user_list = this.$content
            .querySelector('.pop-up-window-list');

        this.$input.addEventListener('input',
            () => this.GetUsersByPrefix(this.$input.value))

        this.$user_list.addEventListener('click',
            (e) => this.SelectUser(e));

        this.GetFriends();

    }

    SendRequest() {

        if (this.#users.length >= 2) {

            var tokenValue = getCookie('.AspNetCore.Mvc.CookieTempDataProvider');
            var userId = UserHubCOR.GetUserIdByJwt(tokenValue);

            this.Delete();

            if (this.#users.includes(userId) == false) {

                let xhr = new XMLHttpRequest();
                let data = new FormData();

                let usersId = [];

                for (let i = 0; i < this.#users.length; i++) {

                    usersId.push(this.#users[i].id.toString());

                }

                data.append('users', JSON.stringify(usersId));
                xhr.open('POST', '/chats/creategroup');

                xhr.onload = function () {

                    if (xhr.status === 200) {

                        location.reload();

                    } else {

                        alert('Request is not valid');

                    }

                };

                xhr.send(data);

            }

        }

        else {

            alert('Something went wrong');

        }

    }

    SelectUser(event) {

        if (event.target.classList.contains('Pop-Up-Window-Friend') && event.target != null) {

            var tokenValue = getCookie('.AspNetCore.Mvc.CookieTempDataProvider');
            var userId = UserHubCOR.GetUserIdByJwt(tokenValue);

            try {

                if (userId != event.target.id) {

                    if (this.#users.includes(event.target) == false) {

                        let account = document.createElement('div');

                        account.className = 'account';
                        account.id = event.target.id;

                        account.innerHTML =

                            `<img class="account-avatar" id="${event.target.id}" src="${event.target.querySelector('img').src}" />
                             <span class="account-nickname" id="${event.target.id}">${event.target.querySelector('p').innerHTML}</span>
                             <img class="account-exit-button" src="Images/pop-up-window/exit.png" />`;

                        this.#users.push(event.target);
                        let accounts = document
                            .querySelector('.pop-up-window-content-header-accouns');

                        event.target.querySelector
                            ('.Pop-Up-Window-Friend-Submit').id = 'active';

                        accounts.append(account);
                        accounts.scrollTop = accounts.scrollHeight;

                    }

                    else {

                        this.#users = this.#users.filter(function (item) {
                            return item !== event.target;
                        });

                        document.querySelector(`.account[id="${event.target.id}"]`).remove();

                        event.target.querySelector
                            ('.Pop-Up-Window-Friend-Submit').id = 'passive';

                    }

                    window.addEventListener('click', (event) => {

                        let isAccount = event.target.className == 'account' ||
                            event.target.closest('.account');

                        if (isAccount) {

                            this.#users = this.#users.filter(function (item) {
                                return item !== event.target.closest('.account');
                            });

                            if (document.querySelector(`.account[id="${event.target.closest('.account').id}"]`)) {

                                document.querySelector(`.account[id="${event.target.closest('.account').id}"]`)
                                    .remove();

                            }

                            document.querySelector
                                (`.Pop-Up-Window-Friend[id="${event.target.closest('.account').id}"] .Pop-Up-Window-Friend-Submit`).id = 'passive';

                        }

                    });

                }

            } catch { }

        }

    }

    GetUsersByPrefix(prefix) {

        let xhr = new XMLHttpRequest();
        let data = new FormData();

        data.append('prefix', prefix);
        xhr.open('POST', '/chats/users');

        xhr.onload = function () {

            if (xhr.status === 200) {


                document.querySelector('.pop-up-window-list')
                    .innerHTML = xhr.response;

            }

        };

        xhr.send(data);

    }

    GetFriends() {

        let xhr = new XMLHttpRequest();

        xhr.open('GET', '/chats/friends');
        xhr.onload = function () {

            if (xhr.status === 200) {

                document.querySelector('.pop-up-window-list')
                    .innerHTML = xhr.response;

            }

        };

        xhr.send();

    }

}