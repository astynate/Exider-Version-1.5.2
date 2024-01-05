function CopyMessageText(text) {

    navigator.clipboard.writeText(text).then().catch(function () {
        new ErrorMessage('The message text was not copied to the clipboard');
    });

}