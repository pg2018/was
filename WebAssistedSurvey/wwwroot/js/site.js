// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function toggleDivByCheckbox(divId, checkboxId) {
    $(divId).hide();

    $(document).ready(function () {
        $(checkboxId).change(function () {
            if (this.checked) {
                $(divId).show();
            } else {
                $(divId).hide();
            }
        });
    });
}