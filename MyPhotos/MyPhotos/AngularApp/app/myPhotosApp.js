var myapp = angular.module('myPhotosApp', ['ngRoute']);
myapp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: '/AngularApp/partials/list-galleries.html',
        controller: 'galleriesListController'
    })
    .when('/ViewGallery/:galleryName/:startImage?', {
        templateUrl: '/AngularApp/partials/view-gallery.html',
        controller: 'galleryViewController'
    })
    .when('/_=_', {redirectTo: '/'}); /* What the fuck Facebook? :( */
}]);