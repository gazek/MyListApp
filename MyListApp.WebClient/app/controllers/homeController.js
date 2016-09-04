'use strict';
app.controller('homeController', ['$scope', 'homeService', function ($scope, homeService) {

    $scope.getAllLists = function () {
        homeService.getAll().then(function (response) {
            $scope.lists = response.data;
        });
    };

    $scope.lists = $scope.getAllLists(); 

}]);