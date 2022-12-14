var dataTable;

$(document).ready(function () {
    loadDataTable();
    
    
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Category/GetAll"
        },
        "columns": [
            { "data": "id", "width": "5%"},
            { "data": "name", "width": "10%"},

            
          
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center" role="group">
                        <a href="/Admin/Category/Upsert?Id=${data}"
                        class="btn btn-warning"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick=Delete('/Admin/Category/Delete/${data}')
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>

                       
                       
					</div>

                        `
                },
                "width": "10%"
            }
        ],
     
     
        
        

    });
 
    
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}


