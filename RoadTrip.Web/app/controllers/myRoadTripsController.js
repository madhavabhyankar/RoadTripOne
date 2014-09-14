'use strict';

app.controller('myRoadTripsController', [
    '$scope', 'roadTripService', function ($scope, roadTripService) {
        $scope.roadTripsIOwn = [];
        $scope.roadTripsIHaveJoined = [];

        roadTripService.getAllRoadTripsIOwn().then(function(data) {
            $scope.roadTripsIOwn = data;
        }, function(e) {

        });

    }
]);