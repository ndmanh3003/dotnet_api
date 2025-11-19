# .NET API

## Kết nối

### Database
Yêu cầu MySQL. Cấu hình connection string trong `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=mydb;User=your_user;Password=your_password;"
  }
}
```

### JWT Authentication
Cấu hình JWT trong `appsettings.json`:
```json
{
  "Jwt": {
    "Issuer": "your_issuer",
    "Audience": "your_audience",
    "Key": "your_secret_key",
    "ExpirationDays": 7
  }
}
```

### Google OAuth
Cấu hình Google OAuth:
```json
{
  "Authentication": {
    "Google": {
      "ClientId": "your_google_client_id",
      "ClientSecret": "your_google_client_secret"
    }
  }
}
```

### Client URL
Cấu hình CORS client URL:
```json
{
  "Client": {
    "BaseUrl": "http://localhost:3000"
  }
}
```

## Endpoints

| Method | Endpoint | Mô tả | Quyền |
|--------|----------|-------|-------|
| POST | `/auth/callback` | Google OAuth callback, trả về JWT token và user info | Public |
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
