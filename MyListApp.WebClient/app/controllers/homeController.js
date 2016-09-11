'use strict';
app.controller('homeController', ['$scope', 'listRepositoryService', 'listItemRepositoryService', function ($scope, listRepositoryService, listItemRepositoryService) {

    $scope.searchString = '';

    $scope.onSearchStringUpdate = function (newstring) {
        $scope.searchString = newstring;
    };

    $scope.retrieveAll = function () {
        listRepositoryService.retrieveAll().then(function (response) {
            $scope.lists = response.data;
            $scope.numCols = 4;
            $scope.colClass = "col-sm-" + Math.floor(12/$scope.numCols);
            $scope.listCols = [];
            // create columns
            for (var i = 0; i < $scope.numCols; i++) {
                $scope.listCols[i] = [];
            }
            // add lists to columns
            for (var l in $scope.lists) {
                var mod = $scope.lists[l].position % $scope.numCols;
                var list = $scope.lists[l];
                // store data to make it easier to find a listObjS
                // regardless of what identifying info we have
                // TODO: set up a watcher for these
                list.index = l;
                list.listCol = mod;
                list.listColIndex = $scope.listCols[mod].length;
                $scope.listCols[mod].push(list);
            }

            // create an array of lists accessible by ID
            // TODO: set up a watcher for these
            $scope.listLookup = {};
            for (var l in $scope.lists) {
                $scope.listLookup[$scope.lists[l].id] = $scope.lists[l];
            }
            // create a list of items accessible by ID
            // TODO: set up a watcher for these
            $scope.listItemLookup = {};
            for (var l in $scope.lists) {
                var completedCount = 0;
                for (var i in $scope.lists[l].items) {
                    var itemObj = $scope.lists[l].items[i];
                    itemObj.index = parseInt(i);
                    $scope.listItemLookup[$scope.lists[l].items[i].id] = itemObj;
                    if (itemObj.isComplete) {
                        completedCount = completedCount + 1;
                    }
                }
                $scope.lists[l].completedItemCount = completedCount;
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
            position: 0,
            items: [],
            sharing: []
        };
        listRepositoryService.create(newList).then(function (response) {
            if (typeof (response) === 'object' && 'id' in response) {
                response.index = $scope.lists.length;
                response.listCol = 0;
                response.listColIndex = 0;
                sortableUpdate(0);
                // add to all arrays
                $scope.listCols[0].unshift(response);
                $scope.listLookup[response.id] = response;
                $scope.lists.push(response)
            } else {
                alert('Failed to create new list.\n' + response);
            }
        });
    };

    $scope.onDeleteList = function (index) {
        var id = $scope.lists[index].id;
        listRepositoryService.delete(id).then(function (response) {
            if (response) {
                var listObj = $scope.lists[index];
                // remove from all arrays
                $scope.listCols[listObj.listCol].splice(listObj.listColIndex, 1);
                $scope.lists.splice(listObj.index, 1);
                delete $scope.listLookup[listObj.id];
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

    $scope.onDeleteItem = function (itemId) {
        listItemRepositoryService.delete(itemId).then(function (response) {
            if (response) {
                var itemObj = $scope.listItemLookup[itemId];
                var parentListObj = $scope.listLookup[itemObj.listId];
                // remove from arrays/objs
                delete $scope.listItemLookup[itemId];
                parentListObj.items.splice(itemObj.index, 1);
                // correct item index property
                for (var i in parentListObj.items) {
                    parentListObj.items[i].index = i;
                }
            } else {
                alert('Failed to delete  item\n' + response);
            }
        });
    };

    function sortableUpdate(index) {
        for (var l in $scope.listCols[index]) {
            var expectedPosition = index + l * $scope.numCols;
            var list = $scope.listCols[index][l];
            list.listCol = index;
            list.listColIndex = l;
            if (list.position != expectedPosition) {
                list.position = parseInt(expectedPosition);
                listRepositoryService.update(list);
            }
        }
    }

    $scope.sortableOptions = {
        connectWith: '.list-panel-group',
        handle: '.listSortHandle',
        opacity: 0.75,
        placeholder: 'listSortablePlaceholder',
        start: function (e, ui) {
            ui.placeholder.height(ui.helper.outerHeight());
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
            // unset column index
            $scope.listSortFrom = -1;
            $scope.listSortTo = -1;
        }
    };

    $scope.itemFilterExpression = function (value, index, array) {
        if ($scope.searchString == '') {
            return true;
        }

        for (var i in value.items || []) {
            var result = value.items[i].name.toLowerCase().indexOf($scope.searchString.toLowerCase()) >= 0;
            if (result) {
                return true;
            }
        }
        return false;
    }

    // grab all lists readble by the user
    $scope.retrieveAll();
    
    
}]);