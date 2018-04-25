'use strict';
var ajax = {
    getUser: async function () {
        /*axios.get('/api/Account/getUser')
            .then(function (response) {
                //console.log(response);
                app.userName = response.data.name;
            })
            .catch(function (error) {
                console.log(error);
                window.location.replace("/login.html"); //沒通過驗證
            });*/
            try { 
                var response = await axios.get('/api/Account/getUser');
                //console.log(response);
                app.userName = response.data.name;
            } catch (error) {
                console.error(error);
                window.location.replace("/login.html"); //沒通過驗證
            }
    },
    /*getTodos: async function () {
        try { //同步(等待異步)寫法, 但造成todos.filter不能用在promise?
            var response = await axios.get('/api/Todo');
            console.log(response.data);
            return response.data;
        } catch (error) {
            console.error(error);
        }
    },*/
    getTodos: function () {
        axios.get('/api/Todo')
            .then(function (response) {
                //console.log(response.data);
                app.todos = response.data;
            })
            .catch(function (error) {
                console.log(error);
            });
    },
    postTodo: async function (todo) {
        try { 
            var response = await axios.post('/api/Todo', todo);
            //console.log(response.data);
            return response.data;
        } catch (error) {
            console.error(error);
        }
    },
    deleteTodo: async function (todo) {
        try { 
            var response = await axios.delete('/api/Todo/'+todo.Id);
            //console.log(response);
        } catch (error) {
            console.error(error);
        }
    },
    putTodo: async function (todo) {
        try { 
            var response = await axios.put('/api/Todo/'+todo.Id, todo);
        } catch (error) {
            console.error(error);
        }
    },
    logout: async function () {
        try { 
            var response = await axios.get('/api/Account/Logout');
        } catch (error) {
            console.error(error);
        }
    }
};