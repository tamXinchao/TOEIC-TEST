<template>
  <div v-for="test in tests" :key="test.questionId">
    <h1>{{ test.questionContent }}</h1>
    <ul>
      <!-- Lặp qua từng câu trả lời trong mảng answers -->
      <li v-for="(answer, index) in test.answers" :key="answer.answerId">
        <input
          type="radio"
          :id="'answer-' + test.questionId + '-' + answer.answerId"
          :name="'question-' + test.questionId"
          :value="answer.answerId"
          v-model="selectedAnswers[test.questionId]"
        />
        <label :for="'answer-' + test.questionId + '-' + answer.answerId">
          {{ index + 1 }}. {{ answer.answerContent }} (Giải thích:
          {{ answer.explain }})
        </label>
      </li>
    </ul>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      tests: [],
      selectedAnswers: {}, // Khởi tạo một đối tượng để lưu trữ câu trả lời đã chọn
    };
  },
  async mounted() {
    try {
      const response = await axios.get(
        "http://localhost:5082/api/questionApi/getByQuestion"
      );
      this.tests = response.data;
      console.log(this.tests);
    } catch (error) {
      console.error("Có lỗi xảy ra khi gọi API:", error);
    }
  },
};
</script>
