// pengaturan tabel
$(document).ready(function () {
    let masterData = "http://localhost:5058/api/Employees"

    let activeData = "http://localhost:5058/api/Employees/active"
    
    let resignData = "http://localhost:5058/api/Employees/inactive"

    var table = $("#employeeTable").DataTable({
    "responsive": true,
    "lengthChange": false,
    "autoWidth": false,
    "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],
    "ajax": {
        url: masterData,
        type: "GET",
        "datatype": "json",
        "dataSrc": "data"
    },
    "columns": [
        { "data": null },
        { "data": "nik" },
        { "data": null },
        { "data": "email" },
        { "data": "phoneNumber" },
        { "data": "address" },
        { "data": null },
        { "data": "department.name" },
        { "data": null }
    ],
    columnDefs: [
        {
            targets: 0,
            searchable: false,
            orderable: false,
        },
        {
            targets: 2,
            render: function (data) {
                return `${data.firstName} ${data.lastName}`;
            }
        },
        {
            targets: 6,
            render: function (data) {
                let activeBadge = `<span class="badge badge-pill badge-primary">ACTIVE</span>`
                let resignBadge = `<span class="badge badge-pill badge-danger">RESIGN</span>`

                return (data.isActive == true ? activeBadge : resignBadge);
            }
        },
        {
            orderable: false,
            targets: [-1],
            render: function (data) {
                let datastring = encodeURIComponent(JSON.stringify(data))
                return `<div class="d-flex justify-content-around col">
                    <button type="button" class="btn btn-warning" data-toggle="modal" data-target="#employeeModal" data-tooltip="tooltip" data-placement="left" title="Edit employee" onclick="setEditEmployeeModal('${datastring}')">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button type="button" class="btn btn-danger" data-tooltip="tooltip" data-placement="right" title="Delete employee" onclick="deleteEmployee('${data.nik}')">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>`;
            }
        },
        ],
    order: [[1, 'asc']],   
    })

    table.on( 'draw.dt', function () {
        var PageInfo = $('#employeeTable').DataTable().page.info();
             table.column(0, { page: 'current' }).nodes().each( function (cell, i) {
                cell.innerHTML = i + 1 + PageInfo.start;
            } );
        } );
    
    table.buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');

    $("#employeeData").on('change', () => {
        // table.destroy();
        let employeeData = $('#employeeData').val()

        table.ajax.url(employeeData == 'master' || employeeData == '' ? masterData : (employeeData == 'active' ? activeData : resignData)).load();
        
        table.ajax.reload()
    })

    $.ajax({
        type: "GET",
        url: "http://localhost:5058/api/Departments",
        dataType: "json",
        success: function (result) {
            $.each(result.data, function(i, obj)
            {
                var select = `<option value="${obj.dept_ID}">${obj.name}</option>`
                $(select).appendTo('#DepartmentId'); 
            })  
        }
    })

});

// mendapatkan input dari form
var NIK = $('#NIK')
var firstName = $('#FirstName')
var lastName = $('#LastName')
var phoneNumber = $('#PhoneNumber')
var address = $('#Address')
var isActive = $('#IsActive')
var departmentId = $('#DepartmentId')

const setAddEmployeeModal = (e) => {
    $('#modalLabel').text("Add new employee")
    $('#addButton').addClass("d-block")
    $('#editButton').addClass("d-none")
    $('#editButton').removeClass("d-block")

    firstName.removeClass("is-invalid").val('')
    lastName.removeClass("is-invalid").val('')
    phoneNumber.removeClass("is-invalid").val('')
    address.removeClass("is-invalid").val('')
    isActive.removeClass("is-invalid").val('')
    departmentId.removeClass("is-invalid").val('')
    $('#activeEditOption').remove()
    $('#departmentEditOption').remove()
}

// Event listener untuk menghapus kelas is-invalid saat nilai input berubah
firstName.on('input', () => {
    firstName.removeClass("is-invalid")
})

lastName.on('input', () => {
    lastName.removeClass("is-invalid")
})

phoneNumber.on('input', () => {
    phoneNumber.removeClass("is-invalid")
})

address.on('input', () => {
    address.removeClass("is-invalid")
})

isActive.on('change', () => {
    isActive.removeClass("is-invalid")
})

departmentId.on('change', () => {
    departmentId.removeClass("is-invalid")
})

