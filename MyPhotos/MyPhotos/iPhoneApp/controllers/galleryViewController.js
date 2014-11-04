var app = angular.module('iPhoneApp');
app.controller('galleryViewController', ['$scope', '$routeParams', '$window', '$log', 'contentService', function ($scope, $routeParams, $window, $log, contentService) {
    var imageElements;
    var currentIndex;
    var imageCount;
    var viewportOrientation;

    contentService.getGallery($routeParams.galleryName).then(
       function (success) {
           initialize(success.ViewGalleries[0]);
       },
       function (error) {
           $scope.errorMessage = "failed to retrieve galleries";
       }
    );

    $scope.handleLeftSwipe = function () {
        $log.info('swiped left');
        var idx = incrementCurrent();
        showImageAt(idx);
    }

    $scope.handleRightSwipe = function () {
        $log.info('swiped right');
        var idx = decrementCurrent();
        showImageAt(idx);
    }

    var initialize = function(viewGallery)
    {
        setWindowOrientation();

        window.addEventListener('orientationchange', function (e) {
            setWindowOrientation();
            setOrientationBasedStyle();
        });
        currentIndex = 0;
        imageElements = [];
        $scope.ViewGallery = viewGallery;
        $scope.ImageList = $scope.ViewGallery.Gallery.Images;
        imageCount = $scope.ImageList.length;
        createImageObjects($scope);
        setOrientationBasedStyle();
        showImageAt(0);
    }

    var setWindowOrientation = function () {
        if (window.innerHeight >= window.innerWidth) {
            viewportOrientation = 0; // portrait
        }
        else {
            viewportOrientation = 90; // landscape
        }
        debugMessage1('viewportOrientation: ' + viewportOrientation);
    }

    var setOrientationBasedStyle = function()
    {
        if (viewportOrientation == 0)
        {
            // portrait style
            for (var i = 0; i<imageElements.length; i++)
            {
                imageElements[i].removeClass(imageElements[currentIndex].attr('class'));
                imageElements[i].addClass('iphone-gallery-image-portrait');
            }
        }
        else if (Math.abs(viewportOrientation) == 90)
        {
            for (var i = 0; i < imageElements.length; i++) {
                if ($scope.ImageList[i].Orientation == 0) {
                    imageElements[i].removeClass(imageElements[currentIndex].attr('class'));
                    imageElements[i].addClass('iphone-gallery-image-landscape-portrait');
                }
                else if ($scope.ImageList[i].Orientation == 1) {
                    imageElements[i].removeClass(imageElements[currentIndex].attr('class'));
                    imageElements[i].addClass('iphone-gallery-image-landscape-landscape');
                }
            }
        }
        debugMessage2('class: ' + imageElements[currentIndex].attr('class'));
    }

    var debugMessage1 = function(message)
    {
        document.getElementById('debug-message-1').innerHTML = message;
    }
    var debugMessage2 = function (message) {
        document.getElementById('debug-message-2').innerHTML = message;
    }
    var showImageAt = function(index)
    {
        var holder = angular.element(document.getElementById('current-image-holder'));
        holder.empty();
        holder.append(imageElements[index]);
    }


    var incrementCurrent = function()
    {
        if (currentIndex + 1 == imageCount) {
            currentIndex = 0;
        }
        else {
            currentIndex++;
        }
        return currentIndex;
    }

    var decrementCurrent = function()
    {
        if (currentIndex == 0) {
            currentIndex = imageCount - 1;
        }
        else {
            currentIndex--;
        }
        return currentIndex;
    }

    var createImageObjects = function(scope)
    {
        for (var i = 0; i<scope.ImageList.length; i++){
                var image = scope.ImageList[i];
                var imgEl = angular.element("<img>")
                    .attr("src", image.MobileUrl)
                    .attr("alt", image.Name)
                imageElements.push(imgEl);
            };
    }
}]);