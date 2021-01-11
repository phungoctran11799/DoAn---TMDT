$(document).ready(function () {
    $('#dieukhoan').click(function () {
        var url = "thongtindeukhoan.php";
        $("#tt-dkhoan").load(url).slideDown('slow');
        //$("#tt-dkhoan").load(url);
        $('#hide').css('display', 'block');
        $('#hide input').click(function () {
            $("#tt-dkhoan").slideUp('slow');
        });
        $("#hide input.show").click(function () {
            $("#tt-dkhoan").slideDown('slow');
        });
    });
});