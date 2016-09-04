'use strict';
function listController($http, $scope) {

    
    $scope.list = this.list;

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