$(document).ready(function () {
    initialize_property_map_home();
    ScrollToTop();
    StickyMenuOnTop();
    Core_revsliderHome();
    if ($('.body-activity').length > 0) {
        Core_AutoResizeImgBodyContext();
        Core_AutoAddSwipeBoxBodyContext();
    };
    Core_UserOnline();
    $("#video-iframe").fitVids();
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


// StickyMenu On Top
function StickyMenuOnTop() {
    $('.navbar-home').scrollToFixed();
}

var revapi2;

function Core_revsliderHome() {
    if ($('#rev_sliderHome').length <= 0) { return; }
    if ($('#rev_sliderHome').revolution == undefined)
        revslider_showDoubleJqueryError('#rev_sliderHome');
    else
        revapi2 = $('#rev_sliderHome').show().revolution(
         {
             dottedOverlay: "none",
             delay: 9000,
             startwidth: 1900,
             startheight: 600,
             hideThumbs: 200,

             thumbWidth: 100,
             thumbHeight: 50,
             thumbAmount: 2,

             navigationType: "bullet",
             navigationArrows: "solo",
             navigationStyle: "round",

             touchenabled: "on",
             onHoverStop: "on",

             swipe_velocity: 0.7,
             swipe_min_touches: 1,
             swipe_max_touches: 1,
             drag_block_vertical: false,

             keyboardNavigation: "off",

             navigationHAlign: "center",
             navigationVAlign: "bottom",
             navigationHOffset: 0,
             navigationVOffset: 20,

             soloArrowLeftHalign: "left",
             soloArrowLeftValign: "center",
             soloArrowLeftHOffset: 20,
             soloArrowLeftVOffset: 0,

             soloArrowRightHalign: "right",
             soloArrowRightValign: "center",
             soloArrowRightHOffset: 20,
             soloArrowRightVOffset: 0,

             shadow: 0,
             fullWidth: "on",
             fullScreen: "off",

             spinner: "spinner0",

             stopLoop: "off",
             stopAfterLoops: -1,
             stopAtSlide: -1,

             shuffle: "off",

             autoHeight: "off",
             forceFullWidth: "off",

             hideThumbsOnMobile: "off",
             hideNavDelayOnMobile: 1500, hideBulletsOnMobile: "off",
             hideArrowsOnMobile: "off",
             hideThumbsUnderResolution: 0,

             hideSliderAtLimit: 0,
             hideCaptionAtLimit: 0,
             hideAllCaptionAtLilmit: 0,
             startWithSlide: 0,
             fullScreenOffsetContainer: ""
         });
}

// Maps
function initialize_property_map_home() {
    var url = propertyMarkerInfoHome.icon;
    var size = new google.maps.Size(42, 57);

    // retina
    if (window.devicePixelRatio > 1.5) {
        if (propertyMarkerInfoHome.retinaIcon) {
            url = propertyMarkerInfoHome.retinaIcon;
            size = new google.maps.Size(83, 113);
        }
    }

    var image = {
        url: url,
        size: size,
        scaledSize: new google.maps.Size(42, 57),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(21, 56)
    };

    var propertyLocation = new google.maps.LatLng(propertyMarkerInfoHome.lat, propertyMarkerInfoHome.lang);
    var propertyMapOptions = {
        center: propertyLocation,
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        scrollwheel: false
    };
    var propertyMap = new google.maps.Map(document.getElementById("property_map_home"), propertyMapOptions);
    var propertyMarker = new google.maps.Marker({
        position: propertyLocation,
        map: propertyMap,
        icon: image
    });
}

function Core_AutoResizeImgBodyContext() {
    //body-activity
    var wm = $('.body-activity').width();
    $('.body-activity img').each(function (i, v) {
        $(this).css({ height: 'auto' });
        if (wm < $(this).width())
            $(this).css({ width: '100%', height: 'auto', max_width: 'auto' });
    });
    $('.body-activity div, .body-activity table').each(function (i, v) {
        if (wm < $(this).width())
            $(this).css({ width: '100%' });
    });
};

function Core_AutoAddSwipeBoxBodyContext() {
    $('.body-activity img').each(function (i, v) {
        if ($(this).parent().prop("tagName") == 'A') {

        }
        else {
            var htmlt = $("<div />").append($(this).clone()).html();
            var imgurl = $(this).attr('src');
            var title = $(this).attr('alt');
            $(this).replaceWith('<a href="' + imgurl + '" class="swipebox-body-activity" title="' + title + '">' + htmlt + '</a>');
        }
    });
    $('.swipebox-body-activity').swipebox();
};

function Core_UserOnline () {
    // Reference the auto-generated proxy for the hub.  
    var userActivity = $.connection.userActivityHub;
    // Create a function that the hub can call back to display messages.
    userActivity.client.updateUsersOnlineCount = function (ds) {
        // Add the message to the page. 
        $('#usersCount').text(ds.CurrentOnline);
        if (ds.RefreshData) {
            $('#Count_DayView').text(ds.Count_DayView);
            $('#Count_MonthView').text(ds.Count_MonthView);
            $('#Count_TotalView').text(ds.Count_TotalView);
        }                 
    };
    $.connection.hub.start({ transport: 'longPolling' });
}