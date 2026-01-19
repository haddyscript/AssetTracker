-- Adminer 5.4.1 MS SQL  dump

DROP TABLE IF EXISTS [dbo].[menus];
CREATE TABLE [dbo].[menus] (
	[id] int NOT NULL IDENTITY PRIMARY KEY,
	[menu_name] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[route] varchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[icon] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[parent_id] int NULL,
	[sort_order] int NOT NULL DEFAULT '((0))',
	[is_active] bit NOT NULL DEFAULT '((1))',
	[created_at] datetime NOT NULL DEFAULT '(getdate())',
	[updated_at] datetime NOT NULL DEFAULT '(getdate())'
);

SET IDENTITY_INSERT [dbo].[menus] ON;
INSERT INTO [dbo].[menus] ([id], [menu_name], [route], [icon], [parent_id], [sort_order], [is_active], [created_at], [updated_at]) VALUES
(1,	'Asset Management',	'/view/all/asset',	'fas fa-boxes',	NULL,	1,	'1',	'2026-01-19 05:05:19',	'2026-01-19 05:05:19'),
(2,	'Request Management',	'/asset-requests',	'fas fa-clipboard-list',	NULL,	2,	'1',	'2026-01-19 05:05:19',	'2026-01-19 05:05:19'),
(3,	'User Management',	'/users',	'fas fa-users',	NULL,	3,	'1',	'2026-01-19 05:05:19',	'2026-01-19 05:05:19'),
(4,	'System Administration',	'/admin',	'fas fa-cog',	NULL,	4,	'1',	'2026-01-19 05:05:19',	'2026-01-19 05:05:19'),
(5,	'View Asset',	'/view/all/asset',	'fas fa-eye',	1,	1,	'1',	'2026-01-19 15:20:36',	'2026-01-19 15:20:36'),
(6,	'View Request',	'/asset-requests',	'fas fa-eye',	2,	1,	'1',	'2026-01-19 15:30:04',	'2026-01-19 15:30:04'),
(7,	'Menu Management',	'/menus',	'fas fa-bars',	NULL,	5,	'1',	'2026-01-19 15:43:39',	'2026-01-19 15:43:39'),
(8,	'User Profile Permissions',	'/user-profile-permissions',	'fas fa-user-shield',	3,	1,	'1',	'2026-01-19 16:09:10',	'2026-01-19 16:09:10'),
(9,	'User Profile Menu',	'/user-profile-menus',	'fas fa-user-circle',	3,	2,	'1',	'2026-01-19 15:49:16',	'2026-01-19 15:49:16'),
(10,	'All Menu',	'/menus',	'fas fa-eye',	7,	1,	'1',	'2026-01-19 15:52:23',	'2026-01-19 15:52:23');
SET IDENTITY_INSERT [dbo].[menus] OFF;

-- 2026-01-19 08:16:16 UTC