'use strict';
var app = angular.module('MyListApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'ui.bootstrap']);

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "authController",
        templateUrl: "/app/views/auth.html"
    });

    $routeProvider.when("/lists", {
        controller: "listsController",
        templateUrl: "/app/views/lists.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});

app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
}]);

app.run(['authService', function (authService) {
    authService.populateAuthData();
}]);