/*!
 * WeUI.js v0.0.1 (https://github.com/progrape/weui.js)
 * Copyright 2016
 * Licensed under the MIT license
 */
'use strict';

(function ($) {
    $.weui = {
        version: '0.0.1'
    };
})($);
'use strict';

(function ($) {

    var $dialog = null;

    /**
     *  weui dialog
     * @param {Object} options
     */
    $.weui.dialog = function (options) {
        options = $.extend({
            title: '标题',
            content: '内容',
            className: '',
            buttons: [{
                label: '确定',
                type: 'primary',
                onClick: $.noop
            }]
        }, options);

        var buttons = options.buttons.map(function (button) {
            return '<a href="javascript:;" class="weui_btn_dialog ' + button.type + '">' + button.label + '</a>';
        }).join('\n');
        var html = '<div class="' + options.className + '">\n                <div class="weui_mask"></div>\n                <div class="weui_dialog">\n                    <div class="weui_dialog_hd">\n                        <strong class="weui_dialog_title">\n                            ' + options.title + '\n                        </strong>\n                    </div>\n                    <div class="weui_dialog_bd">\n                        ' + options.content + '\n                    </div>\n                    <div class="weui_dialog_ft">\n                        ' + buttons + '\n                    </div>\n                </div>\n            </div>';
        $dialog = $(html);
        $('body').append($dialog);
        $dialog.on('click', '.weui_btn_dialog', function () {
            var button = options.buttons[$(this).index()];
            var cb = button.onClick || $.noop;
            cb.call();
            $.weui.closeDialog();
        });
    };

    /**
     * close dialog
     */
    $.weui.closeDialog = function () {
        if ($dialog) {
            $dialog.off('click', '.weui_btn_dialog');
            $dialog.remove();
            $dialog = null;
        }
    };
})($);
'use strict';

(function ($) {
    /**
     * alert
     * @param {String} content
     * @param {Object} options
     * @param {Function} yes
     */
    $.weui.alert = function (content, options, yes) {

        var type = typeof options === 'function';
        if (type) {
            yes = options;
        }

        options = $.extend({
            title: '警告',
            content: content || '警告内容',
            className: '',
            buttons: [{
                label: '确定',
                type: 'primary',
                onClick: yes
            }]
        }, type ? {} : options);
        options.className = 'weui_dialog_alert ' + options.className;

        $.weui.dialog(options);
    };
})($);
'use strict';

(function ($) {

    var $topTips = null;
    var timer = null;

    /**
     * show top tips
     * @param {String} content
     * @param {Object|Number} options
     */
    $.weui.topTips = function () {
        var content = arguments.length <= 0 || arguments[0] === undefined ? 'topTips' : arguments[0];
        var options = arguments[1];


        if ($topTips) {
            $topTips.remove();
            timer && clearTimeout(timer);
            $topTips = null;
        }

        if (typeof options === 'number') {
            options = {
                duration: options
            };
        }

        options = $.extend({
            duration: 3000
        }, options);
        var html = '<div class="weui_toptips weui_warn" style="display: block;">' + content + '</div>';
        $topTips = $(html);
        $('body').append($topTips);

        timer = setTimeout(function () {
            $topTips && $topTips.remove();
            $topTips = null;
        }, options.duration);
    };
})($);
'use strict';

