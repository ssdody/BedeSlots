$(document).ready(function () {
    $("#table-users").DataTable({
        "responsive": true,
        "processing": false, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once,
        "scrollX": true,

        "ajax": {
            "url": "/Admin/Users/LoadData",
            "type": "POST",
            "headers": {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            "datatype": "json"
        },
        "initComplete": function () {
            var input = $('.dataTables_filter input').unbind();
            var self = this.api();
            $('.dataTables_filter input').bind('keyup', function (e) {
                if (e.keyCode == 13) {
                    self.search(input.val()).draw();
                }
            });
            $searchButton = $('<button>')
                .text('Search')
                .on('click keyup', (function (e) {
                    if (e.keyCode == 13) {
                        self.search(input.val()).draw();
                    }
                    self.search(input.val()).draw();
                })),
                $clearButton = $('<button>')
                    .text('Clear')
                    .click(function () {
                        input.val('');
                        $searchButton.click();
                    })
            $('.dataTables_filter').append($searchButton, $clearButton);
        },
        "columnDefs":
            [{
                "targets": [6],
                "orderable": false,
                "searchable": false,
                "targets": [7],
                "orderable": false,
                "searchable": false,
                "targets": [8],
                "orderable": false,
                "searchable": false,
            },
            { className: 'text-center', targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] },
            ],
        "columns": [
            { "data": "username", "name": "Username", "autoWidth": true },
            { "data": "firstname", "name": "Firstname", "autoWidth": true },
            { "data": "lastname", "name": "Lastname", "autoWidth": true },
            { "data": "email", "name": "Email", "autoWidth": true },
            { "data": "balance", "name": "Balance", "autoWidth": true },
            { "data": "currency", "name": "Currency", "autoWidth": true },
            { "data": "role", "name": "Role", "autoWidth": true },
            {
                "data": "userid",
                "autowidth": true,
                "render": function (userid, type, full, meta) {
                    return '<a href="#" class="btn btn-success" onclick="EditRole(&quot;' + userid + '&quot;)"> Edit </a>'
                }
            },
            {
                "data": "userid",
                "autowidth": true,
                "render": function (userid, type, full, meta) {
                    return '<a href="#" class="btn btn-danger" onclick="Delete(&quot;' + userid + '&quot;)"> Delete </a>'
                }
            },
        ]
    });

    $("#edit").click(function () {
        $.ajax({
            url: "/Admin/Users",
            type: "Get",
            data: userid,
            success: function () {
                $("#myModalBodyDiv1").load(url, function () {
                    $("#myModal1").modal("show");

                })
            },
            error: function (arg, data, value) {
                console.log(arg + data + value);
            }
        })
    });
});

let EditRole = function (userid) {
    let url = "/Admin/Users/EditRole?userid=" + userid;

    $("#myModalBodyDiv1").load(url, function () {
        $("#myModal1").modal("show");
    })
};

let Delete = function (userid) {
    let url = "/Admin/Users/Delete?userid=" + userid;

    $("#myModalBodyDiv1").load(url, function () {
        $("#myModal1").modal("show");
    })
};