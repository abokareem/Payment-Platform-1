#Полезные ссылки и информация о Serilog

https://github.com/serilog/serilog/wiki/Configuration-Basics
https://github.com/serilog/serilog-settings-configuration/issues/116
https://www.c-sharpcorner.com/article/file-logging-and-ms-sql-logging-using-serilog-with-asp-net-core-2-0/
https://github.com/serilog/serilog-sinks-mssqlserver

CREATE TABLE [Serilog] (

   [Id] int IDENTITY(1,1) NOT NULL,
   [Message] nvarchar(max) NULL,
   [Level] nvarchar(128) NULL,
   [TimeStamp] datetime NOT NULL,
   [Exception] nvarchar(max) NULL,
   [Properties] nvarchar(max) NULL

   CONSTRAINT [PK_Serilog] PRIMARY KEY CLUSTERED ([Id] ASC) 
);

"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PaymentPlatformApplication;Trusted_Connection=True;MultipleActiveResultSets=true" - Use for Windows

https://docs.docker.com/engine/examples/dotnetcore/
https://docs.microsoft.com/en-us/dotnet/core/docker/build-container
https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-setup-tools?view=sql-server-ver15
https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-bash
https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-configure-docker?view=sql-server-ver15

docker-compose up -d

docker exec -it <container> /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Your_password123

USE [PaymentPlatformApplication]
GO

SELECT * FROM [dbo].[Account]
GO

UPDATE [dbo].[Account] SET Role = 2 WHERE Email = 'a2e77161-dcf3-4913-8df3-4ee9c56291d3@outlook.com'
GO
