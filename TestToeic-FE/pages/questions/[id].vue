<template>
  <div>
    <QuestionDetail :question="question" />
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import axios from "axios";

definePageMeta({
  layout: "questions",
});

const question = ref([]);
const route = useRoute();
const { id } = route.params;

onMounted(async () => {
  try {
    const { data: question } = await axios.get(
      `http://localhost:5082/api/questionApi/getQuestion?id=${id}`
    );
    question.value = data;
  } catch (error) {
    console.error("Error fetching question details:", error);
  }
});
</script>
