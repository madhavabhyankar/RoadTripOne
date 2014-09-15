'use strict';

app.controller('addNewRoadTripController', [
    '$scope', '$location', 'roadTripService', function ($scope, $location, roadTripService) {
        $scope.newRoadTrip = {}
        
        $scope.AddNewRoadTrip = function () {
            
            roadTripService.addNewRoadTrip($scope.newRoadTrip);
            $location.path('/myroadtrips');
        }

        function generatePassword() {
            var length = 8,
                charset = "abcdefghijklnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                retVal = "";
            for (var i = 0, n = charset.length; i < length; ++i) {
                retVal += charset.charAt(Math.floor(Math.random() * n));
            }
            return retVal;
        }
        var getHasCodeForTrip = function () {
            var seed = new Date().toString() + generatePassword();
            var hash = 0, i, chr, len;
            if (seed.length == 0) return hash;
            for (i = 0, len = seed.length; i < len; i++) {
                chr = this.charCodeAt(i);
                hash = ((hash << 5) - hash) + chr;
                hash |= 0; // Convert to 32bit integer
            }
            return hash;
        };
        
        alert(getHasCodeForTrip());
    }
]);