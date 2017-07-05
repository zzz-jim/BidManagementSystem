using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.DomainModel.Identity
{
    public interface IUserCardManager : IBaseStore<UserCard>
    {
        /// <summary>
        /// 验证身份证号码是否在平台中存在
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
         Task<bool> CheckIdCardNum(string idCard);

        /// <summary>
         /// 获取病人已绑定的用户卡/号
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        // Task<UserCard> GetUserCards(Guid patientId);
    }
}
