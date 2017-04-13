$(document).ready(function () {
    ScrollToTop();
    StickyMenuOnTop();

});

// Scroll To Top
function ScrollToTop() {
    if ($('#back-to-top').length) {
        var scrollTrigger = 100, // px
            backToTop = function () {
                var scrollTop = $(window).scrollTop();
                if (scrollTop > scrollTrigger) {
                    $('#back-to-top').addClass('show');
                } else {
                    $('#back-to-top').removeClass('show');
                }
            };
        backToTop();
        $(window).on('scroll', function () {
            backToTop();
        });
        $('#back-to-top').on('click', function (e) {
            e.preventDefault();
            $('html,body').animate({
                scrollTop: 0
            }, 700);
        });
    }
}

// Maps
function myMap() {
var mapOptions = {
    center: new google.maps.LatLng(10.7766897, 106.6319297),
    zoom: 16,
    mapTypeId: 'roadmap'
}
var map = new google.maps.Map(document.getElementById("map"), mapOptions);
}

// StickyMenu On Top
function StickyMenuOnTop() {
    $('.navbar-home').scrollToFixed();
}