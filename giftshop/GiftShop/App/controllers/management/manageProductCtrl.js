(function (app) {
    'use strict';

    app.controller('manageProductCtrl', manageProductCtrl);
    manageProductCtrl.$inject = ['$scope', '$rootScope', '$state', 'toastr', 'apiService', 'modalService', 'itemsPerPage'];

    function manageProductCtrl($scope, $rootScope, $state, toastr, apiService, modalService, itemsPerPage) {
        $scope.category = undefined;
        $scope.items = [];
        $scope.categories = [];
        $scope.pager = {
            TotalItems: 0,
            ItemsPerPage: itemsPerPage,
            PageNumber: 0
        };

        $scope.Delete = function (product) {
            var modalOptions = {
                showCancelButton: true,
                headerBackgroundCss: 'bg-red',
                closeButtonCss: 'btn-danger',
                closeButtonText: 'Cancel',
                actionButtonText: 'Delete',
                headerText: product.Name,
                bodyText: 'Are you sure you want delete this product?'
            };

            modalService.showModal({}, modalOptions)
                .then(function (result) {
                    apiService.post('/api/product/delete', product, null, function (response) {
                        toastr.success('The product ' + product.Name + ' was deleted successfully');
                        $scope.Load($scope.pager.PageNumber);
                    });
                });
        };
        $scope.Load = function (page) {
            if (!page) {
                page = 0;
            }
            else if (page > 0) {
                page = page - 1;
            }

            var config = {
                params: {
                    page: page,
                    pageSize: $scope.pager.ItemsPerPage,
                    name: $scope.name,
                    categoryID: $scope.category
                }
            };

            apiService.get('/api/product/list', config, function (response) {
                $scope.items = response.data.Items;

                $scope.pager = {
                    TotalItems: response.data.TotalCount,
                    ItemsPerPage: itemsPerPage,
                    PageNumber: response.data.Page + 1
                };
            });
        };

        $scope.Load($scope.pager.PageNumber);
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