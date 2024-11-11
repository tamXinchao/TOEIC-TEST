<template>
  <NuxtLink
    :to="`/questions/${questions.questionId}`"
    class="text-lg font-semibold text-gray-700"
  >
    <div
      class="bg-gray-100 p-4 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-200"
    >
      <!-- Link đến trang chi tiết câu hỏi -->

      Câu {{ index }}: {{ questions.questionContent }}
    </div>
  </NuxtLink>
</template>

<script setup>
const { questions, index } = defineProps(["questions", "index"]);

// Kiểm tra console xem dữ liệu nhận được có đúng không
console.log(questions);
</script>

<style scoped></style>
