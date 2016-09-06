'use strict';
function navbarController($location, authService) {

    this.logout = function () {
        authService.logout();
        $location.path('/auth');
    };

    this.onNewToDoList = function () {
        this.newToDoList();
    };

    this.onNewToBuyList = function () {
        this.newToBuyList();
    };

    this.authentication = authService.authentication;

}