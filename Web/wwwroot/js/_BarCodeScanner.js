
var _Barcode = {
    Scan: function () {
        var barcode = "";
        var interval;
        document.addEventListener('keydown', function (evt) {
            debugger;
            if (interval) {
                clearInterval(interval);
            }
            if (evt.code == 'Enter') {
                if (barcode) {
                    handleBarcode(barcode);
                }
                barcode = "";
                return;
            }
            if (evt.key != "Shift") {
                barcode += evt.key;
                //added this so to add barcode manually
                //handleBarcode(barcode);
            }
            interval = setInterval(() => barcode = "", 20)
        });
        function handleBarcode(scanned_barcode) {
            //document.querySelector('#last-barcode').innerHTML = scanned_barcode;
            document.querySelector('#scanned-barcode').value = scanned_barcode;
        }
    }
}

var _AjaxCall = {
    Call: function () {
        $.ajax({
            url: 'https://localhost:7099/en/Admin/Sales/GetItem',
            type: 'GET',
            data: {
                "id": "1010"
            },
            contentType: "application/json; charset = utf-8",
            datatype: "json",
            success: function (response) {
                alert('Data: ' + data);
            },
            error: function (request, error) {
                alert("Request: " + JSON.stringify(request));
            }
        });
    }
}
