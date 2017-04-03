define(['./module'], function (directives) {
    'use strict';

    directives.directive('bordermouseover', function () {

        return {
            restrict: 'A',
            replace: true,
            link: function (scope, element) {
                var $container = element.parent().find(".textarea-container");

                element.parent().bind('mouseenter', function () {                    
                    $container.addClass("textarea-containerTable-hover");
                });
                element.parent().bind('mouseleave', function () {                    
                    $container.removeClass("textarea-containerTable-hover");
                });


                element.parent().bind('focusin', function () {
                    $container.addClass("textarea-containerTable-hover");
                });
                element.parent().bind('focusout', function () {
                    $container.removeClass("textarea-containerTable-hover");
                });






                element.on('change', 'input, textarea, select', function () {
                    $(this).parent().addClass('input-changed');

                    $(window).bind('beforeunload', function () {
                        return 'Dina ändringar är inte sparade är du säker på att du vill lämna sidan?';
                    });
                });




            }
        };
    })



});