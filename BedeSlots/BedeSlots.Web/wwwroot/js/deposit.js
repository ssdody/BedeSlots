$(function () {
    const $dform = $('#depositform');

    $dform.on('submit', function (e) {
        e.preventDefault();
        var f = $(this);
        $.post(f.attr('action'), f.serialize(), function (serverData) {        
            $('#status-msg').html(serverData); 

            $('#deposit-amount').val('0');

            let container = $("#component-balance");
            $.get(MyAppUrlSettings.UserBalanceComponent, function (data) { container.html(data); });
        });
    });

    let depositAmount = document.querySelector('#deposit-amount');
    depositAmount.addEventListener("keyup", function () {
        depositAmount.value = depositAmount.value.match(/^\d+\.?\d{0,2}/);
    });
});

