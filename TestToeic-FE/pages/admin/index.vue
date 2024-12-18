<template>
  <div class="max-w-4xl mx-auto bg-white p-6 rounded-lg shadow-lg">
    <h2 class="text-2xl font-semibold mb-4 text-center">
      Tìm kiếm thông tin bài kiểm tra
    </h2>

    <form @submit.prevent="submitForm">
      <table class="min-w-full table-auto">
        <thead>
          <tr class="bg-gray-200"></tr>
        </thead>
        <tbody>
          <tr class="border-b">
            <td class="px-4 py-2">
              <input
                v-model="name"
                type="text"
                class="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Tìm theo người tạo, tên bài thi hoặc id..."
              />
            </td>
            <td class="px-4 py-2 text-right">
              <button
                type="submit"
                class="px-6 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                Tìm kiếm
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </form>

    <!-- Bảng dữ liệu kết quả tìm kiếm -->
    <div class="relative overflow-x-auto shadow-md sm:rounded-lg mt-4">
      <table
        class="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400"
      >
        <thead
          class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400"
        >
          <tr>
            <th scope="col" class="px-6 py-3">#</th>
            <th scope="col" class="px-6 py-3">Người tạo</th>
            <th scope="col" class="px-6 py-3">Ngày tạo</th>
            <th scope="col" class="px-6 py-3">Thời gian</th>
            <th scope="col" class="px-6 py-3">Bài thi</th>
            <th scope="col" class="px-6 py-3">Điểm bài thi</th>
            <th scope="col" class="px-6 py-3">Xem chi tiết</th>
          </tr>
        </thead>
        <tbody>
          <!-- Lặp qua dữ liệu tìm kiếm để hiển thị -->
          <tr
            v-for="(user, index) in users"
            :key="user.id"
            class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
          >
            <td class="px-6 py-4">{{ user.id }}</td>
            <th
              scope="row"
              class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
            >
              {{ user.userCreate }}
            </th>

            <td class="px-6 py-4">{{ user.stringDateCreateTest }}</td>
            <td class="px-6 py-4">{{ user.testTimeMinutes }} phút</td>
            <td class="px-6 py-4">{{ user.title }}</td>
            <!-- Ví dụ cho "Bài thi" -->
            <td class="px-6 py-4">{{ user.point }}</td>
            <!-- Ví dụ cho "Số điểm" -->
            <td class="px-6 py-4">
              <NuxtLink
                :to="`/admin/class/test/${user.id}`"
                class="font-medium text-blue-600 dark:text-blue-500 hover:underline"
                >Xem chi tiết</NuxtLink
              >
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import axios from "axios";
definePageMeta({
  layout: "admin",
  middleware: "auth",
});
export default {
  data() {
    return {
      name: "",
      users: [],
    };
  },
  mounted() {
    // Gọi API ngay khi component được mount
    this.fetchData();
  },
  methods: {
    async fetchData() {
      try {
        const response = await axios.get(
          `http://localhost:5082/api/testApi/list`
        );
        if (response.status === 200) {
          this.users = response.data;

          // Sắp xếp danh sách theo ngày hoàn thành (completion) từ mới nhất đến cũ nhất
          this.sortUsersByDate();
          console.log(this.users);
        } else {
          alert("Unexpected response: " + response.data);
          console.log(response.data);
        }
      } catch (error) {
        this.handleApiError(error);
      }
    },

    async submitForm() {
      try {
        console.log("Searching for:", this.name);
        const response = await axios.get(
          "http://localhost:5082/api/testApi/list?id=" + this.name
        );
        if (response.status === 200) {
          this.users = response.data;
          console.log(this.users);
          // Sắp xếp lại kết quả sau khi tìm kiếm
          this.sortUsersByDate();
        } else {
          alert("Unexpected response: " + response.data);
          console.log(response.data);
        }
      } catch (error) {
        this.handleApiError(error);
      }
    },

    // Phương thức để sắp xếp danh sách theo ngày hoàn thành (completion)
    sortUsersByDate() {
      this.users.sort(
        (a, b) => new Date(b.dateCreate) - new Date(a.dateCreate)
      );
    },

    // Phương thức xử lý lỗi API
    handleApiError(error) {
      if (error.response) {
        alert(`${error.response.data.message || error.response.statusText}`);
        console.error("Error response data:", error.response.data);
      } else if (error.request) {
        alert("Error: No response from server. Please try again later.");
        console.error("Error request:", error.request);
      } else {
        alert("Error: " + error.message);
        console.error("Error message:", error.message);
      }
    },
  },
};
</script>

<style lang="scss" scoped></style>
