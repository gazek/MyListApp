'use strict';
app.controller('listsController', ['$scope', '$location', 'listService', function ($scope, $location, listService) {

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

}]);