(function () {
    'use strict';

    var app = angular.module('GiftShopApp', ['common.core', 'common.ui'])
        .config(config)
        .run(run);

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
                }
            })
            .state("main.account", {
                url: '/account',
                views: {
                    'content@main': {
                        templateUrl: './App/views/account/index.html?' + new Date().getMilliseconds(),
                        controller: "accountCtrl"
                    }
                }
            })
            .state("main.home", {
                url: '/home',
                views: {
                    'content@main': {
                        templateUrl: './App/views/home/index.html?' + new Date().getMilliseconds(),
                        controller: "homeCtrl"
                    }
                }
            });

        $locationProvider.html5Mode(true);
    }

    run.$inject = ['$rootScope', 'localStorageService', '$http', '$state', 'membershipService'];
    function run($rootScope, localStorageService, $http, $state, membershipService) {
        // handle page refreshes
        $rootScope.repository = localStorageService.get('repository') || {};
        $rootScope.isLoadingApp = true;

        if ($rootScope.repository && $rootScope.repository.Authdata && $rootScope.repository.Authdata.length > 0) {
            $http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.repository.Authdata;
        }

        $rootScope.$on('$stateChangeStart', function (e, toState, toParams, fromState, fromParams) {
            $rootScope.previousState = fromState;
            $rootScope.previousStateParams = fromParams;
            var isUserLogged = membershipService.isUserLoggedIn();
        });
        $(document).ready(function () {
            $rootScope.isLoadingApp = false;
            setTimeout(function () { $('.page-loader-wrapper').fadeOut(); }, 50);
        });
    }

    
})();