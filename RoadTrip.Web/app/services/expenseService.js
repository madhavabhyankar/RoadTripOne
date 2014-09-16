app.factory('expenseService', [
    '$http', '$q', 'ngAuthSettings',
        function ($http, $q, ngAuthSettings) {
            var serviceBase = ngAuthSettings.apiServiceBaseUri;
            var _getAllExpenses = function(roadTripId) {
                var deferred = $q.defer();
                $http.get(serviceBase + 'api/Expense/GetExpensesForTrip/' + roadTripId).then(
                    function(data) {

                        deferred.resolve(data.data);
                    }, function(e) {
                        deferred.reject(e);
                    });
                return deferred.promise;
            }

            return {
                getAllExpenses: _getAllExpenses
            };
        }
    ]);