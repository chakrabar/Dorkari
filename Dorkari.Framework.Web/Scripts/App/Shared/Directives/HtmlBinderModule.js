angular.module('HtmlBinderModule', [])
    .directive('bindHtmlString', ['$compile', '$parse', function ($compile, $parse) { //use: <div bind-html-simple="viewData" ng-init="funcToPopulateViewData()"></div>
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                //element.addClass('ng-binding').data('$binding', attr.bindNgHtml);
                scope.$watch(attr.bindHtmlSimple, function () {
                    element.html($parse(attr.bindHtmlSimple)(scope));
                    $compile(element.contents())(scope);
                }, true);
            }
        };
    } ]);