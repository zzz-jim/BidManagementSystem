function addBidSection(listCount) {
    if (listCount <= 0) {
        listCount = 1;
    }
    listCount += 1;
    var count = 1;
    $("#count").val(count);
    $("#add").click(function () {
        var flag = true;
        $("#tb1 tr:last-child td").each(function () {
            if ($(this).index() <= 6) {
                //前两列
                if ($(this).children("span").length > 0) {
                    if ($(this).children("span").children("span").children().val() == "") {
                        flag = false;
                    }
                }
                //3~7列
                else {
                    if ($(this).children().val() == "") {
                        flag = false;
                    }
                }
            }
        });

        if (flag == false) {
            alert("请完整填写每个标段的编号和名称！");
        }

        else {
            $("#tb1").append('<tr><td><input style="width:100px" name = "item' + listCount + 'SectionId" id="item' + listCount + 'SectionId" type="hidden" /><input style="width:100px" name = "item' + listCount + 'SectionName" id="item' + listCount + 'SectionName" type="text" /></td><td><input style="width:100px" name = "item' + listCount + 'SectionNumber" id="item' + listCount + 'SectionNumber" type="text" /></td><td><input style="width:160px" name = "item' + listCount + 'CreatedTime" id="item' + listCount + 'CreatedTime" type="text" /></td></tr>>');
            $("#item" + listCount + "CreatedTime").kendoDateTimePicker(
                {
                    format: "yyyy-MM-dd HH:mm",
                    timeFormat: "HH:mm" //24小时制格式
                }
            );

            listCount++;
            count++;
            $("#count").val(count);
        }
        $("#count").val(count);
    });

    $("input[name='Reported'],input[name='Save'] ").click(function () {

        var errorMessage = "";
        //if ($("#WenHao").val().trim() == "") {
        //    errorMessage = errorMessage + "项目名称不能为空！";
        //}

        //if ($("#BeiYong1").val().trim() == "") {
        //    errorMessage = errorMessage + "_目编号不能为空！";
        //}

        //if ($("#Text111111111").val().trim() == "") {
        //    errorMessage = errorMessage + "项目内容不能为空！";
        //}

        //if ($("#Date222222222").val().trim() == "") {
        //    errorMessage = errorMessage + "委托日期不能为空！";
        //}

        //if ($("#Text333333333").val().trim() == "") {
        //    errorMessage = errorMessage + "开标场所不能为空！";
        //}

        //if ($("#Text444444444").val().trim() == "") {
        //    errorMessage = errorMessage + "采购单位不能为空！";
        //}

        //if ($("#Text555555555").val().trim() == "") {
        //    errorMessage = errorMessage + "采购单位联系人不能为空！";
        //}

        //if ($("#Text666666666").val().trim() == "") {
        //    errorMessage = errorMessage + "采购单位联系电话不能为空！";
        //}

        //if ($("#Text777777777").val().trim() == "") {
        //    errorMessage = errorMessage + "采购单位联系地址不能为空！";
        //}

        //if ($("#Text888888888").val().trim() == "") {
        //    errorMessage = errorMessage + "项目审批（核准）单位不能为空！";
        //}

        //if ($("#Text999999999").val().trim() == "") {
        //    errorMessage = errorMessage + "项目审批（核准）文号不能为空！";
        //}

        //if ($("#Text111111112").val().trim() == "") {
        //    errorMessage = errorMessage + "项目备案单位不能为空！";
        //}

        //if ($("#Text111111113").val().trim() == "") {
        //    errorMessage = errorMessage + "项目备案文号不能为空！";
        //}

        //if ($("#Text111111114").val().trim() == "") {
        //    errorMessage = errorMessage + "采购预算不能为空！";
        //}

        //if ($("#Text111111115").val().trim() == "") {
        //    errorMessage = errorMessage + "项目负责人不能为空！";
        //}

        //if ($("#Text111111116").val().trim() == "") {
        //    errorMessage = errorMessage + "项目负责人电话不能为空！";
        //}

        //if ($("#item1SectionName").val().trim() == "") {
        //    errorMessage = errorMessage + "项目至少包含一个标段！";
        //}

        //if ($("#item1SectionNumber").val().trim() == "") {
        //    errorMessage = errorMessage + "项目至少包含一个标段编号！";
        //}

        if (errorMessage != "") {
            alert(errorMessage);
            return false;
        }

        //isOrNotReport为true，就提交表单
        var isOrNotReport = true;
        $("#tb1 tr").each(function () {
            //从第二行开始
            if ($(this).index() > 0) {
                //flag为false代表本行没有值，否则，代表本行填写的有值
                var flag = false;
                $(this).children("td").each(function () {
                    if ($(this).children("span").length > 0) {
                        if ($(this).children("span").children("span").children().val() != "") {
                            flag = true;
                        }
                    }
                    else {
                        if ($(this).children().val() != "") {
                            flag = true;
                        }
                    }
                });
                //若本行未填写数据，则删除本行
                if (flag == false) {
                    $(this).remove();
                    listCount--;
                    count--;
                    $("#count").val(count);
                }
                //若本行有数据，数据不完整，则给出提示
                else {
                    var isAllData = true;//为true表示前7列灰填写有数据，否则表示前7列数据未填写完整
                    $(this).children("td").each(function () {
                        if ($(this).index() <= 6) {
                            //前两列
                            if ($(this).children("span").length > 0) {
                                if ($(this).children("span").children("span").children().val() == "") {
                                    isAllData = false;
                                }
                            }
                            //3~7列
                            else {
                                if ($(this).children().val() == "") {
                                    isAllData = false;
                                }
                            }
                        }
                    });
                    if (isAllData == false) {
                        alert("请完整填写每个标段的编号和名称！");
                        isOrNotReport = false;
                    }
                }
            }
        });

        //提交前重新设置现有表中有效行的input中的Id和Name值
        //获取表行数
        var rows_Count = $("#tb1 tr").size();
        //name,id属性值的左半部分
        var Attr_left = "item";
        //name,id属性值的右半部分
        var Attr_right = [];
        Attr_right.push("SectionName");
        Attr_right.push("SectionNumber");
        //只对表行数大于1时重新设置input中的Id和Name值
        if (rows_Count > 1) {
            for (var i = 1; i < rows_Count; i++) {
                for (var j = 0; j < 9; j++) {
                    //前两列
                    if (j == 0 || j == 1) {
                        //设置ID属性
                        $("#tb1 tr").eq(i).children("td").eq(j).children("span").children("span").children("input").attr("id", Attr_left + i + Attr_right[j]);
                        //设置name属性
                        $("#tb1 tr").eq(i).children("td").eq(j).children("span").children("span").children("input").attr("name", Attr_left + i + Attr_right[j]);
                    }
                    //后7列
                    else {
                        //设置ID属性
                        $("#tb1 tr").eq(i).children("td").eq(j).children("input").attr("id", Attr_left + i + Attr_right[j]);
                        //设置name属性
                        $("#tb1 tr").eq(i).children("td").eq(j).children("input").attr("name", Attr_left + i + Attr_right[j]);
                    }
                }
            }
        }

        $("#count").val(rows_Count - 1);
        if (isOrNotReport == false) {
            return false;
        }
    });
}