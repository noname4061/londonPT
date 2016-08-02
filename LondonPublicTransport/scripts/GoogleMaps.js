/// <reference path="jquery-3.1.0.js" />
/// <reference path="jquery.numeric.min.js" />

$(document).ready(function () {
    $('#bikePointsButton').click(function (e) {
        ShowBikePoints();
    });

    $('#busStopsButton').click(function (e) {
        ShowBusStops();
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
            
        for (var i = 0; i < bikeMarkers.length; i++) {
            bikeMarkers[i].setMap(null);
        }
        bikeMarkers = [];

        for (var i = 0; i < busStopsMarkers.length; i++) {
            busStopsMarkers[i].setMap(null);
        }
        busStopsMarkers = [];

        $('#radiusInput').css("display", "none");

        HideFooter();
    });

    $('#dimentionInput')[0].addEventListener('input', function () {
        var num = this.value.match(/^\d+$/);
        if (num === null) {
            this.value = "";
        }
    }, false);

    $('#hideSidePanelButton').click(function (e) {
        HideFooter();
    });
});


function ShowBikePoints() {
    $.ajax({
        url: typeof userMarker !== "undefined" && userMarker ?
            "/transportapi/bikeplaces/" + userMarker.position.lat() + "/" + userMarker.position.lng() + "/" + $('#dimentionInput').val()
            : "/transportapi/bikeplaces",
        //url: "https://api.tfl.gov.uk/BikePoint",
        type: "GET",
        complete: function(data) {
            ProcessResponceData(data, "/Content/Icons/bicycle_pointer.png", "bikeMarkers", GetBikePointInfoWindowContent)
        },
        succses: function (data) {
            console.log("succses");
        },
        error: function (jqXHR, status, error) {
            console.log(error);
        }
    });
};

function ShowBusStops() {
    $.ajax({
        url: typeof userMarker !== "undefined" && userMarker ?
            "/transportapi/busstops/" + userMarker.position.lat() + "/" + userMarker.position.lng() + "/" + $('#dimentionInput').val()
            : "/transportapi/busstops",
        type: "GET",
        complete: function (data) {
            ProcessResponceData(data, "/Content/Icons/bus_pointer.png", "busStopsMarkers", GetBusStopInfoWindowContent, SetBusStopFooterMenuContent)
        },
        succses: function (data) {
            console.log("succses");
        },
        error: function (jqXHR, status, error) {
            console.log(error);
        }
    });
};

function ProcessResponceData(data, iconLocation, markersArray, infoWindowFunction, footerMenuSetOnElementClickFunction) {
    if (window[markersArray] == undefined) {
        window[markersArray] = [];
    }
    for (var i = 0; i < window[markersArray].length; i++) {
        window[markersArray][i].setMap(null);
    }
    window[markersArray] = [];

    for (var i = 0; i < data.responseJSON.length; i++) {
        var placePosition = new google.maps.LatLng(data.responseJSON[i].Lat, data.responseJSON[i].Lon);
        var marker = new google.maps.Marker({
            position: placePosition,
            icon: iconLocation,
        });

        google.maps.event.addListener(marker, 'click', (function (marker, content, infowindow, element) {
            return function () {
                infowindow.setContent(content);
                infowindow.open(map, marker);
                if (typeof footerMenuSetOnElementClickFunction != "undefined") {
                    footerMenuSetOnElementClickFunction(element);
                }
            };
        })(marker, infoWindowFunction(data.responseJSON[i]), new google.maps.InfoWindow(), data.responseJSON[i]));

        marker.setMap(map);
        window[markersArray].push(marker);
    }
}

function GetBikePointInfoWindowContent(responseElement) {
    var Name = '"' + responseElement.CommonName + '"';
    var Locked = "Locked: ";
    var NbBikes = "Bikes: ";
    var NbEmptyDocks = "Empty Docks: ";
    var Distance = typeof responseElement.Distance != "undefined" ? "<br/>Distance: " + responseElement.Distance.toFixed(0) + " (m)" : "";
    for (var j = 0; j < responseElement.AdditionalProperties.length; j++) {
        switch (responseElement.AdditionalProperties[j].Key) {
            case "Locked":
                Locked += responseElement.AdditionalProperties[j].Value == "true" ? "yes" : "no";
                break;
            case "NbBikes":
                NbBikes += responseElement.AdditionalProperties[j].Value.toString();
                break;
            case "NbEmptyDocks":
                NbEmptyDocks += responseElement.AdditionalProperties[j].Value.toString();
                break;
            default:
        }
    }

    return "<b>" + Name + "</b><br/>" + Locked + "<br/>" + NbBikes + "<br/>" + NbEmptyDocks + Distance;
}

function GetBusStopInfoWindowContent(responseElement) {
    var Name = '"' + responseElement.CommonName + '"';
    var BusNumbers = "Bus Numbers: ";
    var Distance = typeof responseElement.Distance != "undefined" ? "<br/>Distance: " + responseElement.Distance.toFixed(0) + " (m)" : "";
    
    for (var i = 0; i < responseElement.LineModeGroups.length; i++) {
        if (responseElement.LineModeGroups[i].ModeName == "bus") {
            var lineIdentifier = responseElement.LineModeGroups[i].LineIdentifier;
            var k = 0;
            while (k < lineIdentifier.length) {
                lineIdentifier.splice(k, 0, "<br/>");
                k += 7;
            }
            BusNumbers += lineIdentifier.join(" ");
            break;
        }
    }

    return "<b>" + Name + "</b><br/>" + BusNumbers + Distance;
}

function SetBusStopFooterMenuContent(responseElement) {
    var footer = $('#sidePanelShiftedContemt')[0];

    var naptanIds = [];
    for (var i = 0; i < responseElement.LineGroup.length; i++) {
        naptanIds.push(responseElement.LineGroup[i].NaptanIdReference);
    }

    if (naptanIds.length > 0) {
        $.ajax({
            url: "/transportapi/stoppointarrivals/" + naptanIds.toString(),
            type: "GET",
            complete: function (data) {
                if (data.responseJSON != "") {
                    if (typeof isShowFooter == "undefined") {
                        ShowFooter();
                    }
                    footer.innerHTML = data.responseText.slice(1, data.responseText.length - 1);
                }
                else {
                    HideFooter();
                }
            },
            succses: function (data) {
                console.log("succses");
            },
            error: function (jqXHR, status, error) {
                console.log(error);
            }
        });
    }
    else {
        HideFooter();
    }
}

function ShowFooter() {
    $('#footer').css("display", "block");
    isFooterVisible = true;
}

function HideFooter() {
    $('#footer').css("display", "none");
    isFooterVisible = false;
}