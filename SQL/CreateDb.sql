use master
go

create database FacturasAxoft
go

use FacturasAxoft
go


CREATE TABLE [dbo].[Articulos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](10) NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
	[Precio] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_Articulo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 16/5/2024 18:46:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Cuil] [varchar](12) NOT NULL,
	[Nombre] [varchar](30) NOT NULL,
	[Direccion] [varchar](50) NOT NULL,
	[PorcentajeIva] [decimal](18, 4) NOT NULL
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FacturaRenglones]    Script Date: 16/5/2024 18:46:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FacturaRenglones](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FacturaId] [int] NOT NULL,
	[ArticuloId] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	    [Subtotal] [decimal](18, 4) NOT NULL
 CONSTRAINT [PK_FacturaRenglones] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Facturas]    Script Date: 16/5/2024 18:46:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Facturas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Numero] [int] NOT NULL,
	[Fecha] [date] NOT NULL,
	[ClienteId] [int] NOT NULL,
	[TotalIVa] [decimal](18, 4) NOT NULL,
	[TotalConImpuestos] [decimal](18, 4) NOT NULL,
	[TotalSinImpuestos] [decimal](18, 4) NOT NULL,
    [PorcentajeIva] [decimal](18, 4) NOT NULL

 CONSTRAINT [PK_Facturas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FacturaRenglones]  WITH CHECK ADD  CONSTRAINT [FK_FacturaRenglones_Articulos] FOREIGN KEY([ArticuloId])
REFERENCES [dbo].[Articulos] ([Id])
GO
ALTER TABLE [dbo].[FacturaRenglones] CHECK CONSTRAINT [FK_FacturaRenglones_Articulos]
GO
ALTER TABLE [dbo].[FacturaRenglones]  WITH CHECK ADD  CONSTRAINT [FK_FacturaRenglones_Facturas] FOREIGN KEY([FacturaId])
REFERENCES [dbo].[Facturas] ([Id])
GO
ALTER TABLE [dbo].[FacturaRenglones] CHECK CONSTRAINT [FK_FacturaRenglones_Facturas]
GO
ALTER TABLE [dbo].[Facturas]  WITH CHECK ADD  CONSTRAINT [FK_Facturas_Clientes] FOREIGN KEY([ClienteId])
REFERENCES [dbo].[Clientes] ([Id])
GO
ALTER TABLE [dbo].[Facturas] CHECK CONSTRAINT [FK_Facturas_Clientes]
GO


-- Insercion de datos 
insert into Clientes (Cuil, Nombre, Direccion, PorcentajeIVA) values
('20345678901','Juan Perez','Calle 123, Ciudad',21);

insert into Clientes (Cuil, Nombre, Direccion, PorcentajeIVA) values
('30567890123','María Gómez','Avenida 456, Ciudad',21);

insert into Clientes (Cuil, Nombre, Direccion, PorcentajeIVA) values
('90876543210','Luis Rodríguez','Plaza 789, Ciudad',21);


insert into Articulos (Codigo, Descripcion, Precio) values
('AR001','Producto 1', 30);

insert into Articulos (Codigo, Descripcion, Precio) values
('AR002','Producto 2', 20);

insert into Articulos (Codigo, Descripcion, Precio) values
('AR003','Producto 3', 15);

insert into Articulos (Codigo, Descripcion, Precio) values
('AR020','Producto 20', 25);

GO

-- Stored Procedures
CREATE or ALTER PROCEDURE SP_Traer3ArticulosMasVendidos
	 
AS
BEGIN
select top(3) max(ar.Codigo) as Codigo, max(ar.Descripcion) as Descripcion, sum(fr.Cantidad) as Total_Vendido
from FacturaRenglones as fr inner join
Articulos as ar on fr.ArticuloId = ar.Id 
group by fr.ArticuloId order by SUM(fr.Cantidad) desc;

END
GO

CREATE or ALTER PROCEDURE SP_Traer3ClientesQueMasCompraron 

AS
BEGIN

select top(3) max(cl.Cuil) as cuil, max(cl.Nombre) nombre , sum(fc.Totalconimpuestos) as Total_Comprado
  from Facturas as fc inner join Clientes as cl
  on fc.ClienteId=cl.Id
  group by fc.ClienteId
  order by sum(fc.totalconimpuestos) desc 
END
GO

CREATE or ALTER PROCEDURE sp_PromedioComprasporClienteYarticuloMasComprado
	 @cuil varchar(11)
AS
BEGIN
 declare 

 @clientenombre varchar(50) ,
 @promediocompras decimal(18,4),
 @articulomascomprado varchar(50),
 @idCliente int


 select @clientenombre = max(cl.Nombre), @promediocompras = avg(fc.Totalconimpuestos)  , @idCliente=max(cl.id)
  from Facturas as fc inner join Clientes as cl
  on fc.ClienteId=cl.Id
  where cl.Cuil=@cuil ;
 
  select top(1) @articulomascomprado= max(ar.Descripcion)  
  from  Facturas fc inner join FacturaRenglones rg on fc.id = rg.FacturaId 
   inner join Articulos ar on rg.ArticuloId = ar.Id
  where fc.ClienteId= @idCliente 
    group by rg.ArticuloId
	order by sum(rg.cantidad) desc ;


  select @clientenombre as ClienteNombre, @promediocompras as PromedioCompras , @articulomascomprado as ArticuloMasComprado

END
GO

CREATE or ALTER PROCEDURE sp_TotalFacturadoyPromedioImporteFacturasPorFecha
	 @fecha Date
AS
BEGIN
  
 select sum(fc.TotalConImpuestos) as TotalFacturado , avg(fc.TotalConImpuestos) PromedioImportesFactura
 from facturas fc
 where fc.Fecha=@fecha
 
END
GO


create or ALTER  PROCEDURE [dbo].[sp_Top3ClientesMasCompradoresArticulo]
	 @codigo varchar(10)
AS
BEGIN
  
 select top(3) max(cl.nombre) as NombreCliente, sum(rg.cantidad) as CantidadComprada, max(ar.descripcion) as DescripcionArticulo 
   from Facturas fc inner join FacturaRenglones rg on fc.Id = rg.FacturaId 
       inner join Clientes cl on fc.ClienteId =cl.Id
	   inner join Articulos ar on rg.ArticuloId = ar.Id
	   where ar.Codigo=@codigo
	   group by fc.ClienteId
	   order by sum(rg.cantidad) desc

END
go

 
 
create or ALTER   PROCEDURE sp_TotalIvaPorFechas 
	 @fechaDesde Date,
	 @fechaHasta Date
AS
BEGIN
  
 Select sum(fc.TotalIVA) as TotalIva  
 from facturas fc
 where fc.Fecha between  @fechaDesde and  @fechaHasta;
 
END
go
