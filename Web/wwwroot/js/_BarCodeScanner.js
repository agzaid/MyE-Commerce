
var _Barcode = {
    Scan: function () {
        debugger;
        var barcode = "";
        var interval;
        $(document).on('keydown', function (evt) {
            if (interval) {
                clearInterval(interval);
            }
            if (evt.code == 'Enter') {
                evt.preventDefault();
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


