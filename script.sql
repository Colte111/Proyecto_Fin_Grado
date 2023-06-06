USE [Carmaps]
GO
/****** Object:  Table [dbo].[ALARMA]    Script Date: 25/05/2023 16:38:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ALARMA](
	[ALARMAid] [int] IDENTITY(1,1) NOT NULL,
	[AUTOMOVILid] [int] NULL,
	[Nombre] [varchar](50) NULL,
	[Fecha] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ALARMAid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AUTOMOVIL]    Script Date: 25/05/2023 16:38:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AUTOMOVIL](
	[AUTOMOVILid] [int] IDENTITY(1,1) NOT NULL,
	[Marca] [varchar](25) NULL,
	[Matricula] [varchar](12) NULL,
	[USUARIOid] [int] NULL,
	[Latitud] [varchar](max) NULL,
	[Longitud] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[AUTOMOVILid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UBICACION]    Script Date: 25/05/2023 16:38:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Table [dbo].[USUARIO]    Script Date: 25/05/2023 16:38:31 ******/
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
ALTER TABLE [dbo].[ALARMA]  WITH CHECK ADD  CONSTRAINT [FK_ALARMA_AUTOMOVIL] FOREIGN KEY([AUTOMOVILid])
REFERENCES [dbo].[AUTOMOVIL] ([AUTOMOVILid])
GO
ALTER TABLE [dbo].[ALARMA] CHECK CONSTRAINT [FK_ALARMA_AUTOMOVIL]
GO
ALTER TABLE [dbo].[AUTOMOVIL]  WITH CHECK ADD FOREIGN KEY([USUARIOid])
REFERENCES [dbo].[USUARIO] ([USUARIOid])
ON UPDATE CASCADE
GO
/****** Object:  StoredProcedure [dbo].[ACTUALIZARALARMA]    Script Date: 25/05/2023 16:38:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ACTUALIZARALARMA] (@ALARMAid int,@fecha datetime)
AS
	BEGIN
		UPDATE ALARMA
		SET Fecha = @fecha
		WHERE ALARMAid = @ALARMAid
	END
GO
CREATE PROCEDURE [dbo].[CAMBIARUBIGRUPO] @grupoid int, @lat1 varchar(max),@long varchar(max)
AS
declare @numauto int
select @numauto=AUTOMOVIL.AUTOMOVILid from AUTOMOVIL,GRUPOFAMILIAR
where AUTOMOVIL.AUTOMOVILid=GRUPOFAMILIAR.AUTOMOVILid
and GRUPOFAMILIAR.GRUPOid=@grupoid
update AUTOMOVIL
set Latitud=@lat1 , Longitud=@long
where AUTOMOVIL.AUTOMOVILid=@numauto


GO
/****** Object:  StoredProcedure [dbo].[ELIMINARALARMA]    Script Date: 25/05/2023 16:38:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ELIMINARALARMA] (@ALARMAid int)
AS
	BEGIN
		DELETE FROM ALARMA
		WHERE ALARMAid = @ALARMAid
	END
GO
/****** Object:  StoredProcedure [dbo].[ELIMINARCOCHE]    Script Date: 25/05/2023 16:38:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ELIMINARCOCHE] @idauto int
AS
delete from ALARMA where AUTOMOVILid=@idauto
delete from AUTOMOVIL where AUTOMOVILid=@idauto
GO
/****** Object:  StoredProcedure [dbo].[INSERTAR_ALARMA]    Script Date: 25/05/2023 16:38:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[INSERTAR_ALARMA](@AUTOMOVILid int,@fecha datetime)
AS
    BEGIN
        INSERT INTO ALARMA (AUTOMOVILid,Fecha )
        VALUES
        (@AUTOMOVILid,@fecha)
    END
GO
/****** Object:  StoredProcedure [dbo].[INSERTAR_AUTOMOVIL]    Script Date: 25/05/2023 16:38:31 ******/
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
			insert into AUTOMOVIL (Matricula , Marca ,USUARIOid, Latitud ,Longitud)
			values 
			(@matricula,@marca,@usuarioid,0,0)
end
GO
/****** Object:  StoredProcedure [dbo].[insertarusu]    Script Date: 25/05/2023 16:38:31 ******/
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
/****** Object:  StoredProcedure [dbo].[Insertarusugrupo]    Script Date: 25/05/2023 16:38:31 ******/
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
/****** Object:  StoredProcedure [dbo].[mostrarusucorr]    Script Date: 25/05/2023 16:38:31 ******/
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
/****** Object:  StoredProcedure [dbo].[selectusuario]    Script Date: 25/05/2023 16:38:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[selectusuario] 
as
select * from USUARIO
GO
/****** Object:  StoredProcedure [dbo].[VER_AUTOMOVIL]    Script Date: 25/05/2023 16:38:31 ******/
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
