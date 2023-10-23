IF DB_ID('Shop') IS NOT NULL
BEGIN
	USE master;
	ALTER DATABASE Shop SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Shop;
END
GO

CREATE DATABASE Shop;
GO

USE Shop
GO

CREATE TABLE Categories
(
    Id int NOT NULL IDENTITY PRIMARY KEY,
    [Name] nvarchar(30) NOT NULL CHECK ([Name] <> N'' AND [Name] LIKE '%') UNIQUE
);
GO

CREATE TABLE Manufacturers
(
    Id int NOT NULL IDENTITY PRIMARY KEY,
    [Name] nvarchar(15) NOT NULL CHECK([Name] <> N'' AND [Name] LIKE '%') UNIQUE
); 
GO

CREATE TABLE Products (
	Id int NOT NULL IDENTITY PRIMARY KEY,
	[Name] nvarchar(MAX) NOT NULL CHECK ([Name] <> N'' AND [Name] LIKE '%'),	
	Color nvarchar(20) NOT NULL CHECK (Color <> N'' AND Color LIKE '%'),
	CategoryFK int NOT NULL FOREIGN KEY REFERENCES Categories(Id) ON DELETE CASCADE ON UPDATE CASCADE,
	ManufacturerFK int NOT NULL FOREIGN KEY REFERENCES Manufacturers(Id) ON DELETE NO ACTION ON UPDATE CASCADE,
	[Description] nvarchar(MAX) NULL,
	[Count] int NOT NULL,
	Price money NOT NULL
);
GO

CREATE TABLE Images (
	Id int NOT NULL IDENTITY PRIMARY KEY,
	[Url] nvarchar(100) NOT NULL CHECK ([Url] <> N'') UNIQUE,
	ProductFK int NOT NULL FOREIGN KEY REFERENCES Products(Id) ON DELETE CASCADE ON UPDATE CASCADE
);
GO

CREATE TABLE Roles
(
	Id int NOT NULL IDENTITY PRIMARY KEY,
    [Name] nvarchar(20) NOT NULL CHECK ([Name] <> N'' AND [Name] LIKE '%')
);
GO

CREATE TABLE Buyers
(
    Id int NOT NULL IDENTITY PRIMARY KEY,
    FirstName nvarchar(20) NOT NULL CHECK (FirstName <> N'' AND FirstName LIKE '%'),
	LastName nvarchar(20) NOT NULL CHECK (LastName <> N'' AND LastName LIKE '%'),
	Phone nvarchar(13) NOT NULL CHECK (Phone LIKE '+380[1-9][1-9][1-9][1-9][1-9][1-9][1-9][1-9][1-9]'),
	Email nvarchar(30) NOT NULL CHECK (Email <> N'' AND Email LIKE '%@%.%'),
	Birthday date NULL,
	[Password] nvarchar(50) NOT NULL CHECK ([Password] <> N''),
	RoleFK int NOT NULL FOREIGN KEY REFERENCES Roles(Id) ON DELETE NO ACTION ON UPDATE CASCADE
);
GO

CREATE TABLE Orders
(
    BuyerFK int NOT NULL FOREIGN KEY REFERENCES Buyers(Id) ON DELETE NO ACTION ON UPDATE CASCADE,
    ProductFK int NOT NULL FOREIGN KEY REFERENCES Products(Id) ON DELETE CASCADE ON UPDATE CASCADE,
	[Count] int NOT NULL CHECK ([Count] >= 1),
	[DateTime] datetime2 NOT NULL CHECK ([DateTime] <= GETDATE()),
	PRIMARY KEY(BuyerFK, ProductFK)
);
GO

--заполнение
INSERT INTO Categories([Name])
VALUES('Laptops and computers'), ('Smartphones'), ('TV'), ('Electronics')

INSERT INTO Manufacturers([Name])
VALUES('Apple'), ('Samsung'), ('Acer'), ('Asus')

