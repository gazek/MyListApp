app.factory('homeService', ['$http', function ($http) {

    var homeService = {};

    var _baseUrl = 'http://localhost:62357/';

    var _getAll = function () {

        return $http({
            method: 'GET',
            url: _baseUrl+'api/lists',
            headers: { 'Content-Type': 'application/json' }
        });
    };

    homeService.getAll = _getAll;

    return homeService;
}]);