<Query Kind="Program" />

void Main()
{
	//create an instance of the string class called message
	string message = "hello world"; //instance of string class
	
	Console.WriteLine(message); //access the instance contents
	Console.WriteLine(message.Length); //a property of the string class
	Console.WriteLine(message.Substring(3)); //a method of the string class 
	
	//what if I would like my string to "Quack"
	Console.WriteLine(message.Quack()); //.Quack() is NOT part of the C# string class, you need to create an extension method
	Console.WriteLine(message.Quack(5)); //.Quack(argument) method does not exist; you need to create an overload extension 
	string cheers = "Go Oilers Go";
	Console.WriteLine(cheers.Quack(3)); //the .Quack() extends the string class. It is NOT tied to a specific string instance.
}

//Examine what an extension method is.

//Create extension method(s) for the following C# class: string

//step 1: make a static class to hold the extension method(s)
//  	  this class can be called anything you like
public static class MyExtensionStringMethods
{
	//Step 2: Add your public static string method(s) to this class
	
	public static string Quack(this string self)
	{
		//the return datatype from this method will be a string
		//this is the datatype of the class instance we are extending
		//
		//NOTE: you do not necessarily need to return a value; that is the rdt could be void
		//
		//the 1st parameter (the error msg does used the word argument) of the method
		//	signature identifies the class the extension method is associate with
		//
		//the parameter rquires the following synta ->  this datatype parametername
		//the contents of the parameter will be the contents of the calling instance (eg. message)
		
		//your logic for the method
		string result = "quack " + self + " quack";
		return result;
	}
	
	public static string Quack(this string self, int parameterTimes)
	{
		//any additional parameters for the extension method follows
		//	the required datatype 1st parameter (this datatype parametername)
		//you may have any number of additional parameters
		//code the additiona parameters just like another other method parameter
		string quacks = "";
		for(int i = 0; i < parameterTimes; i++)
		{
			quacks += "..quack.. ";
		}
		return $"{self} ({quacks})";
	}
}
