app.controller('roadTripAddExpenseController', [
    '$scope', '$routeParams', '$location', 'expenseService', function($scope, $routeParams, $location, expenseService) {
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.opened = true;
        };

        $scope.expenses = {};
        $scope.expenses.tripId = $routeParams.roadTripId;
        $scope.expenses.expenseDate = new Date();
        $scope.saveExpense = function() {
            expenseService.addExpense($scope.expenses).then(function(data) {
                $location.path('/tripdetails/' + data.roadTripId);
            }, function(e) {

            });
        }
    }
]);