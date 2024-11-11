<template>
  <div>
    <QuestionDetail
      :question="question"
      :selectedAnswers="selectedAnswers"
      @update-selected-answer="updateSelectedAnswer"
    />
  </div>
</template>

<script setup>
import axios from "axios";
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";

definePageMeta({
  layout: "questions",
});

const question = ref([]);
const selectedAnswers = ref({}); // Lưu trữ câu trả lời đã chọn
const route = useRoute();

const { id } = route.params;

onMounted(async () => {
  try {
    const { data } = await axios.get(
      `http://localhost:5082/api/questionApi/getByQuestionByTest?id=${id}`
    );
    question.value = data;
  } catch (error) {
    console.error("Có lỗi xảy ra khi gọi API:", error);
  }
});

// Cập nhật selectedAnswers khi có câu trả lời mới
const updateSelectedAnswer = (questionId, answerId) => {
  selectedAnswers.value = { ...selectedAnswers.value, [questionId]: answerId };
};
</script>

<style scoped></style>
