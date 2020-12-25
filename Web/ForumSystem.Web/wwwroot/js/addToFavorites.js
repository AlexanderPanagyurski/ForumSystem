function addToFavorites(postId) {
    var token = $("#favoritesForm input[name=__RequestVerificationToken]").val();
    var json = { postId: postId };
    $.ajax({
        url: "/api/favorites",
        type: "POST",
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'X-CSRF-TOKEN': token },
        success: function (data) {
            $("#favoritesCount").html(data.favoritesCount);
        }
    });
}