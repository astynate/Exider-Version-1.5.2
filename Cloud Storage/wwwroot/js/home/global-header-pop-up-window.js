// (c) Andreev S. 2023

class PopUpWindow {

    // Main Constructor
    constructor(pop_up_window, caller) {

        this.$pop_up_window = document.querySelector(pop_up_window);
        this.$caller = document.querySelector(caller);
        this.$caller.addEventListener('click', this.ChangeDisplay.bind(this));

        document.addEventListener('click', (event) => {

            if (!this.$caller.contains(event.target)) {
               
                this.$pop_up_window.id = 'passive';

            }

        });

    }

    // Change Display Method
    ChangeDisplay() {

        this.$pop_up_window.id = (this.$pop_up_window.id == 'active') ?
        'passive' : 'active';

    }

}