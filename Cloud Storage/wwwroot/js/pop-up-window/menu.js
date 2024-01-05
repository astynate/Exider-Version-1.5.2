class Menu {

    constructor(buttons, args) {

        this.$window = document.createElement('div');
        this._height = 350;
        this._width = 200;

        for (const button in buttons) {

            if (buttons.hasOwnProperty(button)) {

                const buttonObject = document.createElement('div');

                buttonObject.className = 'context-menu-button';
                buttonObject.id = button.split(" ")[0];
                buttonObject.addEventListener('click', 
                    () => { this[button.split(" ")[0]]() });
                
                buttonObject.innerHTML = 

                    `<span>${button}</span>
                     <img src="${buttons[button]}">`;

                this.$window.append(buttonObject);

            }

        }

        if (args != null) {

            if (args.hasOwnProperty('height')) 
                this._height = args['height'];

            if (args.hasOwnProperty('width')) 
                this._width = args['width'];

        }

        this.SetWindowProperties();

    }

    SetWindowProperties() {

        this.$window.className = 'context-menu';
        this.$window.style.height = this._height + 'px';
        this.$window.style.width = this._width + 'px';
        
        document.body.append(this.$window);

    }

}