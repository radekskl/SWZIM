var mrk;
var xmlMarkers = [];
var xmlMarkersName = [];
var defaultImgStart = 'http://chart.apis.google.com/chart?chst=d_map_spin&chld=1.5|0|0059FF|11|_|';
var defaultImgStartZnk = 'http://chart.apis.google.com/chart?chst=d_map_spin&chld=1.5|0|00FF00|11|_|';
var defaultImgStartSygn = 'http://chart.apis.google.com/chart?chst=d_map_spin&chld=1.5|0|FF0000|11|_|';
function showMap(markers, mCount, edit) {
    var latd = '52.2263148'; // Domyslny lang dla woli
    var longd = '20.9654794'; // Domyslny latd dla woli
    map = null;
    load_map(latd, longd, markers, mCount, edit);   // laduj mape
}

function load_map(latd, longd, markers, mCount, edit) {
    var myLatlng = new google.maps.LatLng(latd, longd); // tworzenie latlng z lat/lng

    var mapOptions = {
        // ustawianie opcji mapy
        zoom: 14, // jaki zoom
        center: myLatlng // kordy do centrowania
    }
    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions); // ustawienie mapy w dane miejsce (div + opcje mapy)

    if (edit === 'True') {
        listeners(map); // Dodanie listenera do mapy.
    }
    loadMarkers(map, markers, mCount, edit);

    return ({
        // zwracanie info - (mozna wywolac jak void, a mozna rowniez przypisac dane do var z tej funkcji)
        container: map.getDiv(),
        mapa: map
    });
}

function CenterMapOnGeo(lat, lng) {
    var myLatlng = new google.maps.LatLng(lat, lng);
    map.setCenter(myLatlng);
    map.setZoom(18);
}

function loadMarkers(map, mark, mCount, edit) {
    if (mark != null && map.zoom > 8) {
        var infoWindow = new google.maps.InfoWindow(), marker, mCount;
        var bounds = new google.maps.LatLngBounds();

        for (i = 0; i < mCount; i++) {

            if (!mark[i].isEvent) {
                var image = '/Images/GoogleMapIcon/junction.png';
            } else {
                var image = '/Images/GoogleMapIcon/' + mark[i].assignedImage + '.png';
            }
            var position = new google.maps.LatLng(mark[i].lat, mark[i].long);
            bounds.extend(position);
            marker = new google.maps.Marker({
                position: position,
                map: map,
                title: mark[i].title,
                icon: image,
                visible: true
            });
            // Allow each marker to have an info window          
            google.maps.event.addListener(marker, 'click', (function (marker, i) {
                return function () {
                    if (!mark[i].isEvent) {
                        if (edit === 'True') {
                            var name = '<div class="info_content" style="width:250px !important"><h4>' + mark[i].title + '</h4><p>' + mark[i].information + '<br />' + '<a href="http://' + window.location.hostname + ":" + window.location.port + '/Junction/Details/' + mark[i].id + '">Detail</a></p></div>';
                        } else {
                            var name = '<div class="info_content" style="width:250px !important"><h4>' + mark[i].title + '</h4><p>' + mark[i].information + '</p></div>';
                        }
                    } else {
                        if (edit === 'True') {
                            var name = '<div class="info_content" style="width:250px !important"><h4>' + mark[i].title + '</h4><p>' + mark[i].information + '<br />' + '<a href="http://' + window.location.hostname + ":" + window.location.port + '/Event/Details/' + mark[i].id + '">Detail</a></p></div>';
                        } else {
                            var name = '<div class="info_content" style="width:250px !important"><h4>' + mark[i].title + '</h4><p>' + mark[i].information + '</p></div>';
                        }
                    }
                    infoWindow.setContent(name);
                    infoWindow.open(map, marker);
                }
            })(marker, i));
            // Automatically center the map fitting all markers on the screen
        }
    }
}

