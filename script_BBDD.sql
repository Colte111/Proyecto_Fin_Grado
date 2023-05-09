USE [CARMAPS3]
GO
/****** Object:  Table [dbo].[ALARMA]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ALARMA](
	[ALARMAid] [int] IDENTITY(1,1) NOT NULL,
	[USUARIOid] [int] NULL,
	[Nombre] [varchar](50) NULL,
	[Fecha] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ALARMAid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AUTOMOVIL]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AUTOMOVIL](
	[AUTOMOVILid] [int] IDENTITY(1,1) NOT NULL,
	[Marca] [varchar](25) NULL,
	[Matricula] [varchar](12) NULL,
	[Propietario] [varchar](30) NULL,
	[USUARIOid] [int] NULL,
	[Latitud] [varchar](max) NULL,
	[Longitud] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[AUTOMOVILid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UBICACION]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UBICACION](
	[UBICACIONid] [int] IDENTITY(1,1) NOT NULL,
	[AUTOMOVILid] [int] NULL,
	[Nombre] [varchar](50) NULL,
	[URLGoogle] [varchar](900) NULL,
	[Calle] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[UBICACIONid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USUARIO]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIO](
	[USUARIOid] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](20) NULL,
	[Apellidos] [varchar](35) NULL,
	[Correo] [varchar](40) NULL,
	[FechaNac] [date] NULL,
	[Genero] [varchar](10) NULL,
	[contraseña] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[USUARIOid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ALARMA]  WITH CHECK ADD FOREIGN KEY([USUARIOid])
REFERENCES [dbo].[USUARIO] ([USUARIOid])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[AUTOMOVIL]  WITH CHECK ADD FOREIGN KEY([USUARIOid])
REFERENCES [dbo].[USUARIO] ([USUARIOid])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[UBICACION]  WITH CHECK ADD FOREIGN KEY([AUTOMOVILid])
REFERENCES [dbo].[AUTOMOVIL] ([AUTOMOVILid])
ON UPDATE CASCADE
GO
/****** Object:  StoredProcedure [dbo].[CAMBIARUBIGRUPO]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CAMBIARUBIGRUPO] @grupoid int, @lat1 varchar(max),@long varchar(max)
AS
declare @numauto int
select @numauto=AUTOMOVIL.AUTOMOVILid from AUTOMOVIL,GRUPOFAMILIAR
where AUTOMOVIL.AUTOMOVILid=GRUPOFAMILIAR.AUTOMOVILid
and GRUPOFAMILIAR.GRUPOid=@grupoid
update AUTOMOVIL
set lat1=@lat1 , long=@long
where AUTOMOVIL.AUTOMOVILid=@numauto


GO
/****** Object:  StoredProcedure [dbo].[ELIMINARCOCHE]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ELIMINARCOCHE] @idauto int
AS
delete from AUTOMOVIL where AUTOMOVILid=@idauto
GO
/****** Object:  StoredProcedure [dbo].[INSERTAR_ALARMA]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[INSERTAR_ALARMA](@id int,@fecha datetime)
AS
    BEGIN


        INSERT INTO ALARMA (USUARIOid,Fecha)
        VALUES
        (@id,@fecha)
    END
GO
/****** Object:  StoredProcedure [dbo].[INSERTAR_AUTOMOVIL]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[INSERTAR_AUTOMOVIL] @matricula varchar(max),@marca varchar(max),@usuarioid int
as
	declare @propietario varchar(max)
	set @propietario = (
		select u.Nombre
		from USUARIO u
		where u.USUARIOid = @usuarioid
	)
		begin
			insert into AUTOMOVIL (Matricula , Marca , Propietario ,USUARIOid, Latitud ,Longitud)
			values 
			(@matricula,@marca,@propietario,@usuarioid,0,0)
end
GO
/****** Object:  StoredProcedure [dbo].[insertarusu]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[insertarusu] (@nombre varchar(max),@ape varchar(max), @correo varchar(max), @fechanac date, @genero varchar(max),@contra varchar(max))
as
INSERT INTO USUARIO (Nombre,Apellidos,Correo,FechaNac,Genero,Contraseña)
VALUES
(@nombre,@ape,@correo,@fechanac,@genero,@contra)
GO
/****** Object:  StoredProcedure [dbo].[Insertarusugrupo]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[Insertarusugrupo] @correo varchar(max),@grupo int


AS 
DECLARE @id int


select @id=USUARIO.USUARIOid from USUARIO
where USUARIO.Correo=@correo


INSERT INTO USUARIOGRUPO VALUES (@id,@grupo)
GO
/****** Object:  StoredProcedure [dbo].[mostrarIdusuario]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mostrarusucorr] @correo varchar(max), @contra varchar(max)
as
begin
select Correo,contraseña,USUARIO.USUARIOid from USUARIO where correo=@correo AND  contraseña=@contra
end
GO
/****** Object:  StoredProcedure [dbo].[selectusuario]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[selectusuario] 
as
select * from USUARIO
GO
/****** Object:  StoredProcedure [dbo].[VER_AUTOMOVIL]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VER_AUTOMOVIL] @id int as
begin
declare @idusu int=0
select @idusu= AUTOMOVIL.USUARIOid
from AUTOMOVIL
where AUTOMOVIL.USUARIOid=@id
if @id<>0
begin
select * from AUTOMOVIL
where AUTOMOVIL.USUARIOid=@id
end


end
GO
/****** Object:  StoredProcedure [dbo].[VERGRUPO]    Script Date: 24/04/2023 18:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
