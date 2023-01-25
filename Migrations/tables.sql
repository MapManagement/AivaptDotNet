-- InitialCreate
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `guild` (
    `Id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `OwnerId` bigint unsigned NOT NULL,
    CONSTRAINT `PK_guild` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `quote` (
    `Id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `UserId` bigint unsigned NOT NULL,
    `Text` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    CONSTRAINT `PK_quote` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `role` (
    `RoleId` bigint unsigned NOT NULL AUTO_INCREMENT,
    `GuildId` bigint unsigned NOT NULL,
    `ModPermissions` tinyint(1) NOT NULL,
    CONSTRAINT `PK_role` PRIMARY KEY (`RoleId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `simple_command` (
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Text` longtext CHARACTER SET utf8mb4 NULL,
    `Active` tinyint(1) NOT NULL,
    `CreatorId` bigint unsigned NOT NULL,
    `Color` longtext CHARACTER SET utf8mb4 NOT NULL DEFAULT ('#1ABC9C'),
    CONSTRAINT `PK_simple_command` PRIMARY KEY (`Name`)
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230121210207_InitialCreate', '6.0.7');

COMMIT;

-- AddMinecraftModels
START TRANSACTION;

CREATE TABLE `mc_coordinates` (
    `CoordinatesId` int NOT NULL AUTO_INCREMENT,
    `X` bigint unsigned NOT NULL,
    `Y` bigint unsigned NOT NULL,
    `Z` bigint unsigned NOT NULL,
    `SubmitterId` bigint unsigned NOT NULL,
    CONSTRAINT `PK_mc_coordinates` PRIMARY KEY (`CoordinatesId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `mc_location` (
    `LocationId` int unsigned NOT NULL AUTO_INCREMENT,
    `LocationName` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_mc_location` PRIMARY KEY (`LocationId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `McCoordinatesMcLocation` (
    `LinkedMcCoordinatesCoordinatesId` int NOT NULL,
    `LocationsLocationId` int unsigned NOT NULL,
    CONSTRAINT `PK_McCoordinatesMcLocation` PRIMARY KEY (`LinkedMcCoordinatesCoordinatesId`, `LocationsLocationId`),
    CONSTRAINT `FK_McCoordinatesMcLocation_mc_coordinates_LinkedMcCoordinatesCo~` FOREIGN KEY (`LinkedMcCoordinatesCoordinatesId`) REFERENCES `mc_coordinates` (`CoordinatesId`) ON DELETE CASCADE,
    CONSTRAINT `FK_McCoordinatesMcLocation_mc_location_LocationsLocationId` FOREIGN KEY (`LocationsLocationId`) REFERENCES `mc_location` (`LocationId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_McCoordinatesMcLocation_LocationsLocationId` ON `McCoordinatesMcLocation` (`LocationsLocationId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230125210401_AddMinecraftModels', '6.0.7');

COMMIT;
