<template>
  <div>
    <div class="grid grid-cols-3 gap-4">
      <div v-for="q in questions">
        <QuestionList :questions="q" />
      </div>
    </div>
  </div>
  <div>
    <slot />
  </div>
</template>
<script setup>
definePageMeta({
  layout: "questions",
});
import axios from "axios";
const { data: questions } = await axios.get(
  `http://localhost:5082/api/testApi/1`
);
</script>

<style scoped></style>
