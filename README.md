# AOL Portal

A modern web application built with ASP.NET Core API and Angular frontend, featuring JWT authentication and Identity management.

## 🏗️ Architecture

- **Backend**: ASP.NET Core 8.0 Web API with Entity Framework Core
- **Frontend**: Angular 17 with Fuse UI Framework
- **Authentication**: JWT Bearer tokens with ASP.NET Core Identity
- **Database**: SQL Server with Identity tables
- **CORS**: Configured for cross-origin requests

## 📋 Prerequisites

- .NET 8.0 SDK
- Node.js 18+ and npm
- SQL Server (or SQL Server Express)
- Visual Studio 2022 (recommended) or VS Code

## 🚀 Quick Start

### 1. Clone and Setup

```bash
git clone <repository-url>
cd AOL_Portal
```

### 2. Database Setup

```bash
# Navigate to API project
cd AOL_Portal

# Create initial migration (if not exists)
dotnet ef migrations add InitialIdentityCreate

# Apply migrations to database : Note we dont do this ! we only created the first immigration for identity tables
# We DO NOT user code first, but manually set the dbcontext after database changes
dotnet ef database update
```

### 3. Start the API

#### Option A: Visual Studio
1. Open `AOL_Portal.sln` in Visual Studio
2. Select **"API Only"** profile from the dropdown
3. Click **Start** (F5)

#### Option B: Command Line
```bash
# Navigate to API project
cd AOL_Portal

# Start API with HTTPS
dotnet run --launch-profile https

# Or start API only (no Angular auto-start)
dotnet run --launch-profile "API Only"
```

### 4. Start Angular Frontend

```bash
# Navigate to Angular project
cd ClientApp

# Install dependencies (first time only)
npm install

# Start development server
ng serve
```

## 🌐 Access Points

- **API**: https://localhost:7117
- **API Documentation**: https://localhost:7117/swagger
- **Angular App**: http://localhost:7054
- **Health Check**: https://localhost:7117/api/health

## 🛣️ Routing

- **Default Page**: `/sign-in` (redirects from root `/`)
- **After Login**: `/dashboard` (redirects from `/signed-in-redirect`)
- **Admin Panel**: `/admin`
- **Available Routes**:
  - `/sign-in` - Authentication page
  - `/dashboard` - Main dashboard (requires authentication)
  - `/admin` - Admin panel (requires authentication)
  - `/home` - Landing page

## 🔐 Authentication

### Authentication Flow

1. **User Registration**: Admin creates user account via `/api/auth/register`
2. **Email Confirmation**: User receives email with confirmation link
3. **Email Validation**: User clicks link, calls `/api/auth/registrationConfirmation`
4. **Password Setup**: User receives password reset email after email confirmation
5. **Login**: User logs in with email and password via `/api/auth/login`
6. **Token Refresh**: User can refresh access tokens via `/api/auth/refresh`
7. **Logout**: User can logout and invalidate refresh tokens via `/api/auth/logout`
8. **Password Reset**: User can request password reset via `/api/auth/forgotpassword`

### User Management Features

- **Email Confirmation**: Required for account activation
- **Password Reset**: Email-based verification code system
- **Role-based Access**: Admin and regular user roles
- **Account Status**: Active/Inactive status management
- **JWT Tokens**: Secure authentication with configurable expiration
- **Refresh Token Management**: Secure token refresh and invalidation system

### API Endpoints

#### Authentication Endpoints (`/api/auth`)
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration (UserAdmin role only)
- `GET /api/auth/me` - Get current user (authenticated)
- `POST /api/auth/logout` - User logout and refresh token invalidation (requires authentication)
- `POST /api/auth/refresh` - Refresh access token
- `GET /api/auth/registrationConfirmation` - Confirm email registration
- `POST /api/auth/resendEmailConfirmationCode` - Resend email confirmation code (UserAdmin role only)
- `POST /api/auth/forgotpassword` - Request password reset
- `POST /api/auth/resetpassword` - Reset password with verification code
- `POST /api/auth/unlockuser` - Unlock locked user account (UserAdmin role only)
- `PUT /api/auth/updateuser` - Update user information (UserAdmin role only)
- `POST /api/auth/adduserrole` - Add role to user (site admin only)
- `POST /api/auth/createrole` - Create new role (site admin only)
- `POST /api/auth/removeuserrole` - Remove role from user (site admin only)

