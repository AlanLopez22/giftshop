(function (app) {
    'use strict';
    app.controller('productDetailCtrl', productDetailCtrl);
    productDetailCtrl.$inject = ['$scope', '$rootScope', '$state', '$stateParams', 'localStorageService', 'apiService', 'toastr'];

    function productDetailCtrl($scope, $rootScope, $state, $stateParams, localStorageService, apiService, toastr) {
        $scope.productID = $stateParams.id;
        $scope.quantity = 1;

        $scope.Load = function () {
            var config = {
                params: {
                    id: $scope.productID
                }
            };

            apiService.get('/api/product/getByID', config, function (response) {
                $scope.product = response.data;
                $scope.selectedImage = $scope.product.Images[0];
                $scope.updateAmount(1);
            });
        };
        $scope.SelectImage = function (image) {
            $scope.selectedImage = image;
        };
        $scope.updateAmount = function (quantity) {
            if (!quantity || quantity.length == 0) {
                $scope.amount = 0;
                return;
            }

            $scope.amount = $scope.product.Price * parseInt(quantity);
        };
        $scope.AddToCart = function () {
            var orderDetail = {
                ProductID: $scope.product.ID,
                Product: $scope.product,
                Quantity: $scope.quantity,
                OrderID: $rootScope.order.ID
            };

            apiService.post('/api/order/addproduct', orderDetail, null, function (response) {
                $rootScope.order = response.data;
                $rootScope.repository.order = $rootScope.order;
                localStorageService.set('repository', $rootScope.repository);
                toastr.success('Product added to the shopping cart');
                $state.go('main.home');
            });
        };
    }
})(angular.module('GiftShopApp'));