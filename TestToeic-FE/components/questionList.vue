<template>
  <div
    v-for="(question, index) in questions.questions"
    :key="question.questionId"
  >
    <NuxtLink :to="`/questions/${question.questionId}`">
      <p class="btn mt-4">{{ question.questionId }}</p>
    </NuxtLink>
  </div>
</template>

<script setup>
const { questions } = defineProps(["questions"]);
console.log(questions);
</script>

<style scoped></style>
