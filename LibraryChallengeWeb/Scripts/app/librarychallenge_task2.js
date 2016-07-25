var uri = '/api/library/categorizeditems';
$(document).ready(function () {
    // Send an AJAX request
    $.getJSON(uri)
        .done(function (data) {
            var str = formatItem(data);
            $("#panelSub").append(str);
            openDropDownList();
        });
});
function openDropDownList() {
    $('.parent').on('click', function (item) {
        var temp = $(item.currentTarget);
        temp.find('.books').toggle();
    });
}

function formatItem(data) {
    var str = "";
    $.each(data, function (key, item) {
        str += '<div class="parent"><div class="title"><a href="#" class="dropbtn">' + item.categoryTitle + '</a> (' + item.bookQuantity + ') Category Fine: $' + item.bookFine + '</div>';
        str += '<ul class="books">';

        if (item.bookList.length > 0) {
            str += formatSubItem(item.bookList);
        }
        str += '</ul></div>';
    });
    return str;
}

function formatSubItem(array) {
    var str = "";
    $.each(array, function (key, itemBookList) {
        var d = Date.parse(itemBookList.dueDate);
        var _date = '';
        var _class = '';
        if ((d != undefined) && (!isNaN(d)) )
        {
            var date = new Date(d);
            _date = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();

            var today = new Date();
            if (+date > +today)
            {
                _class = 'class=green';
            }
            else if (date.setHours(0, 0, 0, 0) === today.setHours(0, 0, 0, 0)) {
                _class = 'class=orange';
            }
            else {
                _class = 'class=red';
            }
        }
        //addClass
        str += '<li ' + _class + '>' + itemBookList.title +  itemBookList.author + '  ' + itemBookList.isbn + '  ' + _date + '</li>';

    });
    return str;
}

