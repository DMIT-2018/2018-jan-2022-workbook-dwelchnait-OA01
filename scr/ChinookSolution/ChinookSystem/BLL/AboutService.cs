#nullable disable //suppress the null warning on code
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespace
using ChinookSystem.DAL;
using ChinookSystem.Entities;
using ChinookSystem.ViewModels;
#endregion

namespace ChinookSystem.BLL
{
    //this class needs to be accessed by an "outside user" (WebApp)
    //therefore the class needs to be public 
    public class AboutService
    {
        #region Constructor and Context Dependency
        private readonly ChinookContext _context;

        internal AboutService(ChinookContext context)
        {
            _context = context;
        }
        #endregion

        #region Services
        public DbVersionInfo GetDbVersion()
        {
            DbVersionInfo info= _context.DbVersions
                                    .Select(x => new DbVersionInfo
                                    {
                                        Major = x.Major,
                                        Minor = x.Minor,
                                        Build = x.Build,
                                        ReleaseDate = x.ReleaseDate
                                    })
                                    .SingleOrDefault();
            return info;
        }
        #endregion
    }
}
