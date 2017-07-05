using System;

namespace ScientificResearch.ViewModel
{
    public class ERPRiZhiViewModel
    {
        public ERPRiZhiViewModel()
        {
        }

        public ERPRiZhiViewModel(
            string userName,
            string doSomething,
            string ip,
            string fkFormName,
            string fkAction,
            string fkApplicationId
            )
        {
            this.UserName = userName;
            this.DoSomething = doSomething;
            this.IpStr = ip;
            this.FkFormName = fkFormName;
            this.FKAction = fkAction;
            this.FKApplicationID = fkApplicationId;
        }

        public ERPRiZhiViewModel(
           string userName,
           string doSomething,
           string ip,
           string fkFormName,
           string fkAction,
           string modulename,
           string fkApplicationId,
            DateTime time
           )
        {
            this.UserName = userName;
            this.DoSomething = doSomething;
            this.IpStr = ip;
            this.FkFormName = fkFormName;
            this.FKAction = fkAction;
            this.ModuleName = modulename;
            this.FKApplicationID = fkApplicationId;
            this.TimeStr = time;
        }

        public ERPRiZhiViewModel(
          string userName,
          string doSomething,
          string ip,
          string fkFormName,
          string fkAction,
          string modulename,
           DateTime time
          )
        {
            this.UserName = userName;
            this.DoSomething = doSomething;
            this.IpStr = ip;
            this.FkFormName = fkFormName;
            this.FKAction = fkAction;
            this.ModuleName = modulename;
            this.TimeStr = time;
        }


        public ERPRiZhiViewModel(
          int rizhiResultId,
          string doSomething,
          string ip,
          string fkFormName,
          string fkAction,
          string modulename,
          string fkApplicationId,
           DateTime time
          )
        {
            this.ID = rizhiResultId;
            this.DoSomething = doSomething;
            this.IpStr = ip;
            this.FkFormName = fkFormName;
            this.FKAction = fkAction;
            this.ModuleName = modulename;
            this.FKApplicationID = fkApplicationId;
            this.TimeStr = time;
        }

        public int ID { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> TimeStr { get; set; }
        public string DoSomething { get; set; }
        public string IpStr { get; set; }
        public string NotificationContent { get; set; }
        public string FkFormName { get; set; }
        public string FKAction { get; set; }
        public string FKApplicationID { get; set; }
        public string ModuleName { get; set; }
    }
}
