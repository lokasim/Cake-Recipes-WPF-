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
	UserID INT FOREIGN KEY REFERENCES tblUser(UserID),
	Changed				NVARCHAR(100)
);

CREATE TABLE tblIngredientAmount(
	IngredientAmountID		INT IDENTITY(1,1) PRIMARY KEY 	NOT NULL,
	Amount					INT								NOT NULL,
	RecipeID INT FOREIGN KEY REFERENCES tblRecipe(RecipeID) NOT NULL,
	IngredientID INT FOREIGN KEY REFERENCES tblIngredient(IngredientID)	NOT NULL,
);

CREATE TABLE tblIngredientStorage(
	IngredientStorageID		INT IDENTITY(1,1) PRIMARY KEY 	NOT NULL,
	Amount					INT								NOT NULL,
	UserID INT FOREIGN KEY REFERENCES tblUser(UserID),
	IngredientID INT FOREIGN KEY REFERENCES tblIngredient(IngredientID)	NOT NULL,
);

CREATE TABLE tblShoppingBasket(
	ShoppingBasketID		INT IDENTITY(1,1) PRIMARY KEY 	NOT NULL,
	Amount					INT								NOT NULL,
	UserID INT FOREIGN KEY REFERENCES tblUser(UserID),
	IngredientID INT FOREIGN KEY REFERENCES tblIngredient(IngredientID)	NOT NULL,
);


GO
CREATE VIEW vwRecipe AS
	SELECT        dbo.tblRecipe.RecipeID, dbo.tblRecipe.RecipeName, dbo.tblRecipe.RecipeType, dbo.tblRecipe.NoPeople, dbo.tblRecipe.RecipeDescription, dbo.tblRecipe.CreationDate, dbo.tblRecipe.UserID, dbo.tblRecipe.Changed, 
                         dbo.tblUser.UserID AS Expr1, dbo.tblUser.FirstLastName, dbo.tblUser.Username, dbo.tblUser.UserPassword, dbo.tblIngredient.IngredientID, dbo.tblIngredient.IngredientName, dbo.tblIngredientAmount.IngredientAmountID, 
                         dbo.tblIngredientAmount.Amount, dbo.tblIngredientAmount.RecipeID AS Expr2, dbo.tblIngredientAmount.IngredientID AS Expr3
FROM            dbo.tblIngredient INNER JOIN
                         dbo.tblIngredientAmount ON dbo.tblIngredient.IngredientID = dbo.tblIngredientAmount.IngredientID INNER JOIN
                         dbo.tblRecipe ON dbo.tblIngredientAmount.RecipeID = dbo.tblRecipe.RecipeID INNER JOIN
                         dbo.tblUser ON dbo.tblRecipe.UserID = dbo.tblUser.UserID

GO
CREATE VIEW vwIngredientStorage AS
	SELECT        dbo.tblIngredientStorage.IngredientStorageID, dbo.tblIngredientStorage.Amount, dbo.tblIngredientStorage.UserID, dbo.tblIngredientStorage.IngredientID, dbo.tblIngredient.IngredientID AS Expr1, dbo.tblIngredient.IngredientName, 
                         dbo.tblUser.UserID AS Expr2, dbo.tblUser.FirstLastName, dbo.tblUser.Username, dbo.tblUser.UserPassword
FROM            dbo.tblIngredientStorage INNER JOIN
                         dbo.tblUser ON dbo.tblIngredientStorage.UserID = dbo.tblUser.UserID INNER JOIN
                         dbo.tblIngredient ON dbo.tblIngredientStorage.IngredientID = dbo.tblIngredient.IngredientID

GO
CREATE VIEW vwShoppingBasket AS
	SELECT        dbo.tblShoppingBasket.ShoppingBasketID, dbo.tblShoppingBasket.Amount, dbo.tblShoppingBasket.UserID, dbo.tblShoppingBasket.IngredientID, dbo.tblIngredient.IngredientID AS Expr1, dbo.tblIngredient.IngredientName, 
                         dbo.tblUser.UserID AS Expr2, dbo.tblUser.FirstLastName, dbo.tblUser.Username, dbo.tblUser.UserPassword
FROM            dbo.tblShoppingBasket INNER JOIN
                         dbo.tblUser ON dbo.tblShoppingBasket.UserID = dbo.tblUser.UserID INNER JOIN
                         dbo.tblIngredient ON dbo.tblShoppingBasket.IngredientID = dbo.tblIngredient.IngredientID
	