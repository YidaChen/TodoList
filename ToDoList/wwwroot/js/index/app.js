
(function (window, ajax) {
    'use strict';
    var filters = {
		all: function (todos) {
			return todos;
		},
		active: function (todos) {
			return todos.filter(function (todo) {
				return !todo.IsCompleted;
			});
		},
		completed: function (todos) {
			return todos.filter(function (todo) {
				return todo.IsCompleted;
			});
		}
	};
    window.app = new Vue({
        el: '#todoapp',
        created() {
            ajax.getUser();
        },
        data: {
            userName: ''    ,  //登入用戶名字
            todos: ajax.getTodos(), //todos的array
            visibility: 'active',  //當前顯示待辦,完成,全部哪種
            newTodo: '',    //新增的todo
            editedTodo: null,   //正在編輯的todo, 用來判斷li是否顯示輸入框
            beforeEditCache: '' //取消編輯, 復原todo的內容用
        },
        computed: {
            filteredTodos: function(){
                if(!this.todos) return; //防止調用時, ajax.getTodos()還沒取得, 有更好的做法嗎?
                return filters[this.visibility](this.todos);
            },
			remaining: function () {
                if(!this.todos) return;
				return filters.active(this.todos).length;
			}
        },
        methods: {
            addTodo: async function(){
                var value = this.newTodo.trim();
                if(!value) return;
                var todo = {Content: value, IsCompleted: false};
                todo = await ajax.postTodo(todo);//獲取新增成功todo的其他欄位資訊
                this.todos.push(todo);
				this.newTodo = '';
            },
            deleteTodo: async function(todo){
                await ajax.deleteTodo(todo);
                var index = this.todos.indexOf(todo);
                this.todos.splice(index, 1);
            },
            putTodo: async function(todo){
                await ajax.putTodo(todo);
            },
            editingTodo: function(todo){
                this.beforeEditCache = todo.Content;
				this.editedTodo = todo;
            },
            doneEdit: function(todo){
                if (!this.editedTodo) //已經執行過cancelEdit
                    return;
                this.editedTodo = null;
                todo.Content = todo.Content.trim();
                if (!todo.Content) 
                    this.deleteTodo(todo);
                ajax.putTodo(todo);   //這裡需要找時間改改更嚴謹的寫法(ex.更新失敗)
                //console.log(todo);
            },
            cancelEdit: function(todo){
                this.editedTodo = null;
                todo.Content = this.beforeEditCache;
            },
            logout: async function(){
                await ajax.logout();
                window.location.replace("/");
            }
        },
        directives: {
			'todo-focus': function (el, binding) {
				if (binding.value) {
					el.focus();
				}
			}
		}
    });
})(window, ajax);