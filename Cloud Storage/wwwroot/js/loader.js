window.onload = function() {

    setTimeout(function(){

        var loader = document.getElementsByClassName('loader')[0];
        
        if (loader.id = 'active') {
            
            loader.id = 'passive';

        }

    }, 3000);

}