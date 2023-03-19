CREATE TABLE [dbo].[PeopleAddress]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PeopleId] INT NOT NULL, 
    [AddressId] INT NOT NULL
)
