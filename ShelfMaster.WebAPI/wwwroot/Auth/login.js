document.addEventListener("DOMContentLoaded", () => {

    const loginForm = document.getElementById("loginForm");
    if (!loginForm) return;

    loginForm.addEventListener("submit", async (e) => {
        e.preventDefault();

        const data = {
            email: document.getElementById("loginEmail").value.trim(),
            password: document.getElementById("loginPassword").value.trim()
        };

        try {
            const res = await fetch("https://localhost:7207/api/Auth/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            });

            if (res.ok) {
                const result = await res.json();
                // فرض می‌کنیم API یک JWT token برمی‌گردونه
                localStorage.setItem("token", result.token);
                alert("ورود موفقیت‌آمیز ✅");
                window.location.href = "../index.html";
            } else {
                const err = await res.json();
                alert(`❌ خطا در ورود: ${err.message || "ایمیل یا رمز اشتباه است"}`);
            }
        } catch (error) {
            alert("❗ ارتباط با سرور برقرار نشد");
            console.error(error);
        }
    });

});
