
-- Script para crear la base de datos 

-- Crear la base de datos
CREATE DATABASE TiendaDB;
GO

-- Usar la base de datos
USE TiendaDB;
GO

-- Crear la tabla Productos
CREATE TABLE Productos (
    Id     INT IDENTITY(1,1) PRIMARY KEY,  -- se llena solo: 1, 2, 3...
    Nombre NVARCHAR(100)  NOT NULL,
    Precio DECIMAL(10,2)  NOT NULL,
    Stock  INT            NOT NULL
);
GO

-- Datos de ejemplo
INSERT INTO Productos (Nombre, Precio, Stock) VALUES ('Cuaderno', 35.50, 20);
INSERT INTO Productos (Nombre, Precio, Stock) VALUES ('Pluma azul', 12.00, 50);
INSERT INTO Productos (Nombre, Precio, Stock) VALUES ('Mochila', 450.00, 5);
GO

-- Revisar que si se guardaron
SELECT * FROM Productos;
