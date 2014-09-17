app.controller('roadTripAddExpenseController', [
    '$scope', '$routeParams', '$location', 'expenseService', function($scope, $routeParams, $location, expenseService) {
        $scope.expenses = {};
        $scope.expenses.tripId = $routeParams.roadTripId;
        $scope.expenses.notes = "";
        $scope.expenses.dollarAmount = 0;
        $scope.expenses.expenseDate = new Date();
        $scope.saveExpense = function() {
            expenseService.addExpense($scope.expenses).then(function(data) {
                $location.path('/expenses/' + data.roadTripId);
            }, function(e) {

            });
        }
    }
]);