app.factory('listRepositoryService', ['$http', function ($http) {

    var self = this;

    var listRepositoryService = {};

    var _baseUrl = 'http://localhost:62357/';

    var _retrieveAll = function () {
        return $http({
            method: 'GET',
            url: _baseUrl+'api/lists',
            headers: { 'Content-Type': 'application/json' }
        }).then(function (response) {
            return response;
        })
    };

    var _create = function (listObj) {
        return $http({
            method: 'post',
            url: _baseUrl + 'api/lists',
            headers: { 'Content-Type': 'application/json' },
            data: listObj
        }).then(function (response) {
            return response.data;
        },
        _errorResponse
        );
    };

    var _delete = function (id) {
        return $http({
            method: 'delete',
            url: _baseUrl + 'api/lists/' + id,
            headers: { 'Content-Type': 'application/json' },
        }).then(function (response) {
            return true;
        },
        function (response) {
            return response.message;
        });
    };

    var _update = function (listObj) {
        console.log('list update repo');
        return $http({
            method: 'put',
            url: _baseUrl + 'api/lists/' + listObj.id,
            headers: { 'Content-Type': 'application/json' },
            data: listObj
        }).then(function (response) {
            return true;
        },
        _errorResponse
        );
    };

    // concat error messages
    var _errorResponse = function (response) {
        if ('modelState' in response.data) {
            var messages = [];
            for (var item in response.data.modelState) {
                for (var m in response.data.modelState[item]) {
                    messages.push(response.data.modelState[item][m]);
                }
            }
            return messages.join('\n');
        }

        return response.message;
    }

    listRepositoryService.retrieveAll = _retrieveAll;
    listRepositoryService.create = _create;
    listRepositoryService.update = _update;
    listRepositoryService.delete = _delete;

    return listRepositoryService;
}]);