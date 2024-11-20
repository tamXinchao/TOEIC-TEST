<template>
  <div></div>
</template>

<script setup>
import axios from "axios";
// Khởi tạo biến reactive để lưu câu hỏi

const response = await axios.get(
  "http://localhost:5082/api/questionApi/getQuestion"
);
</script>

<style scoped></style>
