var db = null;
var options = null;
var browserInfo = null;
var connection = null;
var interval = 15 * 1000;

const objectStoreName = 'pushgenger'

importScripts('./webworker/signalr.min.js')
importScripts('./helper.js')

function InitializeIndexDb() {
    console.log('Db Initializing...')
    return new Promise((mainResolve, mainReject) => {
        new Promise((resolve, reject) => {
            const request = self.indexedDB.open(objectStoreName, 1);

            request.onerror = event => {
                console.log('error openin IndexedDb')
                reject();
            }

            request.onsuccess = event => {
                const db = event.target.result;
                db.onerror = event => {
                    console.log('error openin IndexedDb')
                }
                resolve(db)
            }

            request.onupgradeneeded = event => {
                event.target.result.createObjectStore(objectStoreName);
            }
        }).then((_db) => {
            console.log('Db Initialized Successfully...')
            db = _db;
            
            mainResolve(_db)
        }).catch((ex) => {
            console.log(ex)
            console.log('Db Initialize Unsuccessfully...')
            mainReject('Db Initialize Unsuccessfully...');
        })
    })
}

function InitializePushengerApp(_options) {
    console.log('Initialize Pushenger Application')
    options = _options;

    InitializeIndexDb().then(() => {
        CreateConnection(options.notificationCallback);
    })
}

function GetBrowserInfo() {
    if (browserInfo)
        browserInfo = GetInfo();
}

function GetOldConnectionId() {
    return new Promise((resolve) => {
        const transaction = db.transaction(objectStoreName, 'readwrite');
        transaction.objectStore(objectStoreName).get('connectionId').onsuccess = event => {
            var oldConnection = event.target.result;
            if (oldConnection === undefined)
                resolve("")
            else
                resolve(event.target.result)
        }
    })
}

function CreateConnectionUrl() {
    return new Promise((resolve) => {
        const deviceInfo = GetInfo();
        GetOldConnectionId().then((oldConnectionId) => {
            const url = `http://localhost:51291/subscription?ProjectKey=${options.projectUniqueKey}&TopicKey=${options.topicUniqueKey}&Os=${deviceInfo.os}&OsVersion=${deviceInfo.osVersion}&App=${deviceInfo.browser}&AppVersion=${deviceInfo.browserVersion}&OldConnectionId=${oldConnectionId}`
            resolve(url);
        })

    })
}

function CreateConnection(notificationCallback) {
    CreateConnectionUrl().then((connectionUrl) => {
        GetOldConnectionId().then((oldConnectionId) => {
            connection = new signalR.HubConnectionBuilder().withUrl(connectionUrl, {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
            }).build();
            connection.start().then(() => {
                if (oldConnectionId) {
                    connection.invoke('getOldNotification');
                    connection.on('getOldNotifications', (notifications) => {
                        notifications.map(n => {
                            SendNotificationService(n)
                        });
                    })
                }

                connection.on('subscribed', (connectionId) => {
                    const transaction = db.transaction(objectStoreName, 'readwrite');
                    transaction.objectStore(objectStoreName).put(connectionId, 'connectionId').onsuccess = event => {

                    }
                })

                connection.on("onmessage", (notificationItem) => {
                    SendNotificationService(notificationItem)
                })

                connection.on("unSubscribed", () => {
                    const transaction = db.transaction(objectStoreName, 'readwrite');
                    transaction.objectStore(objectStoreName).delete('connectionId').onsuccess = event => {

                    }
                })
            }).catch((err) => {
                console.log(err)
            }).finally(() => {
                CheckConnectionState();
            })
        })
    })

}

function SendNotificationService(notification) {
    let data = {
        UniqueKey: notification.uniqueKey
    }
    connection.invoke('onReceive', data);
    options.notificationCallback(notification);
}

function CheckConnectionState() {
    setInterval(() => {
        console.log(connection)
        if (connection != null && connection.state === signalR.HubConnectionState.Connected)
            return
        connection = null;
        clearInterval(this);
        CreateConnection(options.notificationCallback);
    }, interval)
}