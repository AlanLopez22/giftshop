(function (app) {
    'use strict';
    app.controller('accountCtrl', accountCtrl);
    accountCtrl.$inject = ['$scope', '$rootScope', '$state', 'toastr', '$base64', 'apiService', 'membershipService'];

    function accountCtrl($scope, $rootScope, $state, toastr, $base64, apiService, membershipService) {
        $scope.loginFormData = {
            UserName: '',
            Password: ''
        };
        $scope.signupFormData = {
            UserName: '',
            Password: '',
            ConfirmPassword: ''
        };

        $scope.login = function () {
            var authdata = $base64.encode($scope.loginFormData.UserName + ':' + $scope.loginFormData.Password);

            var config = {
                headers: {
                    Authorization: 'Basic ' + authdata
                }
            };

            apiService.post('./api/account/authenticate', null, config, function (response) {
                membershipService.saveCredentials($scope.loginFormData, response.data);
                $scope.displayUserInfo();
                redirectToPage();
            });
        };
        $scope.signup = function () {
            $rootScope.isTryToLogOut = true;
            $scope.signupFormData.headers = null;

            apiService.post('./api/account/signup', $scope.signupFormData, null, function (response) {
                if (response.data) {
                    membershipService.saveCredentials($scope.loginFormData, response.data);
                    $scope.displayUserInfo();
                    redirectToPage();
                }
                else {
                    for (var i = 0; i < response.data.errors.length; i++) {
                        var error = response.data.errors[i];
                        toastr.error(error);
                    }
                }
            });
        };

        function redirectToPage() {
            if ($scope.userInfo && $scope.userInfo.IsUserLogged) {
                $state.go('main.home');
            }
        }
    }
})(angular.module('GiftShopApp'));