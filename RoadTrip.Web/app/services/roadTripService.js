

app.factory('roadTripService', [
    '$http', '$q', 'ngAuthSettings', function ($http, $q, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var roadTripServiceFactory = {};
        _roadTripsOwnedByMe = [];
        _roadTripsIHaveJoined = [];

        var _getAllRoadTripsIOwn = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Trips/TripsIOwn').then(function (trips) {

                _roadTripsOwnedByMe = trips.data;
                deferred.resolve(trips.data);
            }, function (e) {
                deferred.reject(e);
                //error handling
            });
            //$http.get(serviceBase + 'api/Trips/TripsIHaveJoined').then(function(trips) {
            //    roadTripServiceFactory.roadTripsIHaveJoined = trips;
            //}, function(e) {
            //    //error handling
            //});
            return deferred.promise;

        }
        var _addNewRoadTrip = function (newRoadTirp) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Trips/NewRoadTrip', newRoadTirp).then(
                function(trip) {
                    deferred.resolve(trip);
                },
                function(e) {
                    deferred.reject(e);
                });

            return deferred.promise;
        }
        roadTripServiceFactory.addNewRoadTrip = _addNewRoadTrip;
        roadTripServiceFactory.getAllRoadTripsIOwn = _getAllRoadTripsIOwn;

        return roadTripServiceFactory;
    }
]);