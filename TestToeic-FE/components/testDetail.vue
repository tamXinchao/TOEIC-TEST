<template>
  <div class="relative">
    <!-- Kiểm tra nếu test có dữ liệu -->
    <div v-if="tests">
      <h2 class="font-bold text-2xl mb-4">{{ tests.title }}</h2>
      <p class="mb-2">Số lượng câu hỏi: {{ tests.questionDtos.length }}</p>
      <p class="mb-4">Thời gian làm bài: {{ tests.testTimeMinutes }} phút</p>

      <div>
        <h3 class="font-bold text-xl mb-2">Danh sách câu hỏi:</h3>
        <ul>
          <li v-for="(question, index) in tests.questionDtos" :key="index">
            <NuxtLink
              :to="`/tests/${tests.id}/question/${question.questionId}`"
              class="block p-4 mb-2 rounded-lg bg-gray-100 hover:bg-blue-500 hover:text-white transition-all duration-300 transform hover:scale-105 cursor-pointer"
            >
              Câu {{ index + 1 }}: {{ question.questionContent }} ({{
                question.pointOfQuestion
              }}
              điểm)
            </NuxtLink>
          </li>
        </ul>
      </div>
    </div>

    <!-- Hiển thị thông báo nếu test không có dữ liệu -->
    <div v-else>
      <p>Không tìm thấy bài thi.</p>
    </div>
  </div>
</template>

<script setup>
import { defineProps } from "vue";

// Định nghĩa props
const props = defineProps({
  tests: {
    type: Object,
    required: true,
  },
});

// Log để kiểm tra dữ liệu nhận được
console.log("Tests received in TestDetail:", props.tests);
</script>

<style scoped>
/* Các kiểu tùy chỉnh có thể thêm vào ở đây */
</style>
