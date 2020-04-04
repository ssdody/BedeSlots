$(function () {
    $("#details-btn").click(function () {
        const cardId = $('#select-card').val();
        console.log(cardId);

        $.ajax({
            method: 'GET',
            url: MyAppUrlSettings.CardDetailsUrl,
            data: { cardId: cardId },
            success: function (partialViewResult) {
                $("#details-result").empty();
                $("#details-result").html(partialViewResult);
            },
            error: function (arg, data, value) {
                console.log(arg + data + value);
            }
        })
    });
});