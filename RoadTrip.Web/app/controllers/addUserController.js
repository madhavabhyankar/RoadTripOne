app.controller("addUserController", ['$scope', '$routeParams', '$location', 'roadTripService',
    function($scope, $routeParams, $location, roadTripService) {
        $scope.newUser = {};
        $scope.isError = false;
        $scope.error = "";
        $scope.newUser.roadTripId = $routeParams.roadTripId;

        $scope.addUser = function() {
            roadTripService.addUserToRoadTrip($scope.newUser).then(
                function(data) {
                    $location.path('#/roadTripDetails/' + data);
                }, function(e) {
                $scope.error = e;
                $scope.isError = true;
            });
        }
    }
])