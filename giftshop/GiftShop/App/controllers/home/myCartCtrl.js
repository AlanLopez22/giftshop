(function (app) {
    'use strict';
    app.controller('myCartCtrl', myCartCtrl);
    myCartCtrl.$inject = ['$scope', '$rootScope', '$state', 'localStorageService', 'toastr', 'apiService'];

    function myCartCtrl($scope, $rootScope, $state, localStorageService, toastr, apiService) {
        $scope.RemoveProduct = function (orderDetail) {
            apiService.post('/api/order/removeproduct', orderDetail, null, function (response) {
                $rootScope.order = response.data;
                $rootScope.repository.order = $rootScope.order;
                localStorageService.set('repository', $rootScope.repository);
                toastr.success('Product removed from the shopping cart');
            });
        };

        $scope.ConfirmOrder = function () {
            apiService.post('/api/order/confirmorder', $rootScope.order, null, function (response) {
                $rootScope.order = undefined;
                $rootScope.repository.order = $rootScope.order;
                localStorageService.set('repository', $rootScope.repository);
                $rootScope.getOrder();
                toastr.success('The order was confirmed successfully');
                $state.go('main.home');
            }, function (response) {
                if (response.status === 401) {
                    $state.go('main.account');
                }
            });
        };
    }
})(angular.module('GiftShopApp'));