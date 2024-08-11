CREATE DATABASE treasure_hunt_db;
USE treasure_hunt_db;

CREATE TABLE users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) UNIQUE NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE roles (
    role_id INT AUTO_INCREMENT PRIMARY KEY,
    role_name VARCHAR(100) UNIQUE NOT NULL
);

-- Create UserRoles Table
CREATE TABLE user_roles (
    user_id INT,
    role_id INT,
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES roles(role_id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, role_id)
);

CREATE TABLE treasure_maps (
    map_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT,
    rows_count INT NOT NULL,
    cols_count INT NOT NULL,
    p INT NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE
);

CREATE TABLE treasure_cells (
    cell_id INT AUTO_INCREMENT PRIMARY KEY,
    map_id INT,
    row_num INT NOT NULL,
    col_num INT NOT NULL,
    chest_number INT NOT NULL,
    FOREIGN KEY (map_id) REFERENCES treasure_maps(map_id) ON DELETE CASCADE
);

CREATE TABLE results (
    result_id INT AUTO_INCREMENT PRIMARY KEY,
    map_id INT,
    fuel_used DECIMAL(10, 5) NOT NULL,
    calculated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (map_id) REFERENCES treasure_maps(map_id) ON DELETE CASCADE
);

INSERT INTO roles (role_name) VALUES ('Admin'), ('User');
