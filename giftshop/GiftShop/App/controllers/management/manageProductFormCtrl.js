(function (app) {
    'use strict';
    app.controller('manageProductFormCtrl', manageProductFormCtrl);
    manageProductFormCtrl.$inject = ['$scope', '$rootScope', '$state', '$stateParams', 'toastr', '$filter', 'Upload', '$http', 'apiService'];

    function manageProductFormCtrl($scope, $rootScope, $state, $stateParams, toastr, $filter, Upload, $http, apiService) {
        $scope.productID = $stateParams.id;
        $scope.formData = undefined;
        $scope.categories = [];
        
        $scope.addImage = function (image, file) {
            image.File = file;
            $scope.isLoading = true;

            Upload.upload({
                url: './api/product/UploadImage',
                data: {
                    file: file
                },
                headers: {
                    Authorization: $http.defaults.headers.common['Authorization']
                }
            })
                .then(function (response) {
                    $scope.isLoading = false;

                    if (response.data.success) {
                        image.ImagePath = response.data.image;

                        if (image.ID == -1) {
                            $scope.formData.Images.push(getEmptyImage());
                        }
                    }
                    else {
                        toastr.error('Could not upload the image. Please try again');
                    }
                }, function (err) {
                    var errorMessage = apiService.getErrorMessage(err);
                    toastr.error(errorMessage);
                    $scope.isLoading = false;
                });
        };
        $scope.Save = function () {
            apiService.post('/api/product/Save', $scope.formData, null, function (result) {
                toastr.success('The product was saved successfully');
                $state.go('main.manage-product');
            }, function (error) {
                var messageError = apiService.getErrorMessage(error);
                toastr.error(messageError);
            });
        };
        $scope.Cancel = function () {
            $state.go('main.manage-product');
        };

        load();

        function load() {
            apiService.get('/api/category/list/', null, function (response) {
                $scope.categories = response.data;
            });

            apiService.get('/api/product/getByID/', { params: { id: $scope.productID } }, function (response) {
                $scope.formData = response.data || {
                    ID: -1, Name: '', Description: '', Images: []
                };
                $scope.formData.Images.push(getEmptyImage());
            });
        }
        function getEmptyImage() {
            return { ID: -1, ImagePath: "./Images/noimage.png" };
        }
    }
})(angular.module('GiftShopApp'));