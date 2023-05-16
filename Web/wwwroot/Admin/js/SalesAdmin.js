
var idIncrementer = 1;
var destroyTable = true;
var itemsObj = [];
var i = 0;
var totalPrice = 0;
var totalQuantity = 0;

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
    idIncrementer++;
    i++;
}

var _SalesFunctions = {

    AddingItems: function (data) {
        if (itemsObj.length == 0) {
            itemsObj.push(data);
            _SalesFunctions.AppendRow(data);
        } else {
            //need another condition here when item not duplicated and not available 
            debugger;
            //itemsObj.forEach(function (arrayItem) {
            for (var i = 0; i < itemsObj.length; i++) {

                //var existsObj = itemsObj.filter(function (el) { return el.name == itemsObj[i].name });
                var existsObj = itemsObj.filter(function (el) {
                    if (el.name == itemsObj[i].name && el.price == itemsObj[i].price) {
                        console.log("false");
                        return false;
                    }
                    else {
                        console.log("true");
                        return true;
                    }
                });

                //if (existsObj.length != 0 && itemsObj[i].name == data.name && itemsObj[i].price != data.price) {
                //    itemsObj[i].quantity += 1;
                //    $(`[name='Quantity[` + (itemsObj[i].id - 1) + `]']`).val(itemsObj[i].quantity);
                //    break;
                //} else if (existsObj.length == 0 && (itemsObj[i].name == data.name && itemsObj[i].price != data.price)) {
                //    itemsObj.push(data);
                //    _SalesFunctions.AppendRow(data);
                //    break;
                //} else {
                //    itemsObj.push(data);
                //    _SalesFunctions.AppendRow(data);
                //    break;
                //}

                if (itemsObj[i].name == data.name && itemsObj[i].price == data.price ) {
                    debugger;
                    itemsObj[i].quantity += 1;
                    $(`[name='Quantity[` + (itemsObj[i].id - 1) + `]']`).val(itemsObj[i].quantity);
                    break;
                    //var s = $(`[name='Qunatity[0]']`);
                    //var o = $(`[name='Qunatity[0]']`).val(5);

                } else if (itemsObj[i].name == data.name && itemsObj[i].price != data.price) {
                    debugger;
                    itemsObj.push(data);
                    _SalesFunctions.AppendRow(data);
                    break;
                } else if (itemsObj[i].name != data.name) {
                    debugger;
                    itemsObj.push(data);
                    _SalesFunctions.AppendRow(data);
                    break;
                } else {
                    continue;
                }
            }
            //});
        }

    },
    CountTotalQuantity: function () {
        totalQuantity = 0;
        itemsObj.forEach(function (arrayItem) {
            totalQuantity += (arrayItem.quantity);
            console.log(totalQuantity);
            document.querySelector('#kt_file_manager_items_counter').innerText = totalQuantity;
        });
    },
    ComputeTotalPrice: function () {
        totalPrice = 0;
        itemsObj.forEach(function (arrayItem) {
            totalPrice += (arrayItem.price * arrayItem.quantity);
            console.log(totalPrice);
            document.querySelector('#total_price').innerText = totalPrice;
        });
        //for (var i = 0; i < itemsObj.length; i++) {
        //    totalPrice += itemsObj[i].price;
        //    console.log(totalPrice);
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
                if (arrayItem.name == name && arrayItem.quantity >= 2) {
                    arrayItem.quantity -= 1;
                    $(`[name='` + elName + `']`).val(arrayItem.quantity);
                    _SalesFunctions.CountTotalQuantity();
                    _SalesFunctions.ComputeTotalPrice();
                } else {
                    var h = itemsObj;
                    arrayItem.quantity -= 1;
                    _SalesFunctions.CountTotalQuantity();
                    _SalesFunctions.ComputeTotalPrice();
                    itemsObj = itemsObj.filter(function (el) { return el.quantity != 0 });
                    $(el).closest('tr').remove();
                }
            });

            --i;
            --idIncrementer;
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
}