//prevent submit on enter
document.body.addEventListener('keypress', e => {
    if (e.keyCode == 13) {
        e.preventDefault();
        return false;
    }
});