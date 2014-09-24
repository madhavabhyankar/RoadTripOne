'use strict';
app.controller('roadTripDetailsController', [
    '$scope', '$routeParams', 'roadTripService', 'expenseService',
    function ($scope, $routeParams, roadTripService, expenseService) {
        $scope.routeTripDetails = {};
        $scope.roadTripExpenses = {};
        roadTripService.getRoadTripDetailById($routeParams.roadTripId).then(
            function(data) {
                $scope.routeTripDetails = data;

            }, function(e) {

            });

        expenseService.getAllExpenses($routeParams.roadTripId).then(function(data) {
            $scope.roadTripExpenses.expenses = data;
            $scope.roadTripExpenses.totalExpenses = _.reduce(data, function (memo, item) { return memo + item.dollarAmount; }, 0);
        }, function(e) {

        });

        expenseService.mySharedCost($routeParams.roadTripId).then(function(data) {
            $scope.mySharedCost = data;
        }, function(e) {

        });

        expenseService.completeSharedCost($routeParams.roadTripId).then(function(data) {
            $scope.completeSharedCost = data;
        }, function(e) {

        });

    }
]);