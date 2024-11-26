<template>
  <div>
    <div v-if="question">
      <p>
        Câu hỏi: {{ question.questionContent }} ({{
          question.pointOfQuestion
        }}
        điểm)
      </p>
      <ul>
        <li v-for="(answer, index) in question.answers" :key="answer.answerId">
          <input
            type="radio"
            :id="'answer-' + question.questionId + '-' + answer.answerId"
            :name="'question-' + question.questionId"
            :value="answer.answerId"
          />
          <label :for="'answer-' + question.questionId + '-' + answer.answerId">
            {{ String.fromCharCode(65 + index) }}. {{ answer.answerContent }}
          </label>
        </li>
      </ul>
    </div>
    <div v-else>
      <p>Đang tải câu hỏi...</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import axios from "axios";

// Khai báo biến phản ứng cho câu hỏi
const question = ref(null);

// Lấy ID câu hỏi từ URL params
const route = useRoute();
const { questionId } = route.params;

// Lấy dữ liệu câu hỏi từ API khi component được mount
onMounted(async () => {
  try {
    const { data } = await axios.get(
      `http://localhost:5082/api/questionApi/getQuestion?id=${questionId}`
    );
    question.value = data[0];
    console.log("Câu hỏi nhận được:", question.value);
  } catch (error) {
    console.error("Có lỗi khi lấy câu hỏi:", error);
  }
});
</script>

<style scoped></style>
