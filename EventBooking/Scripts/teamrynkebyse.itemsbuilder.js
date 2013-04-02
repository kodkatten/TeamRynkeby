
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
        var viewData = { index: count };
        var result = $(itemsContainerId)
                        .mustache(itemsTemplateId, viewData)
                        .children()
                        .last();

        if (count > 0) {
            $('#remove_' + (count - 1)).hide();
        }

        count++;
        result.find(undoClass).on('click', function () {
            result.remove();
            count--;
            if (count > 0) {
                $('#remove_' + (count - 1)).show();
            }

        });
    }

    function addSpecificItem() {
        var viewData = { index: count };
        var result = $(itemsContainerId)
                        .mustache(specificItemsTemplateId, viewData)
                        .children()
                        .last();

        
        if (count > 0) {
            $('#remove_' + (count - 1)).hide();
        }

        count++;
        result.find(undoClass).on('click', function () {
            result.remove();
            count--;
            if (count > 0) {
                $('#remove_' + (count - 1)).show();
            }
        });
    }

    return {
        init: function () {
            $(addMoreItemsId).on('click', addItem);
            $(addSpecificItemsId).on('click', addSpecificItem);
        }
    };
})();
