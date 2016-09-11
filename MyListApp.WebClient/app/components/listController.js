'use strict';
function listController($http, $scope, confirmActionService, $sce) {

    var ctrl = this;
    $scope.deleteList = this.deleteList;
    $scope.deleteItem = this.deleteItem;
    this.listNameEditable = false;
    this.itemEditable = {};
    this.itemCopy = {};

    this.onDeleteList = function () {
        var modalOptions = {
            closeButtonText: 'Cancel',
            actionButtonText: 'Delete list',
            headerText: 'Delete ' + this.list.name + '?',
            bodyText: 'Are you sure you want to delete this list?'
        };

        confirmActionService.showModal({}, modalOptions).then(function (result) {
            $scope.deleteList({ index: ctrl.list.index });
        });
    }

    this.onDeleteCompletedListItems = function () {
        console.log('listController: onDeleteCompletedListItems')
        var names = [];
        $scope.completedItems = [];
        for (var i in this.list.items) {
            if (this.list.items[i].isComplete) {
                $scope.completedItems.push(this.list.items[i].id);
                names.push(this.list.items[i].name);
            }
        }
        if (names.length > 1) {
            var bodyText = 'Are you sure you want to delete these items?<br/><ul><li>' + names.join('</li><li>') + '</ul>';
        } else {
            var bodyText = 'Are you sure you want to delete item "' + names[0]+ '"?';
        }

        var modalOptions = {
            closeButtonText: 'Cancel',
            actionButtonText: 'Delete items',
            headerText: 'Delete all completed items in ' + this.list.name + '?',
            bodyText: $sce.trustAsHtml(bodyText)
        };

        confirmActionService.showModal({}, modalOptions).then(function (result) {
            for (var i in $scope.completedItems) {
                $scope.deleteItem({ itemId: $scope.completedItems[i] });
            }
            $scope.completedItems = [];
        });
    }

    this.editListName = function () {
        this.listNameCopy = this.list.name;
        this.listNameEditable = true;
    };

    this.editListNameSubmit = function () {
        this.listNameEditable = false;
        this.updateList({ id: this.list.id });
    }

    this.editListNameCancel = function () {
        this.list.name = this.listNameCopy;
        this.listNameEditable = false;
    };

    this.addListItem = function () {
        this.createItem({listId: this.list.id});
    };

    this.editListItem = function (itemObj) {
        this.itemEditable[itemObj.id] = true;
        this.itemCopy[itemObj.id] = itemObj;
    };

    this.editListItemSubmit = function (itemObj) {
        this.itemEditable[itemObj.id] = false;
        delete this.itemCopy[itemObj.id];
        this.updateItem({ itemId: itemObj.id });
    }

    this.editListItemCancel = function (itemObj) {
        itemObj.name = this.itemCopy[itemObj.id].name;
        itemObj.url = this.itemCopy[itemObj.id].url;
        itemObj.price = this.itemCopy[itemObj.id].price;
        delete this.itemCopy[itemObj.id];
        this.itemEditable[itemObj.id] = false;
    };

    this.toggleCompletedItems = function () {
        this.list.showCompletedItems = !this.list.showCompletedItems;
        this.updateList({ id: this.list.id });
    }

    this.itemCompleteToggle = function (itemId) {
        if ($scope.$parent.listItemLookup[itemId].isComplete) {
            this.list.completedItemCount = this.list.completedItemCount + 1;
        } else {
            this.list.completedItemCount = this.list.completedItemCount - 1;
        }
        this.updateItem({ itemId: itemId });
    }

    function sortableUpdate() {
        for (var i in ctrl.list.items) {
            if (ctrl.list.items[i].position != i) {
                ctrl.list.items[i].position = parseInt(i);
                ctrl.updateItem({ itemId: ctrl.list.items[i].id });
            }
        }
    }

    $scope.sortableOptions = {
        handle: '.listItemSortHandle',
        helper: function (e, ui) {
            ui.children().each(function () {
                $(this).width($(this).width());
            });
            return ui;
        },
        stop: function (e, ui) {
            sortableUpdate();
        }
    };

}