const API_BASE_URL = 'https://localhost:7207/api';
const BOOKS_API = API_BASE_URL + '/Book';
const AUTH_API = API_BASE_URL + '/Auth';
const LOAN_API = API_BASE_URL + '/Loan';

const IMAGE_BASE_URL = 'https://localhost:7207';

function setAuthToken(token) {
    localStorage.setItem('authToken', token);
}

function getAuthToken() {
    return localStorage.getItem('authToken');
}

function removeAuthToken() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('isAdmin'); 
}

function setIsAdmin(isAdmin) {
    localStorage.setItem('isAdmin', isAdmin ? 'true' : 'false');
}

function getIsAdmin() {
    return localStorage.getItem('isAdmin') === 'true';
}

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

function formatDate(dateString) {
    if (!dateString) return 'نامشخص';

    let cleanedDateString = dateString;
    if (dateString.length === 10 && dateString.includes('-')) {
        cleanedDateString = dateString + 'T00:00:00Z';
    } else if (dateString.length > 10 && !dateString.includes('T')) {
        cleanedDateString = dateString.replace(' ', 'T') + 'Z';
    }

    const date = new Date(cleanedDateString);

    if (isNaN(date.getTime())) {
        console.error("خطا در تجزیه تاریخ:", dateString, "->", cleanedDateString);
        return 'تاریخ نامعتبر';
    }

    return date.toLocaleDateString('fa-IR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
}


function loadBooks() {
    apiRequest(BOOKS_API + '/GetAll', 'GET')
        .done(function (books) {
            const container = $('#books-container');
            container.empty();

            if (books && books.length > 0) {
                const baseUrl = IMAGE_BASE_URL.replace(/\/$/, ''); 

                books.forEach(book => {
                    let imageUrl = `${baseUrl}/images/placeholder.jpg`; 

                    const bookImagePath = book.imageURL || book.coverImageUrl;

                    if (bookImagePath && bookImagePath.length > 5) {
                        if (bookImagePath.startsWith('http')) {
                            imageUrl = bookImagePath;
                        } else {
                            const path = bookImagePath.replace(/^[\\/]/, '').replace(/^images\//i, '');

                            imageUrl = `${baseUrl}/Images/${path}`;
                        }
                    }

                    const bookCard = `
                        <div class="book-card" data-book-id="${book.id}">
                            <img src="${imageUrl}" alt="${book.title}" 
                                // **استفاده از آدرس کامل و صحیح برای fallback**
                                onerror="this.onerror=null;this.src='${baseUrl}/Images/placeholder.jpg';">
                            <h3>${book.title}</h3>
                            <p>${book.author}</p>
                            <button class="loan-btn" ${!getAuthToken() ? 'disabled' : ''}>قرض گرفتن</button>
                        </div>
                    `;
                    container.append(bookCard);
                });
            }
            else {
            }
        });
}

function openAuthModal(mode = 'login') {
    $('#auth-modal').css('display', 'block');
    if (mode === 'login') {
        $('#modal-title').text('ورود');
        $('#login-form').show();
        $('#signin-form').hide();
        $('#switch-to-login').hide();
        $('#switch-to-signin').show();
    } else {
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

function updateNav() {
    const dynamicLinks = $('#dynamic-links-container');
    dynamicLinks.empty(); 

    if (getAuthToken()) {
        if (getIsAdmin()) {
            dynamicLinks.append('<li><a href="#" id="admin-link">پنل مدیریت</a></li>');
        }

        if (!getIsAdmin()) {
            dynamicLinks.append('<li><a href="#" id="dashboard-link">حساب کاربری</a></li>');
        }

        dynamicLinks.append('<li><a href="#" id="logout-btn">خروج</a></li>');
    } else {
        dynamicLinks.append('<li><a href="#" id="login-link">ورود</a></li>');
        dynamicLinks.append('<li><a href="#" id="signin-link">ثبت‌نام</a></li>');
    }

    $('.loan-btn').prop('disabled', !getAuthToken());
}

function loanBook(bookId) {
    if (getIsAdmin()) {
        alert('❌ ادمین عزیز، شما نمی‌توانید از طریق پنل اصلی کتاب قرض بگیرید. لطفاً از حساب کاربری عادی خود استفاده کنید.');
        return;
    }

    const loanData = { bookId: bookId };

    apiRequest(LOAN_API + '/LoanBook', 'POST', loanData, true)
        .done(function (response) {
            alert('✅ کتاب با موفقیت قرض گرفته شد!');
            loadBooks();
        })
        .fail(function (xhr) {
            let errorMessage = 'خطا در قرض گرفتن کتاب. لطفاً دوباره تلاش کنید.';
            if (xhr.status === 401) {
                errorMessage = 'برای قرض گرفتن باید وارد شوید.';
            } else if (xhr.responseJSON && xhr.responseJSON.message) {
                errorMessage = 'خطا: ' + xhr.responseJSON.message;
            }
            alert(errorMessage);
        });
}

function loadUserLoans() {
    apiRequest(LOAN_API + '/UserLoan', 'GET', null, true)
        .done(function (loans) {
            const container = $('#dashboard-content');
            container.empty();

            const activeLoans = loans.filter(loan => loan.status === 1);

            if (activeLoans && activeLoans.length > 0) {
                let html = '<h2>کتاب‌های امانتی شما:</h2>';
                html += '<ul class="loan-list">';

                activeLoans.forEach(loan => {
                    html += `<li data-loan-id="${loan.id}" data-book-id="${loan.bookId}">`;
                    html += `کتاب: <strong>${loan.bookTitle}</strong> (تاریخ امانت: ${formatDate(loan.loanDate)})`;

                    html += `<button class="return-btn" data-loan-id="${loan.id}">بازگرداندن</button>`;

                    html += '</li>';
                });
                html += '</ul>';
                container.html(html);

                $('#dashboard-modal').css('display', 'block');

            } else {
                container.html('<h2>کتاب‌های امانتی شما:</h2><p>شما در حال حاضر هیچ کتابی را به امانت نگرفته‌اید.</p>');
                $('#dashboard-modal').css('display', 'block');
            }
        })
        .fail(function (xhr) {
            if (xhr.status === 401) {
                alert('لطفاً ابتدا وارد حساب کاربری خود شوید.');
            } else {
                alert('خطا در بارگذاری امانت‌ها. شاید داده‌های امانت شما مشکل دارد.');
                $('#dashboard-modal').css('display', 'block');
                $('#dashboard-content').html('<h2>خطا در بارگذاری</h2><p style="color:red;">خطا در دریافت اطلاعات امانت از سرور.</p>');
            }
        });
}

function returnBook(loanId) {
    apiRequest(LOAN_API + '/ReturnBook/' + loanId, 'PUT', null, true)
        .done(function (response) {
            alert('کتاب با موفقیت بازگردانده شد.');
            loadUserLoans();
        })
        .fail(function (xhr) {
            alert('خطا در بازگرداندن کتاب. لطفاً دوباره تلاش کنید.');
        });
}

function loadAdminBooks() {
    apiRequest(BOOKS_API + '/GetAll', 'GET')
        .done(function (books) {
            const container = $('#admin-books-container');
            container.empty();

            if (books && books.length > 0) {
                let html = '<table><tr><th>عنوان</th><th>نویسنده</th><th>موجودی</th><th>عملیات</th></tr>';
                books.forEach(book => {
                    html += `
                        <tr>
                            <td>${book.title}</td>
                            <td>${book.author}</td>
                            <td>${book.availableCount}</td>
                            <td>
                                <button class="edit-book-btn" data-book-id="${book.id}">ویرایش</button>
                                <button class="delete-book-btn" data-book-id="${book.id}">حذف</button>
                            </td>
                        </tr>
                    `;
                });
                html += '</table>';
                container.html(html);
            } else {
                container.html('<p>کتابی برای مدیریت وجود ندارد.</p>');
            }
        })
        .fail(function () {
            $('#admin-books-container').html('<p style="color:red;">خطا در بارگذاری لیست کتاب‌ها. (دسترسی ادمین؟)</p>');
        });
}

function saveBook(bookData) {
    const isUpdate = bookData.id;
    let url;
    const method = isUpdate ? 'PUT' : 'POST';

    if (isUpdate) {
        https://localhost:7207/api/Book/
        url = BOOKS_API + '/' + bookData.id; 
    } else {
        url = BOOKS_API + '/Create'; 
    }

    apiRequest(url, method, bookData, true)
        .done(function (response) {
            alert(`کتاب با موفقیت ${isUpdate ? 'به‌روزرسانی' : 'ایجاد'} شد.`);
            loadAdminBooks();
            loadBooks();
            $('#book-form')[0].reset();
            $('#submit-book-btn').text('ذخیره کتاب');
            $('#book-id').val('');
        })
        .fail(function (xhr) {
            let msg = 'خطا در عملیات کتاب.';
            if (xhr.status === 400 && xhr.responseJSON && xhr.responseJSON.errors) {
                msg = "خطا در اعتبارسنجی ورودی‌ها. جزئیات را در کنسول بررسی کنید.";
                console.error("Validation Errors:", xhr.responseJSON.errors); 
            }
            else if (xhr.responseJSON && xhr.responseJSON.message) {
                msg = 'خطا: ' + xhr.responseJSON.message;
            }
            alert(`خطا: ${msg}`);
        });
}

function deleteBook(bookId) {
    if (!confirm('آیا مطمئن هستید که می‌خواهید این کتاب را حذف کنید؟')) return;

    apiRequest(BOOKS_API + '/Delete/' + bookId, 'DELETE', null, true)
        .done(function () {
            alert('کتاب با موفقیت حذف شد.');
            loadAdminBooks();
            loadBooks();
        })
        .fail(function (xhr) {
            let msg = 'خطا در حذف کتاب. شاید هنوز امانت فعال داشته باشد.';
            if (xhr.responseJSON && xhr.responseJSON.message) {
                msg = xhr.responseJSON.message;
            }
            alert(`خطا: ${msg}`);
        });
}

function loadAdminLoans(userId = null) {
    let url = LOAN_API + '/GetAll';

    if (userId) {
        url = LOAN_API + `/UserLoan?id=${userId}`;
    } else {
        url = LOAN_API + '/GetAll';
    }

    apiRequest(url, 'GET', null, true)
        .done(function (loans) {
            const container = $('#admin-loans-container');
            container.empty();

            if (loans && loans.length > 0) {
                let html = '<table><tr><th>کتاب</th><th>کاربر</th><th>وضعیت</th><th>تاریخ امانت</th></tr>';
                loans.forEach(loan => {
                    let statusText;
                    if (loan.status === 1) {
                        statusText = 'فعال';
                    } else if (loan.status === 3) {
                        statusText = 'بازگردانده شده';
                    } else {
                        statusText = 'نامشخص';
                    }
                    html += `
                        <tr>
                            <td>${loan.bookTitle}</td>
                            <td>${loan.userEmail || 'نامشخص'}</td>
                            <td>${statusText}</td>
                            <td>${formatDate(loan.loanDate)}</td>
                        </tr>
                    `;
                });
                html += '</table>';
                container.html(html);
            } else {
                container.html('<p>امانتی برای نمایش یافت نشد.</p>');
            }
        })
        .fail(function (xhr) {
            let msg = 'خطا در بارگذاری امانت‌ها.';

            if (userId) {
                if (xhr.status === 404) {
                    msg = `امانتی برای کاربر با ID ${userId} یافت نشد.`;
                } else if (xhr.responseJSON && xhr.responseJSON.message) {
                    msg = `خطا در جستجو: ${xhr.responseJSON.message}`;
                } else {
                    msg = 'خطای نامشخص در جستجوی امانت کاربر.';
                }
            }
            else {
                if (xhr.status === 403 || xhr.status === 401) {
                    msg = 'شما دسترسی ادمین برای دیدن تمام امانت‌ها را ندارید.';
                } else if (xhr.responseJSON && xhr.responseJSON.message) {
                    msg = `خطا در بارگذاری: ${xhr.responseJSON.message}`;
                } else {
                    msg = 'خطا در بارگذاری تمام امانت‌ها. شاید شما دسترسی ادمین ندارید.';
                }
            }

            $('#admin-loans-container').html(`<p style="color:red;">خطا: ${msg}</p>`);
        });
}

function openAdminModal() {
    $('#admin-modal').css('display', 'block');
    loadAdminBooks();
    loadAdminLoans();
}

$(document).ready(function () {
    loadBooks();
    updateNav();

    $(document).on('click', '#explore-btn', function () {
        $('html, body').animate({
            scrollTop: $('.book-list').offset().top
        }, 800);
    });

    $('#books-container').on('click', '.loan-btn:not(:disabled)', function () {
        const bookId = $(this).closest('.book-card').data('book-id');
        loanBook(bookId);
    });

    $(document).on('click', '.return-btn', function (e) {
        e.preventDefault();
        const loanId = $(this).data('loan-id');
        if (confirm("آیا مطمئن هستید که می‌خواهید این کتاب را بازگردانید؟")) {
            returnBook(loanId);
        }
    });

    $(document).on('click', '#login-link', function (e) {
        e.preventDefault();
        openAuthModal('login');
    });

    $(document).on('click', '#signin-link', function (e) {
        e.preventDefault();
        openAuthModal('signin');
    });

    $(document).on('click', '#dashboard-link', function (e) {
        e.preventDefault();
        loadUserLoans();
    });

    $(document).on('click', '#admin-link', function (e) {
        e.preventDefault();
        if (getIsAdmin()) {
            openAdminModal();
        } else {
            alert('شما دسترسی مدیریت ندارید.');
        }
    });

    $('.close-btn').on('click', function () {
        $('#auth-modal').css('display', 'none');
        $('#dashboard-modal').css('display', 'none');
        $('#admin-modal').css('display', 'none');
    });


    $('#switch-to-signin').on('click', function (e) {
        e.preventDefault();
        openAuthModal('signin');
    });

    $('#switch-to-login').on('click', function (e) {
        e.preventDefault();
        openAuthModal('login');
    });

    $(document).on('click', '#logout-btn', function (e) {
        e.preventDefault();
        removeAuthToken();
        updateNav();
        alert('با موفقیت خارج شدید.');
        loadBooks();
    });

    $('#login-form').on('submit', function (e) {
        e.preventDefault();
        const email = $('#login-email').val();
        const password = $('#login-password').val();

        apiRequest(AUTH_API + '/Login', 'POST', { email, password })
            .done(function (response) {
                if (response.token) {
                    setAuthToken(response.token);

                    const isAdmin = response.isAdmin === true || response.isAdmin === 'true';
                    setIsAdmin(isAdmin);

                    updateNav();
                    closeAuthModal();
                    alert('ورود موفقیت‌آمیز بود!');
                    loadBooks();
                } else {
                    alert('خطا در ورود: توکنی دریافت نشد.');
                }
            })
            .fail(function (xhr) {
                alert('ایمیل یا رمز عبور اشتباه است.');
            });
    });

    $('#signin-form').on('submit', function (e) {
        e.preventDefault();

        const firstName = $('#signin-firstname').val();
        const lastName = $('#signin-lastname').val();
        const email = $('#signin-email').val();
        const phoneNumber = $('#signin-phone').val();
        const password = $('#signin-password').val();

        apiRequest(AUTH_API + '/SignUp', 'POST', { firstName, lastName, email, phoneNumber, password })
            .done(function (response) {
                alert('ثبت‌نام با موفقیت انجام شد. حالا وارد شوید.');
                openAuthModal('login');
            })
            .fail(function (xhr) {
                let errorMessage = 'خطا در ثبت‌نام. ممکن است ایمیل، شماره تلفن یا نام کاربری تکراری باشد.';
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    errorMessage = 'خطا در ثبت‌نام: ' + xhr.responseJSON.message;
                }
                alert(errorMessage);
            });
    });

    $(document).on('click', '.tab-button', function () {
        $('.tab-button').removeClass('active');
        $(this).addClass('active');

        $('.tab-content').removeClass('active').hide();
        const tabId = $(this).data('tab');
        $('#' + tabId).addClass('active').show();
    });

    $('#book-form').on('submit', function (e) {
        e.preventDefault();

        const bookData = {
            id: parseInt($('#book-id').val()) || 0,
            title: $('#book-title-admin').val(),
            author: $('#book-author-admin').val(),
            imageURL: $('#book-imageurl-admin').val(), 
            availableCount: parseInt($('#book-count-admin').val()),
            description: $('#book-description-admin').val()
        };
        if (bookData.id === 0) delete bookData.id;

        saveBook(bookData);
    });

    $(document).on('click', '.edit-book-btn', function () {
        const bookId = $(this).data('book-id');
        alert(`برای ویرایش کتاب با ID: ${bookId} مقادیر را دستی پر کنید.`);
        $('#book-id').val(bookId);
        $('#submit-book-btn').text('به‌روزرسانی کتاب');
    });

    $(document).on('click', '.delete-book-btn', function () {
        const bookId = $(this).data('book-id');
        deleteBook(bookId);
    });

    $(document).on('click', '#load-all-loans-btn', function () {
        $('#loan-user-id').val('');
        loadAdminLoans(null);
    });

    $(document).on('click', '#load-user-loans-btn', function () {
        const userId = parseInt($('#loan-user-id').val());
        if (!isNaN(userId) && userId > 0) {
            loadAdminLoans(userId);
        } else {
            alert('لطفاً یک ID کاربر معتبر وارد کنید.');
        }
    });
});