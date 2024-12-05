<template>
  <div class="max-w-4xl mx-auto p-6 bg-white rounded-lg shadow-md">
    <h1
      class="text-3xl font-semibold text-gray-800 mb-4 flex justify-between items-center"
    >
      {{ classes.nameofClass || "Chưa có tên lớp" }}
      <button
        v-if="
          !isAdminPath &&
          !classes.hasJoin &&
          classes.message !== 'Bạn đã gửi yêu cầu tham gia vui lòng đợi'
        "
        @click="joinClass(classes.idOfClass)"
        class="bg-green-500 text-white text-sm px-4 py-2 rounded hover:bg-green-600"
      >
        Tham gia lớp
      </button>
      <span
        v-if="classes.message == 'Bạn đã gửi yêu cầu tham gia vui lòng đợi'"
        class="bg-yellow-500 text-white text-sm px-4 py-2 rounded hover:bg-yellow-600"
      >
        Vui lòng đợi
      </span>

      <button
        v-if="!isAdminPath && classes.hasJoin"
        @click="leaveClass(classes.idOfClass)"
        class="bg-red-500 text-white text-sm px-4 py-2 rounded hover:bg-red-600"
      >
        Rời lớp
      </button>
    </h1>

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
          <div class="flex mt-2 justify-end space-x-2" v-if="isAdminPath">
            <!-- Nút chỉnh sửa -->
            <button
              @click="openModal(test)"
              class="p-2 bg-green-600 hover:bg-green-700 text-white text-sm flex items-center justify-center font-medium rounded-full"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-4 w-4"
                viewBox="0 0 24 24"
                fill="currentColor"
              >
                <path
                  d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z"
                />
              </svg>
            </button>
            <!-- Nút xóa -->
            <button
              class="p-2 bg-red-600 hover:bg-red-700 flex items-center justify-center text-white text-sm font-medium rounded-full"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-4 w-4"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
                />
              </svg>
            </button>
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
const joinClass = async (classId) => {
  try {
    // Định nghĩa payload
    const memberInfo = {
      MemberId: "1a2b3c4d-5678-90ab-cdef-1334567890ab",
      ClassId: classId,
    };

    // Gửi yêu cầu POST tới API và chờ phản hồi
    const response = await axios.post(
      `http://localhost:5082/api/MemberOfClassApi/joinClass`,
      memberInfo
    );

    // Xử lý kết quả từ API
    if (response.status === 200) {
      const { message } = response.data; // Lấy message từ API
      alert(message || "Bạn đã gửi yêu cầu tham gia. Vui lòng đợi duyệt!");
      window.location.reload();
    } else {
      alert("Đã xảy ra lỗi khi gửi yêu cầu!");
    }
  } catch (error) {
    console.error("Lỗi khi gửi yêu cầu:", error);
    alert("Không thể gửi yêu cầu. Vui lòng thử lại!");
  }
};
const leaveClass = async (classId) => {
  try {
    // Định nghĩa payload
    const memberInfo = {
      MemberId: "1a2b3c4d-5678-90ab-cdef-1334567890ab",
      ClassId: classId,
    };

    // Gửi yêu cầu POST tới API và chờ phản hồi
    const response = await axios.delete(
      `http://localhost:5082/api/MemberOfClassApi/leaveClass/${memberInfo.MemberId}?classId=${memberInfo.ClassId}` // Thay thế classId theo đúng logic,
    );

    // Xử lý kết quả từ API
    if (response.status === 200) {
      const { message } = response.data; // Lấy message từ API
      alert(message || "Bạn đã rời lớp thành công!");
      window.location.reload();
    } else {
      alert("Đã xảy ra lỗi khi bạn đang cố rời lớp!");
    }
  } catch (error) {
    console.error("Lỗi khi rời lớp:", error);
    alert("Không thể rời lớp. Vui lòng thử lại!");
  }
};
</script>

<style scoped>
/* Scoped styling if needed */
button {
  transition: all 0.3s ease;
}
</style>
