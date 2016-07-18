angular.module('app', []).controller('ConfigController', function ($window, $http) {
    var configCtrl = this;

    configCtrl.loadConfig = function () {
        $http.get("/api/config").then(
            function (response) {
                console.log(response.data);
                configCtrl.config = response.data;
            },
            function(error) {
                console.log(error);
            });
    };

    configCtrl.saveConfig = function () {
        $http.post("/api/saveConfig", configCtrl.config).then(
            function(response) {
                $window.location.href = '/index.html';
            },
            function (error) {
                console.log(error);
            });
    };

    configCtrl.cancel = function () {
        $window.location.href = '/index.html';
    };

    configCtrl.loadConfig();
});