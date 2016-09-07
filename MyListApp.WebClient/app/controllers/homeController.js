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
                console.log('l: ' + l);
                console.log('position: ' + $scope.lists[l].position);
                console.log('mod: ' + $scope.lists[l].position % $scope.numCols);
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

    function sortableUpdate(e) {
        var index = e.target.id.substr(7);
        for (var l in $scope.listCols[index]) {
            var expectedPosition = parseInt(index + l * $scope.numCols);
            console.log('expectedPosition: ' + expectedPosition);
            console.log('position: ' + $scope.listCols[index][l].position);
            if ($scope.listCols[index][l].position != expectedPosition) {
                $scope.listCols[index][l].position = expectedPosition;
                listRepositoryService.update($scope.listCols[index][l]);
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
            console.log('homeController: ui-sortable receive event '+e.target.id);
            sortableUpdate(e);
        },
        remove: function (e, ui) {
            console.log('homeController: ui-sortable remove event ' + e.target.id);
        },
        stop: function (e, ui) {
            console.log('homeController: ui-sortable stop event ' + e.target.id);
            sortableUpdate(e);
        }
    };
    
    // grab all lists readble by the user
    $scope.retrieveAll();
    
    
}]);