/* =====================================================
   Crear DB distribuidoraX_DB
   ===================================================== 
*/

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'distribuidoraX_DB')
BEGIN
	CREATE DATABASE distribuidoraX_DB;
END
GO

USE distribuidoraX_DB;
GO

/* =====================================================
   Crear tablas de la DB distribuidoraX_DB
   ===================================================== 
*/

CREATE TABLE Proveedor (
	ProveedorId INT IDENTITY(1,1) NOT NULL,
	Nombre VARCHAR(100) NOT NULL,
	Descripcion VARCHAR(200) NULL,
	Activo Bit DEFAULT 1,
	FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
	FechaActualizado DATETIME NOT NULL DEFAULT GETDATE(),
	EstadoEliminado Bit DEFAULT 0,
	CONSTRAINT PK_ProveedorId PRIMARY KEY (ProveedorId)
);
GO

CREATE TABLE TipoProducto (
	TipoProductoId INT IDENTITY(1,1) NOT NULL,
	Nombre VARCHAR(100) NOT NULL,
	Descripcion VARCHAR(200) NULL,
	Activo Bit DEFAULT 1,
	FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
	FechaActualizado DATETIME NOT NULL DEFAULT GETDATE(),
	EstadoEliminado Bit DEFAULT 0,
	CONSTRAINT PK_TipoProductoId PRIMARY KEY (TipoProductoId)
);
GO

CREATE TABLE Producto (
	ProductoId INT IDENTITY(1,1) NOT NULL,
	TipoProductoId INT NOT NULL,
	ClaveProducto CHAR(30) NOT NULL UNIQUE,
	Nombre VARCHAR(70) NOT NULL,
	Precio DECIMAL(14,2) NULL,
	Activo Bit NULL,
	FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
	FechaActualizado DATETIME NOT NULL DEFAULT GETDATE(),
	EstadoEliminado Bit DEFAULT 0,
	CONSTRAINT PK_ProductoId PRIMARY KEY (ProductoId),
	CONSTRAINT FK_Producto_TipoProducto FOREIGN KEY (TipoProductoId) REFERENCES TipoProducto (TipoProductoId)
);
GO

CREATE TABLE ProductoProveedor (
	ProductoProveedorId INT IDENTITY(1,1) NOT NULL,
	ProductoId INT NOT NULL,
	ProveedorId INT NOT NULL,
	ClaveProductoProveedor CHAR(30) NOT NULL UNIQUE,
	Costo DECIMAL(9,2) NOT NULL,
	FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
	FechaActualizado DATETIME NOT NULL DEFAULT GETDATE(),
	EstadoEliminado Bit DEFAULT 0,
	CONSTRAINT PK_ProductoProveedorId PRIMARY KEY (ProductoProveedorId),
	CONSTRAINT FK_ProductoProveedor_ProductoId FOREIGN KEY (ProductoId) REFERENCES Producto (ProductoId),
	CONSTRAINT FK_ProductoProveedor_ProveedorId FOREIGN KEY (ProveedorId) REFERENCES Proveedor (ProveedorId)
);
GO

/* =====================================================
   Insertar registros iniciales
   ===================================================== 
*/

INSERT INTO TipoProducto(Nombre) VALUES('Limpieza');
INSERT INTO TipoProducto(Nombre) VALUES('Higiene Personal');
INSERT INTO TipoProducto(Nombre) VALUES('Embutidos');
GO

INSERT INTO Proveedor(Nombre) VALUES('Distribuidora Mexico');
INSERT INTO Proveedor(Nombre) VALUES('Abarrotes a Granel Ruiz');
INSERT INTO Proveedor(Nombre) VALUES('Surtidora La Morena');
GO


/* =====================================================
   Crear Stored Procedures "TipoProducto"
   ===================================================== 
*/
CREATE PROCEDURE sp_ObtenerListaTipoProducto
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TipoProductoId,Nombre AS TipoProductoNombre 
		FROM TipoProducto;
END
GO


/* =====================================================
   Crear Stored Procedures "Proveedor"
   ===================================================== 
*/
CREATE PROCEDURE sp_ObtenerListaProveedor
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ProveedorId, Nombre AS ProveedorNombre 
		FROM Proveedor;
END
GO

------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_ObtenerProveedorPorId
    @ProveedorId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ProveedorId, Nombre
		FROM Proveedor
	WHERE ProveedorId=@ProveedorId;
END
GO


/* =====================================================
   Crear Stored Procedures "ProductoProveedor"
   ===================================================== 
*/
CREATE PROCEDURE sp_ObtenerListaProductoProveedorPorProductoId
	@ProductoId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT pp.ProductoProveedorId,pp.ClaveProductoProveedor,pp.Costo,pv.ProveedorId,
			pp.ProductoId, pp.EstadoEliminado, pp.FechaActualizado
	FROM ProductoProveedor pp
		INNER JOIN Proveedor pv
		ON pv.ProveedorId=pp.ProveedorId
	WHERE pp.ProductoId=@ProductoId
		AND pp.EstadoEliminado=0;

