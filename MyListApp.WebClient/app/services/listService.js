app.factory('listService', ['$http', function ($http) {

    var listService = {};

    var _baseUrl = 'http://localhost:62357/';

    var _getAll = function () {

        return $http({
            method: 'GET',
            url: _baseUrl+'api/lists',
            headers: { 'Content-Type': 'application/json' }
        });
    };

    listService.getAll = _getAll;

    return listService;
}]);