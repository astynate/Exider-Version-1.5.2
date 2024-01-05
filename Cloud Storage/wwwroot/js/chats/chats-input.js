class TypingMessage {

    static isTyping = false;

    static $typingField = document
        .querySelector('.chat-typing-notification');

    static SendTypingMessageNotification(name) {

        if (TypingMessage.isTyping == false) {

            TypingMessage.$typingField.querySelector('span')
                .innerHTML = `${name} is typing`;

            TypingMessage.$typingField.id = 'active';
            TypingMessage.isTyping = true;

            document.getElementById('chat')
                .scrollTop += 50;

            setTimeout(() => {

                TypingMessage.$typingField.id = 'passive';
                TypingMessage.isTyping = false;
      
            }, 1500);

        }

    }

}

const messageField = document
    .querySelector('#message-text');

function FocusAtLineEnd() {

    var range = document.createRange();
    var sel = window.getSelection();
  
    range.selectNodeContents(messageField);
    range.collapse(false);
  
    sel.removeAllRanges();
    sel.addRange(range);
  
    messageField.focus();

}

messageField.addEventListener('input', function() {

    var tokenValue = getCookie('.AspNetCore.Mvc.CookieTempDataProvider');
    var userEmail = UserHubCOR.GetEmailByJwt(tokenValue);

    connection.invoke('TypingNotification', userEmail.toString(), chatId.toString())
        .catch(function (err) {

        return console.error(err.toString());

    });

});

/*FocusAtLineEnd();*/