(function ($) {

    var $actionSheetWrapper = null;

    /**
     * show actionSheet
     * @param {Array} menus
     * @param {Array} actions
     */
    $.weui.actionSheet = function () {
        var menus = arguments.length <= 0 || arguments[0] === undefined ? [] : arguments[0];
        var actions = arguments.length <= 1 || arguments[1] === undefined ? [{ label: '取消' }] : arguments[1];

        var cells = menus.map(function (item, idx) {
            return '<div class="weui_actionsheet_cell">' + item.label + '</div>';
        }).join('');
        var action = actions.map(function (item, idx) {
            return '<div class="weui_actionsheet_cell">' + item.label + '</div>';
        }).join('');
        var html = '<div>\n            <div class="weui_mask_transition"></div>\n            <div class="weui_actionsheet">\n                <div class="weui_actionsheet_menu">\n                    ' + cells + '\n                </div>\n                <div class="weui_actionsheet_action">\n                    ' + action + '\n                </div>\n            </div>\n        </div>';

        $actionSheetWrapper = $(html);
        $('body').append($actionSheetWrapper);

        // add class
        $actionSheetWrapper.find('.weui_mask_transition').show().addClass('weui_fade_toggle');
        $actionSheetWrapper.find('.weui_actionsheet').addClass('weui_actionsheet_toggle');

        // bind event
        $actionSheetWrapper.on('click', '.weui_actionsheet_menu .weui_actionsheet_cell', function () {
            var item = menus[$(this).index()];
            var cb = item.onClick || $.noop;
            cb.call();
            $.weui.hideActionSheet();
        }).on('click', '.weui_mask_transition', function () {
            $.weui.hideActionSheet();
        }).on('click', '.weui_actionsheet_action .weui_actionsheet_cell', function () {
            var item = actions[$(this).index()];
            var cb = item.onClick || $.noop;
            cb.call();
            $.weui.hideActionSheet();
        });
    };

    $.weui.hideActionSheet = function () {
        if (!$actionSheetWrapper) {
            return;
        }

        var $mask = $actionSheetWrapper.find('.weui_mask_transition');
        var $actionsheet = $actionSheetWrapper.find('.weui_actionsheet');

        $mask.removeClass('weui_fade_toggle');
        $actionsheet.removeClass('weui_actionsheet_toggle');

        $actionsheet.on('transitionend', function () {
            $actionSheetWrapper.remove();
            $actionSheetWrapper = null;
        }).on('webkitTransitionEnd', function () {
            $actionSheetWrapper.remove();
            $actionSheetWrapper = null;
        });
    };
})($);
'use strict';

(function ($) {
    /**
     * confirm
     * @param {String} content
     * @param {String} options
     * @param {Function} yes
     * @param {Function} no
     */
    $.weui.confirm = function (content, options, yes, no) {

        var type = typeof options === 'function';
        if (type) {
            no = yes;
            yes = options;
        }

        options = $.extend({
            title: '确认',
            content: content || '确认内容',
            className: '',
            buttons: [{
                label: '取消',
                type: 'default',
                onClick: no || $.noop
            }, {
                label: '确定',
                type: 'primary',
                onClick: yes || $.noop
            }]
        }, type ? {} : options);
        options.className = 'weui_dialog_confirm ' + options.className;

        $.weui.dialog(options);
    };
})($);
"use strict";

/**
 * Created by bearyan on 2016/2/16.
 */
(function () {
    function _validate($input) {
        var input = $input[0],
            val = $input.val();

        if (input.tagName == "INPUT" || input.tagName == "TEXTAREA") {
            var reg = input.getAttribute("required") || input.getAttribute("pattern") || "";

            if (!$input.val().length) {
                return "empty";
            } else if (reg) {
                return new RegExp(reg).test(val) ? null : "notMatch";
            } else {
                return null;
            }
        } else if (input.getAttribute("type") == "checkbox" || input.getAttribute("type") == "radio") {
            // 没有正则表达式：checkbox/radio要checked
            return input.checked ? null : "empty";
        } else if (val.length) {
            // 有输入值
            return null;
        }

        return "empty";
    }
    function _showErrorMsg(error) {
        if (error) {
            var $dom = error.$dom,
                msg = error.msg,
                tips = $dom.attr(msg + "Tips") || $dom.attr("tips") || $dom.attr("placeholder");
            if (tips) $.weui.topTips(tips);
            $dom.parents(".weui_cell").addClass("weui_cell_warn");
        }
    }

    var oldFnForm = $.fn.form;
    $.fn.form = function () {
        return this.each(function (index, ele) {
            var $form = $(ele);
            $form.find("[required]").on("blur", function () {
                var $this = $(this),
                    errorMsg;
                if ($this.val().length < 1) return; // 当空的时候不校验，以防不断弹出toptips

                errorMsg = _validate($this);
                if (errorMsg) {
                    _showErrorMsg({
                        $dom: $this,
                        msg: errorMsg
                    });
                }
            }).on("focus", function () {
                var $this = $(this);
                $this.parents(".weui_cell").removeClass("weui_cell_warn");
            });
        });
    };
    $.fn.form.noConflict = function () {
        return oldFnForm;
    };

    var oldFnValidate = $.fn.validate;
    $.fn.validate = function (callback) {
        return this.each(function () {
            var $requireds = $(this).find("[required]");
            if (typeof callback != "function") callback = _showErrorMsg;

            for (var i = 0, len = $requireds.length; i < len; ++i) {
                var $dom = $requireds.eq(i),
                    errorMsg = _validate($dom);
                if (errorMsg) {
                    callback({
                        $dom: $dom,
                        msg: errorMsg
                    });
                    break;
                }
            }
            callback(null);
        });
    };
    $.fn.validate.noConflict = function () {
        return oldFnValidate;
    };
})();
'use strict';

