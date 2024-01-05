class ChatRendering {

    static RenderChats(modelData) {

        var tokenValue = getCookie('.AspNetCore.Mvc.CookieTempDataProvider');
        var userId = UserHubCOR.GetUserIdByJwt(tokenValue);
        var chatsWrapper = document.querySelector('#all-chats');

        modelData.dialogues = ChatRendering
            .SortChatsByDate(modelData.dialogues);

        for (let i = 0; i < modelData.dialogues.length; i++) {

            let $chat = document.createElement('div');
            let dialogUser = modelData.users.find(user => user.root ==
                ((modelData.dialogues[i].user_id != userId) ? modelData.dialogues[i].user_id : modelData.dialogues[i].admin_id));

            $chat.setAttribute('data-chat-id',
                modelData.dialogues[i].chat_id); 

            $chat.setAttribute('data-user-id',
                dialogUser.root); 

            $chat.id = "chat-link";

            $chat.innerHTML =

                `<div class="Main-Content-Account">
                    <div class="avatar">
                        <img src="data:image/png;base64,${dialogUser.avatar}" alt="avatar">
                        <div class="status" id="online"></div>
                    </div>
                    <div class="Main-Content-Account-Name">
                        <h1>${dialogUser.name} ${dialogUser.surname}</h1>
                        ` + ((modelData.dialogues[i].lastMessage.type == "invite") ? '<p>A chat has been created</p>' : `<p>${((modelData.dialogues[i].lastMessage.user_id == userId) ? 'You: ' : '')} ${modelData.dialogues[i].lastMessage.text}</p>`) + `
                    </div>
                    <span>${modelData.dialogues[i].lastMessage.time.match(/\d{2}\.\d{2}\.\d{4} \d{1,2}:\d{2}/)[0].split(" ")[1]}</span>
                </div>`;

            chatsWrapper.append($chat);

        }

    }

    static SortChatsByDate(chats) {

        return chats.sort((a, b) =>
            ChatRendering.CompareDates(a, b));

    }

    static CompareDates(a, b) {

        const dateA = new Date(a.lastMessage.time
            .replace(/(\d+).(\d+).(\d+) (\d+):(\d+):(\d+)/, "$2/$1/$3 $4:$5:$6"));

        const dateB = new Date(b.lastMessage.time
            .replace(/(\d+).(\d+).(\d+) (\d+):(\d+):(\d+)/, "$2/$1/$3 $4:$5:$6"));

        return dateB - dateA;

    }

}

function getCookie(name) {

    var cookies = document.cookie.split(';');

    for (var i = 0; i < cookies.length; i++) {

        var cookie = cookies[i].trim();

        if (cookie.startsWith(name + '=')) {

            return cookie.substring(name.length + 1);

        }

    }

    return null;

}