'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    //TODO: move baseUrl to a config
    var baseUrl = 'http://localhost:62357';
    var authService = {};

    //Login

    //Logout


    authService.baseUrl = baseUrl;
    authService.activeTab = 'login';

    return authService;
}]);