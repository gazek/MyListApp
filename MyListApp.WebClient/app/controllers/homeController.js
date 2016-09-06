'use strict';
app.controller('homeController', ['$scope', 'listRepositoryService', 'listItemRepositoryService', function ($scope, listRepositoryService, listItemRepositoryService) {

    $scope.retrieveAll = function () {
        listRepositoryService.retrieveAll().then(function (response) {
            $scope.lists = response.data;
        }, function (response) {
            if (response.status !== 401) {
                $scope.lists = [];
                alert('Failed to retrieve lists');
            }
        });
    };

    $scope.onCreateToDoList = function () {
        $scope.onCreateList(0);
    }

    $scope.onCreateToBuyList = function () {
        $scope.onCreateList(1);
    }

    $scope.onCreateList = function (type) {
        var listNum = $scope.lists.length+1;
        var postList = {
            ownerId: 'fake', // API will supply value
            name: 'New List' + listNum,
            type: type,
            items: [],
            sharing: []
        };
        listRepositoryService.create(postList).then(function (response) {
            if (typeof(response) === 'object' && 'id' in response) {
                $scope.lists.push(response);
            } else {
                alert('Failed to create new list.\n' + response);
            }
        });
    };

    $scope.onDeleteList = function (index) {
        var id = $scope.lists[index].id;
        listRepositoryService.delete(id).then(function (response) {
            if (response) {
                $scope.lists.splice(index, 1);
            } else {
                alert('Failed to delete list\n'+response);
            }
        });
    };

    $scope.onUpdateList = function (index) {
        var list = $scope.lists[index];
        console.log('list update home controller');
        console.log(list);
        listRepositoryService.update(list).then(function (response) {
            if (!response) {
                alert('Failed to delete list\n' + response);
            }
        });
    };

    $scope.lists = $scope.retrieveAll();

}]);