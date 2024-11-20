<template>
  <div>
    <!-- Truyền `tests` xuống TestDetail -->
    <TestDetail :tests="tests" />
  </div>
  <div class="container mx-auto p-4">
    <!-- Dùng slot để hiển thị nội dung của các trang con -->
    <slot />
  </div>
</template>

<script setup>
import axios from "axios";
import { ref } from "vue";
import { useRoute } from "vue-router";

definePageMeta({
  layout: "tests",
});

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
