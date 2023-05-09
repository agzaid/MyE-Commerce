
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


