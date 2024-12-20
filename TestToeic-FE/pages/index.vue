<template>
  <div class="max-w-9xl mx-auto bg-white p-6 rounded-lg shadow-lg">
    <div class="image-container my-4">
      <img
        src="https://www.fpacademy.co.in/blog/wp-content/uploads/2022/09/How-to-Learn-English-Speaking-at-Home-960x540-1.jpg"
        class="object-cover h-48 w-full"
      />
    </div>
    <div class="my-8 px-4">
      <h2 class="text-2xl font-bold text-gray-800 mb-4">Khóa học nổi bật</h2>
      <div class="flex flex-wrap gap-4">
        <NuxtLink
          v-for="level in levelOfClass"
          :key="level.levelOfClassId"
          :to="'/levelOfClass/' + level.levelOfClassId + '/classes'"
          class="px-4 py-2 bg-blue-100 text-blue-700 font-medium rounded-full shadow-sm hover:bg-blue-200 hover:shadow-md transition-all duration-200"
        >
          <span class="font-semibold">{{ level.leveName }}</span>
          <span class="ml-2 text-sm text-gray-600"
            >({{ level.classCount }})</span
          >
        </NuxtLink>
      </div>
    </div>

    <div class="my-8 px-4">
      <h2 class="text-2xl font-bold text-gray-800 mb-6">Đề thi mới nhất</h2>
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        <div
          v-for="test in newTests"
          :key="test.id"
          class="bg-white p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 border border-gray-200"
        >
          <NuxtLink :to="'/testDetail/' + test.id" class="block">
            <h3 class="text-xl font-semibold text-gray-900 mb-2">
              {{ test.title || "Chưa có tiêu đề" }}
            </h3>
            <p class="text-gray-600 text-sm mb-2">
              <span class="font-bold">Số lượng câu hỏi:</span>
              {{ test.questionDtos?.length || 0 }}
            </p>
            <p class="text-gray-600 text-sm mb-2">
              <span class="font-bold">Thời gian làm bài:</span>
              {{ test.testTimeMinutes || "Chưa có thông tin" }} phút
            </p>
            <p class="text-gray-600 text-sm mb-4">
              <span class="font-bold">Trạng thái: </span>
              <span :class="test.isActive ? 'text-green-500' : 'text-red-500'">
                {{
                  test.isActive !== undefined
                    ? test.isActive
                      ? "Đang mở"
                      : "Đã đóng"
                    : "Không xác định"
                }}
              </span>
            </p>

            <!-- Display stickers -->
            <div v-if="test.stickers && test.stickers.length > 0" class="mt-2">
              <div class="flex flex-wrap gap-2">
                <div
                  v-for="sticker in test.stickers"
                  :key="sticker.stickerId"
                  class="bg-blue-100 text-blue-600 text-xs font-medium px-3 py-1 rounded-full hover:bg-blue-200"
                >
                  #{{ sticker.stickerName }}
                </div>
              </div>
            </div>
          </NuxtLink>

          <!-- Action button -->
          <div class="mt-4 flex justify-end">
            <NuxtLink
              :to="'/testDetail/' + test.id"
              class="bg-blue-500 text-white px-4 py-2 text-sm font-medium rounded-md hover:bg-blue-600 transition-colors"
            >
              Chi tiết
            </NuxtLink>
          </div>
        </div>
      </div>

      <!-- See more button -->
      <div class="mt-6 flex justify-center">
        <NuxtLink
          :to="'/levelOfClass'"
          class="bg-blue-500 text-white px-6 py-3 text-sm font-medium rounded-lg hover:bg-blue-600 transition-colors"
        >
          Xem thêm
        </NuxtLink>
      </div>
    </div>
    <footer class="bg-gray-800 text-white py-6 mt-8 rounded-lg shadow-lg">
      <div class="container mx-auto px-4">
        <div class="flex flex-col md:flex-row justify-between items-center">
          <div class="mb-4 md:mb-0">
            <h3 class="text-lg font-semibold">Liên hệ</h3>
            <p class="text-sm">Email: info@example.com</p>
            <p class="text-sm">Phone: +123 456 7890</p>
          </div>
          <div class="mb-4 md:mb-0">
            <h3 class="text-lg font-semibold">Theo dõi chúng tôi</h3>
            <div class="flex space-x-4">
              <button
                type="button"
                data-twe-ripple-init
                data-twe-ripple-color="light"
                class="mb-2 inline-block rounded bg-[#1877f2] px-6 py-2.5 text-xs font-medium uppercase leading-normal text-white shadow-md transition duration-150 ease-in-out hover:shadow-lg focus:shadow-lg focus:outline-none focus:ring-0 active:shadow-lg"
              >
                <span class="[&>svg]:h-4 [&>svg]:w-4">
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="currentColor"
                    viewBox="0 0 320 512"
                  >
                    <!--!Font Awesome Free 6.5.1 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license/free Copyright 2024 Fonticons, Inc. -->
                    <path
                      d="M80 299.3V512H196V299.3h86.5l18-97.8H196V166.9c0-51.7 20.3-71.5 72.7-71.5c16.3 0 29.4 .4 37 1.2V7.9C291.4 4 256.4 0 236.2 0C129.3 0 80 50.5 80 159.4v42.1H14v97.8H80z"
                    />
                  </svg>
                </span>
              </button>
            </div>
          </div>
          <div class="text-sm">
            &copy; 2023 Your Company. All rights reserved.
          </div>
        </div>
      </div>
    </footer>
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
      levelOfClass: ref([]),
      newTests: ref([]),
    };
  },
  mounted() {
    this.fetchData();
    this.fetchLevel();
    this.fetchNewTest();
  },
  methods: {
    async fetchData() {
      try {
        const response = await axios.get(
          "http://localhost:5082/api/StudentApi/getListStudentPoint"
        );
        if (response.status === 200) {
          this.users = response.data;

          // Sắp xếp danh sách theo ngày hoàn thành (completion) từ mới nhất đến cũ nhất
          this.sortUsersByDate();
        } else {
          alert("Unexpected response: " + response.data);
          console.log(response.data);
        }
      } catch (error) {
        this.handleApiError(error);
      }
    },

    async fetchLevel() {
      const response = await axios.get(
        "http://localhost:5082/api/LevelOfClassApi"
      );
      if (response.status === 200) {
        this.levelOfClass = response.data;
      } else {
        alert("Unexpected response: " + response.data);
        console.log(response.data);
      }
    },

    async fetchNewTest() {
      const response = await axios.get(
        "http://localhost:5082/api/TestApi/list"
      );
      if (response.status === 200) {
        this.newTests = response.data;
        this.newTests = response.data.slice(0, 9);
        this.newTests.sort((a, b) => {
          const now = new Date(); // Lấy ngày hiện tại
          const distanceA = Math.abs(
            new Date(a.dateCreate).getTime() - now.getTime()
          );
          const distanceB = Math.abs(
            new Date(b.dateCreate).getTime() - now.getTime()
          );
          return distanceA - distanceB; // Sắp xếp theo khoảng cách gần nhất
        });
      } else {
        alert("Unexpected response: " + response.data);
        console.log(response.data);
      }
    },
    async submitForm() {
      try {
        const response = await axios.get(
          "http://localhost:5082/api/StudentApi/getListStudentPoint?username=" +
            this.name
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
        (a, b) => new Date(b.completion) - new Date(a.completion)
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
