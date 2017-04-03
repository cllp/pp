define(['./module'], function (directives) {
    'use strict';

    directives.directive('mouseover', function () {

        return {
            restrict: 'A',            
            replace: true,
            link: function (scope, element) {
                var $container = element.parent().find(".textarea-container");
                var $label = element.parent().find("label");

                element.parent().bind('mouseenter', function () {
                    mouseEnter();
                });
                element.parent().bind('mouseleave', function () {
                    mouseLeave();
                });


                element.parent().bind('focusin', function () {
                    mouseEnter()
                });
                element.parent().bind('focusout', function () {
                    mouseLeave();
                });

                function mouseEnter(){
                    $label.css("color", "#fff");
                    $label.css("background", "#f15a0b");
                    $container.addClass("textarea-container-hover");
                }

                function mouseLeave() {
                    $label.css("color", "inherit");
                    $label.css("background", "none");
                    $container.removeClass("textarea-container-hover");
                }


                element.on('change', 'input, textarea, select', function () {
                    $(this).parent().addClass('input-changed').delay(1000).queue(function () {
                        $(this).parent().removeClass('input-changed').dequeue();
                });


                    $(window).bind('beforeunload', function () {
                        return 'Dina ändringar är inte sparade är du säker på att du vill lämna sidan?';
                    });                   
                });


            }
        };
    })



});