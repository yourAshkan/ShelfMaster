document.addEventListener("DOMContentLoaded", () => {

    const signupForm = document.getElementById("signupForm");
    if (!signupForm) return;

    signupForm.addEventListener("submit", async (e) => {
        e.preventDefault();

        const data = {
            firstName: document.getElementById("firstName").value.trim(),
            lastName: document.getElementById("lastName").value.trim(),
            email: document.getElementById("email").value.trim(),
            password: document.getElementById("password").value.trim()
        };

        try {
            const res = await fetch("https://localhost:7207/api/Auth/register", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            });

            if (res.ok) {
                alert("ثبت‌نام با موفقیت انجام شد ✅");
                window.location.href = "login.html";
            } else {
                const err = await res.json();
                alert(`❌ خطا در ثبت‌نام: ${err.message || "مشکلی پیش آمده است"}`);
            }
        } catch (error) {
            alert("❗ ارتباط با سرور برقرار نشد");
            console.error(error);
        }
    });

});
