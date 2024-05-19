-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: db
-- Generation Time: May 19, 2024 at 04:24 AM
-- Server version: 8.2.0
-- PHP Version: 8.2.8

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `HaloHotPot`
--

-- --------------------------------------------------------

--
-- Table structure for table `BTables`
--

CREATE TABLE `BTables` (
  `BTableId` int NOT NULL,
  `Status` int NOT NULL,
  `Capacity` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `BTables`
--

INSERT INTO `BTables` (`BTableId`, `Status`, `Capacity`) VALUES
(1, 1, 4),
(2, 1, 4),
(3, 0, 6),
(4, 1, 6),
(5, 0, 8),
(6, 1, 8),
(7, 1, 12),
(8, 1, 12);

-- --------------------------------------------------------

--
-- Table structure for table `Categories`
--

CREATE TABLE `Categories` (
  `CategoryId` int NOT NULL,
  `CategoryName` varchar(128) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `LastUpdatedAt` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Categories`
--

INSERT INTO `Categories` (`CategoryId`, `CategoryName`, `CreatedAt`, `LastUpdatedAt`) VALUES
(1, 'Meat v1', '0001-01-01 00:00:00.000000', '2024-05-15 11:28:18.198202'),
(2, 'HotPot v2', '0001-01-01 00:00:00.000000', '2024-05-15 11:28:46.340538'),
(3, 'Dessert', '2024-05-15 11:26:22.184473', '2024-05-15 11:26:22.184473'),
(4, 'Beverage', '2024-05-15 11:30:44.452360', '2024-05-15 11:30:44.452361'),
(5, 'Seafood', '2024-05-15 11:30:49.841898', '2024-05-15 11:30:49.841898');

-- --------------------------------------------------------

--
-- Table structure for table `Images`
--

CREATE TABLE `Images` (
  `ImageId` int NOT NULL,
  `ImageUrl` varchar(128) NOT NULL,
  `ItemId` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Images`
--

INSERT INTO `Images` (`ImageId`, `ImageUrl`, `ItemId`) VALUES
(1, '/Items/3w0zm30v.qha.jpg', 1),
(2, '/Items/matydden.cq2.jpg', 2),
(3, '/Items/dnf5jqtq.nag.jpg', 3);

-- --------------------------------------------------------

--
-- Table structure for table `Ingredients`
--

CREATE TABLE `Ingredients` (
  `IngredientId` int NOT NULL,
  `IngredientName` varchar(128) NOT NULL,
  `IngredientDescription` varchar(255) NOT NULL,
  `UnitOfMeasurement` varchar(128) NOT NULL,
  `ExpiredTime` time(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Ingredients`
--

INSERT INTO `Ingredients` (`IngredientId`, `IngredientName`, `IngredientDescription`, `UnitOfMeasurement`, `ExpiredTime`) VALUES
(1, 'Shallots', 'Shallots are cousins of onions and garlic in the allium family, with hints of each but a subtler flavor. They\'re heavily used in classic French cooking but are frequently encountered in Southeast Asian cuisines', 'grams', '72:17:26.000000'),
(2, 'Fried Flour', 'important ingredient for crispy texture fried food', 'grams', '00:00:00.000000'),
(3, 'Pork Chop', 'Fresh Pork Chap', 'grams', '23:00:00.353000'),
(4, 'Salmon', 'Fresh Salmon From New World', 'grams', '36:00:00.000000'),
(6, 'Shrimp', 'Fresh Shrimp From Binh Dien Market', 'grams', '48:00:00.000000'),
(7, 'Beef', 'Raw Fresh Red Beef', 'grams', '72:00:00.000000'),
(8, 'Chili', 'Red Chilis', 'grams', '168:00:00.000000'),
(9, 'Brasil', 'Imported Brasil for HotPot smell texture', 'grams', '240:00:00.000000'),
(10, 'Fried Oil', 'Fried Oil replace once for every 6h', 'mililiters', '06:00:00.000000'),
(11, 'Xinyan Special Ingredient Bag', 'HotPot Ingredient for Mala HotPot', 'grams', '00:00:00.000000');

-- --------------------------------------------------------

--
-- Table structure for table `Ingredient_Stocks`
--

CREATE TABLE `Ingredient_Stocks` (
  `Id` int NOT NULL,
  `IngredientId` int NOT NULL,
  `StockId` int NOT NULL,
  `CurrentQuantity` int NOT NULL,
  `ExpiredAt` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Ingredient_Stocks`
--

INSERT INTO `Ingredient_Stocks` (`Id`, `IngredientId`, `StockId`, `CurrentQuantity`, `ExpiredAt`) VALUES
(1, 1, 3, 10000, '2024-05-19 09:54:01.086754'),
(2, 2, 3, 10000, NULL),
(4, 7, 4, 20000, '2024-05-16 09:40:50.958247'),
(5, 6, 4, 10500, '2024-05-18 09:40:50.965017'),
(6, 3, 5, 20000, '2024-05-19 02:55:34.439961'),
(7, 6, 5, 20000, '2024-05-20 03:55:34.092176'),
(8, 7, 5, 30000, '2024-05-21 03:55:34.093969'),
(9, 11, 6, 10000, NULL),
(10, 9, 6, 50000, '2024-05-28 04:05:21.900416');

-- --------------------------------------------------------

--
-- Table structure for table `Items`
--

CREATE TABLE `Items` (
  `ItemId` int NOT NULL,
  `ItemName` varchar(128) NOT NULL,
  `ItemDescription` varchar(255) NOT NULL,
  `OriginalPrice` decimal(18,2) NOT NULL,
  `CategoryId` int NOT NULL,
  `IsLocked` tinyint(1) NOT NULL,
  `IsCharged` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Items`
--

INSERT INTO `Items` (`ItemId`, `ItemName`, `ItemDescription`, `OriginalPrice`, `CategoryId`, `IsLocked`, `IsCharged`) VALUES
(1, 'Fried Pork', 'Crispy Fried Pork With Hot Sauces', 11.22, 1, 0, 0),
(2, 'Fried Shrimp', 'Cripsy Shrimp With sepcial sauces', 5.00, 5, 0, 0),
(3, 'Spicy Mala HotPot', 'Oil, Chili, and special Halo\'s Ingredient ', 12.00, 2, 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `Item_Ingredients`
--

CREATE TABLE `Item_Ingredients` (
  `Id` int NOT NULL,
  `IngredientId` int NOT NULL,
  `ItemId` int NOT NULL,
  `MaxQuantity` int NOT NULL,
  `MinQuantity` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Item_Ingredients`
--

INSERT INTO `Item_Ingredients` (`Id`, `IngredientId`, `ItemId`, `MaxQuantity`, `MinQuantity`) VALUES
(1, 2, 1, 500, 300),
(2, 3, 1, 500, 300),
(3, 2, 2, 450, 500),
(4, 6, 2, 300, 200),
(5, 11, 3, 300, 225),
(6, 9, 3, 50, 25),
(7, 7, 3, 250, 250);

-- --------------------------------------------------------

--
-- Table structure for table `OrderDetails`
--

CREATE TABLE `OrderDetails` (
  `OrderDetailId` int NOT NULL,
  `Quantity` int NOT NULL,
  `ItemId` int NOT NULL,
  `OrderId` int NOT NULL DEFAULT '0',
  `CreatedAt` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `DetailNote` longtext NOT NULL,
  `DetailStatus` int NOT NULL DEFAULT '0',
  `LastUpdatedAt` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `OrderDetails`
--

INSERT INTO `OrderDetails` (`OrderDetailId`, `Quantity`, `ItemId`, `OrderId`, `CreatedAt`, `DetailNote`, `DetailStatus`, `LastUpdatedAt`) VALUES
(3, 2, 1, 3, '2024-05-16 16:19:53.526770', 'More Hot Sauces', 3, '2024-05-17 02:24:08.069180'),
(4, 1, 3, 3, '2024-05-16 16:19:53.575629', 'Less Chili Oil', 0, '2024-05-16 16:19:53.575629'),
(5, 1, 1, 4, '2024-05-17 02:40:34.089294', 'More soy sauce', 4, '2024-05-18 04:07:16.469397'),
(6, 1, 2, 4, '2024-05-17 02:40:34.107048', 'Large Shrimp', 2, '2024-05-18 04:08:11.381532'),
(7, 1, 3, 4, '2024-05-17 02:40:34.107614', 'Less Brasil, and more chilis', 5, '2024-05-18 04:08:41.400401'),
(8, 1, 1, 4, '2024-05-17 02:52:46.801008', 'More Red Sauces', 4, '2024-05-18 04:07:27.521589'),
(9, 2, 1, 9, '2024-05-19 04:05:18.311535', 'Add Hot Sauces', 4, '2024-05-19 04:06:45.895126'),
(10, 1, 3, 9, '2024-05-19 04:05:18.360265', '', 4, '2024-05-19 04:06:34.394805');

-- --------------------------------------------------------

--
-- Table structure for table `Orders`
--

CREATE TABLE `Orders` (
  `OrderId` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `LastUpdatedAt` datetime(6) NOT NULL,
  `NumberOfCustomers` int NOT NULL,
  `Status` int NOT NULL,
  `BTableId` int NOT NULL,
  `EmployeeId` varchar(255) NOT NULL DEFAULT '',
  `PaymentId` int NOT NULL DEFAULT '0',
  `RatingId` int NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Orders`
--

INSERT INTO `Orders` (`OrderId`, `CreatedAt`, `LastUpdatedAt`, `NumberOfCustomers`, `Status`, `BTableId`, `EmployeeId`, `PaymentId`, `RatingId`) VALUES
(3, '2024-05-16 12:32:45.176505', '2024-05-16 12:32:45.176521', 4, 0, 1, '1f0abd66-58f4-44ba-8262-92ed7a2d3c6c', 0, 1),
(4, '2024-05-18 02:37:29.227727', '2024-05-17 02:37:29.227743', 1, 0, 2, '1f0abd66-58f4-44ba-8262-92ed7a2d3c6c', 0, 0),
(6, '2024-05-17 02:50:42.988298', '2024-05-17 02:50:42.988318', 5, 1, 3, '1f0abd66-58f4-44ba-8262-92ed7a2d3c6c', 0, 0),
(8, '2024-05-17 07:35:47.895874', '2024-05-17 07:35:47.895874', 6, 1, 5, '1f0abd66-58f4-44ba-8262-92ed7a2d3c6c', 0, 0),
(9, '2024-05-18 08:47:14.140038', '2024-05-19 04:08:28.946935', 4, 0, 1, '1f0abd66-58f4-44ba-8262-92ed7a2d3c6c', 3, 0);

-- --------------------------------------------------------

--
-- Table structure for table `Order_IngredientStocks`
--

CREATE TABLE `Order_IngredientStocks` (
  `Id` int NOT NULL,
  `OrderId` int NOT NULL,
  `IngredientStockId` int NOT NULL,
  `UsedQuantity` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Order_IngredientStocks`
--

INSERT INTO `Order_IngredientStocks` (`Id`, `OrderId`, `IngredientStockId`, `UsedQuantity`) VALUES
(3, 3, 2, 800),
(12, 4, 2, 1275),
(13, 4, 6, 800),
(14, 4, 5, 250),
(15, 9, 2, 800),
(16, 9, 9, 262),
(17, 9, 10, 37),
(18, 9, 8, 250);

-- --------------------------------------------------------

--
-- Table structure for table `Payments`
--

CREATE TABLE `Payments` (
  `PaymentId` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `LastUpdatedAt` datetime(6) NOT NULL,
  `Ammount` double NOT NULL,
  `PaymentMethod` int NOT NULL,
  `Status` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Payments`
--

INSERT INTO `Payments` (`PaymentId`, `CreatedAt`, `LastUpdatedAt`, `Ammount`, `PaymentMethod`, `Status`) VALUES
(1, '2024-05-18 16:06:28.000000', '2024-05-19 16:06:28.000000', 12002312, 2, 1),
(2, '2024-05-19 03:43:43.585509', '2024-05-19 03:43:43.585523', 40000000, 0, 0),
(3, '2024-05-19 04:08:28.967173', '2024-05-19 04:08:28.967173', 1, 0, 1);

-- --------------------------------------------------------

--
-- Table structure for table `Ratings`
--

CREATE TABLE `Ratings` (
  `RatingId` int NOT NULL,
  `Star` int NOT NULL,
  `Comment` longtext
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Ratings`
--

INSERT INTO `Ratings` (`RatingId`, `Star`, `Comment`) VALUES
(1, 5, 'Very Satisfied');

-- --------------------------------------------------------

--
-- Table structure for table `RoleClaims`
--

CREATE TABLE `RoleClaims` (
  `Id` int NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `Roles`
--

CREATE TABLE `Roles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Roles`
--

INSERT INTO `Roles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
('176de4e1-31d4-4bda-bcfe-eaae450b013b', 'Manager', 'MANAGER', NULL),
('2b55e142-ef0f-486d-9c06-146382e26fe8', 'Waiter', 'WAITER', NULL),
('72b748ff-30b9-4553-a974-6f5e890c37b9', 'Chef', 'CHEF', NULL),
('b011a5c0-939c-4f52-a173-c5d79935da29', 'Customer', 'CUSTOMER', NULL),
('b1c916c2-f188-41da-be7b-a9be89b5af70', 'Administrator', 'ADMINISTRATOR', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `Stocks`
--

CREATE TABLE `Stocks` (
  `StockId` int NOT NULL,
  `NumberOfIngredient` int NOT NULL,
  `ArrivedDate` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Stocks`
--

INSERT INTO `Stocks` (`StockId`, `NumberOfIngredient`, `ArrivedDate`) VALUES
(1, 3, '2024-05-16 14:42:42.000000'),
(2, 4, '2024-05-14 14:42:42.000000'),
(3, 0, '2024-05-16 09:36:34.896693'),
(4, 2, '2024-05-16 09:40:50.883702'),
(5, 3, '2024-05-18 03:55:34.019020'),
(6, 2, '2024-05-18 04:05:21.897324');

-- --------------------------------------------------------

--
-- Table structure for table `UserClaims`
--

CREATE TABLE `UserClaims` (
  `Id` int NOT NULL,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `UserLogins`
--

CREATE TABLE `UserLogins` (
  `LoginProvider` varchar(255) NOT NULL,
  `ProviderKey` varchar(255) NOT NULL,
  `ProviderDisplayName` longtext,
  `UserId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `UserRoles`
--

CREATE TABLE `UserRoles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `Users`
--

CREATE TABLE `Users` (
  `Id` varchar(255) NOT NULL,
  `FullName` longtext,
  `Contact` longtext,
  `Address` longtext,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `ConcurrencyStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Users`
--

INSERT INTO `Users` (`Id`, `FullName`, `Contact`, `Address`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`) VALUES
('1f0abd66-58f4-44ba-8262-92ed7a2d3c6c', 'Trần Nhựt Anh', 'số 2 đường 17', 'Quận 7 , HCM', 'user@example.com', 'USER@EXAMPLE.COM', 'user@example.com', 'USER@EXAMPLE.COM', 0, 'AQAAAAIAAYagAAAAEBzV3/l3sHbpN9WPdcee6xHywP8liPmAkRAKfr4kRMSN5Gz1Ao0jQasc0MA84o10kA==', '23PO2RQCGPHFIVF4T7QTUTKAV3X6BN7H', 'fdabfac7-8372-43c3-abcd-7f9d51c20309', NULL, 0, 0, NULL, 1, 0);

-- --------------------------------------------------------

--
-- Table structure for table `UserTokens`
--

CREATE TABLE `UserTokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Value` longtext
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `__EFMigrationsHistory`
--

CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `__EFMigrationsHistory`
--

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20240515100956_InitialCreate', '8.0.3'),
('20240515122601_AddOrderModel', '8.0.3'),
('20240515153504_AddIngredientAndAlterOrder', '8.0.3'),
('20240516091251_AlterIngredientStock', '8.0.3'),
('20240517113041_ChangeRelationshipToOrderStockIngredient', '8.0.3');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `BTables`
--
ALTER TABLE `BTables`
  ADD PRIMARY KEY (`BTableId`);

--
-- Indexes for table `Categories`
--
ALTER TABLE `Categories`
  ADD PRIMARY KEY (`CategoryId`);

--
-- Indexes for table `Images`
--
ALTER TABLE `Images`
  ADD PRIMARY KEY (`ImageId`),
  ADD KEY `IX_Images_ItemId` (`ItemId`);

--
-- Indexes for table `Ingredients`
--
ALTER TABLE `Ingredients`
  ADD PRIMARY KEY (`IngredientId`);

--
-- Indexes for table `Ingredient_Stocks`
--
ALTER TABLE `Ingredient_Stocks`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Ingredient_Stocks_IngredientId` (`IngredientId`),
  ADD KEY `IX_Ingredient_Stocks_StockId` (`StockId`);

--
-- Indexes for table `Items`
--
ALTER TABLE `Items`
  ADD PRIMARY KEY (`ItemId`),
  ADD KEY `IX_Items_CategoryId` (`CategoryId`);

--
-- Indexes for table `Item_Ingredients`
--
ALTER TABLE `Item_Ingredients`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Item_Ingredients_IngredientId` (`IngredientId`),
  ADD KEY `IX_Item_Ingredients_ItemId` (`ItemId`);

--
-- Indexes for table `OrderDetails`
--
ALTER TABLE `OrderDetails`
  ADD PRIMARY KEY (`OrderDetailId`),
  ADD KEY `IX_OrderDetails_ItemId` (`ItemId`),
  ADD KEY `IX_OrderDetails_OrderId` (`OrderId`);

--
-- Indexes for table `Orders`
--
ALTER TABLE `Orders`
  ADD PRIMARY KEY (`OrderId`),
  ADD KEY `IX_Orders_BTableId` (`BTableId`),
  ADD KEY `IX_Orders_EmployeeId` (`EmployeeId`),
  ADD KEY `IX_Orders_PaymentId` (`PaymentId`),
  ADD KEY `IX_Orders_RatingId` (`RatingId`);

--
-- Indexes for table `Order_IngredientStocks`
--
ALTER TABLE `Order_IngredientStocks`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Order_IngredientStocks_IngredientStockId` (`IngredientStockId`),
  ADD KEY `IX_Order_IngredientStocks_OrderId` (`OrderId`);

--
-- Indexes for table `Payments`
--
ALTER TABLE `Payments`
  ADD PRIMARY KEY (`PaymentId`);

--
-- Indexes for table `Ratings`
--
ALTER TABLE `Ratings`
  ADD PRIMARY KEY (`RatingId`);

--
-- Indexes for table `RoleClaims`
--
ALTER TABLE `RoleClaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_RoleClaims_RoleId` (`RoleId`);

--
-- Indexes for table `Roles`
--
ALTER TABLE `Roles`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `RoleNameIndex` (`NormalizedName`);

--
-- Indexes for table `Stocks`
--
ALTER TABLE `Stocks`
  ADD PRIMARY KEY (`StockId`);

--
-- Indexes for table `UserClaims`
--
ALTER TABLE `UserClaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_UserClaims_UserId` (`UserId`);

--
-- Indexes for table `UserLogins`
--
ALTER TABLE `UserLogins`
  ADD PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  ADD KEY `IX_UserLogins_UserId` (`UserId`);

--
-- Indexes for table `UserRoles`
--
ALTER TABLE `UserRoles`
  ADD PRIMARY KEY (`UserId`,`RoleId`),
  ADD KEY `IX_UserRoles_RoleId` (`RoleId`);

--
-- Indexes for table `Users`
--
ALTER TABLE `Users`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  ADD KEY `EmailIndex` (`NormalizedEmail`);

--
-- Indexes for table `UserTokens`
--
ALTER TABLE `UserTokens`
  ADD PRIMARY KEY (`UserId`,`LoginProvider`,`Name`);

--
-- Indexes for table `__EFMigrationsHistory`
--
ALTER TABLE `__EFMigrationsHistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `BTables`
--
ALTER TABLE `BTables`
  MODIFY `BTableId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `Categories`
--
ALTER TABLE `Categories`
  MODIFY `CategoryId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `Images`
--
ALTER TABLE `Images`
  MODIFY `ImageId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `Ingredients`
--
ALTER TABLE `Ingredients`
  MODIFY `IngredientId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `Ingredient_Stocks`
--
ALTER TABLE `Ingredient_Stocks`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `Items`
--
ALTER TABLE `Items`
  MODIFY `ItemId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `Item_Ingredients`
--
ALTER TABLE `Item_Ingredients`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `OrderDetails`
--
ALTER TABLE `OrderDetails`
  MODIFY `OrderDetailId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `Orders`
--
ALTER TABLE `Orders`
  MODIFY `OrderId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `Order_IngredientStocks`
--
ALTER TABLE `Order_IngredientStocks`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT for table `Payments`
--
ALTER TABLE `Payments`
  MODIFY `PaymentId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `Ratings`
--
ALTER TABLE `Ratings`
  MODIFY `RatingId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `RoleClaims`
--
ALTER TABLE `RoleClaims`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `Stocks`
--
ALTER TABLE `Stocks`
  MODIFY `StockId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `UserClaims`
--
ALTER TABLE `UserClaims`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `Images`
--
ALTER TABLE `Images`
  ADD CONSTRAINT `FK_Images_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`ItemId`) ON DELETE CASCADE;

--
-- Constraints for table `Ingredient_Stocks`
--
ALTER TABLE `Ingredient_Stocks`
  ADD CONSTRAINT `FK_Ingredient_Stocks_Ingredients_IngredientId` FOREIGN KEY (`IngredientId`) REFERENCES `Ingredients` (`IngredientId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Ingredient_Stocks_Stocks_StockId` FOREIGN KEY (`StockId`) REFERENCES `Stocks` (`StockId`) ON DELETE CASCADE;

--
-- Constraints for table `Items`
--
ALTER TABLE `Items`
  ADD CONSTRAINT `FK_Items_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`CategoryId`) ON DELETE CASCADE;

--
-- Constraints for table `Item_Ingredients`
--
ALTER TABLE `Item_Ingredients`
  ADD CONSTRAINT `FK_Item_Ingredients_Ingredients_IngredientId` FOREIGN KEY (`IngredientId`) REFERENCES `Ingredients` (`IngredientId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Item_Ingredients_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`ItemId`) ON DELETE CASCADE;

--
-- Constraints for table `OrderDetails`
--
ALTER TABLE `OrderDetails`
  ADD CONSTRAINT `FK_OrderDetails_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`ItemId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_OrderDetails_Orders_OrderId` FOREIGN KEY (`OrderId`) REFERENCES `Orders` (`OrderId`) ON DELETE CASCADE;

--
-- Constraints for table `Orders`
--
ALTER TABLE `Orders`
  ADD CONSTRAINT `FK_Orders_BTables_BTableId` FOREIGN KEY (`BTableId`) REFERENCES `BTables` (`BTableId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Orders_Users_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `Order_IngredientStocks`
--
ALTER TABLE `Order_IngredientStocks`
  ADD CONSTRAINT `FK_Order_IngredientStocks_Ingredient_Stocks_IngredientStockId` FOREIGN KEY (`IngredientStockId`) REFERENCES `Ingredient_Stocks` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Order_IngredientStocks_Orders_OrderId` FOREIGN KEY (`OrderId`) REFERENCES `Orders` (`OrderId`) ON DELETE CASCADE;

--
-- Constraints for table `RoleClaims`
--
ALTER TABLE `RoleClaims`
  ADD CONSTRAINT `FK_RoleClaims_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `UserClaims`
--
ALTER TABLE `UserClaims`
  ADD CONSTRAINT `FK_UserClaims_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `UserLogins`
--
ALTER TABLE `UserLogins`
  ADD CONSTRAINT `FK_UserLogins_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `UserRoles`
--
ALTER TABLE `UserRoles`
  ADD CONSTRAINT `FK_UserRoles_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_UserRoles_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `UserTokens`
--
ALTER TABLE `UserTokens`
  ADD CONSTRAINT `FK_UserTokens_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
