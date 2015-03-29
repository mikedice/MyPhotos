var myapp = angular.module('windowsPhoneApp', ['ngRoute', 'ui.bootstrap']);
myapp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: '/windowsPhoneApp/partials/list-galleries.html',
        controller: 'galleriesListController'
    })
    .when('/ViewGallery/:galleryName/:startImage?', {
        templateUrl: '/windowsPhoneApp/partials/view-gallery.html',
        controller: 'galleryViewController'
    })
    .when('/_=_', {redirectTo: '/'}); /* What the fuck Facebook? :( */
}]);