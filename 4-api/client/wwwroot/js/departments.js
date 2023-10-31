const setAddDepartmentModal = () => {
    $('#modalLabel').text("Add new department") // ganti modal title
    $('#addDepartment').addClass("d-block") // menampilkan form untuk menambahkan data
    $('#editDepartment').removeClass("d-block") // menyembunyikan form edit
    $('#addDepartmentName').val('')
    return false;
}

const addDepartment = (e) => {
    e.preventDefault()
    const form = new FormData(e.target);
    const departmentName = form.get("addDepartmentName");

    if(departmentName == "") {
        Swal.fire({
            icon: 'error',
            title: 'Kolom nama departemen tidak boleh kosong.',
            showConfirmButton: false,
            timer: 2000
          })
    } else {

        var data = {
            name: departmentName
        }
    
        $.ajax({
            type: "POST",
            url: "http://localhost:5058/api/Departments",
            data: JSON.stringify(data),
            contentType: "application/json",
            dataType: "json",
            success:  function(result) {
                console.log(result);
                $('#departmentModal').modal('hide')
                $("#example1").DataTable().ajax.reload()
                // alert(result.message);
                Swal.fire({
                    icon: 'success',
                    title: result.message,
                    showConfirmButton: false,
                    timer: 1500
                  })
                $('#addDepartmentName').val("")
            },
            error: function(e) {
                console.log(e);
                $('#departmentModal').modal('hide')
                // alert("Data gagal dimasukkan");
                Swal.fire({
                    icon: 'error',
                    title: e.responseJSON.message,
                    showConfirmButton: false,
                    timer: 1500
                  })
            }
          });
    }

};$('#editDepartment').addClass("d-block")

const setEditDepartmentModal = (id, name) => {
    $('#modalLabel').text("Edit department") // ganti modal title
     // menampilkan form untuk mengedit data
    $('#addDepartment').removeClass("d-block") // menyembunyikan form tambah
    
    $('#editDepartmentId').val(id)
    $('#editDepartmentName').val(name) // menampilkan data sebelumnya
    return false;
}

const editDepartment = (e) => {
    e.preventDefault()
    const form = new FormData(e.target);
    const departmentName = form.get("editDepartmentName");
    const departmentId = form.get("editDepartmentId");
    console.log(departmentId, departmentName)

    var data = {
        name: departmentName
    }

    $.ajax({
        type: "PUT",
        url: `http://localhost:5058/api/Departments/${departmentId}`,
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: "json",
        success:  function(result) {
            console.log(result);
            $('#departmentModal').modal('hide')
            $("#example1").DataTable().ajax.reload()
            // alert(result.message);
            Swal.fire({
                icon: 'success',
                title: result.message,
                showConfirmButton: false,
                timer: 1500
              })
        },
        error: function(e) {
            console.log(e);
            $('#departmentModal').modal('hide')
            // alert("Data gagal diperbarui");
            Swal.fire({
                icon: 'error',
                title: e.responseJSON.message,
                showConfirmButton: false,
                timer: 1500
              })
        }
      });
}

const deleteDepartment = (departmentId) => {
    Swal.fire({
        title: 'Ingin menghapus data departemen?',
        text: "Data yang telah dihapus tidak dapat dikembalikan!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Hapus'
      }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: `http://localhost:5058/api/Departments/${departmentId}`,
                contentType: "application/json",
                dataType: "json",
                success:  function(result) {
                    console.log(result);
                    $('#departmentModal').modal('hide')
                    $("#example1").DataTable().ajax.reload()
                    // alert(result.message);
                    Swal.fire({
                        icon: 'success',
                        title: result.message,
                        showConfirmButton: false,
                        timer: 1500
                      })
                },
                error: function(e) {
                    console.log(e);
                    $('#departmentModal').modal('hide')
                    // alert("Data gagal dihapus");
                    Swal.fire({
                        icon: 'error',
                        title: e.responseJSON.message,
                        showConfirmButton: false,
                        timer: 1500
                      })
                }
              });
        }
      })
}

// tooltip tombol
$('body').tooltip({
    selector: '[data-tooltip="tooltip"]',
    container: 'body'
});

// pengaturan tabel
$(document).ready(function () {
    var table = $("#example1").DataTable({
    "responsive": true,
    "lengthChange": false,
    "autoWidth": false,
    "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],
    "processing": true,
    "serverSide": true,
    "ajax": 
    {
        url: "http://localhost:5058/api/Departments/paging",
        type: "GET",
        "datatype": "json",
        "dataSrc": "data"
    },
    "columns": [
        { "data": null },
        { "data": "dept_ID", "name": "Dept_ID" },
        { "data": "name", "name": "Name" },
        { "data": null }
    ],
    columnDefs: [
        {
            targets: 0,
            searchable: false,
            // orderable: false,
        },
        {
            orderable: false,
            targets: [-1],
            render: function (data) {
                return `<div>
                    <button type="button" class="btn btn-warning" data-toggle="modal" data-target="#departmentModal" data-tooltip="tooltip" data-placement="left" title="Edit department" onclick="setEditDepartmentModal('${data.dept_ID}', '${data.name}')">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button type="button" class="btn btn-danger" data-tooltip="tooltip" data-placement="right" title="Delete department" onclick="deleteDepartment('${data.dept_ID}')">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>`;
            }
        },
        ],
    order: [[1, 'asc']],    
    })

    table.on( 'draw.dt', function () {
        var PageInfo = $('#example1').DataTable().page.info();
             table.column(0, { page: 'current' }).nodes().each( function (cell, i) {
                cell.innerHTML = i + 1 + PageInfo.start;
            } );
        } );
    
    table.buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');
});