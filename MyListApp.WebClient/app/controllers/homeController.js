'use strict';
app.controller('homeController', ['$scope', '$location', 'homeService', 'authService', function ($scope, $location, homeService, authService) {

    $scope.getAllLists = function () {
        homeService.getAll().then(function (response) {
            $scope.lists = response.data;
        });
    };

    $scope.lists = $scope.getAllLists();

    var _items = [];
    for (var i = 1; i <= 6; i++) {
        _items.push({
            text: 'Item ' + i,
            value: i
        });
    }
    $scope.items = _items;

    // redirect to auth route if not currently authenticated
    if (!authService.authentication.isAuth) {
        authService.logout();
        $location.path('/auth');
    }

    var tmpList = [];

    for (var i = 1; i <= 6; i++) {
        tmpList.push({
            text: 'Item ' + i,
            value: i
        });
    }

    $scope.list = tmpList;


    $scope.sortingLog = [];

    $scope.sortableOptions = {
        helper:function(e, ui) {  
            ui.children().each(function() {  
                $(this).width($(this).width());  
            });  
            return ui;  
        },
        update: function (e, ui) {
            console.log("update");

            var logEntry = tmpList.map(function (i) {
                return i.value;
            }).join(', ');
            $scope.sortingLog.push('Update: ' + logEntry);
        },
        stop: function (e, ui) {
            console.log("stop");

            // this callback has the changed model
            var logEntry = tmpList.map(function (i) {
                return i.value;
            }).join(', ');
            $scope.sortingLog.push('Stop: ' + logEntry);
        }
    };

}]);