app.component('listComponent', {
    bindings: {
        index: '<',
        list: '<',
        deleteList: '&',
        updateList: '&',
        createItem: '&'
    },
    templateUrl: 'app/components/list.html',
    controller: listController
});