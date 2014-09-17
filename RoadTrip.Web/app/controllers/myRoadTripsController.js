'use strict';

app.controller('myRoadTripsController', [
    '$scope', 'roadTripService', function ($scope, roadTripService) {
        $scope.roadTripsIOwn = [];
        $scope.roadTripsIHaveJoined = [];
        $scope.roadTripsExist = false;
        roadTripService.getAllRoadTripsIOwn().then(function(data) {
            $scope.roadTripsIOwn = data;
            $scope.roadTripsExist = ($scope.roadTripsIOwn.length > 0) || (data.length > 0) || ($scope.roadTripsIHaveJoined.length > 0);
        }, function(e) {

        });

    }
]);