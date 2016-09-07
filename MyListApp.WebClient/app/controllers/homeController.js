'use strict';
app.controller('homeController', ['$scope', 'listRepositoryService', 'listItemRepositoryService', function ($scope, listRepositoryService, listItemRepositoryService) {

    $scope.retrieveAll = function () {
        listRepositoryService.retrieveAll().then(function (response) {
            $scope.lists = response.data;
            // create an array of lists accessible by ID
            $scope.listLookup = {};
            for (var l in $scope.lists) {
                $scope.listLookup[$scope.lists[l].id] = $scope.lists[l];
            }
            // create a list of items accessible by ID
            $scope.listItemLookup = {};
            for (var l in $scope.lists) {
                for (var i in $scope.lists[l].items) {
                    $scope.listItemLookup[$scope.lists[l].items[i].id] = $scope.lists[l].items[i];
                }
            }
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
        var newList = {
            ownerId: 'fake', // API will supply value
            name: 'New List' + listNum,
            type: type,
            items: [],
            sharing: []
        };
        listRepositoryService.create(newList).then(function (response) {
            if (typeof(response) === 'object' && 'id' in response) {
                $scope.lists.push(response);
                $scope.listLookup[response.id] = response;
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
        listRepositoryService.update(list).then(function (response) {
            if (!response) {
                alert('Failed to update list\n' + response);
            }
        });
    };

    $scope.onCreateItem = function (listId) {
        var newItem = {
            ListId: listId,
            CreatorId: 'fake', // will be assigned by API
            Name: 'New item',
            position: $scope.listLookup[listId].items.lenth
        };
        
        listItemRepositoryService.create(newItem).then(function (response) {
            if (typeof (response) === 'object' && 'id' in response) {
                $scope.listLookup[response.listId].items.push(response);
            } else {
                alert('Failed to create new list.\n' + response);
            }
        })
    };

    $scope.onUpdateItem = function (itemId) {
        var item = $scope.listItemLookup[itemId];
        listItemRepositoryService.update(item);
    }
    
    // grab all lists readble by the user
    $scope.retrieveAll();
    
    
}]);