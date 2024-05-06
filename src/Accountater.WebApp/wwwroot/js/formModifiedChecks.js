var formModified = false;
var form = document.querySelector('form');

form.addEventListener('change', function () {
  formModified = true;
});

window.onbeforeunload = function () {
  if (formModified) {
    return 'You have unsaved changes! If you leave, your changes will be lost.';
  }
};

form.addEventListener('submit', function () {
  window.onbeforeunload = null;
});
