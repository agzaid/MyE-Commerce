
var idIncrementer = 1;
var destroyTable = true;
var itemsObj = [];
var i = 0;
var totalPrice = 0;
var totalQuantity = 0;
var tableIndex = 0;

$(document).ready(function () {
    var tableCleared = true;
    $("#scanned-barcode").focus();
    timeCount();
    _DataTable.Draw();

    setTimeout(function () {
        $("#scanned-barcode").on('change', function (event) {
            event.preventDefault();
            _Barcode.Scan();
            _AjaxCall.Call($("#scanned-barcode").val());
            //document.querySelector('#total_price').innerText = $("#scanned-barcode").val();
        });
    }, 1000);

    $("#Tendered").on('keyup', function (event) {
        _ChangeTender.Calculate();
    });
});


//swall call was here

function timeCount() {
    var today = new Date();
    var day = today.getDate();
    var month = today.getMonth() + 1;
    var year = today.getFullYear();

    var hour = today.getHours();
    if (hour < 10) hour = "0" + hour;

    var minute = today.getMinutes();
    if (minute < 10) minute = "0" + minute;

    var second = today.getSeconds();
    if (second < 10) second = "0" + second;

    document.getElementById("clock").innerHTML =
        day + "/" + month + "/" + year + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + hour + ":" + minute + ":" + second;

    setTimeout("timeCount()", 1000);
};

function CreateTRow(data) {
    _SalesFunctions.AddingItems(data);
    _SalesFunctions.ComputeTotalPrice();
    _SalesFunctions.CountTotalQuantity();

}

var _SalesFunctions = {

    AddingItems: function (data) {
        if (itemsObj.length == 0) {
            itemsObj.push(data);
            _SalesFunctions.AppendRow(data);
        } else {
            //need another condition here when item not duplicated and not available 
            //itemsObj.forEach(function (arrayItem) {
            //for (var i = 0; i < itemsObj.length; i++) {
            var existsObj = itemsObj.filter(function (el, index) {
                if (el.name == data.name && el.price == data.price) {
                    debugger;
                    tableIndex = index;
                    return true;
                }
                else {
                    debugger;
                    return false;
                }
            });

            if (existsObj.length != 0 && itemsObj[tableIndex].name == data.name && itemsObj[tableIndex].price == data.price) {
                debugger;
                itemsObj[tableIndex].quantity += 1;
                $(`[name='Quantity[` + tableIndex + `]']`).val(itemsObj[tableIndex].quantity);
                //break;
            } else if (existsObj.length == 0) {
                debugger;
                itemsObj.push(data);
                _SalesFunctions.AppendRow(data);
                //break;
            } else {
                //continue;
            }
            //}
            //});
        }

    },
    CountTotalQuantity: function () {
        totalQuantity = 0;
        itemsObj.forEach(function (arrayItem) {
            totalQuantity += (arrayItem.quantity);
            document.querySelector('#kt_file_manager_items_counter').innerText = totalQuantity;
        });
    },
    ComputeTotalPrice: function () {
        totalPrice = 0;
        itemsObj.forEach(function (arrayItem) {
            totalPrice += (arrayItem.price * arrayItem.quantity);
            document.querySelector('#total_price').innerText = totalPrice;
        });
        //for (var i = 0; i < itemsObj.length; i++) {
        //    totalPrice += itemsObj[i].price;
        //    document.querySelector('#total_price').innerText = totalPrice;

        //}
    },
    AppendRow: function (data) {
        var markup = `<tr role="row animate__animated animate__backInLeft" class="odd">
               <td>`+ idIncrementer + `</td>
               <td><input class="form-control form-control-sm text-start" type='text' value='`+ data.name + `' name='Name[` + i + `]' disabled></td>
               <td><input class="form-control form-control-sm text-start" type='number' id='subPrice' name='Price[`+ i + `]' value='` + data.price + `'  disabled></td>
               <td><input class="form-control form-control-sm text-start" type='number' name='Quantity[`+ i + `]' min='1' value="` + data.quantity + `" disabled style='width:100px;'></ td >
               <td><button onclick="return _SalesFunctions.RemoveEle(this)"; class="btn btn-icon btn-active-danger btn-outline btn-outline-default btn-icon-primary btn-active-icon-gray-700" ><i class="fa fa-trash" aria-hidden="true"></i></button></td>
               </tr>`;
        $("table tbody").append(markup);
        idIncrementer++;
        i++;
    },
    RemoveEle: function (el) {
        if (confirm("Are you sure you want to delete this item...?")) {
            var s = $(el).closest('tr');
            debugger;
            var name = s[0].cells[1].firstElementChild.value;
            var price = s[0].cells[2].firstElementChild.value;
            var quantity = s[0].cells[3].firstElementChild.value;
            var elName = s[0].cells[3].firstElementChild.name;
            //delete item from itemObj
            itemsObj.forEach(function (arrayItem) {
                debugger;
                if (arrayItem.name == name && arrayItem.price == price && arrayItem.quantity >= 2) {
                    arrayItem.quantity -= 1;
                    $(`[name='` + elName + `']`).val(arrayItem.quantity);
                    _SalesFunctions.CountTotalQuantity();
                    _SalesFunctions.ComputeTotalPrice();
                } else if (arrayItem.name == name && arrayItem.price == price && arrayItem.quantity >= 1) {
                    var h = itemsObj;
                    arrayItem.quantity -= 1;
                    _SalesFunctions.CountTotalQuantity();
                    _SalesFunctions.ComputeTotalPrice();
                    itemsObj = itemsObj.filter(function (el) { return el.quantity != 0 });
                    s.attr("hidden", "hidden");
                    //$(el).closest('tr').remove();
                    //idIncrementer--;
                    i--;
                }
            });

            return false;
        } else
            return false;

        idIncrementer--;
    },

};

var _AjaxCall = {
    Call: function (barcode_id) {
        $.ajax({
            url: 'https://localhost:7099/en/Admin/Sales/GetItem',
            type: 'GET',
            data: {
                "id": barcode_id
            },
            contentType: "application/json; charset = utf-8",
            datatype: "json",
            success: function (response) {
                debugger;
                if (destroyTable) {
                    $('#SalesInvoice').DataTable().clear().destroy();
                    destroyTable = false;
                }
                CreateTRow(response.data);
                $("#scanned-barcode").val("");
                $("#scanned-barcode").focus();
                //alert('Data: ' + JSON.stringify(response));
            },
            error: function (request, error) {
                debugger;
                $("#scanned-barcode").val("");
                $("#scanned-barcode").focus();
                alert("Request: " + JSON.stringify(request.responseText));
            }
        });
    }
};


var _ChangeTender = {
    Calculate: function () {
        debugger;
        var tender = $("#Tendered").val();
        var change = $("#Change").val();
        change = tender - totalPrice;
        $("#Change").val(change);
    }
};