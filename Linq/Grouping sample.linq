<Query Kind="Expression">
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

//Grouping

//when you create a group it builds two (2) components
//a) Key component (group by)
//		reference this component using the groupname.Key[.Property]
//	(property < - > field < - > attribute < - > value)
//b) data of the group (instances of the collection)

//ways to group
//a) by a single column (field, attribute, property)	groupname.Key
//b) by a set of columns (anonymous dataset)			groupname.Key.Property
//c) by using an entity	(x.navproperty)					groupname.Key.Property

//concept processing
//start with a "pile" of data (original collection)
//specify the grouping property(ies)
//result of the group operation will be to "place the data into smaller piles"
//	the piles are dependant on the grouping property(ies) values
//	the grouping property(ies) become the Key
//	the individual instances are the data in the smaller piles
//	the entire individual instance of the original collection is placed in the
//		smaller pile
//manipulate each of the "smaller piles" using your linq commands

//grouping is different then Ordering
//Ordering is the re-sequencing of a collection for display
//grouping re-organizes a collection in separate, usually smaller
//		collections for processing

//grouping is an excellent way to organize your data especially if
//		you need to process data on a property that is NOT a "relative key"
//		such as a foreign Key which forms a "natural" group using the
//		navigational properties

//Display albums by ReleaseYear
//	this request does not need grouping
//	this request is an ordering of output : OrderBy
//	affects display only
Albums
	.OrderBy(a => a.ReleaseYear)

//Display albums grouped by ReleaseYear
//	explicit request to breakup the display into desired "piles"

Albums
	.GroupBy(a => a.ReleaseYear)

//query syntax
from a in Albums
group a by a.ReleaseYear

//processing on the groups created by the Group command

Albums
	.GroupBy(a => a.ReleaseYear)  //this method returns a collection
	.Select(eachgPile => new
			{
				Year = eachgPile.Key,
				NumberOfAlbums = eachgPile.Count()
			}
			)

//query syntax
//using this syntax you must specific the name you wish to use to refer to the grouped
// collection
//after coding your group command you MUST (are restricted to) use the name you have
// given your group collection
from a in Albums
//orderby a.ReleaseYear would be valid because the original collection reference
//						is still in effect
group a by a.ReleaseYear into eachgPile
//orderby a.ReleaseYear  would be invalid because the group pile name is eachgPile
//orderby eachgPile.Key	 would need to use eachgPile and reference the key [.Key]
select new
{
	Year = eachgPile.Key,
	NumberOfAlbums = eachgPile.Count()
}


//use a multiple set of properties to form the group
//include a nested query to report on the small pile group

//Display albums grouped by ReleaseLabel, ReleaseYear. Display the
//ReleaseYear and the number of albums. List only the years with 2
//or more albums released

//original collection (large pile of data): Albums
//filtering cannot be decide until the groups are created
//grouping: ReleaseLabel, ReleaseYear (an anonymous set)
//filtering: group.Count >= 2
//report year, count, list of albums in the group (nested query)

Albums
	.GroupBy(a => new {a.ReleaseLabel, a.ReleaseYear})
	.Where(eachgPile => eachgPile.Count() >= 2)
	.OrderBy(eachgPile => eachgPile.Key.ReleaseLabel)
	.Select(eachgPile => new
				{
					Label = eachgPile.Key.ReleaseLabel,
					Year = eachgPile.Key.ReleaseYear,
					NumberofAlbums = eachgPile.Count(),
					AlbumItems = eachgPile
									.Select(eachgPileInstance => new
												{
													TitleOnAlbum =eachgPileInstance.Title,
													YearOnAlbum = eachgPileInstance.ReleaseYear
												})
				})


//entity
//why use an entity when an anonymous set would work?
//		if one needed a large number of properties for the report that would
//		be in your key AND also the entity
//CONCERN: the non-required entity fields will also be used in the grouping
//Use of this style should be sparingly done

Tracks
	.GroupBy(tr => tr.Album) 
	
//Better even though it is longer, use the anonymous set and explicitly indicate your fields

Tracks
	.GroupBy(tr => new {tr.Album.Artist.Name, tr.Album.ReleaseYear})

