# AssetTracker 🏢

A comprehensive **Asset Management System** built with ASP.NET Core MVC that enables organizations to efficiently track, manage, and monitor their assets with advanced role-based access control, dynamic menu systems, and asset request workflows.

![AssetTracker Banner](https://img.shields.io/badge/AssetTracker-v2.0.0-blue?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-6.0-purple?style=flat-square&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-2019-blue?style=flat-square&logo=microsoft-sql-server)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.0-blue?style=flat-square&logo=bootstrap)

## 📋 Table of Contents

- [✨ Features](#-features)
- [🛠️ Technology Stack](#️-technology-stack)
- [🚀 Quick Start](#-quick-start)
- [📖 Usage Guide](#-usage-guide)
- [👥 User Roles & Profiles](#-user-roles--profiles)
- [🎨 Screenshots](#-screenshots)
- [🔧 API Endpoints](#-api-endpoints)
- [🗄️ Database Schema](#️-database-schema)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)

## ✨ Features

### 🔐 **Advanced Authentication & Authorization**
- **Dual Authentication**: Separate login for Users and Admins with profile-based access
- **Password Security**: BCrypt hashing with interactive visibility toggle
- **Session Management**: Cookie-based authentication with configurable timeouts
- **Role-Based Access**: Granular permissions through user profiles and menu assignments

### 📊 **Dynamic Dashboard & Analytics**
- **Role-Based Dashboards**: Different interfaces for Users vs Admins
- **Real-time Metrics**: Live counts of assets, requests, and system statistics
- **Interactive Navigation**: Dynamic menu system based on user permissions
- **Profile Management**: User profile assignment and permission management

### 🏷️ **Comprehensive Asset Management**
- **Full CRUD Operations**: Create, Read, Update, Delete assets with detailed tracking
- **Asset Details**: Rich asset information including brand, model, serial numbers, purchase details
- **Assignment Tracking**: Track asset assignments to users with dates and status
- **Asset Requests**: Borrow/return request system with approval workflows

### 🎯 **Asset Request System**
- **Request Types**: Support for borrow and return requests
- **Approval Workflow**: Admin approval process for asset requests
- **Request Tracking**: Complete history of requests with status updates
- **User Self-Service**: Users can view and manage their own requests

### 📋 **Dynamic Menu Management**
- **Hierarchical Menus**: Parent-child menu structure with icons and routing
- **Profile-Based Access**: Assign menus to user profiles with view permissions
- **Menu Permissions**: Control menu visibility based on user roles
- **Admin Configuration**: Full menu management interface for administrators

### 👥 **User Profile & Permission System**
- **User Profiles**: Flexible profile system for different user types
- **Module Permissions**: Granular permissions (View, Create, Edit, Delete) per module
- **Menu Assignments**: Assign specific menus to user profiles
- **Admin Override**: Administrative access control and management

### 🎨 **Modern UI/UX**
- **Responsive Design**: Works seamlessly on desktop, tablet, and mobile
- **Dynamic Navigation**: Menu system that adapts to user permissions
- **Toast Notifications**: Real-time feedback for user actions
- **Password Visibility Toggle**: Eye icon to show/hide passwords
- **Loading States**: Visual feedback during form submissions

### 🔧 **Technical Features**
- **OOP Architecture**: Clean, maintainable code with proper separation of concerns
- **Entity Framework Core**: ORM for database operations with SQL Server
- **Bootstrap 5**: Modern, responsive UI components
- **Font Awesome**: Comprehensive icon library
- **jQuery**: Enhanced interactivity and animations
- **View Components**: Reusable UI components for dynamic content

## 🛠️ Technology Stack

### Backend
- **Framework**: ASP.NET Core 6.0 MVC
- **Language**: C# 10.0
- **Database**: SQL Server (with Entity Framework Core)
- **Authentication**: ASP.NET Core Identity with Cookie Authentication

### Frontend
- **HTML5/CSS3**: Semantic markup and modern styling
- **Bootstrap 5**: Responsive grid system and components
- **JavaScript/jQuery**: Interactive functionality
- **Font Awesome 6**: Icon library

### Development Tools
- **IDE**: Visual Studio 2022 / VS Code
- **Version Control**: Git
- **Package Manager**: NuGet
- **Build Tool**: MSBuild
- **Database**: SQL Server Management Studio

## 🚀 Quick Start

### Prerequisites
- **.NET 6.0 SDK** or later
- **SQL Server** (2019 or later)
- **Git** (optional, for cloning)
- **Web browser** (Chrome, Firefox, Safari, Edge)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/AssetTracker.git
   cd AssetTracker
   ```

2. **Navigate to the app directory**
   ```bash
   cd app
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Set up the database**
   ```bash
   # Run the database schema script
   # Execute database_table.sql and menu_migration.sql in SQL Server
   ```

5. **Update connection string**
   ```bash
   # Update appsettings.json with your SQL Server connection string
   ```

6. **Run the application**
   ```bash
   dotnet run
   ```

7. **Open your browser**
   ```
   Navigate to: https://localhost:5001
   ```

### 🐳 Docker Setup (Alternative)

If you prefer using Docker:

```bash
# Build and run with Docker Compose
docker-compose up --build
```

## 📖 Usage Guide

### First-Time Setup

1. **Access the application** at `https://localhost:5001`
2. **Register an Admin account** at `/Admin/Register`
3. **Login** with your admin credentials at `/Users/Login`
4. **Create User Profiles** and assign permissions
5. **Set up Menus** and assign them to profiles
6. **Create User accounts** and assign profiles

### Daily Operations

#### For Administrators:
1. **Login** to access the admin dashboard
2. **Manage Assets** through the Asset Management section
3. **Review Asset Requests** and approve/reject them
4. **Configure User Profiles** and their permissions
5. **Manage Menus** and menu assignments
6. **Create and manage user accounts**

#### For Users:
1. **Login** to access your personal dashboard
2. **View your assigned assets** and their details
3. **Submit asset requests** for borrowing or returning assets
4. **Track your request status** through My Requests
5. **Access only permitted menus** based on your profile

## 👥 User Roles & Profiles

### 👑 **Administrator**
- **Full System Access**: Complete control over all assets, users, and system configuration
- **User Management**: Create and manage user accounts and profiles
- **Asset Oversight**: Add, edit, delete, and assign all assets
- **Request Management**: Approve/reject asset requests
- **System Configuration**: Manage menus, permissions, and user profiles
- **Dashboard**: Administrative overview with system-wide metrics

### 👤 **User Profiles (Configurable)**
- **Profile-Based Access**: Permissions defined by assigned user profile
- **Asset Requests**: Submit borrow/return requests for assets
- **Personal Dashboard**: View assigned assets and request history
- **Limited Navigation**: Access only to permitted menus and features
- **Self-Service**: Manage personal asset requests and view assignments

### 🔧 **Permission System**
- **Module Permissions**: View, Create, Edit, Delete permissions per module
- **Menu Access**: Control which menus are visible to each profile
- **Granular Control**: Fine-tuned access control for different user types

## 🎨 Screenshots

### Login Page
Modern login interface with password visibility toggle and role-based authentication.

### Admin Dashboard
Comprehensive dashboard showing system statistics, asset categories, and quick management actions.

### User Dashboard
Personal dashboard displaying assigned assets with clean, focused interface.

### Asset Management
Full CRUD interface for managing assets with categories and assignments.

### Asset Request System
Complete request workflow for borrowing and returning assets with approval process.

### Menu Management
Hierarchical menu system with profile-based assignments and permissions.

## 🔧 API Endpoints

### Authentication
- `GET /Users/Login` - User login page
- `POST /Users/Login` - Process user login
- `GET /Admin/Register` - Admin registration page
- `POST /Admin/Register` - Process admin registration
- `POST /Users/Logout` - Logout user

### Assets (Admin Only)
- `GET /Asset/Index` - List all assets
- `GET /Asset/Create` - Add new asset form
- `POST /Asset/Create` - Create new asset
- `GET /Asset/Edit/{id}` - Edit asset form
- `POST /Asset/Edit/{id}` - Update asset
- `GET /Asset/Details/{id}` - View asset details
- `POST /Asset/Delete/{id}` - Delete asset

### Asset Requests
- `GET /AssetRequests/Index` - List all requests (Admin)
- `GET /AssetRequests/MyRequests` - User's requests (User)
- `GET /AssetRequests/CreateBorrowRequest` - Create borrow request form
- `POST /AssetRequests/CreateBorrowRequest` - Submit borrow request
- `GET /AssetRequests/CreateReturnRequest` - Create return request form
- `POST /AssetRequests/CreateReturnRequest` - Submit return request
- `GET /AssetRequests/Details/{id}` - View request details

### User Profile Permissions (Admin Only)
- `GET /UserProfilePermissions/Index` - List all permissions
- `GET /UserProfilePermissions/Create` - Create permission form
- `POST /UserProfilePermissions/Create` - Create new permission
- `GET /UserProfilePermissions/Edit/{id}` - Edit permission form
- `POST /UserProfilePermissions/Edit/{id}` - Update permission
- `GET /UserProfilePermissions/Details/{id}` - View permission details
- `POST /UserProfilePermissions/Delete/{id}` - Delete permission

### Menu Management (Admin Only)
- `GET /Menus/Index` - List all menus
- `GET /Menus/Create` - Create menu form
- `POST /Menus/Create` - Create new menu
- `GET /Menus/Edit/{id}` - Edit menu form
- `POST /Menus/Edit/{id}` - Update menu
- `GET /Menus/Details/{id}` - View menu details
- `POST /Menus/Delete/{id}` - Delete menu

### User Profile Menus (Admin Only)
- `GET /UserProfileMenus/Index` - List menu assignments
- `GET /UserProfileMenus/Create` - Assign menu to profile form
- `POST /UserProfileMenus/Create` - Create menu assignment
- `GET /UserProfileMenus/Edit/{id}` - Edit menu assignment form
- `POST /UserProfileMenus/Edit/{id}` - Update menu assignment
- `POST /UserProfileMenus/Delete/{id}` - Delete menu assignment

### Dashboard
- `GET /Home/Index` - Role-based dashboard (requires authentication)
- `GET /Home/AccessDenied` - Access denied page
- `GET /Home/Privacy` - Privacy policy page

## 🗄️ Database Schema

### Core Tables

#### `user_profile`
- `id` (Primary Key)
- `profile_name` (e.g., "Admin", "User", "Manager")
- `status`, `created_at`, `updated_at`

#### `users`
- `id` (Primary Key)
- `username` (Unique)
- `full_name`
- `email`
- `password` (Hashed)
- `user_profile` (Foreign Key)
- `created_at`, `updated_at`

#### `admins`
- `id` (Primary Key)
- `username` (Unique)
- `full_name`
- `email`
- `password_hash` (Hashed)
- `is_active`
- `user_profile` (Foreign Key)
- `created_at`, `updated_at`

#### `assets`
- `id` (Primary Key)
- `asset_tag` (Unique)
- `asset_name`
- `description`
- `category`, `brand`, `model`
- `serial_number` (Unique)
- `purchase_date`, `purchase_price`
- `status` (Available, Assigned, etc.)
- `condition`
- `assigned_to_user_id` (Foreign Key)
- `assigned_date`
- `created_at`, `updated_at`

#### `asset_requests`
- `id` (Primary Key)
- `user_id` (Foreign Key)
- `asset_id` (Foreign Key)
- `request_type` (Borrow/Return)
- `status` (Pending, Approved, Rejected, Completed)
- `requested_at`, `approved_at`
- `approved_by_admin_id` (Foreign Key)
- `remarks`, `returned_at`

### Permission & Menu System

#### `user_profile_permissions`
- `id` (Primary Key)
- `user_profile_id` (Foreign Key)
- `module_name` (Asset, User, Request, etc.)
- `can_view`, `can_create`, `can_edit`, `can_delete`
- `status`, `created_at`, `updated_at`

#### `menus`
- `id` (Primary Key)
- `menu_name`, `route`, `icon`
- `parent_id` (Self-referencing for hierarchy)
- `sort_order`, `is_active`
- `created_at`, `updated_at`

#### `user_profile_menus`
- `id` (Primary Key)
- `user_profile_id` (Foreign Key)
- `menu_id` (Foreign Key)
- `can_view`, `status`
- `created_at`, `updated_at`

## 🔧 Configuration

### Database Connection
Update `appsettings.json` with your SQL Server connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=AssetTracker;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Authentication Settings
Modify authentication settings in `Program.cs`:

```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Users/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });
```

## 🧪 Testing

### Running Tests
```bash
dotnet test
```

### Manual Testing Checklist
- [ ] Admin registration works
- [ ] User login authentication with profile-based access
- [ ] Dynamic menu rendering based on permissions
- [ ] Asset CRUD operations with proper authorization
- [ ] Asset request submission and approval workflow
- [ ] User profile permission management
- [ ] Menu assignment and visibility control
- [ ] Responsive design on mobile devices
- [ ] Password visibility toggle functionality
- [ ] Session timeout after configured period

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to the branch (`git push origin feature/AmazingFeature`)
5. **Open** a Pull Request

### Development Guidelines
- Follow **C# coding standards** and **ASP.NET Core conventions**
- Use **meaningful commit messages**
- Add **XML documentation** to public methods
- Write **unit tests** for new features
- Ensure **responsive design** for all new UI components
- Test **authorization** and **permission checks** thoroughly
- Follow **Entity Framework Core best practices**

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **ASP.NET Core Team** for the excellent framework
- **Bootstrap Team** for the responsive UI framework
- **Font Awesome** for the comprehensive icon library
- **Entity Framework Core** for the powerful ORM
- **SQL Server** for the robust database platform

---

**Built with ❤️ using ASP.NET Core MVC**

*Last updated: January 19, 2026*</content>