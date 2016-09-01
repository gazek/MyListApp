'use strict';
app.controller('listsController', ['$scope', '$location', 'listService', 'authService', function ($scope, $location, listService, authService) {

    $scope.lists = [];
    $scope.getAllLists = function () {
        listService.getAll().then(function (response) {
            $scope.lists = response.data;
        });
    };
    $scope.tests = [{
        foo: 'value1',
        bar: 'value1'
    },
    {
        foo: 'value3',
        bar: 'value4'
    }];

    if (!authService.authentication.isAuth) {
        authService.logout();
        $location.path('/auth');
    }

}]);