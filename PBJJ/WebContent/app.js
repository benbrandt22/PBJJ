﻿angular.module('app', []).controller('HomeController', function ($timeout, $http) {
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

    home.reHome = function() {
        $http.post("/api/rehome");
    };

    home.runProgram = function () {
        $http.post("/api/runProgram");
    };


    // start the first status update
    home.updateStatus();
});