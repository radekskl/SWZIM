var rozmiar = new google.maps.Size(32, 32);
var punkt_startowy = new google.maps.Point(0, 0);
var punkt_zaczepienia = new google.maps.Point(16, 16);
var dymek = new google.maps.InfoWindow();
var map;
var marker;
function initialize() {
    var mapOptions = {
        center: { lat: 52.233, lng: 21.017 },
        zoom: 16
    };
    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);


    google.maps.event.addListener(map, 'click', function (event) {
        addEditMarker(event.latLng);
    });
    marker = new google.maps.Marker();
}
google.maps.event.addDomListener(window, 'load', initialize);




function addEditMarker(location) {
    marker.setPosition(location);
    marker.setMap(map);
    //wykrycie ktory rodzaj formularza
    if (document.getElementById("Latitude") != null) {
        document.getElementById("Latitude").value = location.lat();
        document.getElementById("Longitude").value = location.lng();
    } else {
        document.getElementById("Event_Latitude").value = location.lat();
        document.getElementById("Event_Longitude").value = location.lng();
    }

}


function update() {
    if (document.getElementById("Latitude") != null) {
        var lat = parseFloat(document.getElementById("Latitude").value);
        var lng = parseFloat(document.getElementById("Longitude").value);
    } else {
        var lat = parseFloat(document.getElementById("Event_Latitude").value);
        var lng = parseFloat(document.getElementById("Event_Longitude").value);
    }
    var position = new google.maps.LatLng(lat, lng);
    marker.setPosition(position);
    marker.setMap(map);
    map.setCenter(position);
}