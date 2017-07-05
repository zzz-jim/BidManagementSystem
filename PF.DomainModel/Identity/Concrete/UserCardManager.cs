using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PF.DomainModel.Identity
{

    public class UserCardManager : BaseStore<UserCard>, IUserCardManager
    {
        /// <summary>
        /// 验证身份证号码是否在平台中存在
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public async Task<bool> CheckIdCardNum(string idCard)
        {
           var card= await this.GetByIdAsync(idCard, 1);
            return card==null?false:true;
        }

       // /// <summary>
       // /// 获取病人已绑定的用户卡/号
       // /// </summary>
       // /// <param name="patientId"></param>
       // /// <returns></returns>
       //public Task<UserCard> GetUserCards(Guid patientId)
       // {
       //    return await this.Filter(c=>c.PatientId==patientId);
       // }
    }
}
