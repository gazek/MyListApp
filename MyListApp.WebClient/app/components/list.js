app.component('listComponent', {
    bindings: {
        index: '<',
        list: '<',
        deleteList: '&',
        updateList: '&',
        createItem: '&',
        updateItem: '&'
    },
    templateUrl: 'app/components/list.html',
    controller: listController
});