angular.module('app', []).controller('HomeController', function ($timeout, $http) {
    var home = this;

    home.refreshIntervalMs = 500;

    home.updateStatus = function () {
        $http.get("/api/status").then(
            function (response) {
                home.status = response.data;
                home.boxes = home.getDrawingBoxes(home.status.ProfileElements);
                $timeout(home.updateStatus, home.refreshIntervalMs);
            },
            function(error) {
                console.log(error);
                $timeout(home.updateStatus, home.refreshIntervalMs);
            });
    };

    home.toggleProfileMode = function () {
        $http.post("/api/toggleProfileMode");
    };

    home.runProgram = function () {
        $http.post("/api/runProgram");
    };

    home.canRunProgram = function () {
        var a = !home.status.ProgramRunning;
        var b = !home.status.OnTable;
        return a && b;
    };

    home.getTotalWidth = function(){
        var total = 0;
        var totalElements = home.status.ProfileElements.length;
        for (var i = 0; i < totalElements; i++) {
            var e = home.status.ProfileElements[i];
            total += e.Width;
        }
        return total;
    };

    home.drawingViewBox = function () {
        var margin = (home.status.KerfWidthInches + 0.05);
        var minx = (-1 * margin);
        var miny = (-1 * margin);
        var width = (home.getTotalWidth() + (2 * margin));
        var height = 0.75 + (2 * margin);
        return (minx + ' ' + miny + ' ' + width + ' ' + height);
    };

    home.getDrawingBoxes = function (elements) {
        var x = 0;
        var boxes = [];
        var totalElements = elements.length;
        for (var i = 0; i < totalElements; i++) {
            var e = elements[i];
            boxes.push({ x: x, y: 0, width: e.Width, height: (0.25 + (e.Type * 0.5)) });
            x = (x + e.Width);
        }
        return boxes;
    };

    // start the first status update
    home.updateStatus();
});