var dataTable;

$(document).ready(function () {
    loadDataTable();
    
    
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Order/GetAll"
        },
        "columns": [
            { "data": "id", "width": "5%"},
            { "data": "createdDate", "width": "10%"},
            { "data": "customer.name", "width": "10%"},
            { "data": "transaction.name", "width": "5%"},
            { "data": "cashier", "width": "5%" },
            { "data": "total", "width": "5%" },
            { "data": "discount", "width": "5%" },
            { "data": "netamount", "width": "5%" },
            { "data": "tendor", "width": "5%" },
            
          
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Order/Edit?orderheaderId=${data}"
                        class="btn btn-warning mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>

                        <a href="/Admin/Order/EditView?orderheaderId=${data}"
                        class="btn btn-info mx-2"> <i class="bi bi-pencil-square"></i> View</a>
                       
					</div>

                        `
                },
                "width": "10%"
            }
        ],
     
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api();

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };

            // Total over all pages
            total = api
                .column(7)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal = api
                .column(7, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            $(api.column(7).footer()).html('Nrs ' + pageTotal + ' ( Nrs ' + total + ' All total)');
        },
        

    });
 
    
}



