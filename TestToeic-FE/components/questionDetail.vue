<!-- QuestionDetail.vue -->
<template>
  <div>
    <div>hello</div>
    <div>Kết quả khi lấy api: {{ question[0].id }}</div>
  </div>
</template>

<script setup>
const { question } = defineProps(["question"]);
</script>
<style scoped></style>
