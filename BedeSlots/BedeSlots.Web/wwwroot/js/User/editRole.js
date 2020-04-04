$(document).ready(function () {

    $("#myModal1").on('click', '#btnSubmit', function (event) {
        event.preventDefault();
        var dataToSend = $("#editRole").serialize();

        $.ajax({
            type: "POST",
            url: "/Admin/Users/EditRole",
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
    });

    $("#myModal1").on('click', '#btnReject', function (event) {
        event.preventDefault();
        $("#myModal1").modal("hide");
    });
})