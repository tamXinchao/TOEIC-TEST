import axios from "axios";

export default defineNuxtPlugin(async (nuxtApp) => {
  const token = useCookie("token"); // Lấy JWT token từ cookie
  const user = useState("user", () => null);
  const role = useState("role", () => null);
  const userInfo = useState("userInfo", () => null);
  const _username = useState("username", () => null);

  const apiAxios = axios.create({
    baseURL: "http://localhost:5082/api",
    headers: {
      "Content-Type": "application/json",
    },
  });

  // Thêm token vào tất cả các request
  apiAxios.interceptors.request.use(
    (config) => {
      if (token.value) {
        config.headers["Authorization"] = `Bearer ${token.value}`;
      }
      return config;
    },
    (error) => Promise.reject(error)
  );

  // Auth object
  const auth = {
    user,
    role,
    userInfo,
    _username,

    async login(username, password) {
      try {
        const response = await apiAxios.post("/userApi/login", {
          username,
          password,
        });

        // Lưu token và thông tin người dùng
        token.value = response.data.token;
        user.value = response.data.userId;
        role.value = response.data.role;
        userInfo.value = response.data.userInfo;
        _username.value = response.data.userInfo?.userName || username;

        console.log("Login success:", response.data);
        return true;
      } catch (error) {
        console.error("Login error:", error);
        return false;
      }
    },

    logout() {
      // Xóa thông tin khi đăng xuất
      token.value = null;
      user.value = null;
      role.value = null;
      userInfo.value = null;
      _username.value = null;
      navigateTo("/login");
    },

    async checkAuth() {
      if (!token.value) {
        console.warn("Token not found, user not authenticated");
        return false;
      }

      try {
        const response = await apiAxios.get("/userApi/verify");
        // Khôi phục thông tin người dùng
        user.value = response.data.userId;
        role.value = response.data.role;
        userInfo.value = response.data.userInfo;
        _username.value = response.data.userInfo?.userName || username;
        return true;
      } catch (error) {
        console.error("Token verification failed:", error);
        auth.logout(); // Đăng xuất nếu token không hợp lệ
        return false;
      }
    },
  };

  // Kiểm tra xác thực khi tải lại trang
  await auth.checkAuth();

  // Cung cấp `auth` cho toàn bộ ứng dụng
  return {
    provide: {
      auth,
    },
  };
});
