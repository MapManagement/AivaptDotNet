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
