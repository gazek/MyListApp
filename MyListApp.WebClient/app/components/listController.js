'use strict';
function listController($http, $scope, confirmActionService) {

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
        console.log('edit');
        this.listNameCopy = this.list.name;
        this.listNameEditable = true;
    };

    this.editListNameSubmit = function () {
        console.log('edit submit list controller');
        this.listNameEditable = false;
        this.updateList({ index: $scope.index });
    }

    this.editListNameCancel = function () {
        console.log('edit cancel');
        this.list.name = this.listNameCopy;
        this.listNameEditable = false;
    };

    $scope.sortingLog = [];
    $scope.sortableOptions = {
        handle: '.listItemSortHandle',
        helper: function (e, ui) {
            ui.children().each(function () {
                $(this).width($(this).width());
            });
            return ui;
        },
        update: function (e, ui) {
            var logEntry =  $scope.list.items.map(function (i) {
                return i.id;
            }).join(', ');
            console.log("Update: " + logEntry);
            $scope.sortingLog.push('Update: ' + logEntry);
        }
    };

}