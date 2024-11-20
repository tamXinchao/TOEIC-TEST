<template>
  <div class="form-container" v-for="test in tests" :key="test.id">
    <NuxtLink :to="`/tests/${test.id}`" class="block">
      <div><span class="font-bold">Đề thi:</span> {{ test.title }}</div>
      <div>Số lượng câu hỏi: {{ test.questionDtos.length }}</div>
      <div>Thời gian làm bài: {{ test.testTimeMinutes }} phút</div>
    </NuxtLink>
  </div>
</template>

<script setup>
import axios from "axios";

const { data: tests } = await axios.get(
  `http://localhost:5082/api/testApi/list`
);
</script>

<style scoped>
.form-container {
  max-width: 400px;
  margin: 10px auto;
  padding: 20px;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  background-color: #f9fafb;
  transition: transform 0.2s ease;
}

.form-container:hover {
  transform: scale(1.03);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
</style>
