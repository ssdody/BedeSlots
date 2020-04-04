$(function () {
    const $rform = $('#retrieve-form');

    $rform.on('submit', function (e) {
        e.preventDefault();
        var f = $(this);
        $.post(f.attr('action'), f.serialize(), function (serverData) {
            $('#status-msg').html(serverData);

            $('#withdraw-amount').val('0');

            let container = $("#component-balance");
            $.get(MyAppUrlSettings.UserBalanceComponent, function (data) { container.html(data); });
        });
    });

    let withdrawAmount = document.querySelector('#withdraw-amount');
    withdrawAmount.addEventListener("keyup", function () {
        withdrawAmount.value = withdrawAmount.value.match(/^\d+\.?\d{0,2}/);
    });
});