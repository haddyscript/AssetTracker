-- Migration script to insert initial menu data
-- Run this after creating the menus table

-- Insert parent menus first (those with parent_id = NULL)
INSERT INTO [dbo].[menus] ([menu_name], [route], [icon], [parent_id], [sort_order], [is_active], [created_at], [updated_at])
VALUES
('Asset Management', '/assets', 'fas fa-boxes', NULL, 1, 1, GETDATE(), GETDATE()),
('Request Management', '/asset-requests', 'fas fa-clipboard-list', NULL, 2, 1, GETDATE(), GETDATE()),
('User Management', '/users', 'fas fa-users', NULL, 3, 1, GETDATE(), GETDATE()),
('System Administration', '/admin', 'fas fa-cog', NULL, 4, 1, GETDATE(), GETDATE());

-- Get the IDs of the parent menus for child menu references
DECLARE @AssetManagementId INT = (SELECT id FROM [dbo].[menus] WHERE menu_name = 'Asset Management');
DECLARE @RequestManagementId INT = (SELECT id FROM [dbo].[menus] WHERE menu_name = 'Request Management');
DECLARE @UserManagementId INT = (SELECT id FROM [dbo].[menus] WHERE menu_name = 'User Management');
DECLARE @SystemAdminId INT = (SELECT id FROM [dbo].[menus] WHERE menu_name = 'System Administration');

-- Insert child menus
INSERT INTO [dbo].[menus] ([menu_name], [route], [icon], [parent_id], [sort_order], [is_active], [created_at], [updated_at])
VALUES
-- Asset Management children
('View All Assets', '/view/all/asset', 'fas fa-list', @AssetManagementId, 1, 1, GETDATE(), GETDATE()),
('Create Asset', '/create/asset', 'fas fa-plus', @AssetManagementId, 2, 1, GETDATE(), GETDATE()),
('Update Asset', '/update/asset', 'fas fa-edit', @AssetManagementId, 3, 1, GETDATE(), GETDATE()),
('View Asset Detail', '/view/asset/detail', 'fas fa-eye', @AssetManagementId, 4, 1, GETDATE(), GETDATE()),

-- Request Management children
('All Requests', '/asset-requests', 'fas fa-clipboard-list', @RequestManagementId, 1, 1, GETDATE(), GETDATE()),
('Create Request', '/asset-requests/create', 'fas fa-plus', @RequestManagementId, 2, 1, GETDATE(), GETDATE()),
('My Requests', '/my-asset-requests', 'fas fa-user', @RequestManagementId, 3, 1, GETDATE(), GETDATE()),

-- User Management children
('Register User', '/register/user', 'fas fa-user-plus', @UserManagementId, 1, 1, GETDATE(), GETDATE()),
('Register Admin', '/register/admin', 'fas fa-user-shield', @UserManagementId, 2, 1, GETDATE(), GETDATE()),
('Login', '/login', 'fas fa-sign-in-alt', @UserManagementId, 3, 1, GETDATE(), GETDATE()),
('Logout', '/logout', 'fas fa-sign-out-alt', @UserManagementId, 4, 1, GETDATE(), GETDATE()),

-- System Administration children
('User Profile Permissions', '/user-profile-permissions', 'fas fa-key', @SystemAdminId, 1, 1, GETDATE(), GETDATE()),
('Create Permission', '/user-profile-permissions/create', 'fas fa-plus', @SystemAdminId, 2, 1, GETDATE(), GETDATE()),
('Menu Management', '/menus', 'fas fa-bars', @SystemAdminId, 3, 1, GETDATE(), GETDATE()),
('Create Menu', '/menus/create', 'fas fa-plus', @SystemAdminId, 4, 1, GETDATE(), GETDATE()),
('User Profile Menus', '/user-profile-menus', 'fas fa-link', @SystemAdminId, 5, 1, GETDATE(), GETDATE()),
('Assign Menu', '/user-profile-menus/create', 'fas fa-plus', @SystemAdminId, 6, 1, GETDATE(), GETDATE()),
('Home', '/home', 'fas fa-home', @SystemAdminId, 7, 1, GETDATE(), GETDATE()),
('Privacy', '/privacy', 'fas fa-shield-alt', @SystemAdminId, 8, 1, GETDATE(), GETDATE());

-- Note: Update the user_profile_menus table to assign these menus to appropriate user profiles
-- For example, assign admin menus to 'Admin' and 'Super Admin' profiles
-- This would be done separately based on your user profile setup