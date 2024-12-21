<template>
  <div class="grid grid-cols-1 md:grid-cols-2 gap-6 p-4 bg-gray-50">
    <!-- Left Side: Schedule and Courses -->
    <div class="space-y-6">
      <div class="bg-white p-4 rounded-lg shadow-md">
        <h2 class="text-xl font-semibold text-gray-800 mb-4">
          Lịch thi của bạn
        </h2>
        <Schedule :schedules="schedules" :tests="tests" />
      </div>
      <div
        class="bg-white p-4 rounded-lg shadow-md max-h-[400px] overflow-auto"
      >
        <h2 class="text-xl font-semibold text-gray-800 mb-4">
          Khóa học của bạn
        </h2>
        <LevelOfClass :levelOfClasses="levelOfClasses" />
      </div>
    </div>

    <!-- Right Side: User Info and Search -->
    <div class="bg-white p-4 rounded-lg shadow-md space-y-6">
      <UserInformation />

      <h2 class="text-2xl font-semibold mb-4 text-center">
        Tìm kiếm thông tin bài thi
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
                  placeholder="Tìm theo tài khoản, email hoặc số điện thoại"
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

      <!-- Search Results Table -->
      <div
        class="relative overflow-x-auto shadow-md sm:rounded-lg mt-4 max-h-[400px] overflow-y-auto"
      >
        <table
          class="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400"
        >
          <thead
            class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400"
          >
            <tr>
              <th scope="col" class="px-6 py-3">Ngày hoàn thành</th>
              <th scope="col" class="px-6 py-3">Thời gian hoàn thành</th>
              <th scope="col" class="px-6 py-3">Bài thi</th>
              <th scope="col" class="px-6 py-3">Số điểm</th>
              <th scope="col" class="px-6 py-3">Xem chi tiết</th>
            </tr>
          </thead>
          <tbody class="bg-white dark:bg-gray-800 dark:border-gray-700">
            <tr
              v-if="users.length > 0"
              v-for="(user, index) in users"
              :key="index"
              class="border-b dark:text-white hover:bg-gray-50 dark:hover:bg-gray-600"
            >
              <td class="px-6 py-4">{{ user.completionString }}</td>
              <td class="px-6 py-4">{{ user.duration }}</td>
              <td class="px-6 py-4">{{ user.title }}</td>
              <td class="px-6 py-4">{{ user.pointOfStudent }}</td>
              <td class="px-6 py-4">
                <NuxtLink
                  :to="`/results/${user.id}`"
                  class="font-medium text-blue-600 dark:text-blue-500 hover:underline"
                  >Xem chi tiết</NuxtLink
                >
              </td>
            </tr>
            <tr v-else>
              <td colspan="5" class="px-6 py-4 text-center text-gray-500">
                Không có kết quả nào
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { ref, onMounted } from "vue";

// Fetch user ID from the auth object
const { $auth } = useNuxtApp();
const userId = $auth.user.value;

const name = ref("");
const users = ref([]);

onMounted(() => {
  fetchUsers();
});
const { data: schedules } = await axios.get(
  `http://localhost:5082/api/ScheduleApi/${userId}`
);
const { data: tests } = await axios.get(
  "http://localhost:5082/api/testApi/list"
);
const { data: levelOfClasses } = await axios.get(
  `http://localhost:5082/api/LevelOfClassApi?userId=${userId}`
);

// Fetch users based on search criteria
const fetchUsers = async () => {
  try {
    const response = await axios.get(
      `http://localhost:5082/api/StudentApi/getByUserId?userId=${userId}`
    );
    users.value = response.data;
    sortUsersByDate();
  } catch (error) {
    console.error("Error fetching user data:", error);
  }
};

// Search function
const submitForm = async () => {
  try {
    const response = await axios.get(
      `http://localhost:5082/api/StudentApi/getByUserId?userId=${userId}&info=${name.value}`
    );
    users.value = response.data;
    sortUsersByDate();
  } catch (error) {
    console.error("Error searching users:", error);
  }
};

// Sort users by completion date
const sortUsersByDate = () => {
  users.value.sort((a, b) => new Date(b.completion) - new Date(a.completion));
};
</script>

<style scoped>
/* Custom styles if needed */
</style>
