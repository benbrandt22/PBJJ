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
    
    profilesCtrl.loadProfiles();
});