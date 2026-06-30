function addData() {
    console.log("Adding data...");
    clearError();

    if (!validateData()) {
        console.log(validateData());
        return;
    }

    if (typeof grecaptcha !== "undefined") {
        const captcha = grecaptcha.getResponse();

        if (!captcha) {
            alert("Vui lòng xác thực Captcha.");
            return;
        }
    }
    const data = {
        fullName: $("#hoTen").val().trim(),
        phoneNumber: $("#dienThoai").val().trim(),
        email: $("#email").val().trim(),
        address: $("#diaChi").val().trim(),
        title: $("#tieuDe").val().trim(),
        content: $("#noiDung").val().trim(),
        isPublic: $("input[name='congKhai']:checked").val() === "1"
    };

    $.ajax({
        url: "/api/contact",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            alert(response.message || "Gửi thành công");
            resetData();
        },
        error: function (xhr) {
            const message = xhr.responseJSON?.message || "Có lỗi xảy ra";
            alert(message);
        }
    });
}

function validateData() {
    let valid = true;

    if (!$("#hoTen").val().trim()) {
        showError("hoTen", "Vui lòng nhập họ tên");
        valid = false;
    }
    const dienThoai = $("#dienThoai").val().trim();

    if (!/^(0|\+84)(3|5|7|8|9)[0-9]{8}$/.test(dienThoai)) {
        showError("dienThoai", "Số điện thoại không hợp lệ");
        valid = false;
    }
    if (!$("#dienThoai").val().trim()) {
        showError("dienThoai", "Vui lòng nhập số điện thoại");
        valid = false;
    }
    const email = $("#email").val().trim();
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
        showError("email", "Email không hợp lệ");
        valid = false;
    }
    if (!$("#email").val().trim()) {
        showError("email", "Vui lòng nhập email");
        valid = false;
    }
    if (!$("#tieuDe").val().trim()) {
        showError("tieuDe", "Vui lòng nhập tiêu đề");
        valid = false;
    }

    if (!$("#noiDung").val().trim()) {
        showError("noiDung", "Vui lòng nhập nội dung");
        valid = false;
    }
    return valid;
}

function showError(field, message) {
    $(`[data-msgf = '${field}']`).text(message);
}

function clearError() {
    $("[data-msgf]").text("");
}

function resetData() {
    $("#hoTen").val("");
    $("#dienThoai").val("");
    $("#email").val("");
    $("#diaChi").val("");
    $("#tieuDe").val("");
    $("#noiDung").val("");
    $("#coCongKhai").prop("checked", true);
    clearError();

    if (typeof grecaptcha !== "undefined") {
        grecaptcha.reset();
    }
}