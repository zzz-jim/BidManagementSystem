function travelExpenses()
{
        var listCount = 2;
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
                alert("请完整填写每一行前7列数据！");
            }

            else {
                $("#tb1").append('<tr><td><input style="width:100px" name = "item' + listCount + 'StartDate" id="item' + listCount + 'StartDate" type="text" /></td><td><input style="width:100px" name = "item' + listCount + 'EndDate" id="item' + listCount + 'EndDate" type="text" /></td><td><input name = "item' + listCount + 'FromAddress" id="item' + listCount + 'FromAddress" type="text" /></td><td><input name = "item' + listCount + 'ToAddress" id="item' + listCount + 'ToAddress" type="text" /></td><td><input name = "item' + listCount + 'Transportation" id="item' + listCount + 'Transportation" type="text" /></td><td><input onkeyup="if(isNaN(value))execCommand(\'undo\')" onafterpaste="if(isNaN(value))execCommand(\'undo\')"  name = "item' + listCount + 'TransportationFee" id="item' + listCount + 'TransportationFee" type="text" /></td><td><input onkeyup="if(isNaN(value))execCommand(\'undo\')" onafterpaste="if(isNaN(value))execCommand(\'undo\')"  name = "item' + listCount + 'HotelFee" id="item' + listCount + 'HotelFee" type="text" /></td><td><input onkeyup="if(isNaN(value))execCommand(\'undo\')" onafterpaste="if(isNaN(value))execCommand(\'undo\')" name = "item' + listCount + 'OtherFee" id="item' + listCount + 'OtherFee" type="text" /></td><td><input name = "item' + listCount + 'OtherFeeDescription" id="item' + listCount + 'OtherFeeDescription" type="text" /</td></tr>>');

                $("#item" + listCount + "StartDate").kendoDatePicker();
                $("#item" + listCount + "EndDate").kendoDatePicker();

                listCount++;
                count++;
                $("#count").val(count);
            }
            $("#count").val(count);
        });

        $("input[name='Reported'],input[name='Save'").click(function () {
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
                    //若本行为填写数据，则删除本行
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
                            alert("请完整填写每一行前7列数据！");
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
            Attr_right.push("StartDate");
            Attr_right.push("EndDate");
            Attr_right.push("FromAddress");
            Attr_right.push("ToAddress");
            Attr_right.push("Transportation");
            Attr_right.push("TransportationFee");
            Attr_right.push("HotelFee");
            Attr_right.push("OtherFee");
            Attr_right.push("OtherFeeDescription");
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

