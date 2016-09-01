'use strict';
var app = angular.module('MyListApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'ui.bootstrap', 'ngAnimate']);

app.config(function ($routeProvider) {

    $routeProvider.when("/auth", {
        controller: "authController",
        templateUrl: "/app/views/auth.html"
    });

    $routeProvider.when("/lists", {
        controller: "listsController",
        templateUrl: "/app/views/lists.html"
    });

    $routeProvider.otherwise({ redirectTo: "/auth" });
});

app.config(function (localStorageServiceProvider) {
    localStorageServiceProvider
      .setStorageType('sessionStorage');
});

app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
}]);

app.run(['authService', function (authService) {
    authService.populateAuthData();
}]);