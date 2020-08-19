-- Dropping the tables before recreating the database in the order depending how the foreign keys are placed.
IF OBJECT_ID('tblShoppingBasket', 'U') IS NOT NULL DROP TABLE tblShoppingBasket;
IF OBJECT_ID('tblIngredientStorage', 'U') IS NOT NULL DROP TABLE tblIngredientStorage;
IF OBJECT_ID('tblIngredientAmount', 'U') IS NOT NULL DROP TABLE tblIngredientAmount;
IF OBJECT_ID('tblRecipe', 'U') IS NOT NULL DROP TABLE tblRecipe;
IF OBJECT_ID('tblIngredient', 'U') IS NOT NULL DROP TABLE tblIngredient;
IF OBJECT_ID('tblUser', 'U') IS NOT NULL DROP TABLE tblUser;
if OBJECT_ID('vwRecipe','V') IS NOT NULL DROP VIEW vwRecipe;
if OBJECT_ID('vwIngredientStorage','V') IS NOT NULL DROP VIEW vwIngredientStorage;
if OBJECT_ID('vwShoppingBasket','V') IS NOT NULL DROP VIEW vwShoppingBasket;

-- Checks if the database already exists.
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CookDB')
CREATE DATABASE CookDB;
GO

USE CookDB

ALTER DATABASE CookDB COLLATE Croatian_CI_AS;
GO

CREATE TABLE tblUser(
	UserID INT			IDENTITY(1,1) PRIMARY KEY 	NOT NULL,
	FirstLastName		NVARCHAR (100)				NOT NULL,
	Username			NVARCHAR (40) UNIQUE		NOT NULL,
	UserPassword		CHAR (1000)					NOT NULL,
);

CREATE TABLE tblIngredient(
	IngredientID		INT IDENTITY(1,1) PRIMARY KEY 	NOT NULL,
	IngredientName		NVARCHAR (40)					NOT NULL,
);

CREATE TABLE tblRecipe(
	RecipeID			INT IDENTITY(1,1) PRIMARY KEY 	NOT NULL,
	RecipeName			NVARCHAR (40)					NOT NULL,
	RecipeType			NVARCHAR (40)					NOT NULL,
	NoPeople			INT								NOT NULL,
	RecipeDescription	NVARCHAR (1000)					NOT NULL,
	CreationDate		DATETIME						NOT NULL,
	UserID INT FOREIGN KEY REFERENCES tblUser(UserID)	NOT NULL,
	Changed				NVARCHAR(100)
);

CREATE TABLE tblIngredientAmount(
	IngredientAmountID		INT IDENTITY(1,1) PRIMARY KEY 	NOT NULL,
	Amount					INT								NOT NULL,
	RecipeID INT FOREIGN KEY REFERENCES tblRecipe(RecipeID)	NOT NULL,
	IngredientID INT FOREIGN KEY REFERENCES tblIngredient(IngredientID)	NOT NULL,
);

CREATE TABLE tblIngredientStorage(
	IngredientStorageID		INT IDENTITY(1,1) PRIMARY KEY 	NOT NULL,
	Amount					INT								NOT NULL,
	UserID INT FOREIGN KEY REFERENCES tblUser(UserID)		NOT NULL,
	IngredientID INT FOREIGN KEY REFERENCES tblIngredient(IngredientID)	NOT NULL,
);

CREATE TABLE tblShoppingBasket(
	ShoppingBasketID		INT IDENTITY(1,1) PRIMARY KEY 	NOT NULL,
	Amount					INT								NOT NULL,
	UserID INT FOREIGN KEY REFERENCES tblUser(UserID)		NOT NULL,
	IngredientID INT FOREIGN KEY REFERENCES tblIngredient(IngredientID)	NOT NULL,
);

GO
CREATE VIEW vwRecipe AS
	SELECT	tblRecipe.*, tblUser.FirstLastName, tblIngredient.IngredientName, tblIngredientAmount.Amount 
	FROM	tblUser, tblRecipe, tblIngredient, tblIngredientAmount
	WHERE	tblUser.UserID = tblRecipe.UserID 
			AND tblRecipe.RecipeID = tblIngredientAmount.RecipeID 
			AND tblIngredient.IngredientID = tblIngredientAmount.IngredientID

GO
CREATE VIEW vwIngredientStorage AS
	SELECT	tblIngredientStorage.*, tblIngredient.IngredientName 
	FROM	tblIngredient, tblIngredientStorage
	WHERE	tblIngredient.IngredientID = tblIngredientStorage.IngredientID

GO
CREATE VIEW vwShoppingBasket AS
	SELECT	tblShoppingBasket.*, tblIngredient.IngredientName 
	FROM	tblIngredient, tblShoppingBasket
	WHERE	tblIngredient.IngredientID = tblShoppingBasket.IngredientID