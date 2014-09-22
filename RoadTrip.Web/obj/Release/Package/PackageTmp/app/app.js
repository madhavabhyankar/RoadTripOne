var app = angular.module("RoadTripApp", ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'ui.bootstrap']);

app.config(function($routeProvider) {
    $routeProvider.when("/home", {
        controller: 'homeController',
        templateUrl: 'app/views/home.html'
    });
    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/app/views/signup.html"
    });
    $routeProvider.when("/refresh", {
        controller: "refreshController",
        templateUrl: "/app/views/refresh.html"
    });

    $routeProvider.when("/tokens", {
        controller: "tokensManagerController",
        templateUrl: "/app/views/tokens.html"
    });

    $routeProvider.when("/associate", {
        controller: "associateController",
        templateUrl: "/app/views/associate.html"
    });

    $routeProvider.when("/myroadtrips", {
        controller: "myRoadTripsController",
        templateUrl: "/app/views/myroadtrips.html"
    });

    $routeProvider.when("/addnewroadtrip", {
        controller: "addNewRoadTripController",
        templateUrl: "/app/views/addnewroadtrip.html"
    });
    $routeProvider.when("/tripdetails/:roadTripId", {
        controller: "roadTripDetailsController",
        templateUrl: "/app/views/roadTripDetails.html"
    });
    $routeProvider.when("/expenses/:roadTripId", {
        controller: "roadTripExpensesContoller",
        templateUrl: '/app/views/expenses.html'
    });
    $routeProvider.when("/addExpense/:roadTripId", {
        controller: "roadTripAddExpenseController",
        templateUrl: '/app/views/addExpense.html'
    });
    $routeProvider.when("/addUser/:roadTripId", {
        controller: 'addUserController',
        templateUrl: '/app/views/adduser.html'
    });
    $routeProvider.when("/joinRoadTrip", {
        controller: 'joinRoadTripController',
        templateUrl: '/app/views/joinRoadTrip.html'
    });
    $routeProvider.otherwise({ redirectTo: "/home" });

});
//var serviceBase = 'http://localhost:9997/';
var serviceBase = 'http://roadtripplus.azurewebsites.net/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    //clientId: 'ManageMyRoadTripAppLocal'
    clientId: 'ManageMyRoadTripApp'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});