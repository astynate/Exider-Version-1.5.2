class ClientDatabase {

    static async Open() {

        return new Promise((resolve, reject) => {

            const request = indexedDB.open('Messenger', 1);

            request.onsuccess = function (event) {
                const db = event.target.result;
                resolve(db);
            };

            request.onerror = function (event) {
                reject(event.target.error);
            };

        });

    }

    static async GetDataByTableName(name) {

        return new Promise(async (resolve, reject) => {

            const db = await ClientDatabase.Open();
            const transaction = db.transaction([name], 'readwrite');
            const store = transaction.objectStore(name);
            const request = store.getAll();

            request.onsuccess = function (event) {
                resolve(event.target.result);
            };

            request.onerror = function (event) {
                reject(event.target.error);
            };

        });

    }

    static async GetEntityFromTable(tableName, value) {

        return new Promise(async (resolve, reject) => {

            const db = await ClientDatabase.Open();
            const transaction = db.transaction([tableName], 'readwrite');
            const store = transaction.objectStore(tableName);
            const request = store.get(value);

            request.onsuccess = function (event) {
                resolve(event.target.result);
            };

            request.onerror = function (event) {
                reject(event.target.error);
            };

        });

    }

    static async GetMessageById(messageId) {

        return new Promise((resolve, reject) => {

            var request = indexedDB.open('Messenger', 1);

            request.onsuccess = function (event) {

                let db = event.target.result;
                let transaction = db.transaction(['messages'], 'readwrite');
                let store = transaction.objectStore('messages');
                let request = store.get(parseInt(messageId));

                request.onsuccess = function (event) {

                    resolve(event.target.result);

                }

            }

        });

    }

}