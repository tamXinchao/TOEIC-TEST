<template>
  <div class="form-container" v-for="test in tests" :key="test.id">
    <div>
      <div>
        <NuxtLink :to="`/questions/1`"
          >Đề thi: {{ test.title }} Số lượng câu hỏi:
          {{ test.questionDtos.length }}
          Thời gian làm bài: {{ test.testTimeMinutes }} phút
        </NuxtLink>
      </div>
    </div>
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
}
</style>
