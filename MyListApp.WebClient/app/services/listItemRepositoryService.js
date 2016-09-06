app.factory('listItemRepositoryService', ['$http', function ($http) {

    // TODO: put this in a config
    var _baseUrl = 'http://localhost:62357/';

    var listItemRepositoryService = {};

    var _create = function (itemObj) {
        return $http({
            method: 'post',
            url: _baseUrl + 'api/items',
            headers: { 'Content-Type': 'application/json' },
            data: itemObj
        }).then(function (response) {
            return response.data;
        },
        _errorResponse);
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

    listItemRepositoryService.create = _create;

    return listItemRepositoryService;
}]);