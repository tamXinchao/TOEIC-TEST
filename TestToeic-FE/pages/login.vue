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
            type="text"
            required
            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
            placeholder="username"
          />
        </div>
        <div>
          <label for="password" class="sr-only">Mật khẩu</label>
          <input
            id="password"
            v-model="form.password"
            name="password"
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
          Đăng nhập
        </button>
      </div>
    </form>
  </div>
</template>

<script>
import { ref } from "vue";

export default {
  data() {
    return {
      name: "",
      users: [],
      form: ref({
        username: "",
        password: "",
      }),
      $toast: useNuxtApp(),
      $auth: useNuxtApp(),
    };
  },
  methods: {
    checkLogin() {
      if (this.$auth.checkAuth()) {
        this.$router.push("/dashboard");
      }
    },

    async submitForm() {
      const request = this.form;
      console.log(request);
      const success = await this.$auth.login(
        this.form.username,
        this.form.password
      );
      if (success) {
        console.log("Login thành công");
        this.$router.push("/");
      } else {
        console.log("Login thất bại");
      }
    },
  },
  mounted() {
    this.checkLogin();
  },
};
</script>

<style lang="scss" scoped></style>
