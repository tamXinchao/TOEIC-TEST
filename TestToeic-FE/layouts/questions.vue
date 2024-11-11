<template>
  <div class="bg-gray-50 min-h-screen">
    <!-- Header -->
    <header class="shadow-sm bg-white">
      <nav class="container mx-auto p-4 flex justify-between items-center">
        <NuxtLink to="/" class="font-bold">Thi đầu vào Toeic</NuxtLink>
        <ul class="flex gap-6">
          <li>
            <NuxtLink
              to="/"
              class="text-blue-500 hover:text-blue-700 transition duration-200"
              >Home</NuxtLink
            >
          </li>
          <li>
            <NuxtLink
              to="/questions"
              class="text-gray-700 hover:text-gray-900 transition duration-200"
            >
              Làm bài thi thử
            </NuxtLink>
          </li>
        </ul>
      </nav>
    </header>

    <!-- Nội dung chính -->
    <div class="container mx-auto p-6 grid grid-cols-1 md:grid-cols-3 gap-8">
      <div class="grid grid-cols-3 gap-4">
        <div v-for="(q, index) in questions">
          <!-- Truyền index + 1 vào component con -->
          <QuestionList :questions="q" :index="index + 1" />
        </div>
      </div>
    </div>
    <main>
      <div class="container mx-auto p-6">
        <slot />
      </div>
    </main>
    <!-- Nội dung của các trang con -->
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

<style scoped>
/* Thêm kiểu CSS cho các link đang active */
.router-link-exact-active {
  color: green;
  font-weight: bold;
}
</style>
