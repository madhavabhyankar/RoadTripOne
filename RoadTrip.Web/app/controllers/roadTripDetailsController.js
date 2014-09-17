'use strict';
app.controller('roadTripDetailsController', [
    '$scope', '$routeParams', 'roadTripService',
    function ($scope, $routeParams, roadTripService) {
        $scope.routeTripDetails = {};

        roadTripService.getRoadTripDetailById($routeParams.roadTripId).then(
            function(data) {
                $scope.routeTripDetails = data;
            }, function(e) {

            });

    }
]);