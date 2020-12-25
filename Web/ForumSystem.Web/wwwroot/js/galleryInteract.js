function galleryInteract() {
    var x = document.getElementById("gallery");
    var y = document.getElementById("show-hide");
    if (x.style.display === "block") {
        x.style.display = "none";
        y.textContent = "Show Gallery";
    } else {
        x.style.display = "block";
        y.textContent = "Hide Gallery";
    }
}