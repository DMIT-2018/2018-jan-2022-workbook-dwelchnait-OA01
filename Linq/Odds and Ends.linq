<Query Kind="Program">
  <Connection>
    <ID>ac19c5d1-a507-4043-bafb-b647e09ebfb9</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
	//Conversions
	//.ToList()
	//can convert a list at any time

	//Display all Albums and their tracks. Display the album title,
	//artist name and album tracks. For each track show the song name
	//and play time. Show only albums with 25 or more tracks.
	
	List<AlbumTracks> albumlist = Albums
					.Where(a => a.Tracks.Count >= 25)
					.Select(a => new AlbumTracks
					         {
							     Title = a.Title,
								 Artist = a.Artist.Name,
						         Songs = a.Tracks
										 .Select(tr => new SongItem
										     {
											    Song = tr.Name,
												Playtime = tr.Milliseconds / 1000.0
											 })
										  .ToList()
							 })
							 .ToList();
	//albumlist.Dump();
	
	//typically if the albumlist was a var variable in your BLL method
	//AND the method return datatype was a List<T>, one could, on the
	//return statement do: return albumlist.ToList(); (typically saw in 1517)
	
	//Using .FirstOrDefault()
	//Find the first album by Deep Purple.
	
	var artistparam = "Deep Purple";
	var resultsFirstOrDefault = Albums
					.Where(a => a.Artist.Name.Equals(artistparam))
					.Select(a => a)
					.OrderBy(a => a.ReleaseYear)
					.FirstOrDefault();
	//if(resultsFirstOrDefault != null)
	//	resultsFirstOrDefault.Dump();
	//else
	//	Console.WriteLine($"No albums found for {artistparam}");

	//Using .SingleOrDefault
	//differs from .FirstOrDefault in that it expect only a single instance
	//   to be returned from your query
	//Find the album by the albumid
	int albumid = 10000;
	var resultsSingleOrDefault = Albums
					.Where(a => a.AlbumId == albumid)
					.Select(a => a)
					.SingleOrDefault();
	//if (resultsSingleOrDefault != null)
	//	resultsSingleOrDefault.Dump();
	//else
	//	Console.WriteLine($"No albums found for id of {albumid}");
	
	//.Distinct()
	//removes duplicate reported lines
	var resultsDistinct = Customers
							.OrderBy(c => c.Country)
							.Select(c => c.Country)
							.Distinct();
	//resultsDistinct.Dump();
	
	//.Take() and .Skip()
	//in 1517, when you want to use your paginator
	//   the query method was to return ONLY the need
	//	 records to display
	//a) the query was executed in full
	//b) obtained the total count of returned records
	//c) calculated the number of records to skip
	//d) on the return statement you used variablename.Skip(rowsSkiped).Take(pagesize).ToList()
	
	//Any and All
	Genres.Count().Dump();
	//25 genres
	
	//show genres that have tracks which are not on any playlist
	Genres
		.Where(g => g.Tracks.Any(tr => tr.PlaylistTracks.Count() == 0))
		.Select(g => g)
		//.Dump()
		;

	//Show lgeneres that have all their tracks appearing at least once
	//on a playlist.
	Genres
		.Where(g => g.Tracks.All(tr => tr.PlaylistTracks.Count() > 0))
		.Select(g => g)
		//.Dump()
		;

	//there maybe times that using a !Any() - > All( !relationship) 
	//and a !All() -> Any( !relationship) results
	
	//Using All and Any in comparing 2 collections
	//if your collection is NOT a complex record there is Linq method
	//	called .Except that can be used to solve your query
	
	//https://dotnettutorials.net/lesson/linq-except-method/
	//https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.except?view=net-6.0
	
	//Compare the track collection of 2 people using All and Any
	
	//create a track collection for each person 
	// Roberto Almeida (AlmeidaR) vs Michelle Brooks (BrooksM)
	
	var almeida = PlaylistTracks
					.Where(x => x.Playlist.UserName.Contains("AlmeidaR"))
					.Select(x => new
					{
						song = x.Track.Name,
						genre = x.Track.Genre.Name,
						id = x.TrackId,
						artist = x.Track.Album.Artist.Name
					})
					.Distinct()
					.OrderBy(x => x.song)
					//.Dump() //110
					;

	var brooks = PlaylistTracks
					.Where(x => x.Playlist.UserName.Contains("BrooksM"))
					.Select(x => new
					{
						song = x.Track.Name,
						genre = x.Track.Genre.Name,
						id = x.TrackId,
						artist = x.Track.Album.Artist.Name
					})
					.Distinct()
					.OrderBy(x => x.song)
					//.Dump()  //88
					;
	
	//List thr tracks that BOTH Roberto and Michelle like.
	//Compare 2 datasets together, data is listA that is also in listB
	//Assume listA is Roberto and listB is Michelle
	//listA is what you wish to report from
	//listB is what you wish to compare to
	
	//What songs does Roberto like but not Michelle
	almeida
		.Where(rob => !brooks.Any(mic => mic.id == rob.id))
		.OrderBy(rob => rob.song)
		//.Dump()
		;

	almeida
		.Where(rob => brooks.All(mic => mic.id != rob.id))
		.OrderBy(rob => rob.song)
		//.Dump()
		;

	//what songs does Michelle like but not Roberto
	brooks
			.Where(mic => !almeida.Any(rob => mic.id == rob.id))
			.OrderBy(mic => mic.song)
			//.Dump()
			;

	//What songs does both michelle and roberto like
	brooks
			.Where(mic => almeida.Any(rob => mic.id == rob.id))
			.OrderBy(mic => mic.song)
			//.Dump()
			;
			
	//Union
	//since linq is converted into Sql one would expect that the
	//  Sql Union rules must be the same in linq
	//concatenating multiple results into one collection
	//syntax (queryA).Union(queryB)[.Union(query....)]
	//rules:
	//	number of columns the same
	//	column datatypes must match
	//	ordering should be done as a method after the last Union
	
	//List the stats of Albums on Tracks(count,cost, average track length)
	//Note: for cost and average, one will need an instance to do the aggregation
	
	//Albums with no tracks on the database will have a count, however cost and
	//	average length will be 0
	//
	//NOTE: if you are hard coding numeric fields, the query with the hard coded
	//		values MUST be the first query
	
	//queryA would be Albums with no tracks (hard code cost and average)
	//queryB would be Albums with tracks(calculate cost and average)
	
	(Albums
		.Where(x => x.Tracks.Count() == 0)
		.Select(x => new
			{
				title = x.Title,
				totalTracks = 0,
				totalCost = 0.00m,
			averagelength = 0.00
		})).Union(Albums
					.Where(x => x.Tracks.Count() > 0)
					.Select(x => new
					{
						title = x.Title,
						totalTracks = x.Tracks.Count(),
						totalCost = x.Tracks.Sum(tr => tr.UnitPrice),
						averagelength = x.Tracks.Average(tr => tr.Milliseconds)
					}))
			.OrderBy(a => a.totalTracks)
			.Dump()
			;
}

public class SongItem
{
	public string Song {get;set;}
	public double Playtime{get;set;}
}

public class AlbumTracks
{
	public string Title {get;set;}
	public string Artist {get;set;}
	public List<SongItem> Songs{get;set;}
}