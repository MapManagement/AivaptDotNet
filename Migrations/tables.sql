CREATE TABLE simple_command (
  name varchar(16) NOT NULL,
  command_text text DEFAULT NULL,
  title varchar(24) DEFAULT NULL,
  active tinyint(1) NOT NULL,
  creator_id bigint(20) UNSIGNED NOT NULL,
  color varchar(7) NOT NULL DEFAULT '#1ABC9C',
  PRIMARY KEY (name)
);

CREATE TABLE role (
  role_id bigint(20) UNSIGNED NOT NULL,
  guild_id bigint(20) UNSIGNED NOT NULL,
  mod_permissions tinyint(1) NOT NULL,
  PRIMARY KEY (role_id,guild_id)
);

CREATE TABLE guild (
  id bigint(20) UNSIGNED NOT NULL,
  name varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  owner_id bigint(20) UNSIGNED NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE quote (
	id BIGINT UNSIGNED auto_increment NOT NULL,
	user_id BIGINT UNSIGNED NOT NULL,
	text varchar(400) NULL,
	created_at DATETIME NOT NULL,
  PRIMARY KEY (id)
);