#### Health Check Endpoints (`/api/health`)
- `GET /api/health` - System health status and database connectivity

#### Test Endpoints (`/api/test`)
- `GET /api/test` - Basic API status check
- `GET /api/test/public` - Public endpoint (no authentication)
- `GET /api/test/protected` - Protected endpoint (requires authentication)
- `GET /api/test/admin` - Admin-only endpoint (requires admin role)

### Test Authentication

```bash
# Test health endpoint
curl -k https://localhost:7117/api/health

# Test basic API status
curl -k https://localhost:7117/api/test

# Test public endpoint
curl -k https://localhost:7117/api/test/public

# Test login (replace with actual credentials)
curl -k -X POST https://localhost:7117/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"Test123!"}'

# Test protected endpoint (requires authentication)
curl -k -X GET https://localhost:7117/api/test/protected \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Test forgot password
curl -k -X POST https://localhost:7117/api/auth/forgotpassword \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com"}'

# Test user registration (UserAdmin role only)
curl -k -X POST https://localhost:7117/api/auth/register \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer USERADMIN_JWT_TOKEN" \
  -d '{"email":"newuser@example.com","firstName":"John","surname":"Doe"}'

# Test unlock user (UserAdmin role only)
curl -k -X POST https://localhost:7117/api/auth/unlockuser \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer USERADMIN_JWT_TOKEN" \
  -d '{"userId":"user-guid-here"}'

# Test update user (UserAdmin role only)
curl -k -X PUT https://localhost:7117/api/auth/updateuser \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer USERADMIN_JWT_TOKEN" \
  -d '{"userId":"user-guid-here","email":"updated@example.com","firstName":"Updated","surname":"Name","statusId":2,"isSiteAdmin":false}'

# Test add user role (site admin only)
curl -k -X POST https://localhost:7117/api/auth/adduserrole \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SITE_ADMIN_JWT_TOKEN" \
  -d '{"userId":"user-guid-here","roleName":"Admin","requestingUserId":"site-admin-guid-here"}'

# Test create role (site admin only)
curl -k -X POST https://localhost:7117/api/auth/createrole \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SITE_ADMIN_JWT_TOKEN" \
  -d '{"roleName":"Manager","requestingUserId":"site-admin-guid-here"}'

# Test remove user role (site admin only)
curl -k -X POST https://localhost:7117/api/auth/removeuserrole \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SITE_ADMIN_JWT_TOKEN" \
  -d '{"userId":"user-guid-here","roleName":"Admin","requestingUserId":"site-admin-guid-here"}'

# Test logout (requires authentication)
curl -k -X POST https://localhost:7117/api/auth/logout \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## 🛠️ Development Commands

### API Commands

```bash
# Build API
dotnet build

# Run API with HTTPS
dotnet run --launch-profile https

# Run API only (no Angular)
dotnet run --launch-profile "API Only"

# Create new migration
dotnet ef migrations add [MigrationName]

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

### Angular Commands

```bash
# Navigate to Angular project
cd ClientApp

# Install dependencies
npm install

# Start development server
ng serve

# Build for production
ng build --prod

# Run tests
ng test

# Lint code
ng lint
```

### Utility Commands

```bash
# Clear authentication tokens (open in browser)
ClientApp/clear-tokens.html

# Test CORS (open in browser)
ClientApp/test-cors.html
```

## 📁 Project Structure

