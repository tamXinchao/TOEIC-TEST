<template>
  <div>
    <div class="grid grid-cols-3 gap-4">
      <div v-for="(q, index) in questions" :key="index">
        <!-- Truyền index + 1 vào component con -->
        <QuestionList :questions="q" :index="index + 1" />
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
// Khởi tạo biến reactive để lưu câu hỏi
const questions = ref([]);

// Gọi API khi component được mounted
onMounted(async () => {
  try {
    const response = await axios.get(
      "http://localhost:5082/api/questionApi/getByQuestionByTest"
    );
    questions.value = response.data; // Gán dữ liệu API vào biến questions
  } catch (error) {
    console.error("Error fetching questions:", error);
  }
});
</script>

<style scoped></style>
