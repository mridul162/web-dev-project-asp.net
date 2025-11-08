# README.md

## Project Overview

This is an ASP.NET Web Forms application named "Furqan" - a food service/restaurant web application built with .NET Framework 4.8. The application features user registration, authentication, profile management, and a product catalog system.

## Build and Development Commands

### Building the Project

```powershell
# Build the solution (Debug configuration)
msbuild Furqan.sln /p:Configuration=Debug

# Build the solution (Release configuration)
msbuild Furqan.sln /p:Configuration=Release

# Restore NuGet packages
nuget restore Furqan.sln
```

### Running the Application

The application is configured to run on IIS Express:
- **Default URL**: `http://localhost:4542/`
- **Development Server Port**: 4542

To run in Visual Studio:
```powershell
# Open the solution in Visual Studio
& "Furqan.sln"
```

The application uses IIS Express by default. Press F5 in Visual Studio to start debugging.

### Database Setup

The application requires SQL Server (SQLEXPRESS) with a database named "Furqan":

**Connection String** (in Web.config):
```
Data Source=.\SQLEXPRESS; Initial Catalog=Furqan; Integrated Security=True; MultipleActiveResultSets=true;
```

The application uses stored procedures for database operations:
- `User_Crud` - Handles user CRUD operations with actions: INSERT, UPDATE, SELECT4LOGIN, SELECT4PROFILE

## Architecture Overview

### Application Structure

This is a classic ASP.NET Web Forms application with the following architecture:

**Three-Layer Structure:**
1. **Presentation Layer**: ASPX pages and Master Pages in the `/User` folder
2. **Business Logic**: Code-behind files (.aspx.cs) handling page logic
3. **Data Access**: Direct ADO.NET calls using SqlConnection, SqlCommand, and SqlDataAdapter

### Key Architectural Components

**Master Page System:**
- `User/user.Master` - Main layout template containing header, navigation, footer
- Dynamically loads `SliderUserControl.ascx` on the homepage
- Session-based authentication state management (Login/Logout toggle)

**Session Management:**
- `Session["userID"]` - Stores logged-in user ID
- `Session["username"]` - Stores username
- `Session["admin"]` - Flags admin users
- Admin credentials hardcoded: username "Admin", password "123"

**Utility Classes (Connections.cs):**
- `Connections.GetConnectionString()` - Centralized connection string retrieval
- `Utils.IsValidExtension()` - Validates image file extensions (.jpg, .png, .jpeg)
- `Utils.GetImageUrl()` - Returns image path or default placeholder

**User Pages:**
- `default.aspx` - Homepage with offers and about sections
- `Login.aspx` - User/admin authentication
- `Reg.aspx` - User registration and profile editing
- `profile.aspx` - View user profile details
- `menu.aspx` - Product categories page
- `about.aspx` - About page
- `contact.aspx` - Contact page

### Data Flow Pattern

All database operations follow this pattern:
1. Create SqlConnection using `Connections.GetConnectionString()`
2. Create SqlCommand with stored procedure name
3. Add parameters with `@Action` to specify operation type
4. Execute using SqlDataAdapter to fill DataTable
5. Bind data to controls or process results
6. Always close connection in finally block

### Image Upload System

User profile images are:
- Validated for .jpg, .jpeg, .png extensions
- Saved to `/Images/User/` with GUID-based filenames
- Database stores relative path (e.g., "Images/User/guid.jpg")
- Falls back to "/Images/No_image.png" if no image exists

### Authentication Flow

**User Authentication:**
1. Check if username is "Admin" with password "123" → redirect to admin dashboard
2. Otherwise, call stored procedure `User_Crud` with action "SELECT4LOGIN"
3. On success, store `Session["userID"]` and `Session["username"]`
4. Redirect to default.aspx

**Session Checks:**
- Most pages check `Session["userID"]` on Page_Load
- Redirect to login if null (user not authenticated)
- Master page toggles Login/Logout button based on session

## Important Configuration Details

### Web.config Settings

**AppSettings:**
- `username`: Admin (admin username)
- `password`: 123 (admin password)
- `ValidationSettings:UnobtrusiveValidationMode`: None

**Compilation:**
- Debug mode enabled
- Target Framework: .NET 4.8

### Project Dependencies

- Microsoft.CodeDom.Providers.DotNetCompilerPlatform 2.0.1
- Bootstrap CSS framework
- jQuery 3.4.1
- Owl Carousel 2.3.4
- Font Awesome icons

### Static Assets

**Frontend Libraries (TemplateFiles/):**
- Bootstrap, Font Awesome, custom CSS/JS
- Responsive design support

**Admin Template (assets/):**
- Separate admin UI assets with dashboard components
- Morris.js for charts
- AmCharts for data visualization

## Development Guidelines

### Working with User Pages

- All user-facing pages inherit from `User/user.Master`
- Use `ContentPlaceHolder1` for main content
- Use `head` ContentPlaceHolder for page-specific CSS/JS

### Database Operations

- Always use parameterized queries (currently using stored procedures)
- Use the `Connections.GetConnectionString()` helper
- Follow the existing pattern of SqlConnection → SqlCommand → SqlDataAdapter → DataTable
- Close connections in finally blocks

### Image Handling

- Use `Utils.IsValidExtension()` before processing uploads
- Save files with GUID filenames to avoid conflicts
- Store only relative paths in database
- Use `Utils.GetImageUrl()` when displaying images

### Session Variable Conventions

- `userID` - Primary user identifier
- `username` - Display name
- `admin` - Admin flag
- Additional profile data stored in session: `name`, `email`, `imageUrl`, `createdDate`

### URL Routing

The application uses query strings for parameterized pages:
- Edit profile: `Reg.aspx?id={userID}`
- Check `Request.QueryString["id"]` for edit mode vs registration mode
