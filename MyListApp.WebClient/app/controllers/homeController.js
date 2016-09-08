'use strict';
app.controller('homeController', ['$scope', 'listRepositoryService', 'listItemRepositoryService', function ($scope, listRepositoryService, listItemRepositoryService) {

    $scope.retrieveAll = function () {
        listRepositoryService.retrieveAll().then(function (response) {
            $scope.lists = response.data;
            $scope.numCols = 4;
            $scope.colClass = "col-sm-" + Math.floor(12/$scope.numCols);
            $scope.listCols = [];
            for (var i = 0; i < $scope.numCols; i++) {
                $scope.listCols[i] = [];
            }
            for (var l in $scope.lists) {
                $scope.listCols[$scope.lists[l].position % $scope.numCols].push($scope.lists[l]);
            }

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
            ownerId: 'fake', // will be assigned by API based on token
            name: 'New List' + listNum,
            type: type,
            items: [],
            sharing: []
        };
        listRepositoryService.create(newList).then(function (response) {
            if (typeof(response) === 'object' && 'id' in response) {
                $scope.listCols[0].unshift(response);
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

    $scope.onUpdateList = function (id) {
        var list = $scope.listLookup[id];
        listRepositoryService.update(list).then(function (response) {
            if (!response) {
                alert('Failed to update list\n' + response);
            }
        });
    };

    $scope.onCreateItem = function (listId) {
        var newItem = {
            ListId: listId,
            CreatorId: 'fake', // will be assigned by API based on token
            Name: 'New item',
            position: $scope.listLookup[listId].items.lenth
        };
        
        listItemRepositoryService.create(newItem).then(function (response) {
            if (typeof (response) === 'object' && 'id' in response) {
                $scope.listLookup[response.listId].items.push(response);
                $scope.listItemLookup[response.id]= response;
            } else {
                alert('Failed to create new list.\n' + response);
            }
        })
    };

    $scope.onUpdateItem = function (itemId) {
        var item = $scope.listItemLookup[itemId];
        listItemRepositoryService.update(item);
    }

    function sortableUpdate(index) {
        for (var l in $scope.listCols[index]) {
            //var expectedPosition = parseInt(index) + (parseInt(l) * parseInt($scope.numCols));
            var expectedPosition = index + l * $scope.numCols;
            var list = $scope.listCols[index][l];
            if (list.position != expectedPosition) {
                list.position = parseInt(expectedPosition);
                listRepositoryService.update(list);
            }
        }
    }

    $scope.sortableOptions = {
        connectWith: '.list-group',
        handle: '.listSortHandle',
        helper: function (e, ui) {
            ui.children().each(function () {
                $(this).width($(this).width());
            });
            return ui;
        },
        receive: function (e, ui) {
            $scope.listSortTo = parseInt(e.target.id.substr(7));
            sortableUpdate(e);
        },
        stop: function (e, ui) {
            $scope.listSortFrom = parseInt(e.target.id.substr(7));
            var colIndex = [$scope.listSortTo, $scope.listSortFrom];
            for (var i in colIndex) {
                if (colIndex[i] >= 0) {
                    sortableUpdate(colIndex[i]);
                }
            }
            // un set column index
            $scope.listSortFrom = -1;
            $scope.listSortTo = -1;
        }
    };
    
    // grab all lists readble by the user
    $scope.retrieveAll();
    
    
}]);