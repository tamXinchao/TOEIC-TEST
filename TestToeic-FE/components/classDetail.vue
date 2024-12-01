<template>
  <div class="max-w-4xl mx-auto p-6 bg-white rounded-lg shadow-md">
    <h1 class="text-3xl font-semibold text-gray-800 mb-4">Bài kiểm tra</h1>

    <!-- Check if classes and the Message are defined -->
    <div
      v-if="classes && classes.message"
      class="text-gray-600 p-4 bg-yellow-100 rounded-md"
    >
      <p>{{ classes.message }}</p>
    </div>

    <!-- Check if there are tests available -->
    <div
      v-if="classes && classes.testOfClasses && classes.testOfClasses.length"
      class="space-y-4"
    >
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        <div
          v-for="test in classes.testOfClasses"
          :key="test.testOfClassId"
          class="relative bg-gray-100 p-4 rounded-lg shadow-sm hover:bg-gray-200 transition"
        >
          <!-- Nút sao chép, chỉ hiển thị nếu là /admin -->
          <button
            v-if="isAdminPath"
            @click="copyTestLink(test.id, test.classId)"
            class="absolute top-2 right-2 bg-blue-500 text-white text-sm px-2 py-1 rounded hover:bg-blue-600"
          >
            Sao chép
          </button>

          <h3 class="text-lg font-semibold text-gray-800">
            {{ test.title || "Chưa có tiêu đề" }}
          </h3>
          <p class="text-gray-500 text-sm">
            <span class="font-bold">Số lượng câu hỏi:</span>
            {{ test.questionDtos?.length || 0 }}
          </p>
          <p class="text-gray-500 text-sm">
            <span class="font-bold">Thời gian làm bài:</span>
            {{ test.testTimeMinutes || "Chưa có thông tin" }} phút
          </p>
          <p class="text-gray-500 text-sm">
            <span class="font-bold me-2">Trạng thái:</span>
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

          <!-- Display sticker if available -->
          <div v-if="test.stickers && test.stickers.length > 0" class="mt-4">
            <div class="flex flex-wrap space-x-2">
              <div
                v-for="sticker in test.stickers"
                :key="sticker.stickerId"
                class="flex items-center"
              >
                <NuxtLink :to="'#'">
                  <p
                    class="text-sm text-gray-600 bg-gray-200 my-1 px-3 py-1 rounded-full inline-flex items-center transition-all duration-300 ease-in-out hover:bg-blue-200 hover:text-white"
                  >
                    <span class="font-semibold text-gray-700">#</span>
                    {{ sticker.stickerName }}
                  </p>
                </NuxtLink>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRoute } from "vue-router";
import axios from "axios";

const { classes } = defineProps(["classes"]);
console.log(classes);
const route = useRoute();

// Xác định nếu đường dẫn hiện tại là /admin
const isAdminPath = route.path.startsWith("/admin");

// Hàm gửi yêu cầu POST để sao chép bài kiểm tra
const copyTestLink = async (testId, classId) => {
  try {
    // Định nghĩa payload
    const clone = {
      Id: testId,
      UserCreate: "5a8f41cb-b4f4-435a-6991-63e7be71b6d4", // Thay bằng ID người dùng thật nếu cần
      ClassId: classId,
    };

    // Gửi yêu cầu POST tới API và chờ phản hồi
    const response = await axios.post(
      `http://localhost:5082/api/TestApi`,
      clone
    );

    // Xử lý kết quả từ API
    if (response.status === 200) {
      const { message } = response.data; // Lấy message từ API
      alert(message || "Đã sao chép liên kết bài kiểm tra thành công!");
      window.location.reload();
    } else {
      alert("Đã xảy ra lỗi khi sao chép liên kết!");
    }
  } catch (error) {
    console.error("Lỗi khi gửi yêu cầu sao chép liên kết:", error);
    alert("Không thể sao chép liên kết. Vui lòng thử lại!");
  }
};
</script>

<style scoped>
/* Scoped styling if needed */
button {
  transition: all 0.3s ease;
}
</style>
