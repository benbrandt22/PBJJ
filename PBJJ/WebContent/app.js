angular.module('app', []).controller('HomeController', function ($timeout, $http) {
    var home = this;

    home.refreshIntervalMs = 500;

    home.updateStatus = function () {
        $http.get("/api/status").then(
            function (response) {
                home.status = response.data;
                $timeout(home.updateStatus, home.refreshIntervalMs);
            },
            function(error) {
                console.log(error);
                $timeout(home.updateStatus, home.refreshIntervalMs);
            });
    };


    // start the first status update
    home.updateStatus();
});