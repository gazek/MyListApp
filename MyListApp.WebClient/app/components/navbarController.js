'use strict';
function navbarController ($location, authService) {

    this.logout = function () {
        authService.logout();
        $location.path('/auth');
    };

    this.authentication = authService.authentication;

}