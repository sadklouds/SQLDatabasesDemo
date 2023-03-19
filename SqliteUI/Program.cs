

using DataAccessLibrary.Models;
using DataAccessLibrary;
using Microsoft.Extensions.Configuration;

SqliteCrud sql = new(GetConnectionString());

//GetAllPeople(sql);

//CreateNewPerson(sql);

//UpdatePersonName(sql);

//ReadPersonById(sql, 3);

//DeleteAddressByID(sql, 5, 1);

//GetAllPeople(sql);


Console.WriteLine("Done processing sqlite");

Console.ReadLine();

static void DeleteAddressByID(SqliteCrud sql, int personId, int addressId)
{
    sql.RemovePhoneNumberFromContact(personId, addressId);
}

static void UpdatePersonName(SqliteCrud sql)
{
    BasicPersonModel person = new BasicPersonModel()
    {
        Id = 3,
        FirstName = "Nito",
        LastName = "of Londor"
    };

    sql.UpdatePersonName(person);
}

static void ReadPersonById(SqliteCrud sql, int id)
{
    FullPersonModel person = sql.GetFullPersonById(id);
    Console.WriteLine($"{person.BasicInfo.Id}: {person.BasicInfo.FirstName} {person.BasicInfo.LastName}");

    foreach (var address in person.Addresses)
    {
        Console.WriteLine($"address ID{address.Id}: {address.StreetAddress},{address.City},{address.County}.{address.PostCode}");
    }
}

static void CreateNewPerson(SqliteCrud sql)
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
    { StreetAddress = "123 Cinder street", City = "AnorLondo", County = "LA", PostCode = "DIE12345" });

    person.Addresses.Add(new AddressModel
    { Id = 1, StreetAddress = "State street 123", City = "Citadel", County = "CD", PostCode = "12345678" });

    sql.CreateContact(person);
}
static void GetAllPeople(SqliteCrud sql)
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