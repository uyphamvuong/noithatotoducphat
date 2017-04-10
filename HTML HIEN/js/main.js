// menu 
function myFunction() {
    var x = document.getElementById("myTopnav");
    if (x.className === "topnav") {
        x.className += " responsive";
    } else {
        x.className = "topnav";
    }
}

// slider

// maps
function myMap() {
var mapOptions = {
    center: new google.maps.LatLng(10.7766897, 106.6319297),
    zoom: 16,
    mapTypeId: 'roadmap'
}
var map = new google.maps.Map(document.getElementById("map"), mapOptions);
}