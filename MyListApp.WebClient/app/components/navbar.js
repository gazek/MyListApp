app.component('navBar', {
    bindings: {
        brand: '@',
        searchStringUpdate: '&',
        newToDoList: '&',
        newToBuyList: '&'
    },
    templateUrl: 'app/components/navbar.html',
    controller: navbarController
});