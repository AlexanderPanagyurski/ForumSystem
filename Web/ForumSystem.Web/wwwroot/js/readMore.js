function readMore(commentId) {
    console.log(commentId);
    var x = document.getElementById(`short-content ${commentId}`);
    var y = document.getElementById(`whole-content ${commentId}`);
    var hyperlink = document.getElementById(`read-more ${commentId}`);
    if (y.style.display === "none") {
        x.style.display = "none";
        y.style.display = "block";
        hyperlink.innerHTML = "Read Less";
    } else {
        x.style.display = "block";
        y.style.display = "none";
        hyperlink.innerHTML = "Read More";
    }
}