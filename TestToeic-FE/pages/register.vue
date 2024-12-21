<template>
  <div class="max-w-4xl mx-auto bg-white p-6 rounded-lg shadow-lg">
    <form class="mt-8 space-y-6" @submit.prevent="submitForm">
      <div class="rounded-md shadow-sm -space-y-px">
        <div>
          <label for="email" class="sr-only">Username</label>
          <input
            id="username"
            v-model="form.username"
            name="username"
            type="username"
            required
            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
            placeholder="Username"
          />
        </div>
        <div>
          <label for="email" class="sr-only">Email</label>
          <input
            id="email"
            v-model="form.email"
            name="email"
            type="email"
            required
            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
            placeholder="Email"
          />
        </div>
        <div>
          <label for="email" class="sr-only">Số điện thoại</label>
          <input
            id="phoneNumber"
            v-model="form.phoneNumber"
            name="phoneNumber"
            type="phoneNumber"
            required
            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
            placeholder="PhoneNumber"
          />
        </div>

        <div>
          <label for="passwordHash" class="sr-only">Mật khẩu</label>
          <input
            id="passwordHash"
            v-model="form.passwordHash"
            name="passwordHash"
            type="password"
            required
            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
            placeholder="Mật khẩu"
          />
        </div>
      </div>

      <div>
        <button
          type="submit"
          class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
        >
          <span class="absolute left-0 inset-y-0 flex items-center pl-3">
          </span>
          Đăng ký
        </button>
      </div>
    </form>
  </div>
</template>

<script>
import axios from "axios";
import { ref } from "vue";
export default {
  data() {
    return {
      name: "",
      users: [],
      form: ref({
        email: "",
        password: "",
        username: "",
        passwordHash: "",
      }),
      $auth: useNuxtApp(),
    };
  },
  methods: {
    checkAuth() {
      if (this.$auth.checkAuth()) {
        this.$router.push({ name: "dashboard" });
      }
    },
    async submitForm() {
      const request = this.form;
      axios
        .post("http://localhost:5082/api/userApi", request)
        .then((response) => {
          console.log(response);
          alert("Đăng ký thành công");
          this.$router.push({ name: "login" });
        })
        .catch((error) => {
          console.log(error);
          alert(error.response.data);
        });
    },
  },
  mounted() {
    this.checkAuth();
  },
};
</script>

<style lang="scss" scoped></style>
