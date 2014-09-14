'use strict';

app.controller('addNewRoadTripController', [
    '$scope', 'roadTripService', function ($scope, roadTripService) {
        $scope.newRoadTrip = {}

        $scope.AddNewRoadTrip = function() {
            roadTripService.addNewRoadTrip($scope.newRoadTrip);

        }
    }
]);