const addEmployee = (e) => {
    e.preventDefault();

    // Memastikan kolom tidak kosong
    if (
        !firstName.val() ||
        !lastName.val() ||
        !phoneNumber.val() ||
        !address.val() ||
        isActive.val() === '' ||
        departmentId.val() === ''
    ) {
        // Menandai kolom yang tidak valid dengan is-invalid
        firstName.toggleClass('is-invalid', !firstName.val());
        lastName.toggleClass('is-invalid', !lastName.val());
        phoneNumber.toggleClass('is-invalid', !phoneNumber.val());
        address.toggleClass('is-invalid', !address.val());
        isActive.toggleClass('is-invalid', isActive.val() === '');
        departmentId.toggleClass('is-invalid', departmentId.val() === '');
    
        return false;
    }

    // Melakukan POST data employee
    var data = {
        firstName: firstName.val(),
        lastName: lastName.val(),
        phoneNumber: phoneNumber.val(),
        address: address.val(),
        isActive: isActive.val() == 'true' ? true : false,
        department_id: departmentId.val()
    };

    $.ajax({
        type: 'POST',
        url: 'http://localhost:5058/api/Employees',
        data: JSON.stringify(data),
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            $('#employeeModal').modal('hide');
            $('#employeeTable').DataTable().ajax.reload();
            Swal.fire({
                icon: 'success',
                title: result.message,
                showConfirmButton: false,
                timer: 1500
            });
        },
        error: function (e) {
            console.error(e);
            if(e.responseJSON.status == 400) {
                phoneNumber.toggleClass('is-invalid')
                $('#PhoneNumberFeedback').text(e.responseJSON.message)
            } else {
                $('#employeeModal').modal('hide');
                Swal.fire({
                    icon: 'error',
                    title: e.responseJSON.message,
                    showConfirmButton: false,
                    timer: 1500
                });
                
            }
        }
    });

    return false;
};

const setEditEmployeeModal = (data) => {
    $('#modalLabel').text("Edit employee data")
    $('#addButton').addClass("d-none")
    $('#addButton').removeClass("d-block")
    $('#editButton').addClass("d-block")
    $('#activeEditOption').remove()
    $('#departmentEditOption').remove()

    var object = JSON.parse(decodeURIComponent(data))
    NIK.val(object.nik)
    firstName.val(object.firstName)
    lastName.val(object.lastName)
    phoneNumber.val(object.phoneNumber)
    address.val(object.address)

    var selectIsActive = `<option id="activeEditOption" value="${object.isActive}" selected>${object.isActive ? 'ACTIVE' : 'RESIGN' }</option>`;
    $(selectIsActive).appendTo('#IsActive');

    var selectDepartment = `<option id="departmentEditOption" value="${object.department.dept_ID}" selected>${object.department.name}</option>`;
    $(selectDepartment).appendTo('#DepartmentId');
}

const editEmployee = (e) => {
    e.preventDefault();

    // Memastikan kolom tidak kosong
    if (
        !firstName.val() ||
        !lastName.val() ||
        !phoneNumber.val() ||
        !address.val() ||
        isActive.val() === '' ||
        departmentId.val() === ''
    ) {
        // Menandai kolom yang tidak valid dengan is-invalid
        firstName.toggleClass('is-invalid', !firstName.val());
        lastName.toggleClass('is-invalid', !lastName.val());
        phoneNumber.toggleClass('is-invalid', !phoneNumber.val());
        address.toggleClass('is-invalid', !address.val());
        isActive.toggleClass('is-invalid', isActive.val() === '');
        departmentId.toggleClass('is-invalid', departmentId.val() === '');
    
        return false;
    }

    // Melakukan POST data employee
    var data = {
        firstName: firstName.val(),
        lastName: lastName.val(),
        phoneNumber: phoneNumber.val(),
        address: address.val(),
        isActive: isActive.val() == 'true' ? true : false,
        department_id: departmentId.val()
    };

    $.ajax({
        type: 'PUT',
        url: `http://localhost:5058/api/Employees/${NIK.val()}`,
        data: JSON.stringify(data),
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            $('#employeeModal').modal('hide');
            $('#employeeTable').DataTable().ajax.reload();
            Swal.fire({
                icon: 'success',
                title: result.message,
                showConfirmButton: false,
                timer: 1500
            });
        },
        error: function (e) {
            console.error(e);
            if(e.responseJSON.status == 400) {
                phoneNumber.toggleClass('is-invalid')
                $('#PhoneNumberFeedback').text(e.responseJSON.message)
            } else {
                $('#employeeModal').modal('hide');
                Swal.fire({
                    icon: 'error',
                    title: e.responseJSON.message,
                    showConfirmButton: false,
                    timer: 1500
                });
                
            }
        }
    });

    return false;   
}

const deleteEmployee = (employeeNIK) => {
    Swal.fire({
        title: 'Ingin mengganti status employee menjadi RESIGN?',
        text: "🚮🚮🚮",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Hapus'
      }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: `http://localhost:5058/api/Employees/${employeeNIK}`,
                contentType: "application/json",
                dataType: "json",
                success:  function(result) {
                    $('#employeeModal').modal('hide')
                    $("#employeeTable").DataTable().ajax.reload()

                    Swal.fire({
                        icon: 'success',
                        title: result.message,
                        showConfirmButton: false,
                        timer: 1500
                      })
                },
                error: function(e) {;
                    $('#employeeModal').modal('hide')
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

$('body').tooltip({
    selector: '[data-tooltip="tooltip"]',
    container: 'body'
});