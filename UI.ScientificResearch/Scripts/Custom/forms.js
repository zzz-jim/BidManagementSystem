//异步提交表单
function submitForm(form, url) {
    $(form).submit(function () {
        jQuery.ajax({
            url: url,
            data: $(form).serialize(),
            type: "POST",
            beforeSend: function () {
                //在异步提交成功前要做的操作
            },
            success: function (data) {
                if (data > 0) {
                    $("#Reported").attr("disabled", "disabled");
                    alert("操作成功");
                    parent.location.reload();//刷新页面
                }
                else {
                    $("#Save").attr("disabled", "disabled");
                    alert("操作失败");

                    parent.location.reload();//刷新页面
                }
            }
        });
        return false;
    });
}