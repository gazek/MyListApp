'use strict';
function listController($http, $scope, confirmActionService) {

    var ctrl = this;
    $scope.list = this.list;
    $scope.index = this.index;
    $scope.deleteList = this.deleteList;
    this.listNameEditable = false;

    this.onDeleteList = function () {
        var modalOptions = {
            closeButtonText: 'Cancel',
            actionButtonText: 'Delete list',
            headerText: 'Delete ' + this.list.name + '?',
            bodyText: 'Are you sure you want to delete this list?'
        };

        confirmActionService.showModal({}, modalOptions).then(function (result) {
            $scope.deleteList({ index: $scope.index });
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

    this.toggleCompletedItems = function () {
        this.list.showCompletedItems = !this.list.showCompletedItems;
        this.updateList({ index: $scope.index });
    }

    this.itemCompleteToggle = function (itemId) {
        this.updateItem({ itemId: itemId });
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
            for (var i in $scope.list.items) {
                if ($scope.list.items[i].position != i) {
                    $scope.list.items[i].position = parseInt(i);
                    ctrl.updateItem({ itemId: $scope.list.items[i].id });
                }
            }

        }
    };

}