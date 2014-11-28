var myapp = angular.module('mobileApp', ['ngRoute', 'ngTouch', 'ui.bootstrap']);
myapp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: '/mobileApp/partials/list-galleries.html',
        controller: 'galleriesListController'
    })
    .when('/ViewGallery/:galleryName/:startImage?', {
        templateUrl: '/mobileApp/partials/view-gallery.html',
        controller: 'galleryViewController'
    })
    .when('/_=_', {redirectTo: '/'}); /* What the fuck Facebook? :( */
}]);