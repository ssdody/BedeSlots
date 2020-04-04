$(function () {
    const $addCardForm = $('#addCardForm');

    $addCardForm.on('submit', function (event) {
        event.preventDefault();
        debugger;
        const dataToSend = $addCardForm.serialize();

        $.post($addCardForm.attr('action'), dataToSend, function (serverData) {

            if (serverData.message != null) {
                debugger;
                //alert(serverData.message);
                $('#status-msg').html(res); 
                return false;
            }
            else {
                debugger;
                $('#AddCardModal').modal('hide');
                $addCardForm.find('input').val('');
                $("#select-card-dropdown").empty();
                $("#select-card-dropdown").html(serverData);
            }
        });
    });
});