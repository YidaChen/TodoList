CREATE TABLE user (
	id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
	name VARCHAR(30) NOT NULL,
	email VARCHAR(30) NOT NULL UNIQUE,
	password VARCHAR(255) NOT NULL,
	registerTime TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);
CREATE TABLE todo (
	id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
	user_id INT UNSIGNED NOT NULL,
	content VARCHAR(30) NOT NULL,
	isCompleted boolean NOT NULL,
	completedTime TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (user_id) REFERENCES user (id)
);