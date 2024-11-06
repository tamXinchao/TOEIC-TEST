<template>
  <div>
    <QuestionDetail :question="question" />
  </div>
</template>
<script setup>
definePageMeta({
  layout: "questions",
});
import axios from "axios";
const { id } = useRoute().params;
console.log("Lấy Id: " + id);
const { data: question } = await axios.get(
  `http://localhost:5082/api/testApi/${id}`,
  { key: id }
);
console.log(question);
</script>

<style scoped></style>
