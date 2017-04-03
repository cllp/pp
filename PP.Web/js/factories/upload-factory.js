define(['./module'], function (factories) {
    'use strict';
    factories.factory('UploadFactory', ['$upload', '$log', '$http', function ($upload, $log, $http, $scope) {

        return {

            UploadPV: function (file, uploadObject) {
                //return 'return from function: ' + id;

                var upload = $upload.upload({
                    url: 'api/upload/projectvolume/upload', //upload.php script, node.js route, or servlet url
                    // method: POST or PUT,
                    // headers: {'headerKey': 'headerValue'},
                    // withCredential: true,
                    data: { object: uploadObject },
                    file: file,
                    // file: $files, //upload multiple files, this feature only works in HTML5 FromData browsers
                    /* set file formData name for 'Content-Desposition' header. Default: 'file' */
                    //fileFormDataName: myFile, //OR for HTML5 multiple upload only a list: ['name1', 'name2', ...]
                    /* customize how data is added to formData. See #40#issuecomment-28612000 for example */
                    //formDataAppender: function(formData, key, val){} //#40#issuecomment-28612000
                }).progress(function (evt) {
                    $log.info('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    // file is uploaded successfully
                    //$log.info(data);
                }).error(function (data, status) {
                    $log.error(data);
                });

                return upload;

            },
            SubmitPV: function (data) {
                return $http.post('api/upload/projectvolume/submit', data);
            }
        }

    }]);
});
