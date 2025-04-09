-- Disable safe updates (not applicable in SQL Server, ignore this line)

-- Drop tables if they exist
IF OBJECT_ID('DrinkOfTheDay', 'U') IS NOT NULL DROP TABLE DrinkOfTheDay;
IF OBJECT_ID('Vote', 'U') IS NOT NULL DROP TABLE Vote;
IF OBJECT_ID('UserDrink', 'U') IS NOT NULL DROP TABLE UserDrink;
IF OBJECT_ID('DrinkCategory', 'U') IS NOT NULL DROP TABLE DrinkCategory;
IF OBJECT_ID('Drink', 'U') IS NOT NULL DROP TABLE Drink;
IF OBJECT_ID('Category', 'U') IS NOT NULL DROP TABLE Category;
IF OBJECT_ID('Brand', 'U') IS NOT NULL DROP TABLE Brand;
IF OBJECT_ID('[User]', 'U') IS NOT NULL DROP TABLE [User];

-- Create tables
CREATE TABLE Brand (
    BrandId INT IDENTITY(1,1) PRIMARY KEY,
    BrandName VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Category (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Drink (
    DrinkId INT IDENTITY(1,1) PRIMARY KEY,
    DrinkURL VARCHAR(455),
    DrinkName VARCHAR(255) NOT NULL,
    BrandId INT NULL,
    AlcoholContent DECIMAL(5, 2) NOT NULL,
    FOREIGN KEY (BrandId) REFERENCES Brand(BrandId) ON DELETE SET NULL
);

CREATE TABLE DrinkCategory (
    DrinkId INT,
    CategoryId INT,
    PRIMARY KEY (DrinkId, CategoryId),
    FOREIGN KEY (DrinkId) REFERENCES Drink(DrinkId) ON DELETE CASCADE,
    FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId) ON DELETE CASCADE
);

CREATE TABLE [User] (
    UserId INT PRIMARY KEY
);

CREATE TABLE Vote (
    VoteId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    DrinkId INT,
    VoteTime DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (DrinkId) REFERENCES Drink(DrinkId) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES [User](UserId) ON DELETE CASCADE
);

CREATE TABLE DrinkOfTheDay (
    DrinkId INT PRIMARY KEY,
    DrinkTime DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (DrinkId) REFERENCES Drink(DrinkId) ON DELETE CASCADE
);

CREATE TABLE UserDrink (
    UserId INT,
    DrinkId INT,
    FOREIGN KEY (UserId) REFERENCES [User](UserId) ON DELETE CASCADE,
    FOREIGN KEY (DrinkId) REFERENCES Drink(DrinkId) ON DELETE CASCADE
);

-- Inserts for Brand
INSERT INTO Brand (BrandName) VALUES 
('Ursugi'), ('Bergenbir'), ('Duvel'), ('Heineken'), ('Guinness'),
('Stella Artois'), ('Corona'), ('BrewDog'), ('Chimay'), ('Trappistes Rochefort');

-- Inserts for Category
INSERT INTO Category (CategoryName) VALUES 
('Lager'), ('IPA'), ('Stout'), ('Pilsner'), ('Wheat Beer'),
('Pale Ale'), ('Sour'), ('Porter'), ('Belgian Dubbel'), ('Belgian Tripel'), ('Lambic');

-- Inserts for Drink
INSERT INTO Drink (DrinkName, DrinkURL, BrandId, AlcoholContent) VALUES
('Ursugi IPA', 'https://floradionline.ro/wp-content/uploads/2023/07/Bere-Ursus-IPA-0.33L-1000x1000-1.jpg', 1, 5.0),
('Bergenbir Lager', 'https://magazin.dorsanimpex.ro/userfiles/944eb0c7-a695-44f0-8596-1da751d9458e/products/66412365_big.jpg', 2, 7.2),
('Duvel Belgian Strong', 'https://vinulbun.ro/custom/imagini/produse/275036008_thb_1_5715_706096_bere-duvel-belgian-strong-blonde-0-33l.JPG', 3, 8.5),
('Heineken Lager', 'https://c.cdnmp.net/877205478/p/l/3/heineken-sticla-0-66l-bax-12-buc~19803.jpg', 4, 5.0),
('Guinness Draught', 'https://www.telegraph.co.uk/content/dam/health-fitness/2024/11/26/TELEMMGLPICT000403161538_17326343319300_trans_NvBQzQNjv4BqqVzuuqpFlyLIwiB6NTmJwfSVWeZ_vEN7c6bHu2jJnT8.jpeg?imwidth=680', 5, 4.2),
('Stella Artois Lager', 'https://www.gourmetencasa-tcm.com/15353-large_default/stella-artois-33cl.jpg', 6, 5.0),
('Corona Extra', 'https://la-bax.ro/wp-content/uploads/2024/10/Bere71.png', 7, 4.5),
('BrewDog Punk IPA', 'https://mcgrocer.com/cdn/shop/files/brewdog-punk-ipa-post-modern-classic-40872180547822_grande.jpg?v=1737433484', 8, 5.6),
('Chimay Rouge', 'https://www.belgasorozo.com/wp-content/uploads/Chimay-Rouge.jpg', 9, 7.0),
('Trappistes Rochefort 8', 'https://belgianmart.com/cdn/shop/products/r8.jpg?v=1538785647', 10, 9.2),
('BrewDog Elvis Juice', 'https://brewdog.com/cdn/shop/files/pdp-elvis-juice-beer-330ml-can-brewdog.jpg?v=1723310594', 8, 6.5),
('Heineken Silver', 'https://nitelashop.ro/media/cache/700x700xf/media/catalog/product/h/e/heineken-silver-bere-0_70296465f7a6d110f.jpeg', 4, 4.0),
('Guinness Foreign Extra Stout', 'https://bellbeverage.com/wp-content/uploads/2020/02/Screen-Shot-2020-05-21-at-4.47.42-PM.png', 5, 7.5);

-- DrinkCategory inserts
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES 
(1, 2), (1, 1), (2, 1), (2, 2), (3, 9), (4, 1), (5, 3), (6, 1), 
(7, 1), (8, 2), (9, 9), (10, 3), (11, 2), (12, 1), (13, 3);

-- Users
INSERT INTO [User] (UserId) VALUES (1), (2), (3), (4), (5);

-- Votes
INSERT INTO Vote (UserId, DrinkId, VoteTime) VALUES 
(1, 1, '2025-03-29 12:00:00'),
(2, 2, '2025-03-29 14:00:00'),
(1, 5, '2025-03-30 16:00:00'),
(3, 8, '2025-03-30 10:00:00'),
(4, 5, '2025-03-30 12:00:00'),
(2, 8, '2025-03-31 09:00:00'),
(5, 9, '2025-03-30 14:00:00');

-- Drink of the Day
INSERT INTO DrinkOfTheDay (DrinkId, DrinkTime) VALUES (1, '2025-04-03 08:00:00');

-- UserDrink
INSERT INTO UserDrink (UserId, DrinkId) VALUES 
(1,1),(1,2),(1,3),(1,4),(1,5),(1,6),(1,7),(1,8),(1,9),
(2,2),(2,1),
(3,5),(3,8),
(4,9),
(5,5),(5,1);

-- Optional: SELECTs
SELECT * FROM Drink;
SELECT * FROM Brand;
SELECT * FROM Category;
SELECT * FROM DrinkCategory;
SELECT * FROM [User];
SELECT * FROM Vote;
SELECT * FROM DrinkOfTheDay;
SELECT * FROM UserDrink;