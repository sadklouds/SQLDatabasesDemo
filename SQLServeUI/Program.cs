using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using DataAccessLibrary;
using DataAccessLibrary.Models;

SQLCrud sql = new(GetConnectionString());

//GetAllPeople(sql);

//CreateNewPerson(sql);

//UpdatePersonName(sql);

//ReadPersonById(sql, 5);

//DeleteAddressByID(sql, 5, 1);


Console.WriteLine("Done Processing SQL Server");

Console.ReadLine();

static void DeleteAddressByID(SQLCrud sql, int personId, int addressId)
{
    sql.RemovePhoneNumberFromContact(personId, addressId);
}

static void UpdatePersonName(SQLCrud sql)
{
    BasicPersonModel person= new BasicPersonModel()
    {
        Id = 5,
        FirstName = "Nito",
        LastName = "of Londor"
    };

    sql.UpdatePersonName(person);
}

static void ReadPersonById(SQLCrud sql, int id)
{
    FullPersonModel person = sql.GetFullPersonById(id);
    Console.WriteLine($"{person.BasicInfo.Id}: {person.BasicInfo.FirstName} {person.BasicInfo.LastName}");

    foreach (var address in person.Addresses)
    {
        Console.WriteLine($"address ID{address.Id}: {address.StreetAddress},{address.City},{address.County}.{address.PostCode}");
    }
}

static void CreateNewPerson(SQLCrud sql)
{

    FullPersonModel person = new FullPersonModel()
    { 
        BasicInfo = new BasicPersonModel()
        {
            FirstName = "Sad",
            LastName = "Kloud"
        }

    };
    person.Addresses.Add(new AddressModel 
        {StreetAddress = "123 Cinder street", City = "AnorLondo", County = "LA", PostCode = "DIE12345" });

    person.Addresses.Add(new AddressModel
        { Id = 1, StreetAddress = "State street 123", City = "Citadel", County = "CD", PostCode = "12345678" });

    sql.CreateContact(person);
}
static void GetAllPeople(SQLCrud sql)
{
    List<BasicPersonModel> people = sql.GetAllPoeple();
    foreach (var person in people)
    {
        Console.WriteLine($"{person.Id}: {person.FirstName} {person.LastName}");
    }
}
static string GetConnectionString(string connectionStringName = "Default")
{
    string output = "";

    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");

    var config = builder.Build();

    output = config.GetConnectionString(connectionStringName);

    return output;
}

