<template>
  <div class="max-w-4xl mx-auto bg-white p-6 rounded-lg shadow-lg">
    <h2 class="text-2xl font-semibold mb-4 text-center">Thông tin Bài Thi</h2>

    <!-- Hiển thị thông tin bài thi -->
    <div class="space-y-4">
      <div class="flex justify-between">
        <div class="text-gray-700 font-medium">Tên bài thi:</div>
        <div class="text-gray-900">
          <span class="font-bold text-xl">{{ result.title }}</span>
        </div>
      </div>
      <div class="flex justify-between">
        <div class="text-gray-700 font-medium">Sinh viên:</div>
        <div class="text-gray-900">
          <span class="font-bold text-xl">{{ result.studentName }}</span>
        </div>
      </div>
      <div class="flex justify-between">
        <div class="text-gray-700 font-medium">Điểm bài thi:</div>
        <div class="text-gray-900">
          <span class="tracking-wide text-3xl font-bold italic text-red-600">{{
            result.pointOfStudent
          }}</span>
        </div>
      </div>
      <div class="flex justify-between">
        <div class="text-gray-700 font-medium">Thời gian làm bài:</div>
        <div class="text-gray-900">
          <span class="font-bold text-xl">{{ result.duration }}</span>
        </div>
      </div>
      <div class="flex justify-between">
        <div class="text-gray-700 font-medium">Thời gian hoàn thành:</div>
        <div class="text-gray-900">
          <span class="italic">{{ result.completionString }}</span>
        </div>
      </div>
      <div class="flex justify-end mt-4">
        <button
          @click="exportResult"
          class="px-4 py-2 bg-green-500 text-white font-bold rounded hover:bg-green-600 disabled:bg-gray-400 disabled:cursor-not-allowed"
        >
          Xuất file PDF
        </button>
      </div>
    </div>

    <h3 class="text-xl font-semibold mt-6">Danh sách câu hỏi</h3>
    <!-- Hiển thị danh sách câu hỏi -->
    <div class="space-y-4 mt-4">
      <div
        v-for="(question, index) in result.questions"
        :key="index"
        class="border p-4 rounded-lg shadow-sm bg-gray-50"
      >
        <div class="font-medium text-gray-800 mb-2">
          Câu hỏi {{ index + 1 }}: {{ question.questionContent }}
        </div>
        <div class="text-gray-600 mb-2">
          Điểm: {{ question.pointOfQuestion }}
        </div>

        <!-- Hiển thị các đáp án -->
        <div class="space-y-2">
          <div
            v-for="(answer, answerIndex) in question.answers"
            :key="answerIndex"
            class="p-2 bg-gray-100 rounded-md"
          >
            <div class="flex justify-between items-center">
              <div class="text-gray-900">
                Đã chọn:
                <span class="italic text-xl">{{ answer.answerContent }}</span>
              </div>
              <div
                :class="{
                  'text-gray-500': answer.answerId === -1,
                  'text-green-600': answer.correct && answer.answerId !== null,
                  'text-red-600': !answer.correct && answer.answerId !== null,
                }"
              >
                {{
                  answer.answerId === null
                    ? "Bỏ trống"
                    : answer.correct
                    ? "Đúng"
                    : "Sai"
                }}
              </div>
            </div>
            <p class="text-gray-500 text-sm mt-1">
              Giải thích: {{ answer.explain }}
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
const { result } = defineProps(["result"]);
// function exportResult() {
//   try {
//     // Gửi yêu cầu API để lấy PDF
//     const response = axios.get(
//       `http://localhost:5082/api/studentApi/${result[0].id}/exportPdf`,
//       {
//         responseType: "blob", // Đảm bảo là file PDF
//       }
//     );

//     // Tạo file PDF và tải xuống
//     const url = window.URL.createObjectURL(new Blob([response.data]));
//     const link = document.createElement("a");
//     link.href = url;
//     link.setAttribute(
//       "download",
//       `Result-student-${result[0].studentName}.pdf`
//     );
//     document.body.appendChild(link);
//     link.click();
//   } catch (error) {
//     console.error("Có lỗi khi xuất file PDF", error);
//   }
// }
function exportResult() {
  const response = axios.get(`http://localhost:5082/api/studentApi/export`);
}
</script>

<style scoped></style>
