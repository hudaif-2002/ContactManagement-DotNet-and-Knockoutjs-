document.addEventListener('DOMContentLoaded', function () {
    const emailInput = document.getElementById('Email');
    const emailError = document.getElementById('emailError'); 

    // Email validation pattern to ensure proper domain (like .com, .net, etc.)
    const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    // Real-time validation as the user types
    emailInput.addEventListener('input', function () {
        if (!emailPattern.test(emailInput.value.trim())) {
            emailError.textContent = "Enter a valid email address (e.g., user@example.com).";
        } else {
            emailError.textContent = ""; // Clear error when valid
        }
    });

    // Form submission validation
    form.addEventListener('submit', function (event) {
        if (!emailPattern.test(emailInput.value.trim())) {
            event.preventDefault(); // Stop form submission
            emailError.textContent = "Enter a valid email address before submitting.";
        }
    });
});
