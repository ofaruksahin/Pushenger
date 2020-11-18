self.addEventListener('install', (event) => {
    console.log('install event trigger')
    event.waitUntil(new Promise((resolve) => {
      importScripts('./pushenger.js')
      resolve();
    }))
  })
  
  self.addEventListener('activate', (event) => {
    console.log('activate event trigger.')
    const notificationCallback = (notificationItem) => {
      console.log('notification callback trigger')
      console.log(notificationItem)
      const notificationOption = {
        body: notificationItem.body,
        icon: notificationItem.icon
      };    
      self.registration.showNotification(notificationItem.title, notificationOption);
    }
  
    InitializePushengerApp({
      projectUniqueKey: 'a0981ee8-6fce-4407-988c-60a58c6814df',
      topicUniqueKey: 'eeb454a1-310a-44d2-bfbe-3753d3a58b61',
      notificationCallback: notificationCallback
    })
  
    return self.clients.claim();
  })
  