/* javascript by Uy Min */

$(function () {
    //$('#header,#body,#footer').css('opacity', 0);
    $('head').ready(function () { CORE_ShowHTML(); });

});

$(window).load(function () {
    //CORE_ShowHTML();
});

$(document).ready(function () {
    "use strict";

    // DETECT IE
    if (CORE_DetectIE() > 0) { $("head").append("<link rel='stylesheet' href='css/iehelp.css'>"); }
    CORE_ScrollAnchor();
    BS_hoverdropdown();
    Core_revsliderLeft();
    Core_revsliderTop();
    Core_revsliderHome();
    Core_AutoResizeImgBodyContext();

    // FUNC here
    
});

function CORE_ShowHTML() {
    // PLAY NOTI WHEN LOAD DONE!!!
    var isPlay = false;
    if (isPlay) {
        $('body').append("<audio id='audio-loading'><source src='upload/noti/noti1.ogg' type='audio/ogg'></audio>");
        var audioElement = document.getElementById('audio-loading');
        audioElement.volume = 0.5;
        audioElement.play();
        $('#audio-loading').bind('ended', function () { $(this).remove(); });
    }
    // SHOW WHEN DONE!!!
    var tFade = 0;
    $("#header").animate({ opacity: 1 }, tFade, function () {
        $('#body').animate({ opacity: 1 }, tFade, function () {
            $('#footer').animate({ opacity: 1 }, tFade, function () {
                $('.loading-container').fadeOut(300);
            });
        });
    });
}
function CORE_DetectIE() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf('MSIE ');
    var trident = ua.indexOf('Trident/');
    if (msie > 0) {
        // IE 10 or older => return version number
        return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
    }
    if (trident > 0) {
        // IE 11 (or newer) => return version number
        var rv = ua.indexOf('rv:');
        return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }
    // other browser
    return 0;
};
function CORE_ScrollAnchor() {
    var $root = $('html, body');
    $('a').click(function () {
        var href = $(this).attr('href');
        var dom = $(this).attr('data-dom');        
        if (href == "#" || dom == null) { return; }
        var offset = $(dom).offset();
        $root.animate({            
            scrollTop: offset.top
        }, 500, function () {
            //if (window.location.hash !== null)
            //    window.location.hash = href;
        });
        return false;
    });

    $("body").append("<div id='scrolltop'>TOP</div>");
    $("#scrolltop").click(function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, 500);
    });

    var isrun = false;
    var reload = function () {
        var height = $(window).scrollTop();
        if (height > 100) {
            if (!isrun) {
                isrun = true;
                $("#scrolltop").fadeIn(500, function () {
                    isrun = false;
                });
            }
        }
        else {
            $("#scrolltop").hide();
        }
    }
    reload();

    $(window).scroll(function () {
        reload();
    });

    $(window).resize(function () {
        reload();
    });
}
function BS_hoverdropdown()
{
    $('.dropdown-toggle').dropdownHover({
        delay: 0,
        instantlyCloseOthers: false
    });
    $('.navbar-menu-t').scrollToFixed({
        preFixed: function () { $(this).addClass("navbar-menu-t-fixed"); },
        postFixed: function () { $(this).removeClass("navbar-menu-t-fixed"); }
    });
    $(window).stellar();
    $('.fit-video').fitVids();
}

var revapi1;

function Core_revsliderLeft() {
    if ($('#rev_sliderLeft').length <= 0) { return; }
    if ($('#rev_sliderLeft').revolution == undefined)
        revslider_showDoubleJqueryError('#rev_sliderLeft');
    else
        revapi1 = $('#rev_sliderLeft').show().revolution(
         {
             dottedOverlay: "none",
             delay: 4000,
             startwidth: 240,
             startheight: 380,
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

var revapi3;

function Core_revsliderTop() {
    if ($('#rev_sliderTop').length <= 0) { return; }
    if ($('#rev_sliderTop').revolution == undefined)
        revslider_showDoubleJqueryError('#rev_sliderTop');
    else
        revapi3 = $('#rev_sliderTop').show().revolution(
         {
             dottedOverlay: "none",
             delay: 4000,
             startwidth: 800,
             startheight: 150,
             hideThumbs: 150,

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

function Core_AutoResizeImgBodyContext()
{
    //body-activity
    var wm = $('.body-activity').width();
    $('.body-activity img').each(function (i, v) {
        if (wm < $(this).width())
            $(this).css({ width: '100%', height: 'auto' });
    });

    
}