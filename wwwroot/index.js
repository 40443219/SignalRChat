(() => {
    const connection = new signalR.HubConnectionBuilder()
                                .withUrl('/chatHub')
                                .build();

    connection.on("ReceiveMessage", (user, msg) => {
        const li = document.createElement('li');
        li.textContent = `[ALL] ${ user } says ${ msg }`;
        document.getElementById('msgDiv').appendChild(li);
    });

    connection.on("ReceiveGroupMessage", (groupName, user, msg) => {
        const li = document.createElement('li');
        li.textContent = `[GROUP: ${ groupName }] ${ user } says ${ msg }`;
        document.getElementById('msgDiv').appendChild(li);
    });

    connection.on('RecGroupMsg', (msg) => {
        const li = document.createElement('li');
        li.textContent = msg;
        document.getElementById('msgDiv').appendChild(li);
    });

    connection.on('UpdateUserList', (userList) => {
        const userListItem = document.getElementById('userList');
        userListItem.innerHTML = '';
        for(const item of userList) {
            const li = document.createElement('li');
            li.textContent = item.username;

            userListItem.appendChild(li);
        }
    });

    // connection.start()
    //             .catch((err) => console.error(err.toString()));

    // document.getElementById('addGroupBtn')
    //         .addEventListener('click', (event) => {
    //             event.preventDefault();

    //             const groupName = document.getElementById('group').value;
    //             const user = document.getElementById('name').value;

    //             connection.invoke('AddGroup', groupName, user)
    //                         .catch((err) => console.error(err.toString()));
    //         });

    document.getElementById('connectionStartBtn')
            .addEventListener('click', (event) => {
                event.preventDefault();

                const groupItem = document.getElementById('group');
                const userItem = document.getElementById('name');
                const selfItem = document.getElementById('connectionStartBtn');
                const msgItem = document.getElementById('msg');
                const submitBtnItem = document.getElementById('submitBtn');
                const submitGroupBtnItem = document.getElementById('submitGroupBtn');

                connection.start()
                        .then(() => {
                            const groupName = groupItem.value;
                            const user = userItem.value;

                            connection.invoke('AddtoUserList', groupName, user)
                                    .catch((err) => console.error(err.toString()));

                            groupItem.disabled = true;
                            userItem.disabled = true;
                            selfItem.disabled = true;
                            msgItem.disabled = false;
                            submitBtnItem.disabled = false;
                            submitGroupBtnItem.disabled = false;
                        })
                        .catch((err) => console.error(err.toString()));

            });

    document.getElementById('submitBtn')
            .addEventListener('click', (event) => {
                event.preventDefault();

                const user = document.getElementById('name').value;
                const msg = document.getElementById('msg').value;

                connection.invoke('SendMessage', user, msg)
                            .catch((err) => console.error(err.toString()));

                // fetch(`Chat/SendMessage?user=${user}&msg=${msg}`,{
                //     method: "POST"
                // });
            });

    document.getElementById('submitGroupBtn')
            .addEventListener('click', (event) => {
                event.preventDefault();

                const user = document.getElementById('name').value;
                const msg = document.getElementById('msg').value;
                const groupName = document.getElementById('group').value;

                connection.invoke('SendGroupMessage', groupName, user, msg)
                            .catch((err) => console.error(err.toString()));
            });
})();