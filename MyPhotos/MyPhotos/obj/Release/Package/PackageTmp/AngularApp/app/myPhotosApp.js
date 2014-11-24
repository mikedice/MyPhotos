var myapp = angular.module('myPhotosApp', ['ngRoute', 'ui.bootstrap']);
myapp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: '/AngularApp/partials/list-galleries.html',
        controller: 'galleriesListController'
    })
    .when('/ViewGallery/:galleryName/:startImage?', {
        templateUrl: '/AngularApp/partials/view-gallery-carousel.html',
        controller: 'galleryViewController'
    })
    .when('/_=_', {redirectTo: '/'}); /* What the fuck Facebook? :( */
}]);