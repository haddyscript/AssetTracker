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


DROP TABLE IF EXISTS [dbo].[assets];
CREATE TABLE [dbo].[assets] (
	[id] int NOT NULL IDENTITY PRIMARY KEY,
	[asset_tag] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[asset_name] nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[category] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[brand] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[model] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[serial_number] nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[purchase_date] date NOT NULL,
	[purchase_price] decimal(18,2) NOT NULL,
	[status] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT 'Available',
	[condition] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[assigned_to_user_id] int NULL,
	[assigned_date] datetime NULL,
	[created_at] datetime NOT NULL DEFAULT '(getdate())',
	[updated_at] datetime NOT NULL DEFAULT '(getdate())',
	CONSTRAINT [UQ__assets__BED14FEEB3AC3B50] UNIQUE ([serial_number]),
	CONSTRAINT [UQ__assets__1FACF043F8E3B474] UNIQUE ([asset_tag])
);


ALTER TABLE [dbo].[assets] ADD
	FOREIGN KEY ([assigned_to_user_id]) REFERENCES [users] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

DROP TABLE IF EXISTS [dbo].[asset_requests];
CREATE TABLE [dbo].[asset_requests] (
	[id] int NOT NULL IDENTITY PRIMARY KEY,
	[user_id] int NOT NULL,
	[asset_id] int NOT NULL,
	[request_type] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT 'Pending',
	[requested_at] datetime NOT NULL DEFAULT '(getdate())',
	[approved_at] datetime NULL,
	[approved_by_admin_id] int NULL,
	[remarks] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
);


ALTER TABLE [dbo].[asset_requests] ADD
	FOREIGN KEY ([approved_by_admin_id]) REFERENCES [admins] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	FOREIGN KEY ([asset_id]) REFERENCES [assets] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	FOREIGN KEY ([user_id]) REFERENCES [users] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


-- 2026-01-16 07:18:10 UTC