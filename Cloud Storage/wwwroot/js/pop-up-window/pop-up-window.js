class Window {

    #height = '60vh';
    #width = '450px';

    constructor(name, footer, content) {

        this.$wrapper = document.createElement('div');
        this.$form = document.createElement('div');
        this.$header = document.createElement('div');
        this.$content = document.createElement('div');
        this.$footer = document.createElement('div');
        
        this.SetClasses();
        this.SetContent(name, content, footer);
        this.CreateForm();

        this.$header.querySelector('.exit-button')
            .addEventListener('click', () => this.Delete());

        this.$footer.addEventListener('click', 
            () => this.SendRequest());

    }

    SetClasses() {

        this.$wrapper.className = 'pop-up-window-wrapper';
        this.$form.className = 'pop-up-window';
        this.$header.className = 'pop-up-window-header';
        this.$content.className = 'pop-up-window-content';
        this.$footer.className = 'pop-up-window-footer';

    }

    SetContent(name, content, footer) {

        this.$header.innerHTML = 

            `<span class="pop-up-window-header-description">${name}</span>
             <img src="Images/pop-up-window/exit.png" class="exit-button">`;

        this.$content.innerHTML = content;
        this.$footer.innerHTML = footer;

    }

    CreateForm() {

        this.$wrapper.append(this.$form);
        this.$form.append(this.$header);
        this.$form.append(this.$content);
        this.$form.append(this.$footer);

        document.body.append(this.$wrapper);

    }

    Delete() {

        document.body.removeChild(this.$wrapper);
        delete this;

    }

}