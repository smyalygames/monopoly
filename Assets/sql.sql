CREATE TABLE properties (
	property_id int(2) AUTO_INCREMENT PRIMARY KEY NOT NULL, #Easy way to identify with code
	property_name TINYTEXT NOT NULL, #The name of the property
	property_group TINYTEXT NOT NULL, #The group that the property is
	property_value int(3), #How much it costs to buy the property
	property_cost int(3), #How much it is to buy a house/hotel
	property_rent int(2), #How much it costs to rent
	property_house1 int(3), #How much it costs with 1 house
	property_house2 int(3), #How much it costs with 2 houses
	property_house3 int(4), #How much it costs with 3 houses
	property_house4 int(4), #How much it costs with 4 houses
	property_hotel int(4) #How much it costs with a hotel
);

CREATE TABLE cards (
	card_id int(11) AUTO_INCREMENT PRIMARY KEY NOT NULL,
	card_group int(1) NOT NULL,
	card_text TEXT,
	card_function int(11) NOT NULL,
	extra int(11)
);

CREATE TABLE users (
	user_id int(11) AUTO_INCREMENT PRIMARY KEY NOT NULL, #This is a uniquie ID for the user
	user_uid TINYTEXT NOT NULL, #This is the username for the user
	user_email TINYTEXT NOT NULL, #This is the email for the user
	user_pwd LONGTEXT NOT NULL #This is where the hashed password would be stored
);

CREATE TABLE password (
	user_id int(11) PRIMARY KEY NOT NULL, #The unique identifier and email used to change the password
	pwd_selector TEXT NOT NULL, #Where the selector is stored in plaintext
	pwd_token LONGTEXT NOT NULL, #Where the hashed token is stored
	pwd_expires INT NOT NULL, #When the token expires
	FOREIGN KEY (user_id) REFERENCES users(user_id) #This links the user_id to the users table
);

CREATE TABLE plays (
	user_id int(11) PRIMARY KEY NOT NULL, #This is the unique player
	user_plays int(11) NOT NULL, #This is how many times the player has played the game
	FOREIGN KEY (user_id) REFERENCES users(user_id) #This links the user_id to the users table
);

#Not in use

property_name,property_group,property_value,property_cost,property_rent,property_house1,property_house2,property_house3,property_house4,property_hotel

CREATE TABLE money_spent(
    user_id int(11) PRIMARY KEY NOT NULL,
    user_spent int(11) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(user_id)
);