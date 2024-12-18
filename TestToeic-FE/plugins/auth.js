import axios from "axios";

export default defineNuxtPlugin(async () => {
  const token = useCookie("token"); // Lấy JWT token từ cookie
  const user = useState("user", () => null);
  const role = useState("role", () => null);
  const apiAxios = axios.create({
    baseURL: "http://localhost:5082/api",
    headers: {
      "Content-Type": "application/json",
    },
  });

  apiAxios.interceptors.request.use(
    (config) => {
      if (token.value) {
        config.headers["Authorization"] = `Bearer ${token.value}`;
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

  const auth = {
    user,
    role,
    async login(username, password) {
      try {
        const response = await apiAxios.post("/userApi/login", {
          username,
          password,
        });

        token.value = response.data.token;
        user.value = response.data.userId;
        role.value = response.data.role;
        console.log(1);
        console.log(response.data);
        console.log(1);
        return true;
      } catch (error) {
        console.error("Login error:", error);
        return false;
      }
    },

    logout() {
      token.value = null;
      user.value = null;
      navigateTo("/login");
    },

    async checkAuth() {
      if (!token.value) return false;
      try {
        const response = await apiAxios.get("/userApi/verify");
        user.value = response.data.userId;
        role.value = response.data.role;
        console.log(2);
        console.log(response.data);
        console.log(2);
        return true;
      } catch (error) {
        token.value = null;
        user.value = null;
        return false;
      }
    },
  };

  return {
    provide: {
      auth,
    },
  };
});