(function ($) {
    var $loading = null;

    /**
     * show loading
     * @param {String} content
     */
    $.weui.loading = function () {
        var content = arguments.length <= 0 || arguments[0] === undefined ? 'loading...' : arguments[0];

        var html = '<div class="weui_loading_toast">\n        <div class="weui_mask_transparent"></div>\n        <div class="weui_toast">\n            <div class="weui_loading">\n                <div class="weui_loading_leaf weui_loading_leaf_0"></div>\n                <div class="weui_loading_leaf weui_loading_leaf_1"></div>\n                <div class="weui_loading_leaf weui_loading_leaf_2"></div>\n                <div class="weui_loading_leaf weui_loading_leaf_3"></div>\n                <div class="weui_loading_leaf weui_loading_leaf_4"></div>\n                <div class="weui_loading_leaf weui_loading_leaf_5"></div>\n                <div class="weui_loading_leaf weui_loading_leaf_6"></div>\n                <div class="weui_loading_leaf weui_loading_leaf_7"></div>\n                <div class="weui_loading_leaf weui_loading_leaf_8"></div>\n                <div class="weui_loading_leaf weui_loading_leaf_9"></div>\n                <div class="weui_loading_leaf weui_loading_leaf_10"></div>\n                <div class="weui_loading_leaf weui_loading_leaf_11"></div>\n            </div>\n            <p class="weui_toast_content">' + content + '</p>\n        </div>\n    </div>';
        $loading = $(html);
        $('body').append($loading);
    };

    /**
     * hide loading
     */
    $.weui.hideLoading = function () {
        $loading && $loading.remove();
        $loading = null;
    };
})($);
'use strict';

(function ($) {
    $.fn.progress = function (options) {
        var _this = this;

        options = $.extend({
            value: 0
        }, options);
        if (options.value < 0) {
            options.value = 0;
        }

        if (options.value > 100) {
            options.value = 100;
        }

        var $progress = this.find('.weui_progress_inner_bar');
        if ($progress.length === 0) {
            var opr = typeof options.onClick === 'function' ? '<a href="javascript:;" class="weui_progress_opr">\n                    <i class="weui_icon_cancel"></i>\n                </a>' : '';
            var html = '<div class="weui_progress">\n                <div class="weui_progress_bar">\n                    <div class="weui_progress_inner_bar" style="width: ' + options.value + '%;"></div>\n                </div>\n                ' + opr + '\n            </div>';
            if (typeof options.onClick === 'function') {
                this.on('click', '.weui_progress_opr', function () {
                    options.onClick.call(_this);
                });
            }
            return this.html(html);
        }

        //return $progress.animate({
        //    width: `${options.value}%`
        //}, 100);
        return $progress.width(options.value + '%');
    };
})($);
'use strict';

(function ($) {
    var oldFnTab = $.fn.tab;
    $.fn.tab = function (options) {
        options = $.extend({
            defaultIndex: 0,
            activeClass: 'weui_bar_item_on'
        });
        var $tabbarItems = this.find('.weui_tabbar_item, .weui_navbar_item');
        var $tabBdItems = this.find('.weui_tab_bd_item');

        // iOS 下不能点击 navbar, 先用js设一下z-index, 后面在 css 设置
        this.find('.weui_navbar').css('z-index', 1);

        this.toggle = function (index) {
            var $defaultTabbarItem = $tabbarItems.eq(index);
            $defaultTabbarItem.addClass(options.activeClass).siblings().removeClass(options.activeClass);

            var $defaultTabBdItem = $tabBdItems.eq(index);
            $defaultTabBdItem.show().siblings().hide();
        };
        var self = this;

        this.on('click', '.weui_tabbar_item, .weui_navbar_item', function (e) {
            var index = $(this).index();
            self.toggle(index);
        });

        this.toggle(options.defaultIndex);

        return this;
    };
    $.fn.tab.noConflict = function () {
        return oldFnTab;
    };
})($);
'use strict';

