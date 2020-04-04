var lat = 42.6509;
var lon = 23.3795;

map = L.map('map-div').setView([lat, lon], 20);
// set map tiles source
L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors',
    maxZoom: 18,
}).addTo(map);
// add marker to the map
marker = L.marker([lat, lon]).addTo(map);
    // add popup to the marker
marker.bindPopup("<b>Bede Slots</b><br />Alexander Malinov 31<br />Sofia").openPopup();