﻿DROP TABLE Brand;
DROP TABLE Category;
DROP TABLE Drink;
DROP TABLE DrinkCategory;
DROP TABLE User;
DROP TABLE Vote;
DROP TABLE DrinkOfTheDay;
DROP TABLE UserDrink;

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
    FOREIGN KEY (DrinkId) REFERENCES Drink(DrinkId),
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

INSERT INTO Category (CategoryName) VALUES ("Lager");
INSERT INTO Category (CategoryName) VALUES ("IPA");
INSERT INTO Category (CategoryName) VALUES ("Stout");
INSERT INTO Category (CategoryName) VALUES ("Pilsner");

INSERT INTO Drink (BrandId, AlcoholContent) VALUES (1, 5.0);
INSERT INTO Drink (BrandId, AlcoholContent) VALUES (2, 7.2);
INSERT INTO Drink (BrandId, AlcoholContent) VALUES (3, 8.5);

INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (1, 1);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (2, 2);
INSERT INTO DrinkCategory (DrinkId, CategoryId) VALUES (3, 3);

INSERT INTO User (UserId) VALUES (1);
INSERT INTO User (UserId) VALUES (2);

INSERT INTO Vote (UserId, DrinkId, VoteTime) VALUES (1, 1, "2025-03-29 12:00:00");
INSERT INTO Vote (UserId, DrinkId, VoteTime) VALUES (2, 2, "2025-03-29 14:00:00");

INSERT INTO DrinkOfTheDay (DrinkId, DrinkTime) VALUES (1, "2025-03-29 10:00:00");

INSERT INTO UserDrink (UserId, DrinkId) VALUES (1, 1);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (1, 2);
INSERT INTO UserDrink (UserId, DrinkId) VALUES (2, 2);