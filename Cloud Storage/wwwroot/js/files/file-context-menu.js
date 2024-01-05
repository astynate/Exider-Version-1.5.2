//document.addEventListener('contextmenu', function (event) {

//    event.preventDefault();

//    if (event.target.classList.contains('main-content-file')) {

//        form.style.left = `${event.clientX}px`;
//        form.style.top = `${event.clientY}px`;

//        currentFile = event.target;

//        if (event.clientX + formObject.offsetWidth + 200 >= window.innerWidth)
//            form.style.left = `${event.clientX - formObject.offsetWidth}px`;

//        if (event.clientY + 300 >= window.innerHeight)
//            form.style.top = `${event.clientY - 300}px`;

//        if (form.id == 'active') {

//            form.id = 'passive';

//            setTimeout(function () {

//                form.id = 'active';

//            }, 500);

//        }

//        else {

//            form.id = 'active';

//        }


//    }

//});