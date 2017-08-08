angular.module('SimpleAjaxLoaderModule', [])
    .directive('simpleAjaxLoader', ['$http', '$rootScope', function ($http, $rootScope) { //use: <div simple-ajax-loader>...</div>
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                scope.isLoading = function () {
                    if (scope.showLoader.value)
                        return scope.showLoader.value;
                    else
                        scope.isLoaderVisible.value = $http.pendingRequests.length > 0;
                    return scope.isLoaderVisible.value;
                };
                var focused = null;
                scope.$watch(scope.isLoading, function (v) {
                    if (v) {
                        element.show(); //shows the directive element (ideally a full-screen overaly) until all ajax calls complete
                    } else {
                        element.hide();
                        //$rootScope.$broadcast('ajaxLoadComplete');
                    }
                });
            }
        };

    }]);