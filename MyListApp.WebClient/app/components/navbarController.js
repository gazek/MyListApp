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

    this.onSearchStringUpdate = function () {
        this.searchStringUpdate({ newString: this.searchString });
    };

}