function ShowButton() {
    document.querySelectorAll(".link")[0].style.opacity = "1";
}

const typingTitleDelay = 130;
const listDelay = 500;

var typed = new Typed('#typed-title', {
    stringsElement: '#typing-title',
    typeSpeed: typingTitleDelay
});

setTimeout(() => {
    document.querySelectorAll("#subtitle p")[0].style.opacity = "1";
}, typingTitleDelay * ("games-cli".length));

setTimeout(() => {
    document.querySelectorAll("#description")[0].style.opacity = "1";

    let list_items = document.querySelectorAll("ul li");
    for (let i = 0; i < list_items.length; i++) {
        setTimeout(() => {
            list_items[i].style.opacity = "1";
        }, listDelay * (i + 1));
    }

    setTimeout(() => {
        ShowButton();
    }, listDelay * (list_items.length + 1));
}, 2000);
