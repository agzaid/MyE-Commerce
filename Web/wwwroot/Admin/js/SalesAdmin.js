
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
    $("#swalCall").on('click', function (event) {
        event.preventDefault();
        //if(loadData==null){
        var records = $('#SkuSubItem').attr("data-AG-records");
        if ($('#SkuSubItem').attr("data-AG-records") == 0 && tableCleared === true) {
            $('#SkuSubItem').DataTable().clear().destroy();
            //tableCleared = false;
        }

        var markup = `<tr role="row animate__animated animate__backInLeft" class="odd">
                         <td>`+ idIncrementer + `</td>
                         <td><input class="form-control form-control-sm text-start" id='scanned-barcode' type='text' name='ListSkuSubItems[`+ i + `].BarCodeNumber' placeholder='1234567890'></td>
                         <td><input class="form-control form-control-sm text-start" type='number' id='subPrice' name='ListSkuSubItems[`+ i + `].Price' min='0' placeholder='0' style='width:100px;'></td>
                         <td><input class="form-control form-control-sm text-start" type='text' name='ListSkuSubItems[`+ i + `].ExpiryDate' placeholder='2024-02-23' pattern="\d{1,2}/\d{1,2}/\d{4}" value=""></ td >
                         <td class="text-end"><div class="badge badge-light-success">Published</div></td>
                         <td><button onclick="removeEle(this)"; class="btn btn-icon btn-active-danger btn-outline btn-outline-default btn-icon-primary btn-active-icon-gray-700" ><i class="fa fa-trash" aria-hidden="true"></i></button></td>
                         </tr>`;
        $("table tbody").append(markup);
        idIncrementer++;
        $(`[name='ListSkuSubItems[` + i + `].BarCodeNumber']`).focus();

        setTimeout(function () {
            $(`[name='ListSkuSubItems[0].BarCodeNumber']`).attr("required", "required")
            $(`[name='ListSkuSubItems[0].ExpiryDate']`).attr("required", "required")
            $(`[name='ListSkuSubItems[0].Price']`).attr("required", "required")
        }, 500);

        tableCleared = false;
        i++;
        //<td><input class="form-control form-control-sm text-start" type='text' name='name' placeholder='Name' style='width:200px;'></td>

        //$(rowNode)
        //    .css('color', 'red')
        //    .animate({ color: 'black' });
    });
    //if i want to use id to remove element must use setTimeout to delay the creation of function before drawing element
    //or i could use callback function after drawing element which is better approach
    setTimeout(function () {
        $("#remove").on('click', function () {
            $(this).closest('tr').remove();
        });
    }, 1000);
    setTimeout(function () {
        $("#scanned-barcode").on('change', function (event) {
            event.preventDefault();
            _Barcode.Scan();
            _AjaxCall.Call("1");
            document.querySelector('#total_price').innerText = $("#scanned-barcode").val();
        });
    }, 1000);

    $(`[name='Price]`).on('keyup', function () {
        if ($(`[name='Price`).val() != "") {
            //$('#subPrice').val($(`[name='Price`).val());
            $('[id=subPrice]').each(function () {
                $(this).val($(`[name='Price`).val());
            })
        } else {
            $('[id=subPrice]').each(function () { $(this).val(0) })
        }
    });
    $(`[name='Quantity]`).on('focus', function () {
        alert("AAAAAAAAAAAAaaa");
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
    AddingItems(data);
    //itemsObj.push(data);

    ComputeTotalPrice();
    CountTotalQuantity();
    idIncrementer++;
    i++;
}

setTimeout(function () {
    ComputeTotalPrice();

}, 1000);

function AddingItems(data) {
    if (itemsObj.length == 0) {
        itemsObj.push(data);
        AppendRow(data);
    } else {
        //need another condition here when item not duplicated and not available 
        debugger;
        itemsObj.forEach(function (arrayItem) {
            if (arrayItem.name == data.name) {
                arrayItem.quantity += 1;
                $(`[name='Quantity[` + (arrayItem.id - 1) + `]']`).val(arrayItem.quantity);
                //var s = $(`[name='Qunatity[0]']`);
                //var o = $(`[name='Qunatity[0]']`).val(5);

            } else {
                itemsObj.push(data);
                AppendRow(data);
            }
        });
    }
    function removeEle(el) {
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
                    CountTotalQuantity();
                    ComputeTotalPrice();
                } else {
                    var h = itemsObj;
                    arrayItem.quantity -= 1;
                    CountTotalQuantity();
                    ComputeTotalPrice();
                    itemsObj = itemsObj.filter(function (el) { return el.quantity != 0 });
                    $(el).closest('tr').remove();
                }
            });

            return false;
        } else
            return false;

        idIncrementer--;

    };

    //totalQ += (arrayItem.quantity);
    //console.log(totalQ);
    //document.querySelector('#kt_file_manager_items_counter').innerText = totalQ;
}
function CountTotalQuantity() {
    totalQuantity = 0;
    itemsObj.forEach(function (arrayItem) {
        totalQuantity += (arrayItem.quantity);
        console.log(totalQuantity);
        document.querySelector('#kt_file_manager_items_counter').innerText = totalQuantity;
    });
}

function ComputeTotalPrice() {
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
}
function AppendRow(data) {
    var markup = `<tr role="row animate__animated animate__backInLeft" class="odd">
               <td>`+ idIncrementer + `</td>
               <td><input class="form-control form-control-sm text-start" type='text' value='`+ data.name + `' name='Name[` + i + `]' disabled></td>
               <td><input class="form-control form-control-sm text-start" type='number' id='subPrice' name='Price[`+ i + `]' value='` + data.price + `'  disabled></td>
               <td><input class="form-control form-control-sm text-start" type='number' name='Quantity[`+ i + `]' min='1' value="` + data.quantity + `" disabled style='width:100px;'></ td >
               <td><button onclick="return removeEle(this)"; class="btn btn-icon btn-active-danger btn-outline btn-outline-default btn-icon-primary btn-active-icon-gray-700" ><i class="fa fa-trash" aria-hidden="true"></i></button></td>
               </tr>`;
    $("table tbody").append(markup);
}

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
                alert('Data: ' + JSON.stringify(response));
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