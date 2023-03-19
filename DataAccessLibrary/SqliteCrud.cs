﻿using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class SqliteCrud
    {
        private readonly string _connectionString;
        private SqliteDataAccess db = new();

        public SqliteCrud(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public List<BasicPersonModel> GetAllPoeple()
        {
            string sql = "select Id, FirstName, LastName from People;";
            return db.LoadData<BasicPersonModel, dynamic>(sql, new { }, _connectionString);

        }


        public FullPersonModel GetFullPersonById(int id)
        {
            // Data tables have a relation ship with each other so need to created a model all table information toghether

            string sql = "select Id, FirstName, LastName from People where Id= @Id;";
            FullPersonModel output = new();
            output.BasicInfo = db.LoadData<BasicPersonModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

            if (output.BasicInfo == null)
            {
                return null;
            }
            //Link PersonAddress and address tables toghther by Id's
            sql = @"select a.*
                    from Addresses a
                    inner join PersonAddress pa on pa.AddressId = a.Id
                    where pa.PersonId = @Id";

            output.Addresses = db.LoadData<AddressModel, dynamic>(sql, new { Id = id }, _connectionString);

            return output;
        }

        public void CreateContact(FullPersonModel person)
        {
            //save basic person 
            string sql = "insert into People (FirstName, LastName) values (@FirstName, @LastName);";
            db.SaveData(sql, new { person.BasicInfo.FirstName, person.BasicInfo.LastName }, _connectionString);

            //get person id
            sql = "select Id from People where FirstName = @FirstName and LastName = @LastName;";
            var personId = db.LoadData<IdLookUpModel, dynamic>(sql,
                                                              new { person.BasicInfo.FirstName, person.BasicInfo.LastName },
                                                              _connectionString).First().Id;

            foreach (var address in person.Addresses)
            {
                // identify if address exists
                if (address.Id == 0)
                {
                    //insert into link table for that address
                    sql = "insert into Addresses (StreetAddress, City, County, PostCode) " +
                          "values (@StreetAddress, @City, @County, @PostCode);";
                    db.SaveData(sql,
                                new { address.StreetAddress, address.City, address.County, address.PostCode },
                                _connectionString);

                    sql = "select Id from Addresses where PostCode = @PostCode;";
                    address.Id = db.LoadData<IdLookUpModel, dynamic>(sql, new { address.PostCode }, _connectionString).First().Id;
                }
                sql = "insert into PersonAddress (PersonId, AddressId) values (@PersonId, @AddressId);";
                db.SaveData(sql, new { PersonId = personId, AddressId = address.Id }, _connectionString);
            }
        }

        public void UpdatePersonName(BasicPersonModel person)
        {
            string sql = " update People set FirstName = @FirstName, LastName = @LastName where Id = @Id";
            db.SaveData(sql, person, _connectionString);
        }

        public void RemovePhoneNumberFromContact(int personId, int addressId)
        {
            // Find all of the usages of the phone number id
            // IF one delete link and phone number
            // if > 1 delete link for contact

            string sql = "select Id, PersonId, AddressId from PersonAddress where addressId = @AddressId;";
            var links = db.LoadData<PersonAddressModel, dynamic>(sql, new { AddressId = addressId }, _connectionString);

            sql = "delete from PersonAddress where AddressId = @AddressId and PersonId = @PersonId;";
            db.SaveData(sql, new { AddressId = addressId, PersonId = personId }, _connectionString);

            if (links.Count == 1)
            {
                sql = "delete from Addresses where Id = @AddressId;";
                db.SaveData(sql, new { AddressId = addressId }, _connectionString);
            }

        }
    }
}
