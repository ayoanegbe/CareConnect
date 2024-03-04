
'use strict'
const sahan = "-@!8A0P.!nm099(+";
const eneh = "i+!_Ay(1_9-*!71O";

var _encryptAES = function (_req) {
    var key = CryptoJS.enc.Utf8.parse(sahan);
    var iv = CryptoJS.enc.Utf8.parse(eneh);
    let json = JSON.stringify(_req);
    return CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(json), key, { iv: iv }).toString();
};

var _decryptAES = function (ciphertext) {
    //console.log(ciphertext);
    var ciphertextWA = CryptoJS.enc.Base64.parse(ciphertext);
    var key = CryptoJS.enc.Utf8.parse(sahan);
    var iv = CryptoJS.enc.Utf8.parse(eneh);
    var ciphertextCP = { ciphertext: ciphertextWA };
    var decrypted = CryptoJS.AES.decrypt(ciphertextCP, key, { iv: iv });
    //console.log(decrypted.toString(CryptoJS.enc.Utf8));
    return JSON.parse(decrypted.toString(CryptoJS.enc.Utf8))
};

$('input.CurrencyInput').on('blur', function () {

    let value = this.value;
    if (isNaN(value))
        value = "0";
    let sign = (value == (value = Math.abs(value)));
    value = Math.floor(value * 100 + 0.50000000001);
    let dec = value % 100;
    value = Math.floor(value / 100).toString();
    if (dec < 10)
        dec = "0" + dec;
    for (var i = 0; i < Math.floor((value.length - (1 + i)) / 3); i++)
        value = value.substring(0, value.length - (4 * i + 3)) + ',' + value.substring(value.length - (4 * i + 3));
    let sg = sign ? '' : '-';
    this.value = sg + value + '.' + dec;
});

const items = ['-', '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

var number_input = document.getElementById('number_input');

number_input.addEventListener('keypress', function (evt) {

    if (!items.includes(evt.key)) {
        evt.preventDefault();
    }
});

var loadingTask = pdfjsLib.getDocument('path/to/your/document.pdf');
loadingTask.promise.then(function (pdf) {
    pdf.getPage(1).then(function (page) {
        var scale = 1.5;
        var viewport = page.getViewport({ scale: scale, });

        // Prepare canvas using PDF page dimensions
        var canvas = document.getElementById('the-canvas');
        var context = canvas.getContext('2d');
        canvas.height = viewport.height;
        canvas.width = viewport.width;

        // Render PDF page into canvas context
        var renderContext = {
            canvasContext: context,
            viewport: viewport,
        };
        page.render(renderContext);
    });
});

function isEmail(email) {
    var emailRegex = /^[a-z0-9]+@[a-z]+\.[a-z]{2,3}$/; return emailRegex.test(email);
}



