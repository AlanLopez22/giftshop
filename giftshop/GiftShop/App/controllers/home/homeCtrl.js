(function (app) {
    'use strict';
    app.controller('homeCtrl', homeCtrl);
    homeCtrl.$inject = ['$scope', '$rootScope', '$state', 'apiService'];

    function homeCtrl($scope, $rootScope, $state, apiService) {
        $scope.category = undefined;
        $scope.items = [];

        $scope.Search = function () {
            var config = {
                params: {
                    name: $scope.name,
                    categoryID: $scope.category
                }
            };

            apiService.get('/api/product/listactives', config, function (response) {
                $scope.items = response.data;
            });
        };

        $scope.Search();
        loadCategories();

        function loadCategories() {
            apiService.get('/api/category/list/', null, function (response) {
                $scope.categories = response.data;
                $scope.categories.splice(0, 0, { ID: 0, Description: 'ALL' });
                $scope.category = $scope.categories[0].ID;
            });
        }
    }
})(angular.module('GiftShopApp'));