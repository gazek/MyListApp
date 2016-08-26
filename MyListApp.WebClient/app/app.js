var app = angular.module('MyListApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar']);

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "authController",
        templateUrl: "/app/views/auth.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});