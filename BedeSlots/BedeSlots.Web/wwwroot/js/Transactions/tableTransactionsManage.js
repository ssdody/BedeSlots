$(document).ready(function () {
    $("#table-transactions").DataTable({
        "processing": false, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once,
        "order": [[0, "desc"]],
        "scrollX": true,

        "ajax": {
            "url": "/Admin/Transactions/LoadData",
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
                "targets": [3],
                "orderable": false,
                "searchable": false
            }],
        "columnDefs": [{
            targets: 1,
            render: function (data, type, row) {
                var color = 'black';
                if (data === "Stake") {
                    color = 'red';
                }
                else if (data === "Win") {
                    color = 'green'
                }

                return '<span style="color:' + color + '">' + data + '</span>';
            }
        },
            { className: 'text-center', targets: [0, 1, 2, 3, 4] },
        ],
        "columns": [
            {
                "data": "date", "name": "Date", "autoWidth": true,
                "render": function (d) {
                    return moment(d).format("DD/MM/YYYY HH:mm:ss");
                }   
            },
            { "data": "type", "name": "Type", "autoWidth": true },
            { "data": "amount", "name": "Amount", "autoWidth": true },
            { "data": "description", "name": "Description", "autoWidth": true },
            { "data": "user", "name": "User", "autoWidth": true }
        ]
    });
});