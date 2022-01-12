<Query Kind="Statements">
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

//Statements IDE

//you can have multiple queries written in this IDE environment
//you can execute a query individually by highlighting
//	the desired query first
//BY DEFAULT executing a file in this environment executes
//		ALL queries, top to bottom

//IMPORTANT
//queries in this environmet MUST be written useing the
//		C# language grammar for a statement. This means that
//		each statement must end in a semi-colon
//results MUST be placed in a receiving variable
//to display the results, use the Linqpad method .Dump()

//query syntax
//query: Find all albums released in 2000. Display the entire
//			album record
var paramyear = 1990;
var resultsq = from x in Albums
				where x.ReleaseYear == paramyear
				select x;
//resultsq.Dump();


//method syntax
Albums
   .Where(x => (x.ReleaseYear == 2000))
   .Select(x => x)
   .Dump();



