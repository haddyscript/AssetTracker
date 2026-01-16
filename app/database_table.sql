-- Adminer 5.4.1 MS SQL  dump

DROP TABLE IF EXISTS [dbo].[admins];
CREATE TABLE [dbo].[admins] (
	[id] int NOT NULL IDENTITY PRIMARY KEY,
	[username] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[email] nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[password_hash] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[full_name] nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[is_active] bit NOT NULL DEFAULT '((1))',
	[created_at] datetime2 NOT NULL DEFAULT '(getdate())',
	[updated_at] datetime2 NOT NULL DEFAULT '(getdate())',
	CONSTRAINT [UQ__admins__AB6E61641A87BE1F] UNIQUE ([email]),
	CONSTRAINT [UQ__admins__F3DBC5723C93FEB7] UNIQUE ([username])
);


DROP TABLE IF EXISTS [dbo].[user_profile];
CREATE TABLE [dbo].[user_profile] (
	[id] int NOT NULL IDENTITY PRIMARY KEY,
	[profile_name] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[status] int NOT NULL,
	[created_at] datetime NOT NULL,
	[updated_at] datetime NOT NULL
);


DROP TABLE IF EXISTS [dbo].[users];
CREATE TABLE [dbo].[users] (
	[id] int NOT NULL IDENTITY PRIMARY KEY,
	[username] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[full_name] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[created_at] datetime NOT NULL DEFAULT '(getdate())',
	[updated_at] datetime NOT NULL DEFAULT '(getdate())',
	[password] varchar(300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[email] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[user_profile] int NULL
);


ALTER TABLE [dbo].[users] ADD
	FOREIGN KEY ([user_profile]) REFERENCES [user_profile] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- 2026-01-16 07:18:10 UTC