angular.module('app', []).controller('ProfilesController', function ($window, $http) {
    var profilesCtrl = this;

    profilesCtrl.loadProfiles = function () {
        $http.get("/api/profiles").then(
            function (response) {
                profilesCtrl.profiles = response.data.Result.ProfileNames
                    .map(function(n) { return {name: n, newName: n}; });
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

    profilesCtrl.rename = function (oldName, newName) {
        var data = { oldName: oldName, newName: newName };
        $http.post("/api/rename", data).then(
            function (response) {
                profilesCtrl.loadProfiles();
            },
            function (error) { console.log(error); });
    };

    profilesCtrl.deleteProfile = function (name) {
        var data = { name: name };
        $http.post("/api/delete", data).then(
            function (response) {
                profilesCtrl.loadProfiles();
            },
            function (error) { console.log(error); });
    };

    profilesCtrl.editProfile = function (name) {
        $window.location.href = ('/edit.html?file=' + encodeURIComponent(name));
    };
    
    profilesCtrl.loadProfiles();
});