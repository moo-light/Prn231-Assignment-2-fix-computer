// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#edit-form input[name$=Password]").attr('placeholder','leave empty to keep password')
updatePrice = () => {
    const carRentalForm = $('#car-rental-form').first();
    const priceInput = $('#inp-rent-price').first();

    console.log(carRentalForm.serialize());
}
const rating = $(`#rating`)
ratingOver = (i) => {
    $(`#rate-container div.star`).removeClass("bg-warning");
    $(`#rate-container div.star:nth-child(n+1):nth-child(-n+${i})`).addClass("bg-warning");
}
ratingOut = () => {
    $(`#rate-container div.star`).removeClass("bg-warning");
    $(`#rate-container div.star:nth-child(n+1):nth-child(-n+${rating.val()})`).addClass("bg-warning");
}
rate = (i) => {
    rating.val(i);
}