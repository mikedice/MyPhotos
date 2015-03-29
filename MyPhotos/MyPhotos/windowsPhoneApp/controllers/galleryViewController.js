
angular.module('windowsPhoneApp').controller('galleryViewController', ['$scope', '$modal', '$routeParams', '$window', '$log', 'contentService', function ($scope, $modal, $routeParams, $window, $log, contentService) {
    var imageElements;
    var currentIndex;
    var imageCount;
    var viewportOrientation;
    var currentClass;
    var viewGallery;
    var swipe = {};
    var scope = $scope;

    contentService.getGallery($routeParams.galleryName).then(
       function (success) {
           initialize(success.ViewGalleries[0]);
       },
       function (error) {
           $scope.errorMessage = "failed to retrieve galleries";
       }
    );

    $scope.showInfoPane = function () {
        $modal.open({
            templateUrl: '/windowsPhoneApp/partials/info-modal.html',
            controller: 'InfoModalController',
            backdrop: 'static'
        });
    }

    var initialize = function(gal)
    {
        currentIndex = 0;
        setWindowOrientation();

        window.screen.addEventListener('MSOrientationChange', function (e) {
            setWindowOrientation();
            setOrientationBasedStyle();
        });

        var holderEl = document.getElementById('current-image-holder');
        holderEl.addEventListener("pointerdown", handleTouchStart, false);
        holderEl.addEventListener("pointerup", handleTouchEnd, false);
        holderEl.addEventListener("pointermove", handleTouchMove, false);
        debugMessage1("registered");
        
        imageElements = [];
        viewGallery = gal;
        $scope.ImageList = viewGallery.Gallery.Images;
        imageCount = $scope.ImageList.length;
        createImageObjects($scope);
        setOrientationBasedStyle();

        var startIndex = 0;
        if ($routeParams.startImage)
        {
            for (var i = 0; i<viewGallery.Gallery.Images.length; i++)
            {
                if (viewGallery.Gallery.Images[i].Name == $routeParams.startImage)
                {
                    startIndex = i;
                    break;
                }
            }
        }
        showImageAt(startIndex);
    }

    var debugMessageObj = function (obj) {
        var msg = "";
        for(var prop in obj)
        {
            msg = msg + prop + "=" + obj[prop] + " ";
        }
        debugMessage1(msg);
    }
    var handleTouchStart = function (evt) {
        evt.preventDefault();
        swipe.status = "start";
        swipe.startX = evt.screenX;
        swipe.startY = evt.screenY;
        debugMessage1("start");
    }

    var handleTouchMove = function (evt) {
        evt.preventDefault();
        swipe.status = "move";
        swipe.lastX = evt.screenX;
        swipe.lastY = evt.screenY;
        debugMessage1("gestureMove");
    }

    var handleTouchEnd = function (evt) {
        evt.preventDefault();
        swipe.status = "end";
        swipe.endX = evt.screenX;
        swipe.endY = evt.screenY;
        debugMessage1("gestureMove");

        var distX = swipe.startX - swipe.endX;
        var distY = swipe.startY - swipe.endY;

        var distYLimit = 75;
        var distXLimit = 50;
        if (Math.abs(distY) < distYLimit && distX < 0 && Math.abs(distX) > distXLimit) {
            handleRightSwipe();
        }
        else if (Math.abs(distY) < distYLimit && distX > 0 && Math.abs(distX) > distXLimit) {
            handleLeftSwipe();
        }

        debugMessage1("gestureEnd ");
    }

    var handleLeftSwipe = function () {
        $log.info('swiped left');
        var idx = incrementCurrent();
        showImageAt(idx);
    }

    var handleRightSwipe = function () {
        $log.info('swiped right');
        var idx = decrementCurrent();
        showImageAt(idx);
    }

    var setWindowOrientation = function () {
        // Windows phone specific orientation handling :'(
        if (window.screen.msOrientation == 'portrait-primary' ||
            window.screen.msOrientation == 'portrait-secondary') {
            viewportOrientation = 0;
        }
        else if (window.screen.msOrientation == 'landscape-primary' ||
            window.screen.msOrientation == 'landscape-secondary') {
            viewportOrientation = 90;
        }

        //debugMessage1('viewportOrientation: ' + viewportOrientation);
    }

    var setCurrentClass = function (className) {
        var oldClass = imageElements[currentIndex].attr('class');
        currentClass = className;
        imageElements[currentIndex].removeClass(oldClass);
        imageElements[currentIndex].addClass(currentClass);
    }

    var setOrientationBasedStyle = function()
    {
        if (viewportOrientation == 0)
        {
            if ($scope.ImageList[currentIndex].Orientation == 0) {
                setCurrentClass('mobile-gallery-image-portrait-portrait')
                setCaptionClass('image-metadata-portrait');
            }
            else if ($scope.ImageList[currentIndex].Orientation == 1) {
                setCurrentClass('mobile-gallery-image-portrait-landscape')
                setCaptionClass('image-metadata-portrait');
            }
            //debugMessage1("mobile-gallery-image-portrait");
        }
        else if (Math.abs(viewportOrientation) == 90)
        {
            if ($scope.ImageList[currentIndex].Orientation == 0) {
                setCurrentClass('mobile-gallery-image-landscape-portrait');
                setCaptionClass('image-metadata-landscape-portrait');
                //debugMessage1('mobile-gallery-image-landscape-portrait');
            }
            else if ($scope.ImageList[currentIndex].Orientation == 1) {
                setCurrentClass('mobile-gallery-image-landscape-landscape');
                setCaptionClass('image-metadata-landscape-landscape');
                //debugMessage1('mobile-gallery-image-landscape-landscape');
            }
            else {
                setCurrentClass('mobile-gallery-image-landscape-portrait');
                setCaptionClass('image-metadata-landscape-portrait');
                //debugMessage1("Unknown image orientation");
            }
        }
    }

    var setCaptionClass = function(className)
    {
        var el = angular.element(document.getElementById('image-metadata-block'));
        var oldClass = el.attr("class");

        var classes = oldClass.split(" ");
        if (classes)
        {
            var result = "";
            for (var i = 0; i<classes.length; i++)
            {
                if (classes[i] != 'image-metadata-portrait' &&
                    classes[i] != 'image-metadata-landscape-portrait' &&
                    classes[i] != 'image-metadata-landscape-landscape')
                {
                    result = result + " " + classes[i];
                }
            }
        }

        var newClass = className + " " + result;
        el.removeClass(oldClass);
        el.addClass(newClass);
    }

    var debugMessage1 = function(message)
    {
        document.getElementById('debug-message-1').innerHTML = message;
    }

    var showImageAt = function(index)
    {
        currentIndex = index;
        var holderEl = document.getElementById('current-image-holder');
        var holder = angular.element(holderEl);
        holder.empty();
        setOrientationBasedStyle();
        $scope.imageCaption = $scope.ImageList[index].ImageMetadata.Caption;
        $scope.imageName = $scope.ImageList[index].Name;
        $scope.imageFullUrl = $scope.ImageList[index].FullUrl;
        holder.append(imageElements[index]);
    }

    var incrementCurrent = function ()
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

