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