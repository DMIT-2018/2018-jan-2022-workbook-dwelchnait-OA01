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
		.Dump()
		;

	//Show lgeneres that have all their tracks appearing at least once
	//on a playlist.
	Genres
		.Where(g => g.Tracks.All(tr => tr.PlaylistTracks.Count() > 0))
		.Select(g => g)
		.Dump()
		;

	//there maybe times that using a !Any() - > All() and a !All() -> Any() results
	
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