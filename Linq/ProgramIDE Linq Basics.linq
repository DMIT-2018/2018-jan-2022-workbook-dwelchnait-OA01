<Query Kind="Program">
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

void Main()
{
	//Program IDE
	//you can have multiple queries written in this IDE environment
	//this environment works "like" a console application

	//This allows one to pret-test complete components that can
	//		be move directly into your backend application (class library)

	//IMPORTANT
	//queries in this environmet MUST be written useing the
	//		C# language grammar for a statement. This means that
	//		each statement must end in a semi-colon
	//results MUST be placed in a receiving variable
	//to display the results, use the Linqpad method .Dump()

	//query syntax
	//query: Find all albums released in 2000. Display the entire
	//			album record
	var paramyear = 1990; //simulates the invoming method parameter
	var resultsq = GetAllQ(paramyear);
	resultsq.Dump();


	//method syntax
	paramyear = 2000; //simulates the invoming method parameter
	var resultsm = GetAllM(paramyear);
	resultsm.Dump();

}

// You can define other methods, fields, classes and namespaces here

//imagine this is a method in your BLL service
public List<Albums> GetAllQ(int paramyear)
{
	var resultsq = from x in Albums
				   where x.ReleaseYear == paramyear
				   select x;
	return resultsq.ToList();
}

public List<Albums> GetAllM(int paramyear)
{
	var resultsm =  Albums
				   .Where(x => (x.ReleaseYear == paramyear))
				   .Select(x => x);
	return resultsm.ToList();
}


