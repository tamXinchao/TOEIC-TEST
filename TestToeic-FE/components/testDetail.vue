<template>
  <div
    class="relative min-h-screen bg-gray-50 p-6"
    v-for="test in tests"
    :key="test.id"
  >
    <button
      @click="submitTest"
      class="px-4 py-2 bg-green-500 text-white font-bold rounded hover:bg-green-600"
    >
      Nộp Bài
    </button>

    <!-- Tiêu đề nằm giữa và to -->
    <div class="text-center mb-8">
      <h1 class="font-extrabold text-4xl text-blue-600">{{ test.title }}</h1>
      <p class="text-gray-500 text-lg">
        Thời gian còn lại: <span class="text-red-500">{{ formattedTime }}</span>
      </p>
    </div>

    <!-- Hiển thị dữ liệu phân trang -->
    <div class="bg-white shadow-md p-6 rounded-lg max-w-3xl mx-auto mb-6">
      <ul>
        <li
          v-for="(question, questionIndex) in paginatedQuestions(test)"
          :key="question.questionId"
          class="mb-6"
        >
          <p class="font-medium text-lg text-gray-800 mb-2">
            {{ question.questionContent }}
          </p>
          <div
            v-for="(answer, answerIndex) in question.answers"
            :key="answer.answerId"
            class="mb-2"
          >
            <label class="flex items-center space-x-3">
              <input
                type="radio"
                :id="'answer-' + question.questionId + '-' + answer.answerId"
                :name="'question-' + question.questionId"
                :value="answer.answerId"
                v-model="selectedAnswers[question.questionId]"
                class="form-radio h-5 w-5 text-blue-500"
              />
              <span :class="['text-gray-700']">
                {{ String.fromCharCode(65 + answerIndex) }}.
                {{ answer.answerContent }}
              </span>
            </label>
          </div>
        </li>
      </ul>
    </div>

    <!-- Phân trang -->
    <div class="flex flex-wrap justify-center gap-3 mt-6 max-w-2xl mx-auto">
      <button
        v-for="page in totalPages"
        :key="page"
        @click="currentPage = page"
        :class="[
          'px-4 py-2 rounded shadow',
          currentPage === page
            ? 'bg-blue-500 text-white font-bold'
            : 'bg-gray-200 hover:bg-gray-300 text-gray-800',
        ]"
      >
        {{ page }}
      </button>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  props: {
    testId: {
      type: Number,
      required: true,
    },
    userId: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      tests: [],
      selectedAnswers: {},
      submittedAnswers: [],
      currentPage: 1,
      itemsPerPage: 1,
      remainingTime: 0,
      timerInterval: null,
    };
  },
  computed: {
    totalQuestions() {
      return this.tests.reduce(
        (total, test) => total + test.questionDtos.length,
        0
      );
    },
    totalPages() {
      return Math.ceil(this.totalQuestions / this.itemsPerPage);
    },
    formattedTime() {
      const minutes = Math.floor(this.remainingTime / 60);
      const seconds = this.remainingTime % 60;
      return `${minutes}:${seconds < 10 ? "0" : ""}${seconds}`;
    },
  },
  methods: {
    paginatedQuestions(test) {
      const allQuestions = test.questionDtos;
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return allQuestions.slice(start, end);
    },
    startTimer(durationInMinutes) {
      this.remainingTime = durationInMinutes * 60;
      this.timerInterval = setInterval(() => {
        if (this.remainingTime > 0) {
          this.remainingTime -= 1;
        } else {
          clearInterval(this.timerInterval);
          alert("Hết thời gian làm bài!");
        }
      }, 1000);
    },
    formatDuration(seconds) {
      const hours = Math.floor(seconds / 3600);
      const minutes = Math.floor((seconds % 3600) / 60);
      const remainingSeconds = seconds % 60;
      return `${hours.toString().padStart(2, "0")}:${minutes
        .toString()
        .padStart(2, "0")}:${remainingSeconds.toString().padStart(2, "0")}`;
    },
    submitTest() {
      const testId = this.testId;
      const now = new Date();
      const vietnamTime = new Date(now.getTime() + 7 * 60 * 60 * 1000);
      const currentTime = vietnamTime.toISOString().split(".")[0] + "Z";

      const questions = this.tests[0].questionDtos.map((question) => {
        const selectedAnswerId =
          this.selectedAnswers[question.questionId] || null;
        return {
          questionId: question.questionId,
          answerId: selectedAnswerId,
        };
      });

      const requestData = {
        testId: testId,
        applicationUserId: this.userId,
        completion: currentTime,
        duration: this.formatDuration(
          this.tests[0].testTimeMinutes * 60 - this.remainingTime
        ),
        answerOfStudentDtos: questions,
      };

      axios
        .post("http://localhost:5082/api/studentApi", requestData)
        .then((response) => {
          console.log("Kết quả nộp bài:", response.data);
          alert("Điểm của bạn là: " + response.data);
        })
        .catch((error) => {
          console.error("Có lỗi xảy ra khi nộp bài:", error);
        });
    },
  },
  async mounted() {
    try {
      const response = await axios.get(
        `http://localhost:5082/api/testApi/${this.testId}`
      );
      if (response.data && Array.isArray(response.data)) {
        this.tests = response.data;
        if (this.tests.length > 0) {
          this.startTimer(this.tests[0].testTimeMinutes);
        }
      }
    } catch (error) {
      console.error("Có lỗi xảy ra khi gọi API:", error);
    }
  },
  beforeDestroy() {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }
  },
};
</script>

<style scoped>
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
  margin-top: 20px;
}
.btn {
  padding: 5px 10px;
  background-color: #007bff;
  color: #fff;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}
.btn.active {
  background-color: #ed05bb;
  font-weight: bold;
}
.btn:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}
</style>
