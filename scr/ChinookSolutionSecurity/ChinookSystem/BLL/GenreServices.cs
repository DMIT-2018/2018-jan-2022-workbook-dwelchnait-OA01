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
    public class GenreServices
    {
        #region Constructor and Context Dependency
        private readonly ChinookContext _context;

        internal GenreServices(ChinookContext context)
        {
            _context = context;
        }
        #endregion

        #region Services : Queries
        //obtain a list of Genres to be used in a select list
        public List<SelectionList> GetAllGenres()
        {
            IEnumerable<SelectionList> info = _context.Genres
                                        .Select(x => new SelectionList
                                        {
                                            ValueId = x.GenreId,
                                            DisplayText = x.Name
                                        });
                                    //  .OrderBy(x => x.DisplayText); this sort is in SQL
            return info.ToList();
            //return info.OrderBy(x => x.DisplayText).ToList();  this sort is in RAM
        }

        
        #endregion

    }
}
