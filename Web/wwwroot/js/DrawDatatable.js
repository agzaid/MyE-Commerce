
$(document).ready(function () {
    debugger;

    var AGelem = $(".AG-data-table");
    var AGelemID = AGelem[0].id;
    var url = $("#" + AGelemID).attr("data-AG-load-url");
    var urlEdit = $("#" + AGelemID).attr("data-AG-edit");
    var urlDelete = $("#" + AGelemID).attr("data-AG-delete");
    var AGelemColumns = $("#" + AGelemID).attr("data-AG-columns");
    var AGelemMessage = $("#" + AGelemID).attr("data-AG-message");
    var c = JSON.parse(AGelemColumns);
    var columnsRendered = [{ "data": "id", "name": "ID", "autowidth": true }];

    c.forEach(CreateColumn);

    columnsRendered.push({
        "render": function (data, type, row) { return `<a href="` + urlEdit + `/` + row.id + `" class="btn btn-warning">Edit</a> <a class="btn btn-danger" href="` + urlDelete + `/` + row.id + `">Delete</a>` },
        "orderable": false
    });

    myToastr(AGelemMessage);

    $("#" + AGelemID).dataTable({
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": url,
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        },
        {
            targets: 1,
            data: name,
            orderable: true,
            className: 'text-start',
            render: function (data, type, row) {
                if (row.thumbnailImage) {
                    return `<div class="d-flex align-items-center"><a class="symbol symbol-50px"><span class="symbol-label" style="background-image:url(` + row.thumbnailImage + `);"></span></a><div class="ms-5"><a href="` + urlEdit + `/` + row.id + `"class="text-gray-800 text-hover-primary fs-5 fw-bolder" data-kt-ecommerce-productfilter="product_name">` + row.name + `</a></div></div > `
                } else {
                    return '<a href="' + '/' + row.id + '" class="text-gray-800 text-hover-primary fs-5 fw-bolder mb-1" data-kaj-filter="item_name">' + row.name + '</a><input type="hidden" data-kaj-filter="item_id" value="' + row.id + '">';;
                }
            },
        },
        {
            targets: -2,
            data: null,
            orderable: false,
            className: 'text-end',
            render: function (data, type, row) {
                switch (data) {
                    case 1:
                        return '<div class="badge badge-light-success">' + 'Published' + '</div>';
                    case 2:
                        return '<div class="badge badge-light-primary">' + 'InActive' + '</div>';
                    case 3:
                        return '<div class="badge badge-light-danger">' + 'Deleted' + '</div>';
                    default:
                        return '';
                }

            }
        }
        ],
        "columns": columnsRendered
    });

    function CreateColumn(c) {

        var o = { "data": c[0].toLowerCase() + c.slice(1), "name": c, "autowidth": true };
        columnsRendered.push(o);
    }
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

});