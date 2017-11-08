



var playerStatusShare = function () {
    if ($('#m_chart_player_status_share').length == 0) {
        return;
    }

    var chart = new Chartist.Pie('#m_chart_player_status_share', {
        series: [{
            value: 46,
            className: 'custom',
            meta: {
                color: mUtil.getColor('success')
            }
        },
            {
                value: 4,
                className: 'custom',
                meta: {
                    color: mUtil.getColor('danger')
                }
            },
            {
                value: 2,
                className: 'custom',
                meta: {
                    color: mUtil.getColor('warning')
                }
            },
            {
                value: 15,
                className: 'custom',
                meta: {
                    color: mUtil.getColor('metal')
                }
            }
        ],
        labels: [1, 2, 3]
    }, {
        donut: true,
        donutWidth: 17,
        showLabel: false
    });

    chart.on('draw', function (data) {
        if (data.type === 'slice') {
            // Get the total path length in order to use for dash array animation
            var pathLength = data.element._node.getTotalLength();

            // Set a dasharray that matches the path length as prerequisite to animate dashoffset
            data.element.attr({
                'stroke-dasharray': pathLength + 'px ' + pathLength + 'px'
            });

            // Create animation definition while also assigning an ID to the animation for later sync usage
            var animationDefinition = {
                'stroke-dashoffset': {
                    id: 'anim' + data.index,
                    dur: 1000,
                    from: -pathLength + 'px',
                    to: '0px',
                    easing: Chartist.Svg.Easing.easeOutQuint,
                    // We need to use `fill: 'freeze'` otherwise our animation will fall back to initial (not visible)
                    fill: 'freeze',
                    'stroke': data.meta.color
                }
            };

            // If this was not the first slice, we need to time the animation so that it uses the end sync event of the previous animation
            if (data.index !== 0) {
                animationDefinition['stroke-dashoffset'].begin = 'anim' + (data.index - 1) + '.end';
            }

            // We need to set an initial value before the animation starts as we are not in guided mode which would do that for us

            data.element.attr({
                'stroke-dashoffset': -pathLength + 'px',
                'stroke': data.meta.color
            });

            // We can't use guided mode as the animations need to rely on setting begin manually
            // See http://gionkunz.github.io/chartist-js/api-documentation.html#chartistsvg-function-animate
            data.element.animate(animationDefinition, false);
        }
    });

    // For the sake of the example we update the chart every time it's created with a delay of 8 seconds
    chart.on('created', function () {
        if (window.__anim21278907124) {
            clearTimeout(window.__anim21278907124);
            window.__anim21278907124 = null;
        }
        window.__anim21278907124 = setTimeout(chart.update.bind(chart), 15000);
    });
}
var changeTab = function (tabDivId) {
    $(".mainPageTabDiv").hide();
    $("#" + tabDivId).show();
    $("#m_ver_menu").find("li.m-menu__item").removeClass("m-menu__item--active");
    $(event.currentTarget).parent("li").addClass("m-menu__item--active");
}

var initTimeOptionsInPlayerEdit = function () {
    $(".player-editor-timeoptions-hidden-input").each(function () {
        $(this).ionRangeSlider({
            type: "double",
            min: 0,
            max: 24,
            from: 0,
            to: 24,
            postfix: " o'clock",
            decorate_both: true,
            grid: true,
        });
    })
}

var initSlideEffectDropdown = function () {
    $('#m_select2_3').select2({
        placeholder: "Select dildeshow effects"
    });
}
var EnableTouchSpin = function () {
    $('#m_touchspin_1,#m_touchspin_4').TouchSpin({
        buttondown_class: 'btn btn-secondary',
        buttonup_class: 'btn btn-secondary',
        verticalbuttons: true,
        verticalupclass: 'la la-angle-up',
        verticaldownclass: 'la la-angle-down',
        min: 0
    });
    $('#m_touchspin_2, #m_touchspin_3').TouchSpin({
        buttondown_class: 'btn btn-secondary',
        buttonup_class: 'btn btn-secondary',
        verticalbuttons: true,
        verticalupclass: 'la la-angle-up',
        verticaldownclass: 'la la-angle-down',
        min: 0,
        max: 60
    });
}
$(document).ready(function ()
{
    playerStatusShare();
    $.localize({});
    $.insmGroup({});
    //$("#span_success").text(100);
    var localizetext = $(".intros");
    $.each(localizetext, function (index, obj) {
        obj.innerHTML = $.localize('translate', $.trim(obj.innerHTML));
    });
    var buttontext = $(".labeltext");
    //$.each(buttontext, function (index, obj) {
    //    obj.html($.localize('translate', $.trim(obj.innerText)));
    //});
    initTimeOptionsInPlayerEdit();
    
    $("#monday_value").data("ionRangeSlider").update({ from: 10, to: 22 });
    initSlideEffectDropdown();
    EnableTouchSpin();
});