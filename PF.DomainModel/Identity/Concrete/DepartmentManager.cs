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
    public class SectionManager : BaseStore<Section>, ISectionManager
    {
        public SectionManager() : base() { }
        public static SectionManager Create()
        {
            return new SectionManager();
        }
       
        //为共享模式准备的构造函数,扩展中要用到其它数据上下文时使用
        public SectionManager(ApplicationDbContext context) : base(context) { }

        
        /// <summary>
        /// 获取用户所有科室列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<IEnumerable<Section>> GetUserSectionsAsync(string userId)
        {
        
            return await this.Filter(s => s.ApplicationUsers.Any(u => u.ApplicationUserId == userId)).ToListAsync();
            //var result = new List<Section>();
            //var userSecions = (from g in this.All()
            //                  where g.ApplicationUsers
            //                    .Any(u => u.ApplicationUserId == userId)
            //                  select g).ToListAsync();
            //return await userSecions;
        }

        /// <summary>
        /// 为用户指定科室
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="secionIds">科室代码</param>
        /// <returns></returns>
        public async Task<IdentityResult> SetUserSectionsAsync(
           string userId, params string[] secionIds)
        {
            // 清除现有科室:
            var currentSections = await this.GetUserSectionsAsync(userId);
            foreach (var section in currentSections)
            {
                section.ApplicationUsers
                    .Remove(section.ApplicationUsers
                    .FirstOrDefault(gr => gr.ApplicationUserId == userId
                ));
            }
            await Context.SaveChangesAsync();

            //加入指定科室:
            foreach (string secionId in secionIds)
            {
                Section newSection = await this.GetByIdAsync(secionId);

                newSection.ApplicationUsers.Add(new ApplicationUserSection
                {
                    ApplicationUserId = userId,
                    SectionId = secionId
                });
            }
            await Context.SaveChangesAsync();

            return IdentityResult.Success;
        }
    }

    public class DepartmentManager : BaseStore<Department>, IBaseStore<Department>
    {
        public static DepartmentManager Create()
        {
            return new DepartmentManager();
        }
    }

    public class HospitalManager : BaseStore<Hospital>, IBaseStore<Hospital>
    {
        public static HospitalManager Create()
        {
            return new HospitalManager();
        }
    }
}
