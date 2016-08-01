angular.module('app', []).controller('EditProfileController', function ($window, $http) {
    var editCtrl = this;

    editCtrl.save = function () {
        var data = {
            name: editCtrl.name,
            data: editCtrl.data,
        };
        $http.post("/api/save", data).then(
            function (response) {
                $window.location.href = '/profiles.html';
            },
            function (error) { console.log(error); });
    }

    editCtrl.loadData = function () {
        var name = decodeURIComponent($window.location.search.replace("?file=", ""));
        $http.get("/api/getProfileData/" + encodeURIComponent(name)).then(
            function (response) {
                editCtrl.name = response.data.Result.FileName;
                editCtrl.data = response.data.Result.FileData;
            },
            function (error) { console.log(error); });
    }

    editCtrl.loadData();
});