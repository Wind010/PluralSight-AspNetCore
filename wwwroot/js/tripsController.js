﻿// tripsController.js


(function () {
    
    "use strict";

    // Get the existing module
    angular.module("app-trips")
        .controller("tripsController", tripsController);

    function tripsController($scope) {

        var vm = this;

        vm.trips = [{
            name: "US Trip",
            created: new Date()
        }, {
            name: "World Trip",
            created: new Date()
        }];

        vm.newTrip = {};

        vm.addTrip = function () {
            vm.trips.push({ name: vm.newTrip.name, created: new Date() });
            vm.newTrip = {};
        };

    }

})();