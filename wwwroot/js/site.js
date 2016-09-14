/* site.js */


// Immediately executed anonymous function for initializations.
(function () {

    var $sidebarAndWrapper = $("#sidebar, #wrapper");
    var $icon = $("#sidebarToggle i.fa");


    $("#sidebarToggle").on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar");

        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $icon.removeClass("fa-angle-left");
            $icon.addClass("fa-angle-right");
        }
        else {
            $icon.addClass("fa-angle-left");
            $icon.removeClass("fa-angle-right");
        }

});



    //$(document).ready(function () {
    //    var element = $("#username");
    //    element.text("Jeff");

    //    var main = $("#main");
    //    main.mouseenter(function () {
    //        main.css("background-color", "#888")
    //    });

    //    main.mouseleave(function () {
    //        main.css("background-color", "")
    //    });

    //    var menuItems = $("ul.menu li a");
    //    menuItems.click(function (e) {
    //        var me = $(this);
    //        alert(me.text());
    //    });
    //});


    //var element = document.getElementById("username");
    //element.innerHTML = "Jeff";


    //var element = document.getElementById("main");
    //main.onmouseenter = function () {
    //    main.style.backgroundColor = "#888";
    //};

    //main.onmouseleave = function () {
    //    main.style.backgroundColor = "";
    //};


})();
