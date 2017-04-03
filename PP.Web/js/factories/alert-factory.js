define(['./module'], function (factories) {
    'use strict';
    factories.factory('AlertFactory', ['$rootScope', '$filter', '$timeout', function ($rootScope, $filter, $timeout) {

        var alertFactory = {};

        // create an array of alerts available globally
        $rootScope.alerts = [];

        alertFactory.add = function (type, msg) {

            var id = (new Date()).getTime();
            var currentDate = $filter('date')(new Date(), "yyyy-MM-dd HH:MM:ss");
            console.log(currentDate);
            var alert = { 'type': type, 'msg': msg, 'time': currentDate, 'id': id };

            $rootScope.alerts.push(alert);

            if (type === 'danger') {
                //Close after 6 seconds
                $timeout(function () {
                    removeAlert(alert);
                }, 10000);
            }
            else {
                //Close after 3 seconds
                $timeout(function () {
                    removeAlert(alert);
                }, 5000);
            }
        };

        alertFactory.closeAlert = function (index) {
            $rootScope.alerts.splice(index, 1);
        };

        return alertFactory;

        function removeAlert(alert) {
            //Find alert by msg
            var index = findById($rootScope.alerts, alert.id);
            if (index >= 0) {
                $rootScope.alerts.splice(index, 1);
            }
        };

        function findById(alerts, id) {
            var i = 0, len = alerts.length;
            for (; i < len; i++) {
                if (alerts[i].id == id) {
                    return i;
                }
            }
            return -1;
        }

    }]);
});