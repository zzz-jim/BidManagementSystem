
//js局部打印
//function areaPrintFunEnd() {
//    areaPrintFun();//打印
//    //(以下代码为进入论文登记页面)
//    $.post("/manage/discusspaperControl!printEnding.action?lssId=274", function (data) {
//        //删除loading        
//        $._deLoading();

//        if (data.success) {
//            parent.location.reload();
//        }

//        // 提示信息
//        //		if(parent){
//        //	        top.createPrompt(data.message);
//        //	    }else{
//        //	    	createPrompt(data.message);
//        //	    }
//    });
//}

function areaPrintFun(printDivId) {
    printing($("#" + printDivId + "")).print();
}

function printing(tb) {
    var nw = window.open('', '', 'width=2000,height=600,toolbar=no,menubar=no');
    nw.document.open("text/html", "utf-8");
    nw.document.write("<html><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /><head>");
    //nw.document.write("<link id='setFrameCss' rel='stylesheet' type='text/css' href='/skin/frameCss/frameComm.css' />");
    nw.document.write("</head><body>");
    nw.document.write(tb.html());
    nw.document.write("</body></html>");
    nw.document.close();
    return nw;
}

function printAllData(htmlList)
{
    var nw = window.open('', '', 'width=2000,height=600,toolbar=no,menubar=no');
    nw.document.open("text/html", "utf-8");
    nw.document.write("<html><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /><head>");
    //nw.document.write("<link id='setFrameCss' rel='stylesheet' type='text/css' href='/skin/frameCss/frameComm.css' />");
    nw.document.write("</head><body>");
    nw.document.write(htmlList);
    nw.document.write("</body></html>");
    nw.document.close();
    nw.print();
}

//$("input[type=text]").focus(function () {
//    this.blur();
//});
