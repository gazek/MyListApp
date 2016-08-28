'use strict';
app.factory('authViewService', function () {
    // need to store view model properties here rather than the controller
    // because switching tabs in the bootstrap nav-tabs seems to trigger a
    // page reload which causes the controller to be destroyed and replaced
    // with a new instance

    var authViewService = {};

    // modelState errors from API call
    //var _message = '';

    // sets .active on nav-tab
    var _activeTab = 'login';

    var _viewData = {
        activeTab: 'login',
        message: ''
    }

    // login form
    var _loginData = {
        userName: "",
        password: ""
    };
    
    //signin form
    var _signupData = {
        userName: "",
        emailAddress: "",
        password: "",
        confirmPassword: ""
    };

    var _getActiveTab = function () {
        return _viewData.activeTab;
    };

    // set active tab on nav-tab
    var _setActiveTab = function (tabName) {
        _viewData.activeTab = tabName;
    };

    // active tab name predicate
    var _isActiveTab = function (tabName) {
        return _viewData.activeTab === tabName;
    }

    // build object
    //authViewService.message = _message;
    authViewService.activeTab = _activeTab;
    authViewService.loginData = _loginData;
    authViewService.SignupData = _loginData;
    authViewService.setActiveTab = _setActiveTab;
    authViewService.getActiveTab = _getActiveTab;
    authViewService.isActiveTab = _isActiveTab;
    authViewService.viewData = _viewData;

    return authViewService;

});