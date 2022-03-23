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
using Microsoft.EntityFrameworkCore.ChangeTracking; //update
#endregion

namespace ChinookSystem.BLL
{
    public class PlaylistTrackServices
    {
        #region Constructor and Context Dependency
        private readonly ChinookContext _context;

        internal PlaylistTrackServices(ChinookContext context)
        {
            _context = context;
        }
        #endregion

        #region Queries
        public List<PlaylistTrackInfo> PlaylistTrack_Fetch_Playlist(string playlistname,
                                                                           string username)
        {
            IEnumerable<PlaylistTrackInfo> info = _context.PlaylistTracks
                                                   .Where(x => x.Playlist.Name.Equals(playlistname)
                                                          && x.Playlist.UserName.Equals(username))
                                                   .Select(x => new PlaylistTrackInfo
                                                   {
                                                       TrackId = x.TrackId,
                                                       TrackNumber = x.TrackNumber,
                                                       SongName = x.Track.Name,
                                                       Milliseconds = x.Track.Milliseconds,
                                                   })
                                                   .OrderBy(x => x.TrackNumber);
            return info.ToList();
        }
        #endregion

        #region Commands
        public void PlaylistTrack_AddTrack(string playlistname, string username, int trackid)
        {
            //create local variables
            Playlist playlistExists = null;
            PlaylistTrack playlistTrackExists = null;
            int trackNumber = 0;
            List<Exception> errorlist = new List<Exception>();

            //Business logic 
            //these are processing rules that need to be satisfied for valid data
            //  rule: a track can only exist once on a playlist
            //  rule: each track on a playlist is assigned a continuous track number
            //
            //if the business rules are passed, consider the data valid, then
            //  a) stage your transaction work
            //  b) execute a SINGLE .SaveChanges() - commit to database
             
            if (string.IsNullOrWhiteSpace(playlistname))
            {
                throw new ArgumentNullException("Playlist name is missing.");
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException("User name is missing. Log in to add tracks to your playlist");
            }

            playlistExists = _context.Playlists
                            .Where(x => x.Name.Equals(playlistname)
                                    && x.UserName.Equals(username))
                            .FirstOrDefault();

            if (playlistExists == null)
            {
                //a new playlist
                playlistExists = new Playlist()
                {
                    Name = playlistname,
                    UserName = username
                };
                //stage (only in memory)
                _context.Playlists.Add(playlistExists);
                trackNumber = 1;
            }
            else
            {
                //existing playlist
                //check if track already is on playlist
                playlistTrackExists = _context.PlaylistTracks
                                    .Where(x => x.Playlist.Name.Equals(playlistname)
                                            && x.Playlist.UserName.Equals(username)
                                            && x.TrackId == trackid)
                                    .FirstOrDefault();
                if(playlistTrackExists != null)
                {
                    var songname = _context.Tracks
                        .Where(x => x.TrackId == trackid)
                        .Select(x => x.Name)
                        .SingleOrDefault();
                    errorlist.Add(new Exception($"Selected track ({songname}) is alread on the playlist"));
                }
                else
                {
                    //find the next tracknumber and increment by 1
                    trackNumber = _context.PlaylistTracks
                                    .Where(x => x.Playlist.Name.Equals(playlistname)
                                            && x.Playlist.UserName.Equals(username))
                                    .Count();
                    trackNumber++;
                }
            }

            //add the track to the playlist
            //create an instance for the track
            playlistTrackExists =new PlaylistTrack();

            //load the properties (fields) of the new PlaylistTrack instance
            playlistTrackExists.TrackId = trackid;
            playlistTrackExists.TrackNumber =  trackNumber;

            //?? what about the second part of the primary key: PlaylistID?
            //if the playlist exists then we know the id : playlistExists.PlaylistID
            //BUT if the playlist is NEW, we DO NOT know the id

            //in the situation of a NEW playlist, even though we have
            //  create the playlist instance (see above) it is ONLY
            //  staged
            //this means that the actual sql record has NOT yet been
            //  created.
            //this means that the IDENTITY value for the new playlist DOES NOT
            //  yet exists. The value on the playllist instance (playlistExist)
            //  is zero (0).
            //therfore we have a serious problem

            //Solution
            // It is built into the EntityFramework and is based using the 
            // navigational property in Playlist pointing to it's "child"

            //staging a typical Add in the past was to reference the entity
            //  and use the .Add(xxxx)
            //      _context.PlaylistTracks.Add(playlistExist)
            //IF you use this statement the playlistid would be zero (0)
            //  causing your transaction to ABORT 
            //Why?? pKeys cannot be zero (0)

            //INSTEAD, do the staging using the "parent.navchildproperty.Add(xxxx)
            playlistExists.PlaylistTracks.Add(playlistTrackExists);

            //Staging is complete
            //Commit the work (Transaction)
            //commiting the work needs a .SaveChanges
            //BUT what if you have discovered errors during the business process??
            if(errorlist.Count > 0)
            {
                throw new AggregateException("Unable to add new track. Check concerns", errorlist);
            }
            else
            {
                _context.SaveChanges();
            }
        }