(function ($) {

    /**
     * show toast
     * @param {String} content
     * @param {Object|Number} options
     */
    $.weui.toast = function () {
        var content = arguments.length <= 0 || arguments[0] === undefined ? 'toast' : arguments[0];
        var options = arguments[1];


        if (typeof options === 'number') {
            options = {
                duration: options
            };
        }

        options = $.extend({
            duration: 3000
        }, options);
        var html = '<div>\n            <div class="weui_mask_transparent"></div>\n            <div class="weui_toast">\n                <i class="weui_icon_toast"></i>\n                <p class="weui_toast_content">' + content + '</p>\n            </div>\n        </div>';
        var $toast = $(html);
        $('body').append($toast);

        setTimeout(function () {
            $toast.remove();
            $toast = null;
        }, options.duration);
    };
})($);
'use strict';

(function ($) {
    var oldFnUploader = $.fn.uploader;
    $.fn.uploader = function (options) {
        options = $.extend({
            title: '图片上传',
            maxCount: 4,
            maxWidth: 500,
            onChange: $.noop
        }, options);

        var html = '<div class="weui_uploader">\n                        <div class="weui_uploader_hd weui_cell">\n                            <div class="weui_cell_bd weui_cell_primary">' + options.title + '</div>\n                            <div class="weui_cell_ft">0/' + options.maxCount + '</div>\n                        </div>\n                        <div class="weui_uploader_bd">\n                            <ul class="weui_uploader_files">\n                            </ul>\n                            <div class="weui_uploader_input_wrp">\n                                <input class="weui_uploader_input" type="file" accept="image/jpg,image/jpeg,image/png,image/gif">\n                            </div>\n                        </div>\n                    </div>';
        this.html(html);

        var $uploader = this;
        var $files = this.find('.weui_uploader_files');
        var $file = this.find('.weui_uploader_input');
        var count = 0;
        $file.on('change', function (event) {
            var files = event.target.files;

            if (files.length === 0) {
                return;
            }

            if (count >= options.maxCount) {
                $.weui.alert('最多只能上传' + options.maxCount + '张图片');
                return;
            }

            $.each(files, function (idx, file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = new Image();
                    img.onload = function () {
                        // 不要超出最大宽度
                        var w = Math.min(options.maxWidth, img.width);
                        // 高度按比例计算
                        var h = img.height * (w / img.width);
                        var canvas = document.createElement('canvas');
                        var ctx = canvas.getContext('2d');
                        // 设置 canvas 的宽度和高度
                        canvas.width = w;
                        canvas.height = h;
                        ctx.drawImage(img, 0, 0, w, h);
                        var base64 = canvas.toDataURL('image/png');

                        $files.append('<li class="weui_uploader_file " style="background-image:url(' + base64 + ')"></li>');
                        ++count;
                        $uploader.find('.weui_uploader_hd .weui_cell_ft').text(count + '/' + options.maxCount);

                        options.onChange.call($uploader, {
                            lastModified: file.lastModified,
                            lastModifiedDate: file.lastModifiedDate,
                            name: file.name,
                            size: file.size,
                            type: file.type,
                            data: base64
                        });
                    };

                    img.src = e.target.result;
                };
                reader.readAsDataURL(file);
            });
        });

        this.update = function (msg) {
            var $preview = $files.find('.weui_uploader_file').last();
            $preview.addClass('weui_uploader_status');
            $preview.html('<div class="weui_uploader_status_content">' + msg + '</div>');
        };

        this.success = function () {
            var $preview = $files.find('.weui_uploader_file').last();
            $preview.removeClass('weui_uploader_status');
            $preview.html('');
        };

        this.error = function () {
            var $preview = $files.find('.weui_uploader_file').last();
            $preview.addClass('weui_uploader_status');
            $preview.html('<div class="weui_uploader_status_content"><i class="weui_icon_warn"></i></div>');
        };

        return this;
    };
    $.fn.uploader.noConflict = function () {
        return oldFnUploader;
    };
})($);