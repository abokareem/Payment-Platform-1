https://github.com/serilog/serilog/wiki/Configuration-Basics
https://github.com/serilog/serilog-settings-configuration/issues/116
https://www.c-sharpcorner.com/article/file-logging-and-ms-sql-logging-using-serilog-with-asp-net-core-2-0/
https://github.com/serilog/serilog-sinks-mssqlserver

CREATE TABLE [Logs] (

   [Id] int IDENTITY(1,1) NOT NULL,
   [Message] nvarchar(max) NULL,
   [MessageTemplate] nvarchar(max) NULL,
   [Level] nvarchar(128) NULL,
   [TimeStamp] datetime NOT NULL,
   [Exception] nvarchar(max) NULL,
   [Properties] nvarchar(max) NULL

   CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([Id] ASC) 
);
