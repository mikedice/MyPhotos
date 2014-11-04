var myapp = angular.module('iPhoneApp', ['ngRoute', 'ngTouch']);
myapp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: '/iPhoneApp/partials/list-galleries.html',
        controller: 'galleriesListController'
    })
    .when('/ViewGallery/:galleryName/:startImage?', {
        templateUrl: '/iPhoneApp/partials/view-gallery.html',
        controller: 'galleryViewController'
    })
    .when('/_=_', {redirectTo: '/'}); /* What the fuck Facebook? :( */
}]);