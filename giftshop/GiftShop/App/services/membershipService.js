(function (app) {
    'use strict';
    app.factory('membershipService', membershipService);
    membershipService.$inject = ['$http', '$base64', 'localStorageService', '$rootScope'];

    function membershipService($http, $base64, localStorageService, $rootScope) {

        var service = {
            saveCredentials: saveCredentials,
            removeCredentials: removeCredentials,
            isUserLoggedIn: isUserLoggedIn,
            getUserName: getUserName
        };

        function saveCredentials(user, response) {
            var membershipData = $base64.encode(user.UserName + ':' + user.Password);

            if (!$rootScope.repository)
                $rootScope.repository = {};

            $rootScope.repository.currentUser = response.user;
            $rootScope.repository.Authdata = membershipData;

            $http.defaults.headers.common['Authorization'] = 'Basic ' + membershipData;
            localStorageService.set('repository', $rootScope.repository);
        }

        function removeCredentials() {
            $rootScope.repository = {};
            localStorageService.remove('repository');
            $http.defaults.headers.common.Authorization = '';
        }

        function isUserLoggedIn() {
            return $rootScope.repository && $rootScope.repository.currentUser !== null && $rootScope.repository.currentUser !== undefined;
        }

        function getUserName() {
            return $rootScope.repository.currentUser !== null ? $rootScope.repository.currentUser.UserName : null;
        }

        return service;
    }
})(angular.module('common.core'));