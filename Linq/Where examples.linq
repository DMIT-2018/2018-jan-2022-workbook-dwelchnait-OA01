<Query Kind="Expression">
  <Connection>
    <ID>2621ab68-ddd0-477b-bd31-283968b8e170</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

//Where
//filter method
//the conditions are setup as you would in C#
//beware that Linqpad may NOT like some C# syntax (DateTime)
//beware that Linq is converted to SQL which may not
//	like certain C# syntax because SQL could not convert

//syntax
//Notice that the method syntax makes uses of Lambda expressions. 
//Lambdas are common when performing LINQ with the Method syntax.
//.Where(LambDa expression)
//.Where(x => condition [logical orperator condition2 ...])

//Find all albums released in 2000. Display the entire
//			album record
Albums
   .Where(x => (x.ReleaseYear == 2000))
   .Select(x => x)
   
//Find all albums released in the 90s (1990 -1999)
//Display the entire album record

Albums
	.Where(x => x.ReleaseYear >= 1990
			&& x.ReleaseYear < 2000)
	.Select(x => x)
	
//Find all the albums of the artist Queen.
//concern: the artist name is in another table
//			in an sql Select query you would be using an inner Join
//			in Linq you DO NOT need to specific your inner Joins
//			instead use the "navigational properties" of your entity
//				to generate the relationship

//.Equals() is an exact match, in sql = (like 'string')
//.Contains() is a string match, in sql like '%' + string + '%'
Albums
	.Where(a => a.Artist.Name.Contains("Queen"))
	.Select(x => x)


//Find all albums where the producer (Label) is unknown (null)

Albums
	.Where(x => x.ReleaseLabel == null)
	.Select(x => x)







