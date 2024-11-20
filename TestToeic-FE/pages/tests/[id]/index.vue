<template>
  <div>
    <!-- Truyền dữ liệu xuống TestDetail -->
    <TestDetail :tests="tests" />
    <!-- Hiển thị nội dung của các trang con -->
    <div v-if="loading" class="text-center mt-4">Đang tải dữ liệu...</div>
    <NuxtPage />
  </div>
</template>

<script setup>
import axios from "axios";
import { ref } from "vue";
import { useRoute } from "vue-router";
const route = useRoute();
const { id } = route.params;

// Khởi tạo biến reactive
const tests = ref([]);

// Gọi API để lấy dữ liệu bài thi
try {
  const { data } = await axios.get(
    `http://localhost:5082/api/testApi/list?id=${id}`
  );
  tests.value = data;
} catch (error) {
  console.error("Error fetching test details:", error);
}
</script>
