class ContextMenu extends Menu  {

    #isOpen = false;

    constructor(buttons, properties, x, y) {

        super(buttons, properties);

        this.$window.id = 'active';
        this.SetCoordinates(x, y);

    }

    ChangeDisplay() {

        this.$window.id = (this.$window.id == 'active') ?
            'passive' : 'active';

    }

    SetCoordinates(x, y) {

        if (y > window.innerHeight - this._height - 30) {

            this.$window.style.top = 
                `${y - this._height}px`; 

        } else {

            this.$window.style.top = 
                `${y}px`; 

        }

        if (x > window.innerWidth - this._width - 30) {

            this.$window.style.left = 
                `${x - this._width}px`; 

        } else {

            this.$window.style.left = 
                `${x}px`; 

        }

    }

    Open() {

        if (this.#isOpen == false) {

            this.$window.style
                .height = this._height + 'px';

            this.$window.style
                .width = this._width + 'px';

            this.#isOpen = true;

        } else {

            this.Close();

        }

    }

    Close() {

        this.$window.style
            .height = 0 + 'px';

        this.$window.style
            .width = 0 + 'px';

        this.#isOpen = false;

    }

}