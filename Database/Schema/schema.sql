SET SQL_SAFE_UPDATES = 0;

DROP TABLE IF EXISTS DrinkOfTheDay;
DROP TABLE IF EXISTS Vote;
DROP TABLE IF EXISTS UserDrink;
DROP TABLE IF EXISTS DrinkCategory;
DROP TABLE IF EXISTS Drink;
DROP TABLE IF EXISTS Category;
DROP TABLE IF EXISTS Brand;
DROP TABLE IF EXISTS User;

CREATE TABLE Brand (
    BrandId INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    BrandName VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Category (
    CategoryId INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    CategoryName VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Drink (
    DrinkId INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    DrinkURL VARCHAR(455),
    DrinkName VARCHAR(255) NOT NULL,
    BrandId INT,
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

CREATE TABLE User (
    UserId INT NOT NULL PRIMARY KEY AUTO_INCREMENT
);

CREATE TABLE Vote (
    VoteId INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    UserId INT,
    DrinkId INT,
    VoteTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (DrinkId) REFERENCES Drink(DrinkId) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES User(UserId) ON DELETE CASCADE
);

CREATE TABLE DrinkOfTheDay (
    DrinkId INT PRIMARY KEY,
    FOREIGN KEY (DrinkId) REFERENCES Drink(DrinkId) ON DELETE CASCADE,
    DrinkTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE UserDrink (
    UserId INT,
    DrinkId INT,
    FOREIGN KEY (UserId) REFERENCES User(UserId) ON DELETE CASCADE,
    FOREIGN KEY (DrinkId) REFERENCES Drink(DrinkId) ON DELETE CASCADE
);

INSERT INTO Brand (BrandName) VALUES ("Ursugi");
INSERT INTO Brand (BrandName) VALUES ("Bergenbir");
INSERT INTO Brand (BrandName) VALUES ("Duvel");
INSERT INTO Brand (BrandName) VALUES ("Heineken");
INSERT INTO Brand (BrandName) VALUES ("Guinness");
INSERT INTO Brand (BrandName) VALUES ("Stella Artois");
INSERT INTO Brand (BrandName) VALUES ("Corona");
INSERT INTO Brand (BrandName) VALUES ("BrewDog");
INSERT INTO Brand (BrandName) VALUES ("Chimay");
INSERT INTO Brand (BrandName) VALUES ("Trappistes Rochefort");

INSERT INTO Category (CategoryName) VALUES ("Lager");
INSERT INTO Category (CategoryName) VALUES ("IPA");
INSERT INTO Category (CategoryName) VALUES ("Stout");
INSERT INTO Category (CategoryName) VALUES ("Pilsner");
INSERT INTO Category (CategoryName) VALUES ("Wheat Beer");
INSERT INTO Category (CategoryName) VALUES ("Pale Ale");
INSERT INTO Category (CategoryName) VALUES ("Sour");
INSERT INTO Category (CategoryName) VALUES ("Porter");
INSERT INTO Category (CategoryName) VALUES ("Belgian Dubbel");
INSERT INTO Category (CategoryName) VALUES ("Belgian Tripel");
INSERT INTO Category (CategoryName) VALUES ("Lambic");

INSERT INTO Drink (DrinkName, DrinkURL, BrandId, AlcoholContent) VALUES
("Ursugi IPA", "https://floradionline.ro/wp-content/uploads/2023/07/Bere-Ursus-IPA-0.33L-1000x1000-1.jpg", 1, 5.0),
("Bergenbir Lager", "https://magazin.dorsanimpex.ro/userfiles/944eb0c7-a695-44f0-8596-1da751d9458e/products/66412365_big.jpg", 2, 7.2),
("Duvel Belgian Strong", "https://vinulbun.ro/custom/imagini/produse/275036008_thb_1_5715_706096_bere-duvel-belgian-strong-blonde-0-33l.JPG", 3, 8.5),
("Heineken Lager", "https://c.cdnmp.net/877205478/p/l/3/heineken-sticla-0-66l-bax-12-buc~19803.jpg", 4, 5.0),
("Guinness Draught", "https://www.telegraph.co.uk/content/dam/health-fitness/2024/11/26/TELEMMGLPICT000403161538_17326343319300_trans_NvBQzQNjv4BqqVzuuqpFlyLIwiB6NTmJwfSVWeZ_vEN7c6bHu2jJnT8.jpeg?imwidth=680", 5, 4.2),
("Stella Artois Lager", "https://www.gourmetencasa-tcm.com/15353-large_default/stella-artois-33cl.jpg", 6, 5.0),
("Corona Extra", "https://la-bax.ro/wp-content/uploads/2024/10/Bere71.png", 7, 4.5),
("BrewDog Punk IPA", "https://mcgrocer.com/cdn/shop/files/brewdog-punk-ipa-post-modern-classic-40872180547822_grande.jpg?v=1737433484", 8, 5.6),
("Chimay Rouge", "https://www.belgasorozo.com/wp-content/uploads/Chimay-Rouge.jpg", 9, 7.0),
("Trappistes Rochefort 8", "https://belgianmart.com/cdn/shop/products/r8.jpg?v=1538785647", 10, 9.2),
("BrewDog Elvis Juice", "https://brewdog.com/cdn/shop/files/pdp-elvis-juice-beer-330ml-can-brewdog.jpg?v=1723310594", 8, 6.5),
("Heineken Silver", "https://nitelashop.ro/media/cache/700x700xf/media/catalog/product/h/e/heineken-silver-bere-0_70296465f7a6d110f.jpeg", 4, 4.0),
("Guinness Foreign Extra Stout", "https://bellbeverage.com/wp-content/uploads/2020/02/Screen-Shot-2020-05-21-at-4.47.42-PM.png", 5, 7.5);

INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (1, 2); 
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (1, 1);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (2, 1);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (2, 2);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (3, 9);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (4, 1);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (5, 3);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (6, 1);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (7, 1);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (8, 2);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (9, 9);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (10, 3);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (11, 2);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (12, 1);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (13, 3);

INSERT INTO User (UserId) VALUES (1);
INSERT INTO User (UserId) VALUES (2);
INSERT INTO User (UserId) VALUES (3);
INSERT INTO User (UserId) VALUES (4);
INSERT INTO User (UserId) VALUES (5);

INSERT INTO Vote (UserId, DrinkId, VoteTime) VALUES (1, 1, "2025-03-29 12:00:00");
INSERT INTO Vote (UserId, DrinkId, VoteTime) VALUES (2, 2, "2025-03-29 14:00:00");
INSERT INTO Vote (UserId, DrinkId, VoteTime) VALUES (1, 5, "2025-03-30 16:00:00");
INSERT INTO Vote (UserId, DrinkId, VoteTime) VALUES (3, 8, "2025-03-30 10:00:00");
INSERT INTO Vote (UserId, DrinkId, VoteTime) VALUES (4, 5, "2025-03-30 12:00:00");
INSERT INTO Vote (UserId, DrinkId, VoteTime) VALUES (2, 8, "2025-03-31 09:00:00");
INSERT INTO Vote (UserId, DrinkId, VoteTime) VALUES (5, 9, "2025-03-30 14:00:00");

INSERT INTO DrinkOfTheDay (DrinkId, DrinkTime) VALUES (1, "2025-03-29 10:00:00");
INSERT INTO DrinkOfTheDay (DrinkId, DrinkTime) VALUES (2, "2025-03-30 11:00:00");
INSERT INTO DrinkOfTheDay (DrinkId, DrinkTime) VALUES (8, "2025-03-31 13:00:00");

INSERT INTO UserDrink (UserId, DrinkId) VALUES (1, 1);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (1, 2);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (1, 3);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (1, 4);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (1, 5);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (1, 6);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (1, 7);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (1, 8);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (1, 9);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (2, 2);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (2, 1);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (3, 5);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (3, 8);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (4, 9);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (5, 5);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (5, 1);

SELECT * FROM Drink;
SELECT * FROM Brand;
SELECT * FROM Category;
SELECT * FROM DrinkCategory;
SELECT * FROM User;
SELECT * FROM Vote;
SELECT * FROM DrinkOfTheDay;
SELECT * FROM UserDrink;