function listeners(map) {
    google.maps.event.addListener(map, 'click', function (event) { // typ listenera
        var evt = event.latLng; // Pobranie geo z klikniecia
        placeMarker(map, event.latLng);
        console.log('latitude:' + evt.lat() + '; longitude:' + evt.lng() + ';'); // wypisanie wartosci w konsoli
        document.getElementById('t1').value = evt.lat(); // wypisanie do dokumentu (textbox id t1)
        document.getElementById('t2').value = evt.lng(); // wypisanie do dokumentu (textbox id t2)
        document.getElementById('NewCrossLat').value = document.getElementById('t1').value;
        document.getElementById('NewCrossLong').value = document.getElementById('t2').value;
        document.getElementById('NewEventLat').value = document.getElementById('t1').value;
        document.getElementById('NewEventLong').value = document.getElementById('t2').value;
        document.getElementById('NewFilterLat').value = document.getElementById('t1').value;
        document.getElementById('NewFilterLong').value = document.getElementById('t2').value;
    });
}

//Ustawianie markera po kliknięciu
function placeMarker(map, location) {

    if (mrk) {
        mrk.setPosition(location);
    } else {
        var img = '/Images/GoogleMapIcon/symbol_plus.png';
        mrk = new google.maps.Marker({
            position: location,
            map: map,
            icon: img,
            visible: true
        });
    }
    map.setCenter(location);
}

function ProcessCrossRoad(item) { // metoda do kopiowania wartosci miedzy textboxami

    if ((document.getElementById('t1').value && document.getElementById('t1').value != '') && (document.getElementById('t2').value && document.getElementById('t2').value.length != '')) {
        document.getElementById('NewCrossLat').value = document.getElementById('t1').value;
        document.getElementById('NewCrossLong').value = document.getElementById('t2').value;
        document.getElementById('NewEventLat').value = document.getElementById('t1').value;
        document.getElementById('NewEventLong').value = document.getElementById('t2').value;
        if (item.id === "Cross") {
            $('div#AddEvent').hide("fast");
            $('div#AddCross').slideToggle("fast");

        } else {
            $('div#AddCross').hide("fast");
            $('div#AddEvent').slideToggle("fast");
        }
    }


}

function loadSingleMap(lat, leng, name, info, isEdit) {
    var myLatlng = new google.maps.LatLng(lat, leng); // tworzenie latlng z lat/lng

    var mapOptions = {
        // ustawianie opcji mapy
        zoom: 18, // jaki zoom
        center: myLatlng // kordy do centrowania
    }
    var img = '/Images/GoogleMapIcon/here.png';
    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions); // ustawienie mapy w dane miejsce (div + opcje mapy)
    marker = new google.maps.Marker({
        position: myLatlng,
        map: map,
        icon: img,
        visible: true
    });
    var name = '<div class="info_content" style="width:250px !important"><h4>' + name + '</h4><p>' + info + '</p></div>';
    if (isEdit) {
        SingleMaplisteners(map);
    }
    var infowindow = new google.maps.InfoWindow({
        content: name
    });

    google.maps.event.addListener(marker, 'click', function () {
        infowindow.open(map, marker);
    });

}

function loadMapXml(lat, leng, name, info, znak, odcinki, sygnal, isEdit) {
    var myLatlng = new google.maps.LatLng(lat, leng); // tworzenie latlng z lat/lng

    xmlMarkers = [];
    xmlMarkersName = [];

    var mapOptions = {
        // ustawianie opcji mapy
        zoom: 18, // jaki zoom
        center: myLatlng // kordy do centrowania
    }
    var img = '/Images/GoogleMapIcon/here.png';
    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions); // ustawienie mapy w dane miejsce (div + opcje mapy)
    marker = new google.maps.Marker({
        position: myLatlng,
        map: map,
        icon: img,
        visible: true
    });

    xmlPlaceObjects(znak, odcinki, sygnal, map);

    var name = '<div class="info_content" style="width:250px !important"><h4>' + name + '</h4><p>' + info + '</p></div>';

    if (isEdit) {
        XmlEditMapListener(map);
    }
    var infowindow = new google.maps.InfoWindow({
        content: name
    });
    google.maps.event.addListener(marker, 'click', function () {
        infowindow.open(map, marker);
    });

}

