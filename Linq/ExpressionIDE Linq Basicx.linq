<Query Kind="Expression">
  <Connection>
    <ID>2f553dd8-a29d-45a1-b6dc-83979d607633</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

// Our code is using C# grammar/syntax

//comments are done with slashes
//hotkey make line comment ctrl+k,ctrl+c
//				uncomment ctrl+k,ctrl+u
//alternate is to use ctrl + / as the toggle

//Expressions
// single linq query staatements without a semi-colon
// you can have multiple statements in your file BUT
//		if you do, you MUST highlight the statement to execute

//execuge using F5 or the green triangle on the query menu

// to toggle your results on and off (visible) use ctrl + R

//query syntax
//  uses a "sql-like" syntax
//	view the Student Notes for examples under
//		Demo/eRestaurant/Linq Query and Method syntax
//  or  Notes/Linq Intro

//remember to set your database usage connection

//query: Find all albums released in 2000. Display the entire
//			album record

from therowinstanceplaceholder in Albums
where therowinstanceplaceholder.ReleaseYear == 2000
select therowinstanceplaceholder

//method syntax
//uses C# method syntax OOP language grammar
// collection: Albums
// to excute a method on the collection you need to use
//	the access operator (dot operator)
// results a resulting collection from the method !!!!!*****
// method name starts with a capital
// methods contan contents with a delegate
// a delgate desrcibes the action to be done

Albums
	.Where(therowinstanceplaceholder => therowinstanceplaceholder.ReleaseYear == 2000)
	.Select(therowinstanceplaceholder => therowinstanceplaceholder)







