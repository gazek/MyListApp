'use strict';

app.factory('authInterceptorService', ['$q', '$location', 'localStorageService', function ($q, $location, localStorageService) {

    var authInterceptorService = {};
    
    var _request = function (config) {
        config.headers = config.headers || {};

        var authData = localStorageService.get('authorizationData');

        if (authData) {
            config.headers.Authorization = 'Bearer ' + authData.token;
        }

        return config;
    };

    var _responseError = function (rejection) {
        if (rejection.status === 401) {
            // TODO: add current user as URL params for token expiration auth redirect
            //       the below doesnt work
            var authData = localStorageService.get('authorizationData');
            var urlParams = '';
            if (authData) {
                urlParams = '?username=' + authData.userName;
            }
            localStorageService.remove('authorizationData');
            $location.path('/auth' + urlParams);
        }
        return $q.reject(rejection);
    };

    authInterceptorService.request = _request;
    authInterceptorService.responseError = _responseError;

    return authInterceptorService;

}]);