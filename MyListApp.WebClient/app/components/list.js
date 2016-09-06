app.component('listComponent', {
    bindings: {
        index: '<',
        list: '<',
        deleteList: '&',
        updateList: '&'
    },
    templateUrl: 'app/components/list.html',
    controller: listController
});