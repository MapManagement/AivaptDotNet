CREATE TABLE simple_command (
  name varchar(16) NOT NULL,
  command_text text DEFAULT NULL,
  title varchar(24) DEFAULT NULL,
  active tinyint(1) NOT NULL,
  creator tinytext NOT NULL,
  color varchar(7) NOT NULL DEFAULT '#1ABC9C',
  PRIMARY KEY (name)
);