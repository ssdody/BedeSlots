$(document).ready(function () {
    $rows = $('#rows').val();
    $cols = $('#cols').val();
    $gameName = $('#game-name').val();

    if ($gameName === "Classic 777") {
        $('#game-div').css('height', '660px');
    }

    let directory = "/images/fruits/";
    //images paths
    let arr = new Array();

    if ($rows == 4) {
        arr[0] = directory + "4b.png";
        arr[1] = directory + "4a.png";
        arr[2] = directory + "4w.png";
        arr[3] = directory + "4p.png";
    }
    else if ($rows == 5) {
        arr[0] = directory + "5b.png";
        arr[1] = directory + "5a.png";
        arr[2] = directory + "5w.png";
        arr[3] = directory + "5p.png";
    }
    else if ($rows == 8) {
        arr[0] = directory + "8b.png";
        arr[1] = directory + "8a.png";
        arr[2] = directory + "8w.png";
        arr[3] = directory + "8p.png";
    }

    document.getElementById('mario-audio').play();

    var isStopped = false;
    const $spinBtn = $('#spin-button');
    const $spinForm = $('#spin-form');

    $spinBtn.on('click', spin);

    $(document).keydown(function (e) {
        if (e.keyCode == 32 && e.target == document.body) {
                e.preventDefault();
            if ($spinBtn.prop('disabled') === false) {
                $spinBtn.click();
            }
        }
    })

    function stop() {
        isStopped = true;
        $("#status-msg").empty();

        $spinBtn.off('click', stop);
        $spinBtn.on('click', spin);
        $spinBtn.text('Spin');
        $spinBtn.removeClass('btn-danger');
    }

    function spin() {
        let $userBalanceNum = parseFloat($('#user-balance').val());
        let $stakeAmountNum = parseFloat($('#stake-amount').val());

        if ($userBalanceNum < $stakeAmountNum || $stakeAmountNum < 1) {
            return;
        }

        $spinBtn.off('click', spin);
        $spinBtn.on('click', stop);
        $spinBtn.text('Stop');
        $spinBtn.addClass('btn-danger');

        const $spinForm = $("#spin-form");
        const dataToSend = $spinForm.serialize();

        document.getElementById('spin-audio').play();
        $spinBtn.prop('disabled', true);

        let partialViewResult;
        let xhr;

        $.ajax({
            url: $spinForm.attr('action'),
            type: "Post",
            data: dataToSend,
            success: function (serverData, textStatus, xhrServer) {
                partialViewResult = serverData;
                xhr = xhrServer;
                $spinBtn.prop('disabled', false);
            }
        });

        $("tr").css('background', '#1c1c1c');
        $('#result-message').text('Good luck!');
        $('#result-message').css('color', 'white');

        shuffle(function () {
            if (xhr.status === 299) {
                $("#status-msg").empty();
                $("#status-msg").html(partialViewResult);
            }
            else {
                $("#partial").empty();
                $("#partial").html(partialViewResult);

                $coef = $('#res-coef').val();
                let $coefDouble = parseFloat($coef);

                if ($coefDouble > 0.0) {
                    document.getElementById('win-audio').play();
                    $('#result-message').css('color', 'yellow');
                }

                if ($coefDouble >= 2.0) {
                    gimmick('body');
                    setTimeout(function () {
                        gimmick('body');

                    }, 1500);
                }

                $winningRows = $('#winning-rows').val();
                debugger;
                for (var i = 0; i < $winningRows.length; i++) {
                    $("#row-" + $winningRows[i]).css('background', '#0bd124');
                }
            }

            let container = $("#component-balance");
            $.get(MyAppUrlSettings.UserBalanceComponent, function (data) { container.html(data); });

            var coinsExist = document.getElementById('gimmick')
            if (coinsExist) {
                coinsExist.parentNode.removeChild(coinsExist);
                return false;
            }
        });
    }

    function shuffle(requestFunction) {
        let Random = setInterval(function () {
            for (var i = 0; i < $rows; i++) {
                for (var j = 0; j < $cols; j++) {
                    let idName = "" + i + j;
                    let rnd = Math.floor(Math.random() * 4);
                    $("#p" + idName).attr("src", arr[rnd]);
                }
            }

            if (isStopped) {
                document.getElementById('spin-audio').pause();
                document.getElementById('spin-audio').currentTime = 0;

                requestFunction();
                isStopped = false;
                clearInterval(Random);
            }
        }, 80);
    }

    let betAmount = document.querySelector('#stake-amount');
    betAmount.addEventListener("keyup", function () {
        betAmount.value = betAmount.value.match(/^\d+\.?\d{0,2}/);
    });

    function gimmick(el) {
        var exists = document.getElementById('gimmick')
        if (exists) {
            debugger;
            exists.parentNode.removeChild(exists);
            return false;
        }

        var element = document.querySelector(el);
        var canvas = document.createElement('canvas'),
            ctx = canvas.getContext('2d'),
            focused = false;

        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;
        canvas.id = 'gimmick'

        var coin = new Image();
        coin.src = 'http://i.imgur.com/5ZW2MT3.png'

        coin.onload = function () {
            element.appendChild(canvas)
            focused = true;
            drawloop();
        }
        var coins = []

        function drawloop() {
            if (focused) {
                requestAnimationFrame(drawloop);
            }

            ctx.clearRect(0, 0, canvas.width, canvas.height)

            if (Math.random() < .3) {
                coins.push({
                    x: Math.random() * canvas.width | 0,
                    y: -50,
                    dy: 3,
                    s: 0.5 + Math.random(),
                    state: Math.random() * 10 | 0
                })
            }
            var i = coins.length
            while (i--) {
                var x = coins[i].x
                var y = coins[i].y
                var s = coins[i].s
                var state = coins[i].state
                coins[i].state = (state > 9) ? 0 : state + 0.1
                coins[i].dy += 0.3
                coins[i].y += coins[i].dy

                ctx.drawImage(coin, 44 * Math.floor(state), 0, 44, 40, x, y, 44 * s, 40 * s)

                if (y > canvas.height) {
                    coins.splice(i, 1);
                }
            }
        }
    }
});

