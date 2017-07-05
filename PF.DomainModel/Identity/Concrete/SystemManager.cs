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

    public class AccessSystemManager : BaseStore<AccessSystem>, IBaseStore<AccessSystem>
    {

    }
}