function XmlEditMapListener(map) {
    google.maps.event.addListener(map, 'click', function (event) { // typ listenera
        var evt = event.latLng; // Pobranie geo z klikniecia

        var val = $("#ModelSide input[type='radio']:checked").val();
        // Dodac tutaj inny place map
        if (val != null) {
            var elem = val + ' t';

            var x = document.getElementById(elem).value;

            placeObject(map, event.latLng, x);

            var lat = val + 'lat';
            var long = val + 'long';
            document.getElementById(lat).value = evt.lat(); // wypisanie do dokumentu (textbox id t1)
            document.getElementById(long).value = evt.lng(); // wypisanie do dokumentu (textbox id t2)
        }
    });
}

function xmlPlaceObjects(znak, odcinki, sygnal, map) {
    for (i = 0; i < znak.length; i++) {
        var addOn = 'Znak | ' + znak[i].id;
        var img = defaultImgStartZnk + addOn;
        var location = new google.maps.LatLng(znak[i].lat, znak[i].long);

        mrk = new google.maps.Marker({
            position: location,
            map: map,
            icon: img,
            visible: true
        });

        xmlMarkers.push(mrk);
        xmlMarkersName.push(addOn);
    }

    for (a = 0; a < odcinki.length; a++) {

        var addOn1 = 'ODC | ' + odcinki[a].id + ' ST';
        var addOn2 = 'ODC | ' + odcinki[a].id + ' EN';
        var img1 = defaultImgStart + addOn1;
        var img2 = defaultImgStart + addOn2;
        var location1 = new google.maps.LatLng(odcinki[a].latStart, odcinki[a].longStart);
        var location2 = new google.maps.LatLng(odcinki[a].latEnd, odcinki[a].longEnd);
        mrk1 = new google.maps.Marker({
            position: location1,
            map: map,
            icon: img1,
            visible: true
        });

        mrk2 = new google.maps.Marker({
            position: location2,
            map: map,
            icon: img2,
            visible: true
        });
        xmlMarkers.push(mrk1);
        xmlMarkersName.push(addOn1);
        xmlMarkers.push(mrk2);
        xmlMarkersName.push(addOn2);
    }

    for (j = 0; j < sygnal.length; j++) {
        var addOn = 'SYGN | ' + sygnal[j].id;
        var img = defaultImgStartSygn + addOn;
        var location = new google.maps.LatLng(sygnal[j].lat, sygnal[j].long);

        mrk = new google.maps.Marker({
            position: location,
            map: map,
            icon: img,
            visible: true
        });

        xmlMarkers.push(mrk);
        xmlMarkersName.push(addOn);
    }

}


//Ustawianie markera po kliknięciu
function placeObject(map, location, value) {
    var img;
    if (value.indexOf('Znak') > -1) {
        var img = defaultImgStartZnk + value;
    } else if (value.indexOf('SYGN') > -1) {
        var img = defaultImgStartSygn + value;
    } else {
        var img = defaultImgStart + value;
    }

    if ((xmlMarkers == null && xmlMarkersName == null) || xmlMarkersName.indexOf(value) == -1) {
        mrk = new google.maps.Marker({
            position: location,
            map: map,
            icon: img,
            visible: true
        });
        xmlMarkers.push(mrk);
        xmlMarkersName.push(value);
    } else {
        for (var i = 0; i < xmlMarkers.length; i++) {
            if (xmlMarkers[i].icon == img) {
                xmlMarkers[i].setPosition(location);
            }
        }
    }
    map.setCenter(location);
}

function SingleMaplisteners(map) {
    google.maps.event.addListener(map, 'click', function (event) { // typ listenera
        var evt = event.latLng; // Pobranie geo z klikniecia
        placeMarker(map, event.latLng);
        console.log('latitude:' + evt.lat() + '; longitude:' + evt.lng() + ';'); // wypisanie wartosci w konsoli
        document.getElementById('Lat').value = evt.lat(); // wypisanie do dokumentu (textbox id t1)
        document.getElementById('Long').value = evt.lng(); // wypisanie do dokumentu (textbox id t2)
    });
}