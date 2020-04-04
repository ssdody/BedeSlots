$(function () {
    $('#addCardForm').card({
        container: '.card-wrapper',

        formSelectors: {
            numberInput: 'input#number',
            expiryInput: 'input#expiry',
            cvcInput: 'input#cvv',
            nameInput: 'input#name'
        },

        messages: {
            validDate: 'valid\ndate', // optional - default 'valid\nthru'
            monthYear: 'month/year', // optional - default 'month/year'
        },

        placeholders: {
            number: '•••• •••• •••• ••••',
            name: 'Full Name',
            expiry: '••/••',
            cvc: '•••'
        },
    });
});