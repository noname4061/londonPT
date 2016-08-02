<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PublicTransport.aspx.cs" Inherits="LondonPublicTransport.Pages.PublicTransport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>London Public Transport</title>
    <meta name="viewport" content="user-scalable=no, initial-scale=1, width=device-width, target-densitydpi=device-dpi"/>
    <link href="~/Content/Styles.css" rel="stylesheet" />
    <%: System.Web.Optimization.Scripts.Render("~/scripts/jquery") %>
    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyBjA2bQEPwToUnyQsEVss63mcL25w8Rddw"></script>
    <script>
        function initialize() {
            var mapProp = {
                center: new google.maps.LatLng(51.508742, -0.120850),
                zoom: 13,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                mapTypeControlOptions: {
                    style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
                    position: google.maps.ControlPosition.TOP_RIGHT
                },
            };
            map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
            bikeMarkers = [];
            busStopsMarkers = [];
        }
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
    <script src="../scripts/GoogleMaps.js"></script>
</head>
<body>
    <div id="workspace">
        <div id="header">
            <button class="actionButton leftAction" id="bikePointsButton">Show bike points</button>
            <button class="actionButton leftAction" id="busStopsButton">Show bus stops</button>

            <div id="radiusInput" class="rightAction radiusInput">
                <input class="meterInput" id="dimentionInput" type="number" value="300" min="10" max="20000"/>
                <div class="dimensionText rightAction">(m)</div>
            </div>
            <button class="togleButtonUnpressed" id="markLocationButton">Mark location</button>
            <button class="actionButton rightAction" id="clearMarkersButton">Clear map</button>
        </div>
        <div id="googleMap"></div>

        <div id="footer">
            <button id="hideSidePanelButton">◄</button>
            <div id="sidePanelContent">
                <br/><br/>
                <div id="sidePanelShiftedContemt"></div>
            </div>
        </div>
    </div>
</body>
</html>
