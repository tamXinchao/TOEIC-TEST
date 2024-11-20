<template>
  <div v-for="test in tests" :key="test.id">
    <h1>{{ test.title }}</h1>
    <h2>Thời gian làm bài: {{ test.testTimeMinutes }} Phút</h2>

    <ul>
      <!-- Lặp qua từng câu hỏi trong mảng questions -->
      <li
        v-for="(question, questionIndex) in test.questionDtos"
        :key="question.questionId"
      >
        <p>
          Câu {{ questionIndex + 1 }}: {{ question.questionContent }} ({{
            "Điểm của câu hỏi nếu trả lời đúng: " + question.pointOfQuestion
          }})
        </p>
        <!-- Lặp qua từng câu trả lời trong mảng answers -->
        <div
          v-for="(answer, answerIndex) in question.answers"
          :key="answer.answerId"
        >
          <input
            type="radio"
            :id="'answer-' + question.questionId + '-' + answer.answerId"
            :name="'question-' + question.questionId"
            :value="answer.answerId"
            v-model="selectedAnswers[question.questionId]"
          />
          <label :for="'answer-' + question.questionId + '-' + answer.answerId">
            {{ String.fromCharCode(65 + answerIndex) }}.
            {{ answer.answerContent }} - ({{ answer.correct ? "Đúng" : "Sai" }})
            (Giải thích: {{ answer.explain }})
          </label>
        </div>
      </li>
    </ul>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      tests: [], // Mảng chứa dữ liệu các bài kiểm tra
      selectedAnswers: {}, // Đối tượng lưu trữ câu trả lời đã chọn
    };
  },
  async mounted() {
    try {
      // Gọi API để lấy dữ liệu bài kiểm tra
      const response = await axios.get(`http://localhost:5082/api/testApi/4`);
      this.tests = response.data; // Lưu dữ liệu bài kiểm tra vào mảng tests
      console.log(this.tests); // In ra kết quả nhận được từ API
    } catch (error) {
      console.error("Có lỗi xảy ra khi gọi API:", error); // Xử lý lỗi nếu có
    }
  },
};
</script>

<style scoped>
/* Có thể thêm CSS tại đây nếu cần thiết */
</style>