END
GO

------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_ObtenerListaCostoProductoProveedorPorProductoId
	 @ProductoId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Costo 
		FROM ProductoProveedor 
	WHERE ProductoId=@ProductoId 
		AND EstadoEliminado=0;

END
GO

-------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_ExisteClaveProductoProveedor
    @ProductoProveedorId INT,
	@ClaveProductoProveedor CHAR(30)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE 
		@existeClave INT,
		@ProductoProveedor_FilasEncontradas INT;

	SELECT @ProductoProveedor_FilasEncontradas = Count(*) 
		FROM ProductoProveedor 
	WHERE ClaveProductoProveedor=@ClaveProductoProveedor 
		AND ProductoProveedorId <> @ProductoProveedorId;

	IF  @ProductoProveedor_FilasEncontradas = 0
		BEGIN
			SET @existeClave = 0;
		END
	ELSE
		BEGIN
			SET @existeClave = 1;
		END

	RETURN @existeClave;
END
GO

-----------------------------------------------------------------------------------------
CREATE PROCEDURE sp_AgregarProductoProveedor
	@ProductoId INT,
    @ProveedorId INT,
    @ClaveProveedorProveedor CHAR(30),
	@CostoProducto DECIMAL(9,2)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
		@idParam INT,
		@ProductoProveedorId INT;

	SELECT @idParam = ProductoProveedorId
		FROM ProductoProveedor
	WHERE ClaveProductoProveedor = @ClaveProveedorProveedor;

	IF @idParam IS NULL
		BEGIN
			INSERT INTO ProductoProveedor(ProveedorId,ProductoId,ClaveProductoProveedor,Costo, FechaActualizado)
				VALUES (@ProveedorId,@ProductoId,@ClaveProveedorProveedor,@CostoProducto, GETDATE());

			SET @ProductoProveedorId = SCOPE_IDENTITY();
		END
	ELSE
		BEGIN
			SET @ProductoProveedorId = 0;
		END

	RETURN @ProductoProveedorId;
END
GO

---------------------------------------------------------------------------------------
CREATE PROCEDURE sp_ActualizarProductoProveedor
	@ProductoProveedorId INT,
	@ProveedorId INT,
	@ProductoId INT,
	@ClaveProveedorProveedor CHAR(30),
	@CostoProducto DECIMAL(9,2),
	@EstadoEliminado BIT,
	@FechaActualizado DATETIME,
	@BanderaRetorno BIT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
		@idParam INT,
		@rowIdActualizado INT;

	SELECT @idParam = ProductoProveedorId
		FROM ProductoProveedor
	WHERE ClaveProductoProveedor = @ClaveProveedorProveedor
		AND ProductoProveedorId <> @ProductoProveedorId;

	IF @idParam IS NULL
		BEGIN
			IF @BanderaRetorno = 0
				BEGIN
					UPDATE ProductoProveedor 
						SET ProveedorId=@ProveedorId, ProductoId=@ProductoId, ClaveProductoProveedor=@ClaveProveedorProveedor, 
							Costo=@CostoProducto, FechaActualizado=GETDATE()
						WHERE ProductoProveedorId=@ProductoProveedorId;
				END
			ELSE
				BEGIN
					UPDATE ProductoProveedor 
						SET ProveedorId=@ProveedorId, ProductoId=@ProductoId, ClaveProductoProveedor=@ClaveProveedorProveedor, 
							Costo=@CostoProducto, FechaActualizado=@FechaActualizado, EstadoEliminado=@EstadoEliminado
						WHERE ProductoProveedorId=@ProductoProveedorId;
				END

			SET @rowIdActualizado = @ProductoProveedorId;
		END
	ELSE
		BEGIN
			SET @rowIdActualizado = 0;
		END

	RETURN @rowIdActualizado;
END
GO

------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_EliminarProductoProveedor
    @ProductoProveedorId INT
AS
BEGIN
	DELETE ProductoProveedor 
		WHERE ProductoProveedorId=@ProductoProveedorId;
END
GO

------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_EliminarActualizarProductoProveedor
	@ProductoProveedorId INT,
	@Estadoliminado BIT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE 
		@idParam INT = 0;

	UPDATE ProductoProveedor 
		SET EstadoEliminado=@Estadoliminado, FechaActualizado=GETDATE()
	WHERE ProductoProveedorId=@ProductoProveedorId;

	SET @idParam = @ProductoProveedorId;

	Return @idParam;
END

Go


