
var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;
teamrynkebyse.itemsbuilder = (function () {

    var addMoreItemsId = '#addMoreItems';
    var addSpecificItemsId = '#addSpecificItem';
    var itemsContainerId = '#itemsContainer';
    var undoClass = '.undo';
    var itemsTemplateId = 'itemsTemplate';
    var specificItemsTemplateId = 'specificItemsTemplate';
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

    function addSpecificItem() {
        count++;
        var viewData = { index: count };
        var result = $(itemsContainerId)
                        .mustache(specificItemsTemplateId, viewData)
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
            $(addSpecificItemsId).on('click', addSpecificItem);
        }
    };
})();
