class MessageWindow extends Window {

    #currenUser = null;

    constructor(header) {

        const $content_html = 
        
            `<div class="pop-up-window-content-header">
                <img src="Images/Home/search.png">
                <input type="text" placeholder="Search for accounts" maxlength="30">
            </div>
            <div class="pop-up-window-list">
            </div>`;

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

        if (this.currenUser.id != null) {

            var tokenValue = getCookie('.AspNetCore.Mvc.CookieTempDataProvider');
            var userId = UserHubCOR.GetUserIdByJwt(tokenValue);

            if (userId != this.currenUser.id) {

                let xhr = new XMLHttpRequest();
                let data = new FormData();

                data.append('userId', this.currenUser.id);
                xhr.open('POST', '/chats/createdialogue');

                xhr.onload = function () {

                    if (xhr.status === 200) {

                        location.reload();

                    } else if (xhr.status === 400) {

                        alert('Request is not valid');

                    } else if (xhr.status === 455) {

                        alert('A chat with this user already exists');

                    }

                };

                xhr.send(data);

            }

        }

    }

    SelectUser(event) {

        if (event.target.classList.contains('Pop-Up-Window-Friend') && event.target != null) {

            var tokenValue = getCookie('.AspNetCore.Mvc.CookieTempDataProvider');
            var userId = UserHubCOR.GetUserIdByJwt(tokenValue);

            if (userId != event.target.id) {

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

    }

    GetUsersByPrefix(prefix) {

        let xhr = new XMLHttpRequest();
        let data = new FormData();

        data.append('prefix', prefix);
        xhr.open('POST', '/chats/users');

        xhr.onload = function() {

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