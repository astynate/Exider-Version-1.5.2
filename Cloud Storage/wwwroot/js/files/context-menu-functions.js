//class FileContextMenu {

//    static context_menu = document.querySelector('.File-Context-Menu');
//    static open = this.context_menu.querySelector('#open');
//    static rename = this.context_menu.querySelector('#rename');
//    static download = this.context_menu.querySelector('#download');
//    static delete = this.context_menu.querySelector('#delete');

//    static Open() {

//        new ErrorMessage('Attention! This function is not supported by the current \
//        version of the program (Exider Alpha V - 0.1)');
//        this.CloseContextMenu();

//    }

//    static Rename() {

//        new RenameForm();
//        this.CloseContextMenu();

//    }

//    static Download() {

//        new AjaxDownloadHandler('files', 'download', 'POST',
//            { id: currentFile.getAttribute('data-file-id') });
//        this.CloseContextMenu();

//    }

//    static Delete() {

//        if (currentFile.classList.contains("folder")) {

//            new AjaxDeleteHandler('files', 'delete', 'DELETE',
//                { id: currentFile.id, folder: true });

//        } else {

//            new AjaxDeleteHandler('files', 'delete', 'DELETE',
//                { id: currentFile.id, folder: false });

//        }

//        this.CloseContextMenu();

//    }

//    static CloseContextMenu() {

//        this.context_menu.id = 'passive';

//    }

//}