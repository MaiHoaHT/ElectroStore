$(document).ready(function () {
    ShowCountNum();
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quatity = 1;
        var tQuantity = $('#quantity_value').val();
        if (tQuantity != 1 && tQuantity != undefined) {
            quatity = parseInt(tQuantity);
        }
        $.ajax({
            url: '/cart/addtocart',
            type: 'POST',
            data: { id: id, quantity: quatity },
            success: function (rs) {
                if (rs.Success) {
                    $('.qty').html(rs.Count);
                    showNotification("Thêm vào giỏ hàng thành công!", 1500); 
                }
            }
        });
    });
    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();
        var conf = confirm('Bạn có chắc muốn xóa hết sản phẩm trong giỏ hàng?');
        //debugger;
        if (conf == true) {
            DeleteAll();
        }

    });

    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?');
        if (conf == true) {
            $.ajax({
                url: '/cart/Delete',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#checkout_items').html(rs.Count);
                        $('#trow_' + id).remove();
                        LoadCart();
                    }
                }
            });
        }

    });
    $('body').on('change', 'input[name = idquantity]', function () {
        debugger;
        var btn = $(this);
        var id = btn.data('id'); // Lấy id của bản ghi tương ứng
        var value = btn.val();
        if (value < 0 || value == 0) {
            $.ajax({
                url: '/cart/Delete',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#checkout_items').html(rs.Count);
                        $('#trow_' + id).remove();
                        LoadCart();
                    }
                }
            });
        }
        Update(id, value);
    });
});

function ShowCountNum() {
    $.ajax({
        url: '/cart/ShowCountNum',
        type: 'GET',
        success: function (rs) {
            $('.qty').html(rs.Count);
        }
    });
}
function DeleteAll() {
    $.ajax({
        url: '/cart/DeleteAll',
        type: 'POST',
        success: function (rs) {
            if (rs.Success) {
                LoadCart();
            }
        }
    });
}
function Update(id, quantity) {
    $.ajax({
        url: '/cart/Update',
        type: 'POST',
        data: { id: id, quantity: quantity },
        success: function (rs) {
            if (rs.Success) {
                LoadCart();
            }
        }
    });
}

function LoadCart() {
    $.ajax({
        url: '/cart/Show_Cart_Items',
        type: 'GET',
        success: function (rs) {
            $('#load_data').html(rs);
        }
    });
}
function showNotification(message, duration) {
    var notification = document.createElement("div");
    notification.innerText = message;
    notification.classList.add("notification");

    document.body.appendChild(notification);

    setTimeout(function () {
        notification.remove();
    }, duration);
}


