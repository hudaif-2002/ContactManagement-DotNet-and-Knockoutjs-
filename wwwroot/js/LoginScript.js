
function LoginViewModel() {
    var self = this;
    self.email = ko.observable('');
    self.password = ko.observable('');
    self.errorMessage = ko.observable('');
    self.showLoginForm = ko.observable(true);

    self.submitLogin = function () {
        var email = self.email();
        var password = self.password();

        self.errorMessage('');
        document.getElementById('loadingIndicator').style.display = 'block';

        fetch('/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email: email, password: password }),
        })
            .then(response => {
                document.getElementById('loadingIndicator').style.display = 'none';
                return response.json();
            })
            .then(data => {
                if (data && data.success) {
                    window.location.href = '/Contacts/GetAllContacts'; // Redirect if login is successful
                } else {
                    self.errorMessage(data.message || 'Invalid login credentials');
                }
            })
            .catch(error => {
                document.getElementById('loadingIndicator').style.display = 'none';  // Hide loading
                if (error.message === 'Failed to fetch') {
                    self.errorMessage('Network error. Please try again.');
                } else {
                    self.errorMessage(error.message || 'Something went wrong. Please try again.');
                }
            });
    };
}

ko.applyBindings(new LoginViewModel());

