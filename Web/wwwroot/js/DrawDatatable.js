
var _DataTable = {
    Draw: function () {

        var AGelem = $(".AG-data-table");
        var AGelemID;
        var AGelemMessage;
        var url;
        var urlEdit;
        var urlDelete;
        var AGelemColumns;
        var c;
        var columnsRendered = [{ "data": "id", "name": "ID", "autowidth": true }];

        if (AGelem[0] != undefined) {
            AGelemID = AGelem[0].id;
            AGelemMessage = $("#" + AGelemID).attr("data-AG-message");
            url = $("#" + AGelemID).attr("data-AG-load-url");
            urlEdit = $("#" + AGelemID).attr("data-AG-edit");
            urlDelete = $("#" + AGelemID).attr("data-AG-delete");
            AGelemColumns = $("#" + AGelemID).attr("data-AG-columns");
            c = JSON.parse(AGelemColumns);
            c.forEach(CreateColumn);
        }
        //var AGelemID = AGelem[0].id;
        //var url = $("#" + AGelemID).attr("data-AG-load-url");
        //var urlEdit = $("#" + AGelemID).attr("data-AG-edit");
        //var urlDelete = $("#" + AGelemID).attr("data-AG-delete");
        //var AGelemColumns = $("#" + AGelemID).attr("data-AG-columns");
        //var AGelemMessage = $("#" + AGelemID).attr("data-AG-message");
        //var c = JSON.parse(AGelemColumns);

        $('.AG-data-table').css('cursor', 'pointer');
        //if (c == undefined) 
        //    c="";

        columnsRendered.push({
            "render": function (data, type, row) { return `<a href="` + urlEdit + `/` + row.id + `" class="btn btn-icon btn-active-warning btn-outline btn-outline-default btn-icon-gray-700 btn-active-icon-primary" title="Edit"><i class="bi bi-pencil-square"></i></a> <a class="btn btn-icon btn-active-danger btn-outline btn-outline-default btn-icon-primary btn-active-icon-gray-700" title="Delete" href="` + urlDelete + `/` + row.id + `"><i class="fa fa-trash" aria-hidden="true"></i></a>` },
            "orderable": false
        });

        //for calling toastr message
        myToastr(AGelemMessage);

        //for highlighting columns on mouse enter but not working yet
        highlighColumn(AGelemID);

        $("#" + AGelemID).dataTable({
            "serverSide": true,
            "filter": true,
            "ajax": {
                "url": url,
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs": [
                {
                    "targets": 0,
                    "visible": false,
                    "searchable": false,
                },
                {
                    //for putting multiple rows in one column using class name of multipleRows and adding that to td element
                    "targets": "multipleRows",
                    data: name,
                    render: function (data, type, row) {
                        if (data.includes(",")) {
                            var multiRows = [];
                            var c = data.split(",");
                            for (var i = 0; i < c.length; i++) {
                                multiRows.push('<td class="text-gray-200">' + c[i] + '</td><br>');
                            }
                            return multiRows;
                        } else
                            return data;
                    },
                    //createdCell: function (td, cellData, rowData, row, col) {
                    //    if (cellData < 1) {
                    //        $(td).addClass("text-center")
                    //    }
                    //}
                },
                {
                    targets: 1,
                    data: name,
                    orderable: true,
                    className: 'text-start',
                    render: function (data, type, row) {
                        if (row.thumbnailImage) {
                            return `<div class="d-flex align-items-center"><a class="symbol symbol-50px"><span class="symbol-label" style="background-image:url(` + row.thumbnailImage + `);"></span></a><div class="ms-5"><a href="` + urlEdit + `/` + row.id + `"class="text-gray-800 text-hover-primary fs-5 fw-bolder" data-kt-ecommerce-productfilter="product_name">` + row.name + `</a></div></div > `
                        } else if (row.barCodeNumber) {
                            return '<a href="' + urlEdit + '/' + row.id + '" class="m-3 text-gray-800 text-hover-primary fs-5 fw-bolder mb-1" data-kaj-filter="item_name">' + row.barCodeNumber + '</a><input type="hidden" data-kaj-filter="item_id" value="' + row.id + '">';
                        } else {
                            return '<a href="' + urlEdit + '/' + row.id + '" class="m-3 text-gray-800 text-hover-primary fs-5 fw-bolder mb-1" data-kaj-filter="item_name">' + row.name + '</a><input type="hidden" data-kaj-filter="item_id" value="' + row.id + '">';;
                        }
                    },
                },
                {
                    targets: -2,
                    data: null,
                    orderable: true,
                    className: 'text-start',
                    render: function (data, type, row) {
                        switch (data) {
                            case 1:
                                return '<div class="badge badge-light-success">' + 'Published' + '</div>';
                            case 2:
                                return '<div class="badge badge-light-secondary">' + 'InActive' + '</div>';
                            case 3:
                                return '<div class="badge badge-light-danger">' + 'Deleted' + '</div>';
                            case 4:
                                return '<div class="badge badge-light-primary">' + 'available' + '</div>';
                            case 5:
                                return '<div class="badge badge-light-dark">' + 'unavailable' + '</div>';
                            case 6:
                                return '<div class="badge badge-light-info">' + 'sold' + '</div>';
                            case 7:
                                return '<div class="badge badge-light-danger">' + 'damaged' + '</div>';
                            case 8:
                                return '<div class="badge badge-light-warning">' + 'returned' + '</div>';
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

        function highlighColumn(id) {
            $(id + 'tbody').on('mouseenter', 'td', function () {
                var colIdx = table.cell(this).index().column;

                $(table.cells().nodes()).removeClass('highlight');
                $(table.column(colIdx).nodes()).addClass('highlight');
            });
        }
    }
}
