// tripsController.js

(function () {
    
    "use strict";

    // Get the existing module
    angular.module("app-trips")
        .controller("tripsController", tripsController);

    funciton tripsController() {
        
        var vm = this;

        vm.name = "Jeff";

    }

})();