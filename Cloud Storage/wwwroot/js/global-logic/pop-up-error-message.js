class ErrorMessage {

    constructor(message) {

        this.$pop_up_window = document.createElement('div');
        this.$pop_up_window.className = 'Error-Message';
        this.$pop_up_window.innerHTML = 

        `<div class="Message">
            <span>${message}</span>
        </div>
        <div class="Error-Footer">
            <span>Close</span>
        </div>`;

        document.body.append(this.$pop_up_window);

        this.$pop_up_window.querySelector('.Error-Footer')

            .addEventListener('click', (event) => {

                document.body.removeChild(this.$pop_up_window);
                delete this;

            });

    }

}

//new Error('Attention! This feature is not supported in this version. (Exider Alpha V-01)');