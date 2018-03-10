(function (app) {
    'use strict';
    app.controller('homeCtrl', homeCtrl);
    homeCtrl.$inject = ['$scope', '$rootScope', '$state', 'apiService', 'toastr'];

    function homeCtrl($scope, $rootScope, $state, apiService, toastr) {
        
    }
})(angular.module('GiftShopApp'));