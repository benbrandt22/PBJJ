angular.module('app', []).controller('NewProfileController', function ($window, $http) {
    var newCtrl = this;

    newCtrl.type = "Standard";

    newCtrl.okToSubmit = function () {

        if (newCtrl.type === 'Standard') {
            var a = (newCtrl.fingerWidth > 0);
            var b = (newCtrl.overallWidth > 0);
            return (a && b);
        }

        if (newCtrl.type === 'Custom') {
            return false;
        }

        return false;
    };

    newCtrl.submitNew = function () {
        if (newCtrl.okToSubmit() === false) {
            return;
        }
        var data = {
            type: newCtrl.type,
            fingerWidth: newCtrl.fingerWidth,
            overallWidth: newCtrl.overallWidth,
        };
        $http.post("/api/createNew", data).then(
            function (response) {
                $window.location.href = '/profiles.html';
            },
            function (error) { console.log(error); });
    }

});