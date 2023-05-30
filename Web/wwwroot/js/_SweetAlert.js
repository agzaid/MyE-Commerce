var _Swal = {
    TextInput: function () {
        const { value: barcode } = Swal.fire({
            title: "Scan Barcode",
            text: "or write serial number",
            input: 'text',
            showCancelButton: true,
            inputValidator: (value) => {
                if (!value) {
                    return 'You need to Scan your Barcode!'
                }
            }
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire('Saved!', '', 'success')
            }
        });

    },
    MultiInput: async function () {
        const { value: formValues } = await Swal.fire({
            title: 'Multiple inputs',
            html:
                'Barcode:<input id="swal-input1" class="swal2-input">' +
                'Price:<input id="swal-input2" class="swal2-input">' +
                'Expiry_Date:<input id="swal-input3" class="swal2-input">',
            focusConfirm: false,
            preConfirm: () => {
                return [
                    document.getElementById('swal-input1').value,
                    document.getElementById('swal-input3').value,
                    document.getElementById('swal-input2').value
                ]
            }
        })

        if (formValues) {
            Swal.fire(JSON.stringify(formValues))
        }
    },
    BasicAlert: function (message) {
        Swal.fire(
            message,
            'You clicked the button!',
            'success'
        );
    },
    AlertWithImage: function (message, barcode) {
        debugger;
        Swal.fire(
            message,
            '<img src="' + barcode + '" />',
            'success'
        );
    } ,
}