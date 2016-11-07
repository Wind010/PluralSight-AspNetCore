// simpleControls.js

(function () {
    "use strict";

    angular.module("simpleControls", [])
        .directive("waitCursor", waitCursor);

    function waitCursor() {
        return {
            scope: {
                show: "=displayWhen"
            },
            // Only Elements
            restrict: "E", 
            templateUrl: "/views/waitCursor.html"
        };
    }

})();