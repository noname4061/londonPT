/// <reference path="jquery-3.1.0.js" />

(function () {
    $(document).ready(function () {
        $('#bikePoints').click(function (e) {
            //window.alert("sometext");
            ShowBikePoints();
        });
    });

    function ShowBikePoints() {
        $.ajax({
            url: "/transportapi/bikeplaces",
            type: "GET",
            complete: function (data) {
                for (var i = 0; i < data.responseJSON.length; i++)
                {
                    var placePosition = new google.maps.LatLng(data.responseJSON[i].Lat, data.responseJSON[i].Lon);
                    var marker = new google.maps.Marker({
                        position: placePosition,
                        icon: "/Content/Icons/bicycle_pointer.png",
                        //label: "42"
                    });

                    var Name = '"' + data.responseJSON[i].CommonName + '"';
                    var Locked = "Locked: ";
                    var NbBikes = "Bikes: ";
                    var NbEmptyDocks = "Empty Docks: ";
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
                    })(marker, "<p><b>" + Name + "</b><br/>" + Locked + "<br/>" + NbBikes + "<br/>" + NbEmptyDocks + "</p>", new google.maps.InfoWindow()));

                    marker.setMap(map);
                }

                //window.alert("complete");
            },
            succses: function (data) {
                window.alert("succses");
            },
            error: function (jqXHR, status, error) {
                window.alert("error");
            }
        });
    };
})();