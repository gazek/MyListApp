'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    // TODO: move baseUrl to a config
    var _baseUrl = 'http://localhost:62357/';
    var authService = {};

    var _authentication = {
        isAuth: false,
        userName: ""
    };

    // Signup
    var _signup = function (signupData) {
        var deferred = $q.defer();

        $http.post(_baseUrl + 'api/account/register', signupData, { headers: { 'Content-Type': 'application/json' } }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    // Login
    var _login = function (loginData) {

        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
        var deferred = $q.defer();

        $http.post(_baseUrl + 'api/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
            localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName });
            _authentication.isAuth = true;
            _authentication.userName = loginData.userName;
            deferred.resolve(response);
        }).error(function (err, status) {
            _logout();
            deferred.reject(err);
        });

        return deferred.promise;
    };

    // Logout
    var _logout = function () {

        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.userName = "";
    };

    // populate auth data
    var _populateAuthData = function () {
        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
        }
    };

    authService.baseUrl = _baseUrl;
    authService.login = _login;
    authService.logout = _logout;
    authService.signup = _signup;
    authService.authentication = _authentication;
    authService.populateAuthData = _populateAuthData;

    return authService;
}]);