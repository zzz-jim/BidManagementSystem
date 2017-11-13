
//检查上传文件的大小
function CheckFileSize() {
    var file = document.getElementById("uploadFile");
    var fileSize = 0;
    var filepath = file.value;
    var filetypes = [".rar", ".txt", ".zip", ".doc", ".ppt", ".xls", ".pdf", ".docx", ".xlsx", ".rar", ".7Z", ".cab", ".iso", ".pdf"];

    var filemaxsize = 1024 * 10;//10M
    if (filepath) {
        var isnext = false;
        var fileend = filepath.substring(filepath.lastIndexOf("."));
        if (filetypes && filetypes.length > 0) {
            for (var i = 0; i < filetypes.length; i++) {
                if (filetypes[i] == fileend) {
                    isnext = true;
                    break;
                }
            }
        }
        if (!isnext) {
            alert("不接受此文件类型！");
            document.getElementById("uploadFile").value = "";
            return false;
        }
    } else {
        return false;
    }

    var isIE = /msie/i.test(navigator.userAgent) && !window.opera;
    if (isIE && !file.files) {
        var filePath = file.value;
        var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
        if (!fileSystem.FileExists(filePath)) {
            alert("附件不存在，请重新输入！");
            return false;
        }
        var file = fileSystem.GetFile(filePath);
        fileSize = file.Size;
    } else {
        fileSize = file.files[0].size;
    }

    var size = fileSize / 1024;
    if (size > filemaxsize) {
        alert("附件大小不能大于" + filemaxsize / 1024 + "M！");
        document.getElementById("uploadFile").value = "";
        return false;
    }
    if (size <= 0) {
        alert("附件大小不能为0M！");
        document.getElementById("uploadFile").value = "";
        return false;
    }
}
//填写申请书过程中已有文件
function areadHaveFillIn() {
    filename = "";
    //如果数据库的FuJianList有值
    if ($("#FuJianList").val() != undefined) {
        filename = $("#FuJianList").val();
    }
    var fujian = "";
    var fujian = filesOfSomeone(filename);
    if (fujian.length > 0) {
        fuJians = fujian[0].split(";");//以分号(;)分隔字符串(申请附件)
        $("#fuJian_Save_P").text("申请人 " + fuJians[0].split("\\")[0] + " 附件:");
        var length = fuJians.length;//获取申请附件的个数
        for (var i = 0; i < length - 1; i++) {
            $("#table_save").append("<p><a href=/FileService/Download2?Path=" + fuJians[i] + ">" + fuJians[i].split("\\")[2] + "</a></p>");
        }
    }
};
//填写申请书新增文件使用
function ajaxFileUploadFillIn() {

    $.ajaxFileUpload
        (
        {
            url: '/FileService/Upload', //用于文件上传的服务器端请求地址
            type: 'post',
            secureuri: false, //一般设置为false
            fileElementId: 'uploadFile', //文件上传空间的id属性  <input type="file" id="uploadFile" name="uploadFile" />
            dataType: 'json', //返回值类型 一般设置为json
            success: function (data, status) //服务器成功响应处理函数
            {
                if (data.isSuccessful) {
                    alert("文件上传成功！");
                    // TODO: jim
                    //$("#downloadFile").text(data.name);
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
//填写申请书新增文件使用
function ajaxFileUploadFillInWithData(data) {

    $.ajaxFileUpload
        (
        {
            data: data,// TODO: 放入applicationId
            url: '/FileService/Upload', //用于文件上传的服务器端请求地址
            type: 'post',
            secureuri: false, //一般设置为false
            fileElementId: 'uploadFile', //文件上传空间的id属性  <input type="file" id="uploadFile" name="uploadFile" />
            dataType: 'json', //返回值类型 一般设置为json
            success: function (data, status) //服务器成功响应处理函数
            {
                if (data.isSuccessful) {
                    alert("文件上传成功！");
                    // TODO: jim
                    //$("#downloadFile").text(data.name);
                    $("#areadyuploadfile").append("<p><a href=/FileService/Download?fileName=" + data.name + ">" + data.name + "</a></p>");
                    //附件路径
                    fuJian = fuJian + data.Name + "\\\\" + data.name + ";";
                    $("#FuJianList").attr("value", fuJian);
                    location.reload();//刷新当前页面
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
//审批的时候使用
function ajaxFileUploadAgree(currentnode, allcount) {
    $.ajaxFileUpload
        (
        {
            url: '/FileService/Upload', //用于文件上传的服务器端请求地址
            type: 'post',
            secureuri: false, //一般设置为false
            fileElementId: 'uploadFile', //文件上传空间的id属性  <input type="file" id="uploadFile" name="uploadFile" />
            dataType: 'json', //返回值类型 一般设置为json
            success: function (data, status) //服务器成功响应处理函数
            {
                if (data.isSuccessful) {
                    alert("文件上传成功！");
                    // TODO: jim
                    $("#uploadingfile").append("<p><a href=/FileService/Download?fileName=" + data.name + ">" + data.name + "</a></p>");
                    //附件路径

                    for (var i = 0; i <= parseInt(allcount); i++) {
                        if (i == currentnode) {
                            if (thisPageFuJian.indexOf("~" + i + "#") >= 0) {
                                thisPageFuJian = thisPageFuJian + data.Name + "\\\\" + data.name + ";";
                            }
                            else {
                                thisPageFuJian = thisPageFuJian + "~" + i + "#" + data.Name + "\\\\" + data.name + ";";
                            }
                        }

                    }
                    $("#FuJianList").attr("value", thisPageFuJian);
                }
                else {
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
//审批过程中已有的文件
function fileListInitializeAgree(currentnode, allcount) {   //初始化页面
    //定义全局变量
    thisPageFuJian = "";
    //如果数据库的FuJianList有值
    if ($("#thisPageFuJian").val() != undefined) {
        thisPageFuJian = $("#thisPageFuJian").val();
    }
    //如果数据库的FuJianList无值
    else {
        thisPageFuJian = "";
    }


    var tableId = new Array();
    //将每个人的文件路径放在fuJian数组中
    var fuJian = filesOfSomeone(thisPageFuJian, currentnode);
    for (var i = 0; i < parseInt(allcount); i++) {
        //第一个节点不执行任何操作
        if (i == 0) {

        }
        else {
            //当前节点JieDianName与某一个节点的NodeName相同，
            if (currentnode == parseInt(i) + 1) {
                var k;
                for (k = 0; k < parseInt(i) + 1; k++) {
                    var pid = "p" + k + "";
                    var divid = "table_Applicant" + k + "";

                    $("#uploadingfile").after('<p id=' + pid + '></p><div id=' + divid + '>');

                    tableId[k] = "#" + "table_Applicant" + k;
                }
            }
        }
    }
    for (var j = 0; j < (parseInt(currentnode)); j++) {
        var fuJians = "";
        if (fuJian[j] != "") {
            fuJians = fuJian[j].split(";");
            //以分号(;)分隔字符串(申请附件)
            if (j == 0) {
                $("#p" + j.toString()).text("申请人 " + fuJians[0].split("\\")[0] + " 附件:");
            }
            else {
                $("#p" + j.toString()).text("审批人 " + fuJians[0].split("\\")[0] + " 附件:");
            }
            var length = fuJians.length;//获取申请附件的个数
            for (var l = 0; l < length - 1; l++) {
                $(tableId[j]).append("<p><a href=/FileService/Download2?Path=" + fuJians[l] + ">" + fuJians[l].split("\\")[2] + "</a></p>");
            }
        }
    }
}
//审批过程中已有的审批意见
function shenPiyiJianListinitialize() {
    var shenpiyijianlist = $("#ShenPiYiJian").val().split("|");
    var html = "";
    for (var i = 0; i < shenpiyijianlist.length; i++) {
        html = html + "<p>" + shenpiyijianlist[i] + "</p>";
    }
    $("#ShenPiYiJianList").append(html);
};

//截取字符串中~，!，^，&这四个字符之间的各个部分）例：11~22!33^44&55 返回数组("11","22","33","44","55")
function filesOfSomeone(str, currrentnode) {
    //审批人的个数
    var approvalpersoncount = parseInt(currrentnode);
    var strarray = new Array();
    //每个人的文件装一个数组项
    for (var i = 0; i < approvalpersoncount; i++) {
        var firstlevaldiffrencechar = "~";
        if (i == 0) {
            if (str.indexOf(firstlevaldiffrencechar) != -1) {
                strarray[i] = str.substring(0, str.indexOf(firstlevaldiffrencechar));
            }
            else {
                strarray[i] = str;
            }
        }
        else {
            var startlevaldiffrencechar = "~" + (parseInt(i) + 1) + "#";
            var endlevaldiffrencechar = "~" + (parseInt(i) + 2) + "#";
            if (str.indexOf(startlevaldiffrencechar) != -1 && str.indexOf(endlevaldiffrencechar) != -1) {
                strarray[i] = str.substring(str.indexOf(startlevaldiffrencechar), str.indexOf(endlevaldiffrencechar)).replace(startlevaldiffrencechar, "");;
            }
            else if (str.indexOf(startlevaldiffrencechar) != -1 && str.indexOf(endlevaldiffrencechar) == -1) {
                strarray[i] = str.substring(str.indexOf(startlevaldiffrencechar), str.length).replace(startlevaldiffrencechar, "");
            }
            else {
                strarray[i] = "";
            }
        }
    }
    return strarray;
}
//当前节点的审批角色是否与登录者角色是否对应，决定是否能审批或查看
function AuthorityAboutSeeOrClick(isRightPersonApproval) {
    if (isRightPersonApproval == "True") {

    }
    else {
        $("input[name='Approval']").hide();
        $("input[name='Reject']").hide();
        $("#uploadFile").hide();
        $("#btnUploadFile").hide();
    }
};