angular.module('windowsPhoneApp').controller('InfoModalController', ['$scope', '$modalInstance', function ($scope, $modalInstance) {
    $scope.ok = function () {
        $modalInstance.close();
    };
}]);