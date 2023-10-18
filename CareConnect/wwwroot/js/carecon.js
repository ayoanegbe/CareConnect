
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

    let num = this.value;
    if (isNaN(num))
        num = "0";
    let sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    let cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
            num.substring(num.length - (4 * i + 3));
    let sg = sign ? '' : '-';
    this.value = sg + num + '.' + cents;
});

const items = [43, 45, 46, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57];

var number_input = document.getElementById('number_input');

number_input.addEventListener('keypress', function (evt) {
    // if (evt.which < 48 || evt.which > 57)  {
    //     evt.preventDefault();
    // }

    if (!items.includes(evt.which)) {
        evt.preventDefault();
    }
});