INSERT INTO Products([Name], Color, CategoryFK, ManufacturerFK, [Description], [Count], Price)
VALUES('Apple iPhone 11 64GB Slim Box (MHDG3)', 'green', 2, 1, 
	   'Экран (6.1", IPS (Liquid Retina HD), 1792x828)/ Apple A13 Bionic/ основная двойная камера: 12 Мп + 12 Мп, фронтальная камера: 12 Мп/ RAM 4 ГБ/ 64 ГБ встроенной памяти/ 3G/ LTE/ GPS/ ГЛОНАСС/ Nano-SIM/ iOS 13 / 3046 мА*ч

Зарядным блоком и наушниками не комплектуется.', 5, 19999),
	  ('Apple iPhone 12 mini 64GB', 'purple', 2, 1,
	  'Экран (5.4", OLED (Super Retina XDR), 2340х1080) / Apple A14 Bionic / двойная основная камера: 12 Мп + 12 Мп, фронтальная камера: 12 Мп / 64 ГБ встроенной памяти / 3G / LTE / 5G / GPS / Nano-SIM, eSIM / iOS 1', 3, 22999),
	  ('Samsung Galaxy S21 8/256GB Phantom (SM-G991BZWGSEK)', 'white', 2, 2,
	  'Экран (6.2", Dynamic AMOLED 2X, 2400x1080)/ Samsung Exynos 2100 (1 x 2.9 ГГц + 3 x 2.8 ГГц + 4 x 2.2 ГГц)/ тройная основная камера: 64 Мп + 12 Мп + 12 Мп, фронтальная 10 Мп/ RAM 8 ГБ/ 256 ГБ встроенной памяти/ 3G/ LTE/ 5G/ GPS/ поддержка 2х SIM-карт (Nano-SIM)/ Android 11.0/ 4000 мА*ч', 7, 25999),
	  ('Samsung Galaxy S20 FE (2021) 6/128GB Cloud (SM-G780GZBDSEK)', 'navy', 2, 2,
	  'Экран (6.5", Super AMOLED, 2400x1080) / Qualcomm Snapdragon 865 (1х2.84 ГГц + 3х2.42 ГГц + 4х1.8 ГГц) / тройная основная камера: 12 Мп + 12 Мп + 8 Мп, фронтальная: 32 Мп / RAM 6 ГБ / 128 ГБ встроенной памяти + microSD (до 1 ТБ) / 3G / LTE / GPS / поддержка 2х SIM-карт (Nano-SIM) / Android 10 / 4500 мА*ч', 9, 16999),
	  ('Apple MacBook Air 13" M1 512GB 2020 (MGNE3)', 'Gold', 1, 1, 'Экран 13.3" Retina (2560x1600) WQXGA, глянцевый / Apple M1 / RAM 8 ГБ / SSD 512 ГБ / Apple M1 Graphics / Wi-Fi / Bluetooth / macOS Big Sur / 1.29 кг / золотой', 3, 42999),
	  ('Asus TUF Gaming F15 FX506LI-HN012 (90NR03T2-M05800)', 'Fortress Gray', 1, 4, 'Экран 15.6" IPS (1920x1080) Full HD 144 Гц, матовый / Intel Core i5-10300H (2.5 - 4.5 ГГц) / RAM 8 ГБ / SSD 512 ГБ / nVidia GeForce GTX 1650 Ti, 4 ГБ / без ОД / LAN / Wi-Fi / Bluetooth / веб-камера / без ОС / 2.3 кг / серый', 10, 24444),
	  ('Acer Nitro 5 AN515-55-52GP (NH.QB2EU.012)', 'Obsidian Black', 1, 3, 'Экран 15.6” IPS (1920x1080) Full HD 144 Гц, матовый / Intel Core i5-10300H (2.5 - 4.5 ГГц) / RAM 16 ГБ / SSD 512 ГБ / nVidia GeForce RTX 3060, 6 ГБ / без ОД / LAN / Wi-Fi / Bluetooth / веб-камера / без ОС / 2.3 кг / черный', 5, 34999),
	  ('Asus Zen AiO 24 F5401WUAK-BA006M (90PT02Z1-M05890)', 'Black', 1, 4, 'Экран 23.8" IPS (1920x1080) Full HD / AMD Ryzen 3 5300U (2.6 - 3.8 ГГц) / RAM 8 ГБ / SSD 256 ГБ / AMD Radeon Graphics / без ОД / LAN / Wi-Fi / Bluetooth / веб-камера / без ОС / 7 кг / черный', 7, 16892)

INSERT INTO Images ([Url], ProductFK)
VALUES ('/images/iPhone-11-64GB-Slim-Box-(MHDG3).jpg', 1), ('/images/iphone-12-mini-purple.jpg', 2),
	   ('/images/Galaxy-S21-Phantom-(SM-G991BZWGSEK).jpg', 3), ('/images/Galaxy-S20-FE-(SM-G780GZBDSEK).jpg', 4),
	   ('/images/Apple-MacBook-Air-13-M1-512GB-2020-(MGNE3).jpg', 5),
	   ('/images/Apple-MacBook-Air-13-M1-512GB-2020-(MGNE3)(2).jpg', 5),
	   ('/images/Asus-TUF-Gaming-F15-FX506LI-HN039-(90NR03T1-M03870).jpg', 6),
	   ('/images/Asus-TUF-Gaming-F15-FX506LI-HN039-(90NR03T1-M03870)(2).jpg', 6),
	   ('/images/Acer-Nitro-5-AN515-55-52GP-(NH.QB2EU.012).jpg', 7),
	   ('/images/Acer-Nitro-5-AN515-55-52GP-(NH.QB2EU.012)(2).jpg', 7),
	   ('/images/Asus-Zen-AiO-24-F5401WUAK-BA006M-(90PT02Z1-M05890).jpg', 8), 
	   ('/images/Asus-Zen-AiO-24-F5401WUAK-BA006M-(90PT02Z1-M05890)(2).jpg', 8)

INSERT INTO Roles([Name])
VALUES ('user'), ('admin')

INSERT INTO Buyers([FirstName], [LastName], [Phone], [Email], [Birthday], [RoleFK], [Password])
VALUES ('Vladislava', 'Golovan', '+380965323364', 'admin@gmail.com', NULL, 2, 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=')

--SELECT *
--FROM Categories

--SELECT *
--FROM Manufacturers

--SELECT Products.Name, Color, Images.Url, Categories.Name AS 'Category', Manufacturers.Name AS 'Manufacturer'
--FROM Products
--	 JOIN Categories ON CategoryFK = Categories.Id
--	 JOIN Manufacturers ON ManufacturerFK = Manufacturers.Id
--	 JOIN Images ON Images.ProductFK = Products.Id

SELECT Email, [Password], Roles.Name AS 'Role'
FROM Buyers JOIN Roles ON Buyers.RoleFK = Roles.Id

--SELECT *
--FROM Users

SELECT *
FROM Orders