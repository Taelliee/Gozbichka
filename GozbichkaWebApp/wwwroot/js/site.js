// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Handle modal switching
document.addEventListener('DOMContentLoaded', function() {
    // Switch from Login to Register
    document.querySelectorAll('.switch-to-register').forEach(function(link) {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            var loginModal = bootstrap.Modal.getInstance(document.getElementById('loginModal'));
            if (loginModal) {
                loginModal.hide();
            }
            setTimeout(function() {
                var registerModal = new bootstrap.Modal(document.getElementById('registerModal'));
                registerModal.show();
            }, 300);
        });
    });

    // Switch from Register to Login
    document.querySelectorAll('.switch-to-login').forEach(function(link) {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            var registerModal = bootstrap.Modal.getInstance(document.getElementById('registerModal'));
            if (registerModal) {
                registerModal.hide();
            }
            setTimeout(function() {
                var loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
                loginModal.show();
            }, 300);
        });
    });
});
