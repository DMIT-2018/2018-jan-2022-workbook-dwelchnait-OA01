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
	//Strongly typed query datasets
	
	//Anonymous dataset from a query does NOT have a specified class definition
	//Strongly typed query dataset HAS a specified class definition
	
	//Find all songs that contain a partial string of the Track name.
	
	//imaging the following is in the code-behind of our Razor Page
	string partialSongName = "dance";
	List<SongList> results = SongsByPartialName(partialSongName);
	results.Dump();
	
}

// You can define other methods, fields, classes and namespaces here

//imagine the following method exists in a service in your BLL

List<SongList> SongsByPartialName(string partialSongName)
{
	//to change an Anonymous dataset to a strongly typed dataset
	//  add the datatype to the new operator
	
	//one could use var variablename = OR
	// 			use IEnumerable<T> variablename = where T is a defined class
	
	IEnumerable<SongList> songCollection = Tracks
						.Where(t => t.Name.Contains(partialSongName))
						.Select(t => new SongList
							{
								Album = t.Album.Title,
								Song = t.Name,
								Artist = t.Album.Artist.Name
							});
	return songCollection.ToList();
}

//developer defined data type
public class SongList
{
	public string Album{get;set;}
	public string Song {get;set;}
	public string Artist {get;set;}
}













