$(document).ready(function () {
    $("#myModal1").on('click', '#deleteBtnSubmit', function (event) {
        event.preventDefault();
        var dataToSend = $("#deleteUser").serialize();
        console.log(dataToSend);

        $.ajax({
            type: "POST",
            url: "/Admin/Users/Delete",
            data: dataToSend,
            success: function (data) {
                $("#myModal1").modal("hide");
                if (data) {
                    oTable = $('#table-users').DataTable();
                    oTable.draw();
                }
                else {
                    alert("Something went wrong!");
                }
            }
        })
    })

    $("#myModal1").on('click', '#deleteBtnReject', function (event) {
        event.preventDefault();
        $("#myModal1").modal("hide");

        if (data) {
            oTable = $('#table-users').DataTable();
            oTable.draw();
        }
        else {
            alert("Something Went Wrong!");
        }
    });
})