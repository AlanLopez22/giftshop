(function (app) {
    'use strict';

    app.controller('mainCtrl', mainCtrl);
    mainCtrl.$inject = ['$scope', '$rootScope', 'membershipService', '$state'];

    function mainCtrl($scope, $rootScope, membershipService, $state) {
        $scope.userInfo = { IsUserLogged: false };
        $scope.displayUserInfo = displayUserInfo;
        $rootScope.logout = logout;        
        displayUserInfo();

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
            $scope.userInfo = undefined;
            membershipService.removeCredentials();
            $state.go('main.home');
        }
    }
})(angular.module('GiftShopApp'));