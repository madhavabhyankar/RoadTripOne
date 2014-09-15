'use strict';

app.controller('addNewRoadTripController', [
    '$scope', '$location', 'roadTripService', function ($scope, $location, roadTripService) {
        $scope.newRoadTrip = {}

        $scope.AddNewRoadTrip = function() {
            roadTripService.addNewRoadTrip($scope.newRoadTrip);
            $location.path('/myroadtrips');
        }

        var getHasCodeForTrip = function () {
            var seed = new Date().toString();
            var hash = 0, i, chr, len;
            if (this.length == 0) return hash;
            for (i = 0, len = this.length; i < len; i++) {
                chr = this.charCodeAt(i);
                hash = ((hash << 5) - hash) + chr;
                hash |= 0; // Convert to 32bit integer
            }
            return hash;
        };
    }
]);