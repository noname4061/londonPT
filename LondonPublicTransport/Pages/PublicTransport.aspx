<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PublicTransport.aspx.cs" Inherits="LondonPublicTransport.Pages.PublicTransport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>London Public Transport</title>
    <link href="~/Content/Styles.css" rel="stylesheet" />
    <%: System.Web.Optimization.Scripts.Render("~/scripts/jquery") %>
    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyBjA2bQEPwToUnyQsEVss63mcL25w8Rddw"></script>
    <script>
        function initialize() {
            var mapProp = {
                center: new google.maps.LatLng(51.508742, -0.120850),
                zoom: 9,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                mapTypeControlOptions: {
                    style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
                    position: google.maps.ControlPosition.TOP_RIGHT
                },
            };
            map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

            map.addListener('click', function (e) {
                if (window.userMarker) {
                    userMarker.setMap(null);
                }

                userMarker = new google.maps.Marker({
                    position: e.latLng,
                    map: map
                });
            });
        }
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
    <script src="../scripts/GoogleMaps.js"></script>
</head>
<body>
    <div id="workspace">
        <div id="header">
            <button class="actionButton" id="bikePoints">Show bike points</button>
        </div>
        <div id="googleMap"/>
        <div id="footer">footer
        </div>
    </div>
</body>
</html>
