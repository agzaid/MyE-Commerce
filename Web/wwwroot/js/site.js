
$(document).ready(function () {
    debugger;
    myToastr("Create");

    function myToastr(message) {
        switch (message) {
            case "Create":
                toastr.success('Created Successfully');
                break;
            case "Edit":
                toastr.success('Edited Successfully');
                break;
            case "Error":
                toastr.error('Failed');
                break;
            case "DeleteTrue":
                toastr.error('Item Deleted');
                break;
            case "Delete":
                toastr.warning('Delete invalid');
                break;

            default:
                toastr.info('Empty');
        }
    }
})