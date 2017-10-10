var playerStatusShare = function ()
{
    if ($('#m_chart_player_status_share').length == 0)
    {
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

    chart.on('draw', function (data)
    {
        if (data.type === 'slice')
        {
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
            if (data.index !== 0)
            {
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
    chart.on('created', function ()
    {
        if (window.__anim21278907124)
        {
            clearTimeout(window.__anim21278907124);
            window.__anim21278907124 = null;
        }
        window.__anim21278907124 = setTimeout(chart.update.bind(chart), 15000);
    });
}

var initGroupTree = function () {
    $("#groupTree").jstree({
        "core": {
            "themes": {
                "responsive": true
            },
            // so that create works
            "check_callback": true,
            'data': [{
                "text": "Tokyo",
                "children": [{
                    "text": "MacDonald",
                    "state": {
                        "selected": true
                    }
                }, {
                    "text": "Starbucks",
                    "icon": "fa fa-warning m--font-danger"
                }, {
                    "text": "KFC",
                    "icon": "fa fa-folder m--font-success",
                    "state": {
                        "opened": true
                    },
                    "children": [
                        { "text": "Big Kfc", "icon": "fa fa-file m--font-waring" }
                    ]
                },{
                    "text": "Burger King",
                    "icon": "fa fa-folder m--font-danger",
                    "children": [
                        { "text": "Shop 1", "icon": "fa fa-file m--font-waring" },
                        { "text": "Shop 2", "icon": "fa fa-file m--font-success" },
                        { "text": "Shop 3", "icon": "fa fa-file m--font-default" },
                        { "text": "Shop 4", "icon": "fa fa-file m--font-danger" },
                        { "text": "Shop 5", "icon": "fa fa-file m--font-info" }
                    ]
                }]
            }
            ]
        },
        "types": {
            "default": {
                "icon": "fa fa-folder m--font-success"
            },
            "file": {
                "icon": "fa fa-file  m--font-success"
            }
        },
        "state": { "key": "demo2" },
        "plugins": ["dnd", "state", "types"]
    });
}

var DatatableResponsiveColumnsDemo = function () {
    var datatable = $('.m_datatable').mDatatable({
        // datasource definition
        data: {
            type: 'remote',
            source: {
                read: {
                    url: 'http://keenthemes.com/metronic/preview/inc/api/datatables/demos/default.php'
                }
            },
            pageSize: 10,
            saveState: {
                cookie: true,
                webstorage: true
            },
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true
        },

        // layout definition
        layout: {
            theme: 'default', // datatable theme
            class: '', // custom wrapper class
            scroll: false, // enable/disable datatable scroll both horizontal and vertical when needed.
            height: 550, // datatable's body's fixed height
            footer: false // display/hide footer
        },

        // column sorting
        sortable: true,

        // column based filtering
        filterable: false,

        pagination: true,

        // columns definition
        columns: [{
            field: "RecordID",
            title: "#",
            sortable: false, // disable sort for this column
            width: 40,
            textAlign: 'center',
            selector: { class: 'm-checkbox--solid m-checkbox--brand' }
        }, {
            field: "OrderID",
            title: "Order ID",
            filterable: false, // disable or enable filtering
            width: 150
        }, {
            field: "ShipCity",
            title: "Ship City",
            responsive: { visible: 'lg' }
        }, {
            field: "Website",
            title: "Website",
            width: 200,
            responsive: { visible: 'lg' }
        }, {
            field: "Department",
            title: "Department",
            responsive: { visible: 'lg' }
        }, {
            field: "ShipDate",
            title: "Ship Date",
            responsive: { visible: 'lg' }
        }, {
            field: "Actions",
            width: 110,
            title: "Actions",
            sortable: false,
            overflow: 'visible',
            template: function (row) {
                var dropup = (row.getDatatable().getPageSize() - row.getIndex()) <= 4 ? 'dropup' : '';

                return '\
						<div class="dropdown '+ dropup + '">\
							<a href="#" class="btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown">\
                                <i class="la la-ellipsis-h"></i>\
                            </a>\
						  	<div class="dropdown-menu dropdown-menu-right">\
						    	<a class="dropdown-item" href="#"><i class="la la-edit"></i> Edit Details</a>\
						    	<a class="dropdown-item" href="#"><i class="la la-leaf"></i> Update Status</a>\
						    	<a class="dropdown-item" href="#"><i class="la la-print"></i> Generate Report</a>\
						  	</div>\
						</div>\
						<a href="#" class="m-portlet__nav-link btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill" title="Edit details">\
							<i class="la la-edit"></i>\
						</a>\
						<a href="#" class="m-portlet__nav-link btn m-btn m-btn--hover-danger m-btn--icon m-btn--icon-only m-btn--pill" title="Delete">\
							<i class="la la-trash"></i>\
						</a>\
					';
            }
        }]
    });

    var query = datatable.getDataSourceQuery();

    $('#m_form_search').on('keyup', function (e) {
        // shortcode to datatable.getDataSourceParam('query');
        var query = datatable.getDataSourceQuery();
        query.generalSearch = $(this).val().toLowerCase();
        // shortcode to datatable.setDataSourceParam('query', query);
        datatable.setDataSourceQuery(query);
        datatable.load();
    }).val(query.generalSearch);

    $('#m_form_status, #m_form_type').selectpicker();
}

$(document).ready(function ()
{
    playerStatusShare();
    initGroupTree();
    DatatableResponsiveColumnsDemo();
});