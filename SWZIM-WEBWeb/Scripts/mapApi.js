var rozmiar = new google.maps.Size(32, 32);
var punkt_startowy = new google.maps.Point(0, 0);
var punkt_zaczepienia = new google.maps.Point(16, 16);
var dymek = new google.maps.InfoWindow();
var map;
var activeLayers = []; // tablica zawierajaca identyfikatory wyswietlanych warst
var marker; // marker nanoszony na mape
var markers = []; // tablica przechowujaca markery

//obiekt pomocniczy do przekazywania danych z widoku
var markerInformations = {
    iconUrl: undefined,
    lat: undefined,
    lng: undefined,
    title: undefined,
    text: undefined
};

//funcja inicjalizujaca mapę i jej ustawienia
// lat - Latitude, szerokość geograficzna
// lng - Longitude, długość geograficzna
// zoom - przybliżenie
function initializeMap(lat, lng, zoom) {
    var mapOptions = {
        center: { lat: lat, lng: lng },
        zoom: zoom
    };
    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);
}

//wariant ustawienia mapy dla akcji create
function initializeCreate() {
    initializeMap(52.233, 21.017, 16)

    google.maps.event.addListener(map, 'click', function (event) {
        addCreateMarker(event.latLng);
    });
    marker = new google.maps.Marker();
}

//wariant ustawienia mapy dla akcji edit
function initializeEdit() {
    initializeMap(markerInformations.lat, markerInformations.lng, 16)
    addEditMarker();
}

//wariant ustawienia mapy dla akcji details
function initializeDetails() {
    initializeMap(markerInformations.lat, markerInformations.lng, 16)
    addDetailsMarker();
}

//wariant ustawienia mapy dla glownej mapy
function initializeMapIndex() {
    initializeMap(52.233, 21.017, 12);
}

//funkcja do ustawiania odpowiedniego listener'a dla danej akcji
function setListener(name) {
    if (name == 'Details') {
        google.maps.event.addDomListener(window, 'load', initializeDetails);
    } else if (name == 'Create') {
        google.maps.event.addDomListener(window, 'load', initializeCreate);
    } else if (name == 'Edit') {
        google.maps.event.addDomListener(window, 'load', initializeEdit);
    } else if (name == 'MapIndex') {
        google.maps.event.addDomListener(window, 'load', initializeMapIndex);
    }
}

//ogolna funkcja tworzaca marker
function addMarker() {
    var iconUrl = markerInformations.iconUrl; // url icony markera
    var ico = new google.maps.MarkerImage(iconUrl, rozmiar, punkt_startowy, punkt_zaczepienia); // ustawienia ikony markera
    //utworzenie obiektu typu marker o zadanych atybutach
    marker = new google.maps.Marker({
        map: map,
        position: new google.maps.LatLng(markerInformations.lat, markerInformations.lng),
        icon: ico,
        title: markerInformations.title,
        text: markerInformations.text
    });
}

//funkcja dodajaca marker z dymkiem
function addDetailsMarker() {
    addMarker();
    //obsluga zdarzeniaklikniecia markera
    google.maps.event.addListener(marker, "click", function () {
        if (marker.text != undefined) {
            //dodanie dymka
            dymek.setContent('<strong>' + marker.title + '</strong><br />' + marker.text);
            dymek.open(map, marker);
        }
    });
}

//funkcja dodajaca marker z możliwoscia przeciagania i wpisywania wspolrzednych do textbox
function addEditMarker() {
    addMarker();
    marker.setDraggable(true); // wlaczenie przesuwania markera
    //obsluga zdarzenia przesuniecia markera
    google.maps.event.addListener(marker, "dragend", function (event) {
        //zapis wspolrzednych do pol tekstowych
        document.getElementById("Latitude").value = this.getPosition().lat();
        document.getElementById("Longitude").value = this.getPosition().lng();
    });
}

//funcja umozliwiajaca dodanie markera do mapy przez klikniecie lewym przyciskiem myszy
// i wpisywania wspolrzednych do textbox 
function addCreateMarker(location) {
    marker.setPosition(location); // ustawienie pozycji markera
    marker.setMap(map); // umieszczenie markera na mapie
    //wykrycie ktory rodzaj formularza i wstawienie wspolrzednych do textbox
    if (document.getElementById("Latitude") != null) {
        document.getElementById("Latitude").value = location.lat();
        document.getElementById("Longitude").value = location.lng();
    } else {
        document.getElementById("Event_Latitude").value = location.lat();
        document.getElementById("Event_Longitude").value = location.lng();
    }

}

// funkcja odpowiedzialna za uaktualnianie pozycji markera po zmianie wartosci w textbox
// oraz ustawienia srodka mapy na pozycje markera
function update() {
    //wykrycie ktory rodzaj formularza i zczytanie wspolrzednych z textbox
    if (document.getElementById("Latitude") != null) {
        var lat = parseFloat(document.getElementById("Latitude").value);
        var lng = parseFloat(document.getElementById("Longitude").value);
    } else {
        var lat = parseFloat(document.getElementById("Event_Latitude").value);
        var lng = parseFloat(document.getElementById("Event_Longitude").value);
    }
    var position = new google.maps.LatLng(lat, lng); //obiekt z wstawionymi wspolrzednymi
    marker.setPosition(position); //ustawienie pozycji markera
    marker.setMap(map); // umieszczenie markera na mapie
    map.setCenter(position); // umieszczenie srodka mapy na pozycji markera
}

