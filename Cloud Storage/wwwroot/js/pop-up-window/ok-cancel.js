class OkCancel {

    constructor(title, text, ok, cancel, messageId) {

        this.$wrapper = document
            .createElement('div');
        this.$window = document
            .createElement('div');
        this.$header = document
            .createElement('div');
        this.$title = document
            .createElement('span');
        this.$description = document
            .createElement('span');
        this.$footer = document
            .createElement('div');
        this.$ok = document
            .createElement('span');
        this.$cancel = document
            .createElement('span');

        this
            .SetClasses();
        this
            .SetContent(title, text, ok, cancel);
        this
            .CreateForm();

        this.messageId = messageId;

        this.$ok.addEventListener('click',
            () => { this.Ok(); });

        this.$cancel.addEventListener('click',
            () => { this.Cancel(); });

    }

    SetClasses() {

        this.$wrapper
            .className = 'pop-up-window-wrapper';
        this.$window
            .className = 'pop-up-window-ok-cancel';
        this.$header
            .className = 'pop-up-window-ok-cancel-header';
        this.$title
            .className = 'pop-up-window-ok-cancel-title';
        this.$description
            .className = 'pop-up-window-ok-cancel-description';
        this.$footer
            .className = 'pop-up-window-ok-cancel-footer';
        this.$ok
            .id = 'ok';
        this.$cancel
            .id = 'cancel';

    }

    SetContent(title, text, ok, cancel) {

        this.$title
            .innerHTML = title;
        this.$description
            .innerHTML = text;
        this.$ok
            .innerHTML = ok;
        this.$cancel
            .innerHTML = cancel;

    }

    CreateForm() {


        document.body
            .append(this.$wrapper);
        this.$wrapper
            .append(this.$window);
        this.$window
            .append(this.$header);
        this.$window
            .append(this.$footer);
        this.$header
            .append(this.$title);
        this.$header
            .append(this.$description);
        this.$footer
            .append(this.$ok);
        this.$footer
            .append(this.$cancel);

    }

    Ok() {

        if (this.messageId != null) {

            connection.invoke("DeleteMessage", chatId, this.messageId.toString())

                .catch(function (err) {

                    return console.error(err.toString());

                });

        }

        this.Cancel();

    }

    Cancel() {

        document.body.removeChild(this.$wrapper);

    }

    static GetInstance(title, text, ok, cancel, messageId) {

        if (!OkCancel.instance) {

            OkCancel.instance = new
                OkCancel(title, text, ok, cancel, messageId);

        } else {

            OkCancel.instance.messageId = messageId;
            OkCancel.instance.CreateForm();

        }

        return OkCancel.instance;

    }

}