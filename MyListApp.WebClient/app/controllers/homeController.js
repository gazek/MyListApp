'use strict';
app.controller('homeController', ['$scope', '$location', 'homeService', 'authService', function ($scope, $location, homeService, authService) {

    $scope.lists = [];
    $scope.getAllLists = function () {
        homeService.getAll().then(function (response) {
            $scope.lists = response.data;
        });
    };

    // redirect to auth route if not currently authenticated
    if (!authService.authentication.isAuth) {
        authService.logout();
        $location.path('/auth');
    }

}]);