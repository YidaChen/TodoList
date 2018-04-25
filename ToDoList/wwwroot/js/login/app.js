document.getElementById("loginform").addEventListener("submit", function (e) {
    event.preventDefault();
    var form = this;

    axios.post('/api/Account/Login', {
        Email: this.email.value,
        Password: this.password.value
      })
      .then(function (response) {
        console.log(response);
        window.location.replace("/");
      })
      .catch(function (error) {
        console.log(error.response);
        form.password.value = "";
        $('#loginFailAlert').text(error.response.data).show();
      });
});