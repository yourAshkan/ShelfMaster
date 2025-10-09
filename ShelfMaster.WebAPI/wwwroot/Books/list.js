$(document).ready(function () {

    // فرض کنیم نقش کاربر از سرور گرفته شده
    let userRole = "Admin"; // یا "User"

    // نمایش فرم اضافه کردن کتاب فقط برای Admin
    if (userRole === "Admin") {
        $("#addBookForm").show();
    }

    // تابع گرفتن لیست کتاب‌ها
    function getBooks() {
        $.ajax({
            url: "/api/Book/GetAll",
            method: "GET",
            success: function (books) {
                $("#bookList ul").empty();
                books.forEach(book => {
                    let li = `<li class='bookItem'>
                                <span><strong>${book.title}</strong> - ${book.author}</span>`;
                    if (userRole === "Admin") {
                        li += `<button class='deleteBookBtn' data-id='${book.id}'>حذف</button>`;
                    }
                    li += `</li>`;
                    $("#bookList ul").append(li);
                });
            },
            error: function (err) {
                console.error(err);
            }
        });
    }

    // اجرای اولیه
    getBooks();

    // اضافه کردن کتاب
    $("#addBookBtn").click(function () {
        let title = $("#bookTitle").val();
        let author = $("#bookAuthor").val();

        if (title === "" || author === "") {
            alert("عنوان و نویسنده را وارد کنید!");
            return;
        }

        $.ajax({
            url: "/api/Book/CreateBook",
            method: "POST",
            data: { title, author },
            success: function () {
                $("#bookTitle, #bookAuthor").val("");
                getBooks();
            },
            error: function (err) {
                console.error(err);
            }
        });
    });

    // حذف کتاب
    $(document).on("click", ".deleteBookBtn", function () {
        let id = $(this).data("id");
        $.ajax({
            url: "/api/Book/DeleteBook/" + id,
            method: "DELETE",
            success: function () {
                getBooks();
            },
            error: function (err) {
                console.error(err);
            }
        });
    });
});
