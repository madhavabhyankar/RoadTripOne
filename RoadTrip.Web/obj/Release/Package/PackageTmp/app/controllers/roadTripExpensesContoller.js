app.controller("roadTripExpensesContoller", [
    '$scope', '$routeParams', 'expenseService',
    function ($scope,$routeParams, expenseService) {
        $scope.expenses = [];
        $scope.roadTripid = $routeParams.roadTripId;
        expenseService.getAllExpenses($routeParams.roadTripId).then(function(data) {
            $scope.expenses = data;
        }, function(e) {
            //error
        });
    }
]);