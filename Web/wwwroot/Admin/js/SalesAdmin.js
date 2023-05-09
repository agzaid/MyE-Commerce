
var idIncrementer = 1;
var destroyTable = true;
var itemsObj = [];
var i = 0;
var totalPrice = 0;

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
            debugger;
            event.preventDefault();
            _Barcode.Scan();
            _AjaxCall.Call("1");
            document.querySelector('#total_price').innerText = $("#scanned-barcode").val();
        });
    }, 1000);

    $(`[name='Price`).on('keyup', function () {
        debugger;
        if ($(`[name='Price`).val() != "") {
            //$('#subPrice').val($(`[name='Price`).val());
            $('[id=subPrice]').each(function () {
                $(this).val($(`[name='Price`).val());
            })
        } else {
            $('[id=subPrice]').each(function () { $(this).val(0) })
        }


    });

});

function removeEle(el) {
    $(el).closest('tr').remove();
    idIncrementer--;

};
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
    itemsObj.push(data);
    var markup = `<tr role="row animate__animated animate__backInLeft" class="odd">
               <td>`+ idIncrementer + `</td>
               <td><input class="form-control form-control-sm text-start" type='text' value='`+ data.name + `' name='Name[` + i + `]' disabled></td>
               <td><input class="form-control form-control-sm text-start" type='number' id='subPrice' name='Price[`+ i + `]' value='` + data.price + `'  disabled></td>
               <td><input class="form-control form-control-sm text-start" type='number' name='Qunatity[`+ i + `]' min='1' value="` + data.quantity + `" style='width:100px;'></ td >
               <td><button onclick="removeEle(this)"; class="btn btn-icon btn-active-danger btn-outline btn-outline-default btn-icon-primary btn-active-icon-gray-700" ><i class="fa fa-trash" aria-hidden="true"></i></button></td>
               </tr>`;
    $("table tbody").append(markup);
    ComputeTotalPrice();
    idIncrementer++;
    i++;
}

setTimeout(function () {
    ComputeTotalPrice();

}, 1000);

function ComputeTotalPrice() {
    debugger
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