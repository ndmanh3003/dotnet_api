# Frontend - Quản lý công việc

React frontend cho ứng dụng quản lý công việc.

## Yêu cầu hệ thống

- Node.js 18+ và npm
- Backend API đang chạy tại `http://localhost:5086`

## Cài đặt

1. **Cài đặt dependencies:**
   ```bash
   npm install
   ```

2. **Cấu hình environment:**
   - Copy `.env.example` thành `.env.local`
   - Cập nhật các giá trị:
     ```
     VITE_API_BASE_URL=http://localhost:5086
     VITE_GOOGLE_CLIENT_ID=your_google_client_id
     VITE_REDIRECT_URI=http://127.0.0.1:5000/login
     ```
   
   **Lưu ý:** 
   - `VITE_GOOGLE_CLIENT_ID` phải trùng với `Authentication:Google:ClientId` trong `appsettings.Development.json` của backend.
   - `VITE_REDIRECT_URI` phải được khai báo ở [Google Cloud Console](https://console.cloud.google.com/)

## Chạy ứng dụng

```bash
npm run dev
```

Frontend sẽ chạy tại: `http://127.0.0.1:5000`

## Pages

### `/login`
- Trang đăng nhập với Google OAuth
- Tự động redirect đến `/todo` nếu đã có token

### `/todo`
- Trang quản lý công việc
- Yêu cầu authentication (tự động redirect về `/login` nếu chưa đăng nhập)
- Tính năng:
  - Xem danh sách công việc
  - Tạo mới công việc
  - Chỉnh sửa công việc
  - Xóa công việc
  - Lọc theo trạng thái