```
AOL_Portal/
├── AOL_Portal/                 # ASP.NET Core API
│   ├── Controllers/            # API Controllers
│   ├── Data/                   # Entity Framework Context
│   ├── Models/                 # API Models
│   ├── Services/               # Business Logic Services
│   ├── Configuration/          # App Configuration
│   └── Program.cs             # API Startup
├── ClientApp/                  # Angular Frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/          # Core Services
│   │   │   ├── shared/        # Shared Components
│   │   │   └── modules/       # Feature Modules
│   │   └── environments/      # Environment Config
│   └── angular.json           # Angular Configuration
└── README.md                  # This file
```

## 🔧 Configuration

### Environment Variables

```json
// appsettings.json
{
  "ConnectionStrings": {
    "AOLPortalConnection": "Server=...;Database=AOL_Portal_Dev;..."
  },
  "ApiService": {
    "ApiKey": "APIKEY-TESTING-SECURE-UMAS-2024",
    "JwtSecret": "ColumboJwtSecretKey2024VeryLongAndSecureForMobileApplications",
    "JwtExpiryMinutes": 60
  }
}
```

### Angular Environment

```typescript
// ClientApp/src/environments/environment.ts
export const environment = {
  production: false,
  apiBaseUrl: 'https://localhost:7117',
  devExpressUrl: 'https://localhost:7117/'
};
```

## 🚨 Troubleshooting

### Common Issues

1. **CORS Errors**
   - Ensure API is running on correct port (7117)
   - Check Angular environment configuration
   - Verify CORS policy in Program.cs

2. **Database Connection**
   - Verify connection string in appsettings.json
   - Ensure SQL Server is running
   - Run migrations: `dotnet ef database update`

3. **Authentication Issues**
   - Clear browser tokens: Open `ClientApp/clear-tokens.html`
   - Check JWT secret in appsettings.json
   - Verify API endpoints are accessible

4. **Port Conflicts**
   - API: 7117 (HTTPS), 5259 (HTTP)
   - Angular: 4200 (HTTP)
   - Check if ports are available

### Debug Commands

```bash
# Check if API is running
Get-Process -Name "AOL_Portal" -ErrorAction SilentlyContinue

# Test API health
Invoke-WebRequest -Uri "https://localhost:7117/api/health"

# Check Angular build
cd ClientApp && ng build --verbose
```

## 📚 API Documentation

Once the API is running, visit:
- **Swagger UI**: https://localhost:7117/swagger
- **Health Check**: https://localhost:7117/api/health
- **Test Endpoint**: https://localhost:7117/api/test

### Request/Response Models

#### Authentication Models
- `LoginRequest`: Email, Password
- `RegisterRequest`: Email, FirstName, Surname
- `AuthResponse`: Success, Token, RefreshToken, ExpiresAt, User, Message
- `UserInfo`: Id, Email, FirstName, Surname, IsSiteAdmin, Roles
- `ForgotPasswordRequest`: Email
- `UnlockUserRequest`: UserId
- `UpdateUserRequest`: UserId, Email, FirstName, Surname, StatusId, IsSiteAdmin
- `AddUserRoleRequest`: UserId, RoleName, RequestingUserId
- `CreateRoleRequest`: RoleName, RequestingUserId
- `RemoveUserRoleRequest`: UserId, RoleName, RequestingUserId
- `ResetPasswordRequest`: Email, VerificationCode, NewPassword, ConfirmPassword
- `ValidateResult`: Success, Errors, Message

#### Health Check Response
```json
{
  "Status": "Healthy",
  "Timestamp": "2024-01-01T00:00:00Z",
  "Database": "Connected",
  "Environment": "Development"
}
```

## 🔒 Security

- JWT tokens with configurable expiration
- Password requirements: 8+ chars, uppercase, lowercase, numbers, symbols
- CORS configured for development
- HTTPS enabled for production

## 📝 Notes

- The API uses ASP.NET Core Identity for user management
- Angular app uses Fuse UI framework for modern UI components
- Proxy configuration forwards API requests from Angular to backend
- Development certificates may need to be trusted: `dotnet dev-certs https --trust`

## 🤝 Contributing

1. Follow the existing code structure
2. Use the provided authentication services
3. Test API endpoints with Swagger
4. Ensure CORS is properly configured for new endpoints

---

**Happy Coding! 🚀** 
