/// <reference path="jquery-3.1.0.js" />

$(document).ready(function () {
    $('#bikePointsButton').click(function (e) {
        ShowBikePoints();
    });

    $('#markLocationButton').click(function (e) {
        if (e.currentTarget.className == "togleButtonUnpressed") {
            e.currentTarget.className = "togleButtonPressed";

            locationMarkerListener = map.addListener('click', function (e) {
                if (window.userMarker) {
                    userMarker.setMap(null);
                }

                userMarker = new google.maps.Marker({
                    position: e.latLng,
                    map: map
                });

                $('#radiusInput').css("display", "block");
            });
        }
        else {
            e.currentTarget.className = "togleButtonUnpressed";
            google.maps.event.removeListener(locationMarkerListener);
        }
    });

    $('#clearMarkersButton').click(function (e) {
        if (typeof userMarker != "undefined") {
            if (userMarker != null) {
                userMarker.setMap(null);
                userMarker = null;
            }
        }
            
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(null);
        }
        markers = [];

        $('#radiusInput').css("display", "none");
    });
});

function ShowBikePoints() {
    $.ajax({
        url: typeof userMarker !== "undefined" && userMarker ?
            "/transportapi/bikeplaces/" + userMarker.position.lat() + "/" + userMarker.position.lng() + "/" + $('#dimentionInput').val()
            : "/transportapi/bikeplaces",
        type: "GET",
        complete: function (data) {
            if (markers == undefined) {
                markers = [];
            }
            for (var i = 0; i < markers.length; i++) {
                markers[i].setMap(null);
            }
            markers = [];

            for (var i = 0; i < data.responseJSON.length; i++)
            {
                var placePosition = new google.maps.LatLng(data.responseJSON[i].Lat, data.responseJSON[i].Lon);
                var marker = new google.maps.Marker({
                    position: placePosition,
                    icon: "/Content/Icons/bicycle_pointer.png"
                });

                var Name = '"' + data.responseJSON[i].CommonName + '"';
                var Locked = "Locked: ";
                var NbBikes = "Bikes: ";
                var NbEmptyDocks = "Empty Docks: ";
                var Distance = typeof data.responseJSON[i].Distance != "undefined" ? "<br/>Distance: " + data.responseJSON[i].Distance.toFixed(0) + " (m)" : "";
                for (var j = 0; j < data.responseJSON[i].AdditionalProperties.length; j++) {
                    switch(data.responseJSON[i].AdditionalProperties[j].Key) {
                        case "Locked":
                            Locked += data.responseJSON[i].AdditionalProperties[j].Value == "true" ? "yes": "no";
                            break;
                        case "NbBikes":
                            NbBikes += data.responseJSON[i].AdditionalProperties[j].Value.toString();
                            break;
                        case "NbEmptyDocks":
                            NbEmptyDocks += data.responseJSON[i].AdditionalProperties[j].Value.toString();
                            break;
                        default:
                    }
                }

                google.maps.event.addListener(marker, 'click', (function (marker, content, infowindow) {
                    return function () {
                        infowindow.setContent(content);
                        infowindow.open(map, marker);
                    };
                })(marker, "<b>" + Name + "</b><br/>" + Locked + "<br/>" +
                NbBikes + "<br/>" + NbEmptyDocks + Distance,
                new google.maps.InfoWindow()));

                marker.setMap(map);
                markers.push(marker);
            }
        },
        succses: function (data) {
            console.log("succses");
        },
        error: function (jqXHR, status, error) {
            console.log("error");
        }
    });
};
