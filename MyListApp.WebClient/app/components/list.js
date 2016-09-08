app.component('listComponent', {
    bindings: {
        index: '<',
        list: '<',
        deleteList: '&',
        updateList: '&',
        createItem: '&',
        updateItem: '&',
        deleteItem: '&'
    },
    templateUrl: 'app/components/list.html',
    controller: listController
});