angular.module('app', []).controller('ProfilesController', function ($window, $http) {
    var profilesCtrl = this;

    profilesCtrl.loadProfiles = function () {
        $http.get("/api/profiles").then(
            function (response) {
                console.log(response.data);
                profilesCtrl.profileNames = response.data.Result.ProfileNames;
            },
            function(error) {
                console.log(error);
            });
    };

    profilesCtrl.loadProfile = function (name) {
        var data = { name: name };
        $http.post("/api/loadProfile", data).then(
            function (response) {
                $window.location.href = '/index.html';
            },
            function (error) { console.log(error); });
    };

    profilesCtrl.editProfile = function (name) {
        $window.location.href = ('/edit.html?file=' + encodeURIComponent(name));
    };
    
    profilesCtrl.loadProfiles();
});