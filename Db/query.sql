CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Monto DECIMAL(18,2) NOT NULL
);
INSERT INTO Usuarios (Nombre, Monto) VALUES
('Joel', 1000.00),
('Bruno', 53040.00),
('string', 0.00),
('Melissa', 55.00),
('asd', 246.00),
('Juancho', 20.00),
('Amelia', 8000.00),
('Juan', 36.00),
('asdas', 0.00),
('sasa', 0.00),
('sadas', 369.00),
('SD', 50.00);
