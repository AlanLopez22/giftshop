(function (app) {
    'use strict';

    app.directive("loadingSpinner", overlaySpinner);

    overlaySpinner.$inject = ["$animate"];

    function overlaySpinner($animate) {
        return {
            templateUrl: "/App/views/templates/loading-spinner.html",
            scope: { active: "=" },
            transclude: true,
            restrict: "E",
            link: link
        };

        function link(scope, element) {

            scope.$watch("active", statusWatcher);

            function statusWatcher(active) {
                $animate[active ? "addClass" : "removeClass"](element, "loading-spinner-active");
            }
        }
    }

})(angular.module('common.core'));