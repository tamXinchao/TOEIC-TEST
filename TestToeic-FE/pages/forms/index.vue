<template>
  <div class="max-w-4xl mx-auto bg-white p-6 rounded-lg shadow-lg">
    <h2 class="text-2xl font-semibold mb-4 text-center">
      Thông Tin Người Dùng
    </h2>

    <form @submit.prevent="submitForm">
      <table class="min-w-full table-auto">
        <thead>
          <tr class="bg-gray-200">
            <th class="px-4 py-2 text-left">Tên Trường</th>
            <th class="px-4 py-2 text-left">Thông Tin</th>
          </tr>
        </thead>
        <tbody>
          <tr class="border-b">
            <td class="px-4 py-2 font-medium">Họ và Tên</td>
            <td class="px-4 py-2">
              <input
                v-model="name"
                type="text"
                class="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Nhập họ và tên"
              />
            </td>
          </tr>
          <tr class="border-b">
            <td class="px-4 py-2 font-medium">Email</td>
            <td class="px-4 py-2">
              <input
                v-model="email"
                type="email"
                class="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Nhập email"
              />
            </td>
          </tr>
          <tr class="border-b">
            <td class="px-4 py-2 font-medium">Số Điện Thoại</td>
            <td class="px-4 py-2">
              <input
                v-model="phone"
                type="tel"
                class="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Nhập số điện thoại"
              />
            </td>
          </tr>
          <tr>
            <td class="px-4 py-2"></td>
            <td class="px-4 py-2 text-right">
              <button
                type="submit"
                class="px-6 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                Lưu Thông Tin
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </form>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      name: "", // Họ và tên
      email: "", // Email
      phone: "", // Số điện thoại
    };
  },
  methods: {
    generate_uuidv4() {
      return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(
        /[xy]/g,
        function (c) {
          var uuid = (Math.random() * 16) | 0,
            v = c == "x" ? uuid : (uuid & 0x3) | 0x8;
          return uuid.toString(16);
        }
      );
    },
    async submitForm() {
      const userId = this.generate_uuidv4();
      const requestData = {
        Id: userId,
        UserName: this.name,
        NormalizedUserName: this.name.toUpperCase(),
        Email: this.email,
        NormalizedEmail: this.email.toUpperCase(),
        PhoneNumber: this.phone,
        PasswordHash: this.generate_uuidv4(),
        SecurityStamp: this.generate_uuidv4(),
        ConcurrencyStamp: this.generate_uuidv4(),
      };
      try {
        const response = await axios.post(
          "http://localhost:5082/api/userApi",
          requestData
        );
        if (response.status === 200) {
          // Redirect to the new route with the ID included in the URL
          this.$router.push(`/forms/${userId}/test`);
        }
      } catch (error) {
        console.error(error);
      }
    },
  },
};
</script>

<style lang="scss" scoped></style>
