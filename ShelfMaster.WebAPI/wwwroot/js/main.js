// آدرس پایه API (این رو بر اساس تنظیمات بک‌اندت تنظیم کن)
const API_BASE_URL = 'https://localhost:7207/api';
const BOOKS_API = API_BASE_URL + '/Book';
const AUTH_API = API_BASE_URL + '/Auth';
const LOAN_API = API_BASE_URL + '/Loan';

// توابع کمکی برای ذخیره و بازیابی توکن (با استفاده از localStorage)
function setAuthToken(token) {
    localStorage.setItem('authToken', token);
}

function getAuthToken() {
    return localStorage.getItem('authToken');
}

function removeAuthToken() {
    localStorage.removeItem('authToken');
}

// تابع کمکی برای انجام درخواست‌های AJAX
function apiRequest(url, method, data = null, needsAuth = false) {
    const settings = {
        url: url,
        method: method,
        dataType: 'json',
        contentType: 'application/json'
    };

    if (data) {
        settings.data = JSON.stringify(data);
    }

    if (needsAuth && getAuthToken()) {
        settings.headers = {
            'Authorization': 'Bearer ' + getAuthToken()
        };
    }

    return $.ajax(settings);
}

// 1. بارگذاری کتاب‌ها در صفحه اصلی
function loadBooks() {
    apiRequest(BOOKS_API + '/GetAll', 'GET')
        .done(function (books) {
            const container = $('#books-container');
            container.empty(); // خالی کردن محتوای قبلی

            if (books && books.length > 0) {
                books.forEach(book => {
                    const bookCard = `
                        <div class="book-card" data-book-id="${book.id}">
                            <img src="${book.coverImageUrl || 'placeholder.jpg'}" alt="${book.title}">
                            <h3>${book.title}</h3>
                            <p>${book.author}</p>
                            <button class="loan-btn" ${!getAuthToken() ? 'disabled' : ''}>قرض گرفتن</button>
                        </div>
                    `;
                    container.append(bookCard);
                });
            } else {
                container.html('<p>متأسفانه کتابی برای نمایش وجود ندارد.</p>');
            }
        })
        .fail(function (xhr, status, error) {
            console.error("خطا در بارگذاری کتاب‌ها:", error);
            $('#books-container').html('<p>خطا در اتصال به سرور.</p>');
        });
}

// 2. مدیریت رویدادها پس از بارگذاری کامل صفحه
$(document).ready(function () {
    loadBooks(); // بارگذاری اولیه کتاب‌ها

    // وقتی روی دکمه "مشاهده کتاب‌ها" کلیک میشه، اسکرول کن به بخش کتاب‌ها
    $('#explore-btn').on('click', function () {
        $('html, body').animate({
            scrollTop: $('.book-list').offset().top
        }, 800);
    });

    // مدیریت کلیک روی دکمه‌های قرض گرفتن (Delegate Event)
    $('#books-container').on('click', '.loan-btn:not(:disabled)', function () {
        const bookId = $(this).closest('.book-card').data('book-id');
        // در اینجا باید منطق قرض گرفتن کتاب (تماس با LoanBook) پیاده‌سازی شود
        alert('کتاب با شناسه ' + bookId + ' به زودی قرض گرفته خواهد شد. (نیاز به پیاده‌سازی LoanBook)');
    });
});
// توابع نمایش/بستن Modal
function openAuthModal(mode = 'login') {
    $('#auth-modal').css('display', 'block');
    if (mode === 'login') {
        $('#modal-title').text('ورود به ShelfMaster');
        $('#login-form').show();
        $('#signin-form').hide();
        $('#switch-to-login').hide();
        $('#switch-to-signin').show();
    } else { // signin
        $('#modal-title').text('ثبت‌نام');
        $('#login-form').hide();
        $('#signin-form').show();
        $('#switch-to-login').show();
        $('#switch-to-signin').hide();
    }
}

function closeAuthModal() {
    $('#auth-modal').css('display', 'none');
}

// به‌روزرسانی نوار ناوبری بر اساس وضعیت لاگین
function updateNav() {
    if (getAuthToken()) {
        // کاربر لاگین کرده
        $('.navbar nav ul li:nth-child(3)').html('<li><a href="#" id="logout-btn">خروج</a></li>');
        $('.navbar nav ul li:nth-child(4)').html('<li><a href="#">حساب کاربری</a></li>');
    } else {
        // کاربر لاگین نیست
        $('.navbar nav ul li:nth-child(3)').html('<li><a href="#" id="login-link">ورود</a></li>');
        $('.navbar nav ul li:nth-child(4)').html('<li><a href="#" id="signin-link">ثبت‌نام</a></li>');
    }
    // اگر دکمه "قرض گرفتن" در صفحه بود، فعال یا غیرفعال شود
    $('.loan-btn').prop('disabled', !getAuthToken());
}


// --- رویدادها ---
$(document).ready(function () {
    // ... (کدهای قبلی) ...
    loadBooks();
    updateNav(); // در ابتدا نوار ناوبری رو به‌روز کن

    // رویدادهای مربوط به Modal
    $('#login-link').on('click', function (e) {
        e.preventDefault();
        openAuthModal('login');
    });

    $('#signin-link').on('click', function (e) {
        e.preventDefault();
        openAuthModal('signin');
    });

    $('.close-btn').on('click', closeAuthModal);

    $('#switch-to-signin').on('click', function (e) {
        e.preventDefault();
        openAuthModal('signin');
    });

    $('#switch-to-login').on('click', function (e) {
        e.preventDefault();
        openAuthModal('login');
    });

    // رویداد خروج از حساب
    $(document).on('click', '#logout-btn', function (e) {
        e.preventDefault();
        removeAuthToken();
        updateNav();
        alert('با موفقیت خارج شدید.');
        loadBooks(); // رفرش کتاب‌ها برای غیرفعال شدن دکمه قرض
    });


    // مدیریت ارسال فرم ورود (LogIn)
    $('#login-form').on('submit', function (e) {
        e.preventDefault();
        const username = $('#login-username').val();
        const password = $('#login-password').val();

        apiRequest(AUTH_API + '/LogIn', 'POST', { username, password })
            .done(function (response) {
                // فرض می‌کنیم API در پاسخ، یک شیء حاوی token داره
                if (response.token) {
                    setAuthToken(response.token);
                    updateNav();
                    closeAuthModal();
                    alert('ورود موفقیت‌آمیز بود!');
                    loadBooks();
                } else {
                    alert('خطا در ورود: توکنی دریافت نشد.');
                }
            })
            .fail(function (xhr) {
                alert('نام کاربری یا رمز عبور اشتباه است.');
            });
    });

    // مدیریت ارسال فرم ثبت‌نام (SignIn)
    $('#signin-form').on('submit', function (e) {
        e.preventDefault();
        const username = $('#signin-username').val();
        const email = $('#signin-email').val();
        const password = $('#signin-password').val();

        apiRequest(AUTH_API + '/SginIn', 'POST', { username, email, password }) // دقت کن که اسم متد رو SignIn گذاشتی
            .done(function (response) {
                alert('ثبت‌نام با موفقیت انجام شد. حالا وارد شوید.');
                openAuthModal('login');
            })
            .fail(function (xhr) {
                // بهتر است پیام خطای دقیق‌تر از سرور نمایش داده شود
                alert('خطا در ثبت‌نام. ممکن است نام کاربری یا ایمیل تکراری باشد.');
            });
    });
});