using System;

namespace ScientificResearch.DataTransferModel
{
    public class ERPRiZhiTransferObject
    {
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
