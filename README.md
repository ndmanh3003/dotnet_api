# Quản lý công việc

Ứng dụng quản lý công việc với .NET backend và React frontend.

## Yêu cầu hệ thống

- .NET 9.0 SDK
- Node.js 18+ và npm
- MySQL 8.0+
- Google OAuth credentials

## Cài đặt

1. **Cài đặt dependencies:**
   ```bash
   dotnet restore
   ```

2. **Cấu hình database:**
   - Copy `appsettings.Example.json` thành `appsettings.Development.json`
   - Cập nhật connection string trong `appsettings.Development.json`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Port=3306;Database=mydb;User=your_user;Password=your_password;"
     }
     ```

3. **Cấu hình Google OAuth:**
   - Lấy Google Client ID và Client Secret từ [Google Cloud Console](https://console.cloud.google.com/)
   - Cập nhật trong `appsettings.Development.json`:
     ```json
     "Authentication": {
       "Google": {
         "ClientId": "your_google_client_id",
         "ClientSecret": "your_google_client_secret"
       }
     }
     ```

4. **Cấu hình JWT:**
   - Tạo secret key (tối thiểu 32 ký tự)
   - Cập nhật trong `appsettings.Development.json`:
     ```json
     "Jwt": {
       "Key": "your_secret_key_at_least_32_characters_long",
       "Issuer": "https://api.yourdomain.com",
       "Audience": "https://yourdomain.com",
       "ExpirationDays": 7
     }
     ```

5. **Cấu hình Client URL:**
   - Cập nhật BaseUrl trong `appsettings.Development.json` để khớp với frontend:
     ```json
     "Client": {
       "BaseUrl": "http://127.0.0.1:5000"
     }
     ```

6. **Chạy migrations:**
   ```bash
   dotnet ef database update
   ```

## Chạy ứng dụng
   ```bash
   dotnet watch run
   ```
## Endpoints
| Method | Endpoint | Mô tả | Quyền |
|--------|----------|-------|-------|
| POST | `/health` | Trạng thái database | Public |
| POST | `/auth/callback` | Google OAuth callback, trả về JWT | Public |
| GET | `/auth/me` | Lấy thông tin user hiện tại | Authenticated |
| GET | `/todo` | Danh sách todos (có phân trang, tìm kiếm, sắp xếp) | Authenticated |
| GET | `/todo/{id}` | Chi tiết todo | Authenticated |
| POST | `/todo` | Tạo todo mới | Authenticated |
| PUT | `/todo/{id}` | Cập nhật todo | Authenticated |
| DELETE | `/todo/{id}` | Xóa todo | Authenticated |
| GET | `/user` | Danh sách users | Admin |
| GET | `/dropdown` | Lấy dữ liệu dropdown cho enums và models | Authenticated |

**Query parameters chung:**
- `Page` - Số trang (mặc định: 1)
- `Limit` - Số item mỗi trang (mặc định: 20)
- `Keyword` - Từ khóa tìm kiếm
- `OrderCol` - Cột sắp xếp (mặc định: "Id")
- `OrderDir` - Hướng sắp xếp: "asc" hoặc "desc" (mặc định: "desc")

**Query parameters cho GET /dropdown:**
- `enums` - Danh sách enum names (ví dụ: "Role,TodoStatus")
- `models` - Danh sách model names

## Response DTOs

### TodoDto / TodoIndexDto
```json
{
  "id": 1,
  "title": "Task title",
  "dueDate": "2024-01-01T00:00:00Z",
  "status": "InProgress",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### UserDto / UserIndexDto
```json
{
  "id": 1,
  "email": "user@example.com",
  "fullName": "Nguyễn Văn A",
  "name": "Nguyễn Văn A",
  "role": "User",
  "picture": "https://example.com/avatar.jpg",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Dropdown Response
```json
{
  "enums": {
    "Role": [
      { "value": "Admin", "label": "Quản trị viên" },
      { "value": "User", "label": "Người dùng" }
    ],
    "TodoStatus": [
      { "value": "InProgress", "label": "Đang thực hiệns" },
      { "value": "Completed", "label": "Hoàn thành" }
    ]
  },
  "models": {
    "User": [
      { "value": 1, "label": "Nguyễn Văn A", "email": "user@example.com" },
      { "value": 2, "label": "Trần Thị B", "email": "user2@example.com" }
    ]
  }
}
```

## Request DTOs

### CallbackRequest (Auth)
```json
{
  "code": "4/0AeanS8...",
  "redirectUri": "/auth/callback"
}
```

### CreateRequest (Todo)
```json
{
  "title": "Task title",
  "dueDate": "2025-01-01",
  "status": "InProgress"
}
```

### UpdateRequest (Todo)
```json
{
  "title": "Updated title",
  "dueDate": "2025-01-01",
  "status": "Completed"
}
```