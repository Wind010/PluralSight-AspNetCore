﻿// tripsController.js

(function () {
    
    "use strict";

    // Get the existing module
    angular.module("app-trips")
        .controller("tripsController", tripsController);

    funciton tripsController() {
        
        var vm = this;

        vm.trips = [{
            name: "US Trip",
            created: new Date()
        }, {
            name: "World Trip",
            created: new Date()
        }];

    }

})();