        public void PlaylistTrack_RemoveTracks(string playlistname, string username,
                                                    List<PlaylistMove> trackstoremove)
        {
            List<Exception> errorList = new List<Exception>();
            Playlist playlistExists = null;
            PlaylistTrack playlisttrackExists = null;   
            int trackNumber = 0;

            if (string.IsNullOrWhiteSpace(playlistname))
            {
                throw new ArgumentNullException("Playlist name is missing.");
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException("User name is missing. Log in to add tracks to your playlist");
            }
            if (trackstoremove.Count == 0)
            {
                throw new ArgumentNullException("Playlist is empty");
            }
            
            playlistExists = _context.Playlists
                           .Where(x => x.Name.Equals(playlistname)
                                   && x.UserName.Equals(username))
                           .FirstOrDefault();
            if (playlistExists == null)
            {
                errorList.Add(new Exception("Play list does not exist"));
            }
            else
            {
                //check to see if a track has been flaged to remove
                IEnumerable<PlaylistMove> removelist = trackstoremove
                                                      .Where(x => x.SelectedTrack);
                IEnumerable<PlaylistMove> keeplist = trackstoremove
                                                      .Where(x => !x.SelectedTrack)
                                                      .OrderBy(x => x.TrackNumber);
                foreach (PlaylistMove track in removelist)
                {
                    playlisttrackExists = _context.PlaylistTracks
                                          .Where(x => x.Playlist.Name.Equals(playlistname)
                                                  && x.Playlist.UserName.Equals(username)
                                                  && x.TrackId == track.TrackId)
                                          .FirstOrDefault();
                    if (playlisttrackExists != null)
                    {
                        _context.PlaylistTracks.Remove(playlisttrackExists);
                    }
                }
                trackNumber = 1;
                foreach (PlaylistMove track in keeplist)
                {
                    playlisttrackExists = _context.PlaylistTracks
                                          .Where(x => x.Playlist.Name.Equals(playlistname)
                                                  && x.Playlist.UserName.Equals(username)
                                                  && x.TrackId == track.TrackId)
                                          .FirstOrDefault();
                    if (playlisttrackExists != null)
                    {
                        playlisttrackExists.TrackNumber = trackNumber;

                        //stage add in local memory
                        EntityEntry<PlaylistTrack> updating = _context.Entry(playlisttrackExists);
                        updating.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        trackNumber++;
                    }
                    else
                    {
                        var songname = _context.Tracks
                                        .Where(x => x.TrackId == track.TrackId)
                                        .Select(x => x.Name)
                                        .SingleOrDefault();
                        errorList.Add(new Exception($"Track {songname} no longer on playlist."));
                    }
                }
            }

            if(errorList.Count > 0)
            {
                throw new AggregateException("Unable to remove tracks. See following concerns:",errorList);
            }
            else
            {
                _context.SaveChanges();
            }
        }
    #endregion
}
}