/* =====================================================
   Crear Stored Procedures "Producto"
   ===================================================== 
*/
CREATE PROCEDURE sp_ObtenerListaProductoPorFiltros
	@Clave CHAR(30)=NULL,
	@NombreTipoProducto VARCHAR(100)=NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.ProductoId,p.Nombre,p.ClaveProducto,
			p.Precio,p.Activo 
	FROM Producto p
		INNER JOIN TipoProducto tp
		ON tp.TipoProductoId=p.TipoProductoId
	WHERE (@clave IS NULL OR p.ClaveProducto = @clave)
		AND (@NombreTipoProducto IS NULL OR tp.Nombre = @NombreTipoProducto)
		AND p.EstadoEliminado=0;

END
GO

----------------------------------------------------------------------------------------
CREATE PROCEDURE sp_ObtenerProductoPorId
	@ProductoId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.ProductoId,p.Nombre,p.ClaveProducto,p.Precio,p.Activo,tp.TipoProductoId,tp.Nombre As TipoProductoNombre 
		FROM Producto p
	INNER JOIN TipoProducto tp
	ON tp.TipoProductoId=p.TipoProductoId
	WHERE p.ProductoId=@ProductoId;

END
GO

---------------------------------------------------------------------------------------
CREATE PROCEDURE sp_ExisteClaveProducto
    @ProductoId INT,
	@ClaveProducto CHAR(30)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE 
		@existeClave INT,
		@Producto_FilasEncontradas INT;

	SELECT @Producto_FilasEncontradas = Count(*) 
		FROM Producto 
	WHERE ClaveProducto=@ClaveProducto AND ProductoId <> @ProductoId;

	IF  @Producto_FilasEncontradas = 0
		BEGIN
			SET @existeClave = 0;
		END
	ELSE
		BEGIN
			SET @existeClave = 1;
		END
	
	RETURN @existeClave;
END
GO

---------------------------------------------------------------------------------------
CREATE PROCEDURE sp_AgregarProducto
    @NombreProducto VARCHAR(70),
    @ClaveProducto CHAR(30),
	@PrecioProducto DECIMAL(14,2),
	@ActivoProducto BIT,
	@TipoProductoId INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE
		@ProductoId INT;

	SELECT @ProductoId = ProductoId
		FROM Producto
	WHERE ClaveProducto = @ClaveProducto;

    IF @ProductoId IS NULL
		BEGIN
            INSERT INTO Producto(TipoProductoId,ClaveProducto,Nombre,Precio,Activo)
            VALUES (@TipoProductoId,@ClaveProducto,@NombreProducto,@PrecioProducto,@ActivoProducto);

            SET @ProductoId = SCOPE_IDENTITY();
		END
	ELSE
		BEGIN
			SET @ProductoId = 0;
		END
	
	RETURN  @ProductoId;
END
GO

---------------------------------------------------------------------------------------
CREATE PROCEDURE sp_ActualizarProducto
    @ProductoId INT,
	@TipoProductoId INT,
	@NombreProducto VARCHAR(70),
    @ClaveProducto CHAR(30),
	@ActivoProducto BIT
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE
		@IdParam INT,
		@ProductoIdParam INT;

	SELECT @ProductoIdParam = ProductoId
		FROM Producto
	WHERE ClaveProducto = @ClaveProducto
		AND ProductoId <> @ProductoId;

	IF @ProductoIdParam IS NULL
		BEGIN
			
			UPDATE Producto 
				SET TipoProductoId=@TipoProductoId, ClaveProducto=@ClaveProducto, 
					Nombre=@NombreProducto, Activo=@ActivoProducto
				WHERE ProductoId=@ProductoId;

			SET @IdParam = @ProductoId;
		END
	ELSE
		BEGIN
			SET @IdParam = 0;
		END

	RETURN @IdParam;
END
GO

-------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_ActualizarProductoPrecio
	 @ProductoId INT,
	 @ClaveProducto CHAR(30),
	 @PrecioProducto DECIMAL(14,2)
	 
AS
BEGIN
	UPDATE Producto 
		SET  Precio=@PrecioProducto
	WHERE ProductoId=@ProductoId AND ClaveProducto=@ClaveProducto;

END
GO

------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_EliminarActualizarProducto
	@ProductoId INT,
	@ClaveProducto CHAR(30)
AS
BEGIN
	DECLARE 
		@idParam INT;


	SELECT @idParam = ProductoProveedorId
		FROM ProductoProveedor
	WHERE ProductoId = @ProductoId;

	IF @idParam IS NULL
		BEGIN
			DELETE Producto 
				WHERE ProductoId=@ProductoId 
					AND ClaveProducto=@ClaveProducto;
		END
	ELSE
		BEGIN
			UPDATE Producto 
				SET EstadoEliminado=1, FechaActualizado=GETDATE()
			WHERE ProductoId=@ProductoId
				AND ClaveProducto = @ClaveProducto;
		END

END
GO



