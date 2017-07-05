using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using PF.Common.Enums.HB.Public;
using PF.Common.Enums.GB;
using System.ComponentModel.DataAnnotations.Schema;

namespace PF.DomainModel.Identity
{
    //病人基础信息
    //    名称	说明	数据类型	类属性
    //病人姓名		String	Name
   
    //出生日期		DateTime	BirthDate
    public class Patient
    {
        [Key] //病人ID	病人在平台系统中的唯一标识，用于调用其它相关接口的输入参数	String	PatientId
           public System.Guid   Id { get; set; }
        [MaxLength(20),Required]
        public string Name { get; set; }
        [Required,EnumDataType(typeof(Gender))] //性别	GB/T 2261.1-2003	Int	Gender
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual ICollection<UserCard> UserCards {get;set;}
    }

    public class UserCard
    {
        //卡号		String	CardNum
        [Key, Column(Order = 1),Required, MaxLength(50)]
        public string CardNum { get; set; }
        //卡类型	见平台支持的卡列表	int	CardType
        [Key, Column(Order = 2), Required,EnumDataType(typeof(SupportCard))]
        public SupportCard CardType { get; set; }
        //卡状态	0 有效1 注销2 挂失	Int	CardStatus
        [EnumDataType(typeof(UserCardState))]
        public UserCardState CardStatus { get; set; }
        //绑定日期		DateTime	BindDate
        public DateTime BindDate { get; set; }

        [Required]
        public System.Guid PatientId { get; set; }
        public virtual Patient Patinet { get; set; }
    }
}
