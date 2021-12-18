CREATE TABLE simple_command (
  name varchar(16) NOT NULL,
  command_text text DEFAULT NULL,
  title varchar(24) DEFAULT NULL,
  active tinyint(1) NOT NULL,
  creator tinytext NOT NULL,
  color varchar(7) NOT NULL DEFAULT '#1ABC9C',
  PRIMARY KEY (name)
);

CREATE TABLE roles (
  role_id bigint(20) NOT NULL,
  guild_id bigint(20) NOT NULL,
  mod_permissions tinyint(1) NOT NULL,
  PRIMARY KEY (role_id,guild_id)
);

CREATE TABLE guilds (
  id bigint(20) NOT NULL,
  name varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  owner_id bigint(20) NOT NULL,
  PRIMARY KEY (id)
);