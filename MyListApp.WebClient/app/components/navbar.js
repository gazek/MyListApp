app.component('navBar', {
    bindings: {
        brand: '@',
        newToDoList: '&',
        newToBuyList: '&'
    },
    templateUrl: 'app/components/navbar.html',
    controller: navbarController
});