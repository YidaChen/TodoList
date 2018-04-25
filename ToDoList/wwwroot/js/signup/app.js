document.getElementById("signupform").addEventListener("submit", function (e) {
    event.preventDefault();
    var form = this;

    axios.post('/api/Account/Signup', {
        Name: this.name.value,
        Email: this.email.value,
        Password: this.password.value
    })
        .then(function (response) {
            console.log(response);
            window.location.replace("/");
        })
        .catch(function (error) {
            console.log(error);
            console.log(error.response);
            form.password.value = "";
            $('#signupFailAlert').text(error.response.data).show();
        });
});