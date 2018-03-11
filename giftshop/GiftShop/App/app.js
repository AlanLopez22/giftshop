(function () {
    'use strict';

    var app = angular.module('GiftShopApp', ['common.core', 'common.ui'])
        .config(config)
        .run(run)
        .constant('itemsPerPage', 5)
        .service('Authorization', Authorization);

    config.$inject = ['$stateProvider', '$locationProvider', 'localStorageServiceProvider', '$urlRouterProvider', '$urlMatcherFactoryProvider', '$uibModalProvider'];
    function config($stateProvider, $locationProvider, localStorageServiceProvider, $urlRouterProvider, $urlMatcherFactoryProvider, $uibModalProvider) {
        $uibModalProvider.options.keyboard = false;
        $uibModalProvider.options.backdrop = 'static';

        localStorageServiceProvider
            .setPrefix('GiftShopApp')
            .setStorageType('sessionStorage')
            .setNotify(true, true);

        $urlRouterProvider.otherwise("/home");
        $urlMatcherFactoryProvider.strictMode(false);

        $stateProvider
            .state("main", {
                views: {
                    'main@': {
                        templateUrl: './App/views/main.html?' + new Date().getMilliseconds(),
                    }
                },
                data: {
                    authorization: false,
                    memory: false,
                    roles: ['Administrator', 'User']
                }
            })
            .state("main.account", {
                url: '/account',
                views: {
                    'content@main': {
                        templateUrl: './App/views/account/index.html?' + new Date().getMilliseconds(),
                        controller: "accountCtrl"
                    }
                },
                data: {
                    authorization: false,
                    memory: false,
                    roles: ['Administrator', 'User']
                }
            })
            .state("main.home", {
                url: '/home',
                views: {
                    'content@main': {
                        templateUrl: './App/views/home/index.html?' + new Date().getMilliseconds(),
                        controller: "homeCtrl"
                    }
                },
                data: {
                    authorization: false,
                    memory: false,
                    roles: ['Administrator', 'User']
                }
            })
            .state("main.manage-product", {
                url: '/manage-product',
                views: {
                    'content@main': {
                        templateUrl: './App/views/management/product/list.html?' + new Date().getMilliseconds(),
                        controller: "manageProductCtrl"
                    }
                },
                data: {
                    authorization: true,
                    redirectTo: 'main.home',
                    memory: true,
                    roles: ['Administrator']
                }
            })
            .state("main.manage-product.form", {
                url: '/:id',
                views: {
                    'content@main': {
                        templateUrl: './App/views/management/product/form.html?' + new Date().getMilliseconds(),
                        controller: "manageProductFormCtrl"
                    }
                },
                data: {
                    authorization: true,
                    redirectTo: 'main.home',
                    memory: true,
                    roles: ['Administrator']
                }
            });

        $locationProvider.html5Mode(true);
    }

    run.$inject = ['$rootScope', 'localStorageService', '$http', '$state', '$filter', 'Authorization'];
    function run($rootScope, localStorageService, $http, $state, $filter, Authorization) {
        // handle page refreshes
        $rootScope.repository = localStorageService.get('repository') || {};
        $rootScope.isLoadingApp = true;

        if ($rootScope.repository && $rootScope.repository.Authdata && $rootScope.repository.Authdata.length > 0) {
            $http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.repository.Authdata;
        }

        $rootScope.$on('$stateChangeStart', function (e, toState, toParams, fromState, fromParams) {
            $rootScope.previousState = fromState;
            $rootScope.previousStateParams = fromParams;

            if (!Authorization.authorized) {
                if (Authorization.memorizedState && (!fromState.data || !fromState.data.redirectTo || toState.name !== fromState.data.redirectTo)) {
                    Authorization.clear();
                }

                if (toState.data && toState.data.authorization && toState.data.redirectTo) {
                    if (!Authorization.isUserLoggedIn()) {
                        if (toState.data.memory) {
                            Authorization.memorizedState = toState.name;
                        }

                        e.preventDefault();
                        $state.go(toState.data.redirectTo);
                    }
                    else if ($rootScope.repository.currentUser) {
                        var userType = $rootScope.repository.currentUser.UserType.Description;
                        var isInRole = $filter('filter')(toState.data.roles, function (item) { return item === userType });

                        if (isInRole.length === 0) {
                            e.preventDefault();
                            $state.go(toState.data.redirectTo);
                        }
                    }
                }
            }
        });
        $(document).ready(function () {
            $rootScope.isLoadingApp = false;
            setTimeout(function () { $('.page-loader-wrapper').fadeOut(); }, 50);
        });
    }

    Authorization.$inject = ['$state', '$rootScope', 'membershipService'];
    function Authorization($state, $rootScope, membershipService) {
        this.authorized = false,
            this.memorizedState = null;

        function clear() {
            this.authorized = false;
            this.memorizedState = null;
        }

        function go(fallback) {
            this.authorized = true;
            var targetState = this.memorizedState ? this.memorizedState : fallback;
            $state.go(targetState);
        };

        function isUserLoggedIn() {
            return membershipService.isUserLoggedIn();
        }

        return {
            authorized: this.authorized,
            memorizedState: this.memorizedState,
            clear: clear,
            go: go,
            isUserLoggedIn: isUserLoggedIn
        };
    }
})();