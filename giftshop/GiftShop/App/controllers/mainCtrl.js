(function (app) {
    'use strict';

    app.controller('mainCtrl', mainCtrl);
    mainCtrl.$inject = ['$scope', '$rootScope', '$state', 'localStorageService', 'apiService', 'membershipService'];

    function mainCtrl($scope, $rootScope, $state, localStorageService, apiService, membershipService) {
        $rootScope.order = undefined;
        $scope.userInfo = { IsUserLogged: false };

        $scope.displayUserInfo = displayUserInfo;
        $scope.logout = logout;
        $rootScope.getOrder = function () {
            var orderID = $rootScope.repository && $rootScope.repository.order ? $rootScope.repository.order.ID : 0;

            apiService.get('/api/order/get', {
                params: { id: orderID }
            }, function (response) {
                $rootScope.order = response.data;
                $rootScope.repository.order = $rootScope.order;
                localStorageService.set('repository', $rootScope.repository);
            });
        };

        newOrder();
        displayUserInfo();

        function newOrder() {
            $scope.getOrder();
        }

        function displayUserInfo() {
            var isUserLogged = membershipService.isUserLoggedIn();

            if (isUserLogged) {
                $scope.userInfo = $rootScope.repository.currentUser;
                $scope.userInfo.IsUserLogged = true;
            }
            else
                $scope.userInfo = { IsUserLogged: false };
        }
        function logout() {
            $rootScope.order = undefined;
            $rootScope.repository.order = $rootScope.order;
            localStorageService.set('repository', $rootScope.repository);
            $scope.userInfo = undefined;
            membershipService.removeCredentials();
            $rootScope.getOrder();
            $state.go('main.home');
        }
    }
})(angular.module('GiftShopApp'));