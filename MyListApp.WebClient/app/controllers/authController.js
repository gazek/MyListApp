'use strict';
app.controller('authController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    // login form submit error messages
    $scope.loginMessage = "";

    // signup form submit error messages
    $scope.signupMessages = [];

    // model requirements
    $scope.userNameMminlength = 6;

    // login form model
    $scope.loginData = {
        userName: "",
        password: ""
    };

    //signin form model
    $scope.signupData = {
        userName: "",
        emailAddress: "",
        password: "",
        confirmPassword: ""
    };

    $scope.login = function () {
        $scope.loginMessage = "";
        authService.login($scope.loginData).then(function (response) {
            $location.path('/home');
        },
         function (err) {
             $scope.loginMessage = err.error_description;
         });
    };

    $scope.signup = function () {
        $scope.signupMessages = [];
        authService.signup($scope.signupData).then(function (response) {
            $scope.loginData.userName = $scope.signupData.userName;
            $scope.loginData.password = $scope.signupData.password;
            $scope.login()
        },
        function (err) {
            for (var key in err.modelState) {
                for (var i in err.modelState[key]) {
                    $scope.signupMessages.push(err.modelState[key][i]);
                }
            }
        })
    }

    $scope.logout = authService.logout;

}]);