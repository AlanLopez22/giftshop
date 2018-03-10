(function (app) {
    'use strict';
    app.factory('apiService', apiService);
    apiService.$inject = ['$http', '$rootScope', '$state', 'toastr'];

    function apiService($http, $rootScope, $state, toastr) {
        var service = {
            get: get,
            post: post,
            showError: requestFailed,
            getErrorMessage: getErrorMessage
        };

        function get(url, config, success, fail) {
            $rootScope.isLoading = true;

            if ($rootScope.repository) {
                config = config || {};
                config.headers = config.headers || {};

                if ($rootScope.repository.Authdata) {
                    config.headers.Authorization = 'Basic ' + $rootScope.repository.Authdata;
                }
            }

            $rootScope.AppPromise = $http.get("." + url, config)
                .then(function (result) {
                    $rootScope.isLoading = false;
                    success(result);
                }, function (error) {
                    $rootScope.isLoading = false;

                    if (error.status === 401) {
                        var errorMessage = getErrorMessage(error) || "Unauthorized access";
                        requestFailed(errorMessage);

                        if ($state.current && $state.current.name !== 'main.account')
                            $state.go("main.home");
                    }
                    else {
                        requestFailed(error);
                    }

                    if (fail) {
                        fail(error);
                    }
                });
            return $rootScope.AppPromise;
        }
        function post(url, data, config, success, fail) {
            $rootScope.isLoading = true;

            if ($rootScope.repository) {
                config = config || {};
                config.headers = config.headers || {};

                if ($rootScope.repository.Authdata && !config.headers.Authorization) {
                    config.headers.Authorization = 'Basic ' + $rootScope.repository.Authdata;
                }
            }

            $rootScope.AppPromise = $http.post("." + url, data, config)
                .then(function (result) {
                    $rootScope.isLoading = false;
                    success(result);
                }, function (error) {
                    $rootScope.isLoading = false;

                    if (error.status === 401) {
                        var errorMessage = getErrorMessage(error) || "Unauthorized access";
                        requestFailed(errorMessage);

                        if ($state.current && $state.current.name !== 'main.account')
                            $state.go("main.home");
                    }
                    else {
                        requestFailed(error);
                    }

                    if (fail)
                        fail(error);
                });

            return $rootScope.AppPromise;
        }
        function requestFailed(response) {
            var message = getErrorMessage(response);

            if (message && message.length > 0) {
                toastr.error(message);
            }
        }
        function getErrorMessage(response) {
            var message = '';

            if (response && response.data)
                message = getError(response.data);
            else if (response)
                message = getError(response);

            return message;
        }

        function getError(obj) {
            var message = '';

            if (obj && obj.ExceptionMessage) {
                message = obj.ExceptionMessage;
            }
            else if (obj && obj.ErrorMessage) {
                message = obj.ErrorMessage;
            }
            else if (obj && obj.Message) {
                message = obj.Message;
            }
            else if (obj && obj.message) {
                message = obj.message;
            }
            else if (obj && obj.error) {
                message = obj.error;
            }
            else if (obj && obj.Errors && angular.isArray(obj.Errors)) {
                message = obj.Errors.join('<br/>');
            }
            else if (obj && obj.errors && angular.isArray(obj.errors)) {
                message = obj.errors.join('<br/>');
            }
            else if (obj && angular.isArray(obj)) {
                message = obj.join('<br/>');
            }
            else {
                message = obj;
            }

            return message;
        }

        return service;
    }

})(angular.module('common.core'));