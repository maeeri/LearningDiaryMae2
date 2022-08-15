// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//shows text when info button is clicked, hides text if it is visible,´when info button is clocked
function showInfo() {
    let isAdded = document.getElementById("added");
    if (isAdded == null) {
        let text = "This is a learning diary app. You can manage your study topics and related tasks here."
        let secondText = "You can navigate the site from the navigation bar.";
        let thirdText = "You can add tasks, edit your topics or delete them from the topics list.";
        let tag = document.createElement("div");

        let firstP = document.createElement("p");
        let secondP = document.createElement("p");
        let thirdP = document.createElement("p");

        firstP.innerHTML = text;
        secondP.innerHTML = secondText;
        thirdP.innerHTML = thirdText;

        tag.appendChild(firstP);
        tag.appendChild(secondP);
        tag.appendChild(thirdP);

        tag.id = "added";
        tag.className = "box";
        tag.style.width = "80%";
        tag.position = "sticky";

        let parent = document.getElementById("main").parentNode;
        let main = document.getElementById("main");
        parent.insertBefore(tag, main);

    } else {
        isAdded.remove();
    }
}

//changes the colours of the input fields
window.onload = () => {
    const inputField = Array.from(document.getElementsByTagName('input'));
    inputField.forEach(el => {
        el.addEventListener('focus',
            (event) => {
                event.target.style.background = 'darkGreen';
                event.target.style.color = 'white';
            });

        el.addEventListener('focusout',
            (event) => {
                event.target.style.background = 'white';
                event.target.style.color = 'darkGreen';
            });
    });
};