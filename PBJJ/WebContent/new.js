angular.module('app', []).controller('NewProfileController', function ($window, $http) {
    var newCtrl = this;

    newCtrl.type = 'Standard';
    newCtrl.name = '';

    newCtrl.okToSubmit = function () {

        if (newCtrl.type === 'Standard') {
            var a = (newCtrl.fingerWidth > 0);
            var b = (newCtrl.overallWidth > 0);
            return (a && b);
        }

        if (newCtrl.type === 'Custom') {
            return (newCtrl.name.length > 0);
        }

        return false;
    };

    newCtrl.submitNew = function () {
        if (newCtrl.okToSubmit() === false) {
            return;
        }
        var data = {
            type: newCtrl.type,
            name: newCtrl.name,
            fingerWidth: newCtrl.fingerWidth,
            overallWidth: newCtrl.overallWidth,
        };
        $http.post("/api/createNew", data).then(
            function (response) {
                var redirectUrl = '/profiles.html';
                if (newCtrl.type === 'Custom') {
                    redirectUrl = ('/edit.html?file=' + encodeURIComponent(data.name));
                }
                $window.location.href = redirectUrl;
            },
            function (error) { console.log(error); });
    }

});