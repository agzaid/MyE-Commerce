
$(document).ready(function () {
    debugger;

    var AGelem = $("#messageCenter");
    var AGelemID = AGelem[0].id;
    var AGelemMessage = $("#" + AGelemID).attr("data-AG-message");



    myToastr(AGelemMessage);

    
});

//putting function outside document ready so it could be accessible globally
function myToastr(message) {
    switch (message) {
        case "Success":
            toastr.success('Login Successful');
            break;
        case "Item Added":
            toastr.success('Item Added Successful');
            break;
        case "Edit":
            toastr.success('Edited Successfully');
            break;
        case "Error":
            toastr.error('Failed');
            break;
        case "Logout":
            toastr.error('You are Logged out');
            break;
        case "Delete":
            toastr.warning('Delete invalid');
            break;

        default:
            toastr.info('Sign UP for more User Experience');
    }
}