//funkcja dodajaca marker do glownej mapy map/index
function addIndexMarker(options) {
    var marker = new google.maps.Marker(options);
    // obsluga zdarzenia klikniecia na marker
    google.maps.event.addListener(marker, "click", function () {
        //ustawienie dymka
        if (marker.text != undefined && marker.title != undefined) {
            dymek.setContent('<strong>' + marker.title + '</strong><br />' + marker.text);
            dymek.open(map, marker);
        }
    });
    markers.push(marker); // dodanie markera do tablicy markerow
}

//funkcja tworzaca nowy wielokat na mapie
function addPolygon(options) {
    var polygon = new google.maps.Polygon({
        paths: options.coords,
        strokeColor: '#FF0000',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#FF0000',
        fillOpacity: 0.35
    });
    polygon.layerid = options.layerid; // dodanie numeru warstwy do wielokata 

    //obsluga zdarzenia kliniecia na wielokat
    google.maps.event.addListener(polygon, "click", function (event) {
        //dodanie dymka z informacja
        if (options.text != undefined && options.title != undefined) {
            dymek.setContent('<strong>' + options.title + '</strong><br />' + options.text);
            dymek.setPosition(event.latLng);
            dymek.open(map, polygon);
        }
    });
    markers.push(polygon); // dodanie wielokata do tablicy markerow
}


//funkcja sluzaca do pobrania zdarzen dla danej warstwy i umieszczeniu ich na mapie w postaci markerow
function getLayer(id) {
    $.ajaxSetup({ async: false });
    $.get("/Map/GetAllEventsForLayout/" + id, function (data) {
        for (var i = 0; i < data.length; i++) {
            var ico = new google.maps.MarkerImage(data[i].icon, rozmiar, punkt_startowy, punkt_zaczepienia);
            addIndexMarker({ position: new google.maps.LatLng(data[i].lat, data[i].lon), icon: ico, title: data[i].name, text: data[i].desc, layerid: id });
        }
    });
}
//funkcja sluzaca do pobrania elementow danej warstwy i umieszczeniu ich na mapie w postaci markerow lub wielokatow
function getAllElementsForLayout(id) {
    $.ajaxSetup({ async: false });
    $.get("/Map/GetAllElementsForLayout/" + id, function (data) {
        for (var i = 0; i < data.length; i++) {
            // dodanie prostego elementu do mapy - marker
            if (data[i].points.length == 0) {
                var ico = new google.maps.MarkerImage(data[i].icon, rozmiar, punkt_startowy, punkt_zaczepienia);
                addIndexMarker({ position: new google.maps.LatLng(data[i].lat, data[i].lon), icon: ico, title: data[i].name, text: data[i].desc, layerid: id });
            } else { // dodanie zlozonego elementu do mapy - wielokat
                var polygonCoords = []; // tablica wspolrzednych krawedzi wielokata
                var firstCoord = new google.maps.LatLng(data[i].lat, data[i].lon);
                //polygonCoords.push(firstCoord);
                for (var j = 0; j < data[i].points.length; j++) {
                    //pobranie wartosci wspolrzednych
                    var lat = data[i].points[j].lat;
                    var lon = data[i].points[j].lon;
                    polygonCoords.push(new google.maps.LatLng(lat, lon)); //dodanie wpolrzednych do tablicy
                }
                //polygonCoords.push(firstCoord); //Api samo moze dorysowac linie do pierwszego punktu
                // ustawienie opcji wielokata
                var options = {
                    coords: polygonCoords,
                    title: data[i].name,
                    text: data[i].desc,
                    layerid: id
                };
                addPolygon(options); //dodanie wielokata
            }
        }
    });
}

//funkcja odpowiedzialana za ustawienie widocznosci markerow na mapie
function setMarkers(mapa) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(mapa);
    }
}

//funkcja obsugujaca klikniecie przycisku z dana warstwa
function onClickLayer(id) {
    var isInActivelayers = false; // przelacznik czy warstwa jest aktywna w danej chwili
    var position; // zmienna z pozycja elementu w tablicy

    //sprawdzenie czy warstwa jest aktualnie aktywna - id znajduje sie w tablicy activeLayers
    for (var i = 0; i < activeLayers.length; i++) {
        if (activeLayers[i] == id) {
            isInActivelayers = true;
            position = i;
            break;
        }
    }
    //spawdzenie czy warstwa jest aktywna
    if (isInActivelayers) {
        deleteLayer(id); //usumiecie markerow aktywnej warstwy
        activeLayers.splice(position, 1); // usuniecie identyfikatora warstwy z tablicy aktywnych warstw
        $('li[layer="' + id + '"]').removeClass('active'); // odznaczenie przycisku
    } else {
        $('li[layer="' + id + '"]').addClass('active'); // zaznaczenie przycisku
        getLayer(id); // pobranie zdarzen
        getAllElementsForLayout(id); // pobranie elementow warstwy
        activeLayers.push(id); // dodanie identyfikatora warstwy do tablicy aktywnych warstw
        setMarkers(map); // ustawienie widocznosci obietkow na mapie
    }
}

// funkcja realizujaca usuniecie ma markerow warstwy o danym identyfikatorze z tablicy markerow 
function deleteLayer(id) {
    var i = markers.length;
    while (i--) {
        if (markers[i].layerid == id) {
            markers[i].setMap(null);
            markers.splice(i, 1);
        }
    }
}

//funkcja czyszczaca mape z wszystkich obiektow 
function clearAllMarkers() {
    setMarkers(null);
    markers = [];
    //usuwanie podswietlenia z aktywna warstwa
    for (var i = 0; i < activeLayers.length; i++) {
        $('li[layer="' + activeLayers[i] + '"]').removeClass('active');
    }
    activeLayers = [];
}