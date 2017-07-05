function UploadFile(){
    var fuJian = "";
    // 判断选择的文件的大小是否超过限制100M
    function CheckFileSize() {
        //var file = $("#uploadFile");
        //$("#uploadFile").val().length
        var file = document.getElementById("uploadFile");
        var fileSize = file.files.item(0).size / (1024 * 1024);
        if (fileSize > 100) {
            alert("上传的文件限制在100M，请重新选择要上传的文件！");
            document.getElementById("uploadFile").value = "";
        }
    }


    $(function () {
        $("#btnUploadFile").click(function () {
            if ($("#uploadFile").val().length > 0) {
                ajaxFileUpload();
            }
            else {
                alert("请选择要上传的文件！！");
            }
        })
    })

    function ajaxFileUpload() {
        $.ajaxFileUpload
        (
            {
                url: '/FileService/Upload', //用于文件上传的服务器端请求地址
                type: 'post',
                data: { Id: '123', name: 'lunis' }, //此参数非常严谨，写错一个引号都不行
                secureuri: false, //一般设置为false
                fileElementId: 'uploadFile', //文件上传空间的id属性  <input type="file" id="uploadFile" name="uploadFile" />
                dataType: 'json', //返回值类型 一般设置为json
                success: function (data, status) //服务器成功响应处理函数
                {
                    if (data.isSuccessful) {
                        alert("Successfully!!!");
                        fuJian = $("#FuJianList").val();
                        $("#areadyuploadfile").append("<p><a href=/FileService/Download?fileName=" + data.name + ">" + data.name + "</a></p>");
                        //附件路径
                        fuJian = fuJian + data.Name + "\\\\" + data.name + ";";
                        $("#FuJianList").attr("value", fuJian);
                    } else {
                        alert(data.name + "上传失败，请重新操作");
                    }

                    if (typeof (data.error) != 'undefined') {
                        if (data.error != '') {
                            alert(data.error);
                        }
                    }
                },
                error: function (data, status, e) //服务器响应失败处理函数
                {
                    alert(e);
                }
            }
        )

        return false;
    }

    $(document).ready(function () {
        filename = "";
        //如果数据库的FuJianList有值
        if ($("#FuJianList").val() != undefined) {
            filename = $("#FuJianList").val();
        }
        var fujian = "";
        var fujian = filesOfSomeone(filename);
        if (fujian[0] != "") {
            fuJians = fujian[0].split(";");//以分号(;)分隔字符串(申请附件)
            $("#fuJian_Save_P").text("申请人 " + fuJians[0].split("\\")[0] + " 附件:");
            var length = fuJians.length;//获取申请附件的个数
            for (var i = 0; i < length - 1; i++) {
                $("#table_save").append("<p><a href=/FileService/Download2?Path=" + fuJians[i] + ">" + fuJians[i].split("\\")[2] + "</a></p>");
            }
        }
    });
}
