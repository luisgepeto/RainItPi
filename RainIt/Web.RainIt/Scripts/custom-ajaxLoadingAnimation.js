(function ($) {

    function getPixelValue(pixelSetting) {
        return parseInt(pixelSetting, 10);
    }

    function getPixelSetting(pixelValue) {
        return pixelValue + "px";
    }

    function getContainerSettings(container) {
        return {
            Width: container.css('width'),
            Height: container.css('height'),
            PaddingLeft: container.css('padding-left'),
            PaddingTop: container.css('padding-top'),
            PaddingBottom: container.css('padding-bottom'),
            PaddingRight : container.css('padding-right')
        };
    }

    $.fn.setAjaxLoadingAnimation = function (displayText) {
        if (displayText == undefined)
            displayText = "";
        var containerSettings = getContainerSettings(this);

        var htmlCoveringDiv = '<div class="k-loading-mask"><span class="k-loading-text">'+displayText+'</span><div class="k-loading-image"></div><div class="k-loading-color"></div></div>';

        
        //please review this, as it will change the container presentation.
        //th.css('position', 'relative');
        this.append(htmlCoveringDiv);

        this.children('.k-loading-mask')
            .css('left', 0)
            .css('top', 0)
            .css('width', containerSettings.Width)
            .css('height', containerSettings.Height)
            .css('text-align', 'center')
            .css('z-index', 999);

        var textSettings = getContainerSettings(this.children().children('.k-loading-text'));
        this.children().children('.k-loading-text')
            .css('text-indent', 0)
            .css('top', getPixelSetting(getPixelValue(containerSettings.Height) / 2.0 - 20))
            .css('margin-top', getPixelSetting(-getPixelValue(textSettings.Height) / 2.0));
        textSettings = getContainerSettings(this.children().children('.k-loading-text'));
        this.children().children('.k-loading-text')
            .css('margin-left', getPixelSetting(-getPixelValue(textSettings.Width) / 2.0))
            .css('font-weight', 'bold');
        this.children().children('.k-loading-color')
            .css('position', "absolute")
            .css('top', 0);
        return this;
    }

    $.fn.removeAjaxLoadingAnimation = function () {
        this.children("div.k-loading-mask").remove();
        return this;
    }

    $.fn.toggleAjaxLoadingAnimation = function () {
        if(this.children("div.k-loading-mask").length)
            this.removeAjaxLoadingAnimation();
        else
            this.setAjaxLoadingAnimation();
        return this;
    }

}(jQuery));