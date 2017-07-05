using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.DomainModel.Identity
{
    public interface ISectionManager:IBaseStore<Section>
    {
        /// <summary>
        /// 获取用户所有科室列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
         Task<IEnumerable<Section>> GetUserSectionsAsync(string userId);
        /// <summary>
        /// 为用户指定科室
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="secionIds">科室代码</param>
        /// <returns></returns>
         Task<IdentityResult> SetUserSectionsAsync(string userId, params string[] secionIds);
    }
}
