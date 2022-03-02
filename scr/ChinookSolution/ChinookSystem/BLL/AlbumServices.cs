#nullable disable
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
    public class AlbumServices
    {
        #region Constructor and Context Dependency
        private readonly ChinookContext _context;

        internal AlbumServices(ChinookContext context)
        {
            _context = context;
        }
        #endregion

        #region Services: Queries
        public List<AlbumsListBy> AlbumsByGenre(int genreid,
                                                int pageNumber,
                                                int pagesize,
                                                out int totalrows)
        {
            //return raw data and let the presentation layer decide ordering
            //pageNumbe, pageSize, totalrows are input parameters used for
            //  paging on the presentation layer
            //this query will return ONLY the rows that will be displayed on the
            //  browser
            IEnumerable<AlbumsListBy> info = _context.Tracks
                                        .Where(x => x.GenreId == genreid
                                            &&  x.AlbumId.HasValue)
                                        .Select(x => new AlbumsListBy
                                        {
                                            AlbumId = (int)x.AlbumId,
                                            Title = x.Album.Title,
                                            ArtistId = x.Album.ArtistId,
                                            ReleaseYear = x.Album.ReleaseYear,
                                            ReleaseLabel = x.Album.ReleaseLabel,
                                            ArtistName = x.Album.Artist.Name,
                                            TrackCount = 0
                                        })
                                        .Distinct()
                                        .OrderBy(x => x.Title);

            //determine the size of the whole query collection
            totalrows = info.Count();

            //calculate the number of rows to SKIP in the query collection
            //this calculation depends on the page number and page size
            //page 1: skip 0 rows; page 2: skip pagesize rows; page 3: skip 2*pagesize rows ...
            int skipRows = (pageNumber - 1) * pagesize;
            //On the return, use the .Skip() and .Take() Linq extensions to extract the
            //  actually row data to return

            return info.Skip(skipRows).Take(pagesize).ToList();
        }

        #endregion
    }
}
