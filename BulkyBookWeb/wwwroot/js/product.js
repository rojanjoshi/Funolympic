﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "name", "width": "10%" },
            { "data": "price", "width": "10%" },
            { "data": "imageUrl", "width": "10%" },
            { "data": "category.name", "width": "10%" },
         
          
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-45 btn-group text-center" role="group">
                        <a href="/Admin/Product/Upsert?id=${data}"
                        class="btn btn-warning mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                       
					</div>
                        `
                },
                "width": "5%"
            }
        ]
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