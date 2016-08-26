'use strict';
app.controller('authController', ['$scope', 'authService', function ($scope, authService) {

    $scope.message = "";

    $scope.loginData = {
        userName: "",
        password: ""
    };

    $scope.activeTab = authService.activeTab;

    $scope.isActiveTab = function (tabName) {
        return tabName === authService.activeTab;
    }

    $scope.setActiveTab = function (tabName) {
        console.info(tabName);
        console.info($scope.activeTab);
        authService.activeTab = tabName;
        console.info($scope.activeTab);
    }

}]);