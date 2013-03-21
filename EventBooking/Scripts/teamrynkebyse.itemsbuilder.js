
var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;
teamrynkebyse.itemsbuilder = (function () {

    var addMoreItemsId = '#addMoreItems';
    var itemsContainerId = '#itemsContainer';
    var undoClass = '.undo';
    var itemsTemplateId = 'itemsTemplate';
    var count = 0;
    
    function addItem() {
        count++;
        var viewData = { index: count };
        var result = $(itemsContainerId)
                        .mustache(itemsTemplateId, viewData)
                        .children()
                        .last();

        result.find(undoClass).on('click', function () {
            result.remove();
            count--;
        });
    }

    return {
        init: function () {
            $(addMoreItemsId).on('click', addItem);
        }
    };
})();
