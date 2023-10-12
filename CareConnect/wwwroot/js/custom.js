'use strict'
function setSelectionRange(input, selectionStart, selectionEnd) {
    if (input.setSelectionRange) {
        input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    } else if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        console.log(collapse);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    }
}

function setCaretToPos(input, pos) {
    setSelectionRange(input, pos, pos);
}


$("#Amount").click(function () {
    var inputLength = $("#Amount").val().length;
    setCaretToPos($("#Amount")[0], inputLength)
});

var options = {
    onKeyPress: function (cep, e, field, options) {
        if (cep.length <= 6) {

            var inputVal = parseFloat(cep);
            $('#Amount').val(inputVal.toFixed(2));
        }

        // setCaretToPos(jQuery('#money')[0], 4);

        var masks = ['#,##0.00', '0.00'];
        mask = (cep == 0) ? masks[1] : masks[0];
        $('#Amount').mask(mask, options);
    },
    reverse: true
};

$('#Amount').mask('#,##0.00', options);

