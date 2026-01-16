# AssetTracker 🏢

A comprehensive **Asset Management System** built with ASP.NET Core MVC that enables organizations to efficiently track, manage, and monitor their assets with role-based access control.

![AssetTracker Banner](https://img.shields.io/badge/AssetTracker-v1.0.0-blue?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-6.0-purple?style=flat-square&logo=dotnet)
![SQLite](https://img.shields.io/badge/SQLite-3.0-green?style=flat-square&logo=sqlite)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.0-blue?style=flat-square&logo=bootstrap)

## 📋 Table of Contents

- [✨ Features](#-features)
- [🛠️ Technology Stack](#️-technology-stack)
- [🚀 Quick Start](#-quick-start)
- [📖 Usage Guide](#-usage-guide)
- [👥 User Roles](#-user-roles)
- [🎨 Screenshots](#-screenshots)
- [🔧 API Endpoints](#-api-endpoints)
- [🗄️ Database Schema](#️-database-schema)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)

## ✨ Features

### 🔐 **Authentication & Authorization**
- **Dual Authentication**: Separate login for Users and Admins
- **Password Security**: BCrypt hashing with interactive visibility toggle
- **Session Management**: Cookie-based authentication with 30-minute sessions
- **Role-Based Access**: Different permissions for Users vs Admins

### 📊 **Dashboard Analytics**
- **Admin Dashboard**: System-wide asset statistics and management overview
- **User Dashboard**: Personal asset assignments and quick actions
- **Real-time Metrics**: Live counts of assets, categories, and quantities
- **Interactive Charts**: Visual representation of asset distributions

### 🏷️ **Asset Management**
- **CRUD Operations**: Create, Read, Update, Delete assets
- **Asset Categories**: Organized asset classification
- **Assignment Tracking**: Track which assets are assigned to which users
- **Quantity Management**: Monitor asset quantities and availability

### 🎨 **Modern UI/UX**
- **Responsive Design**: Works seamlessly on desktop, tablet, and mobile
- **Interactive Navigation**: Role-based menus with smooth animations
- **Toast Notifications**: Real-time feedback for user actions
- **Password Visibility Toggle**: Eye icon to show/hide passwords
- **Loading States**: Visual feedback during form submissions

### 🔧 **Technical Features**
- **OOP Architecture**: Clean, maintainable code with proper separation of concerns
- **Entity Framework**: ORM for database operations
- **Bootstrap 5**: Modern, responsive UI components
- **Font Awesome**: Comprehensive icon library
- **jQuery**: Enhanced interactivity and animations

## 🛠️ Technology Stack

### Backend
- **Framework**: ASP.NET Core 6.0 MVC
- **Language**: C# 10.0
- **Database**: SQLite (with Entity Framework Core)
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

## 🚀 Quick Start

### Prerequisites
- **.NET 6.0 SDK** or later
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
   # The database will be created automatically on first run
   # Or run migrations if needed
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

6. **Open your browser**
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
4. **Create User accounts** and assign assets

### Daily Operations

#### For Administrators:
1. **Login** to access the admin dashboard
2. **View system statistics** (total assets, categories, recent additions)
3. **Manage assets** through the Assets section
4. **Create new user accounts** as needed
5. **Monitor asset assignments** and usage

#### For Users:
1. **Login** to access your personal dashboard
2. **View your assigned assets** and their details
3. **Check asset quantities** and categories
4. **Access asset information** for assigned items

## 👥 User Roles

### 👑 **Administrator**
- **Full System Access**: Complete control over all assets and users
- **User Management**: Create and manage user accounts
- **Asset Oversight**: Add, edit, delete, and assign all assets
- **System Analytics**: View comprehensive statistics and reports
- **Dashboard**: Administrative overview with system-wide metrics

### 👤 **User**
- **Personal Access**: View only their assigned assets
- **Asset Information**: Read-only access to assigned asset details
- **Dashboard**: Personal overview of assigned assets and quantities
- **Limited Navigation**: Focused on personal asset management

## 🎨 Screenshots

### Login Page
Modern login interface with password visibility toggle and role-based authentication.

### Admin Dashboard
Comprehensive dashboard showing system statistics, asset categories, and quick management actions.

### User Dashboard
Personal dashboard displaying assigned assets with clean, focused interface.

### Asset Management
Full CRUD interface for managing assets with categories and assignments.

## 🔧 API Endpoints

### Authentication
- `GET /Users/Login` - User login page
- `POST /Users/Login` - Process user login
- `GET /Admin/Register` - Admin registration page
- `POST /Admin/Register` - Process admin registration
- `POST /Users/Logout` - Logout user

### Assets (Admin Only)
- `GET /Asset/Index` - List all assets
- `GET /Asset/Add` - Add new asset form
- `POST /Asset/Add` - Create new asset
- `GET /Asset/Edit/{id}` - Edit asset form
- `POST /Asset/Edit/{id}` - Update asset
- `POST /Asset/Delete/{id}` - Delete asset

### Dashboard
- `GET /Home/Index` - Role-based dashboard (requires authentication)

## 🗄️ Database Schema

### Tables

#### `Users`
- `id` (Primary Key)
- `username` (Unique)
- `full_name`
- `email` (Unique)
- `password` (Hashed)
- `user_profile_id` (Foreign Key)
- `created_at`, `updated_at`

#### `Admins`
- `id` (Primary Key)
- `username` (Unique)
- `full_name`
- `email` (Unique)
- `password_hash` (Hashed)
- `is_active`
- `created_at`, `updated_at`

#### `Assets`
- `id` (Primary Key)
- `assetName`
- `Quantity`
- `AssignedTo`
- `Category`

#### `UserProfiles`
- `id` (Primary Key)
- `profile_name`

## 🔧 Configuration

### Database Connection
The application uses SQLite by default. To change the database:

1. Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_Connection_String_Here"
  }
}
```

2. Update `Program.cs` with your preferred database provider.

### Authentication Settings
Modify authentication settings in `Program.cs`:

```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Users/Login";
    });
```

## 🧪 Testing

### Running Tests
```bash
dotnet test
```

### Manual Testing Checklist
- [ ] Admin registration works
- [ ] User login authentication
- [ ] Role-based dashboard access
- [ ] Asset CRUD operations
- [ ] Password visibility toggle
- [ ] Responsive design on mobile
- [ ] Session timeout after 30 minutes

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to the branch (`git push origin feature/AmazingFeature`)
5. **Open** a Pull Request

### Development Guidelines
- Follow **C# coding standards**
- Use **meaningful commit messages**
- Add **XML documentation** to public methods
- Write **unit tests** for new features
- Ensure **responsive design** for all new UI components

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **ASP.NET Core Team** for the excellent framework
- **Bootstrap Team** for the responsive UI framework
- **Font Awesome** for the comprehensive icon library
- **Entity Framework** for the powerful ORM

## 📞 Support

If you encounter any issues or have questions:

1. Check the [Issues](https://github.com/yourusername/AssetTracker/issues) page
2. Create a new issue with detailed description
3. Contact the maintainers

---

**Built with ❤️ using ASP.NET Core MVC**

*Last updated: January 16, 2026*</content>
<parameter name="filePath">/Users/fdchadrian-nc-web/Desktop/AssetTracker/README.md