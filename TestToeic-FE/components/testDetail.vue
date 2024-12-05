<template>
  <div
    class="relative min-h-screen bg-gray-50 p-6"
    v-for="test in tests"
    :key="test.id"
  >
    <div class="flex justify-end mt-4">
      <button
        @click="submitTest"
        :disabled="isSubmitted"
        class="px-4 py-2 bg-green-500 text-white font-bold rounded hover:bg-green-600 disabled:bg-gray-400 disabled:cursor-not-allowed"
      >
        Nộp Bài
      </button>
    </div>

    <!-- Tiêu đề nằm giữa và to -->
    <div class="text-center mb-8">
      <h1 class="font-extrabold text-4xl text-blue-600">{{ test.title }}</h1>
      <p v-if="!isSubmitted" class="text-gray-500 text-lg">
        Thời gian còn lại: <span class="text-red-500">{{ formattedTime }}</span>
      </p>
    </div>
    <!-- Phân trang -->
    <div class="flex flex-wrap justify-center gap-3 my-6 max-w-2xl mx-auto">
      <button
        v-for="page in totalPagesParent"
        :key="page"
        @click="changePageParent(page)"
        :class="[
          'px-4 py-2 rounded shadow',
          currentPagePrimary === page
            ? 'bg-blue-500 text-white font-bold'
            : 'bg-gray-200 hover:bg-gray-300 text-gray-800',
        ]"
      >
        {{ page }}
      </button>
    </div>
    <!-- Hiển thị các câu hỏi Primary = true trong các thẻ div riêng biệt -->
    <div class="bg-white shadow-md rounded-lg max-w-3xl mx-auto mb-6">
      <ul>
        <li
          v-for="(primaryQuestion, index) in relatedQuestionsForCurrentPage(
            test
          )"
          :key="primaryQuestion.questionId"
          class="mb-6"
        >
          <div class="border p-4 mb-2">
            <p class="font-medium text-lg text-gray-800 mx-4 my-5">
              {{ primaryQuestion.questionContent }}
            </p>
          </div>
        </li>
      </ul>
      <p
        v-if="relatedQuestionsForCurrentPage(test).length === 0"
        class="text-gray-500"
      >
        Không có câu hỏi đặc biệt nào.
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
                :disabled="isSubmitted"
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
        v-for="page in totalPagesChild"
        :key="page"
        @click="changePage(page)"
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

    <div
      v-if="showSubmittedAnswers"
      class="bg-white shadow-md p-6 rounded-lg max-w-3xl mx-auto mt-6"
    >
      <h2 class="text-xl font-bold text-green-500 mb-4">Kết quả bài thi</h2>
      <p
        v-html="showSubmittedAnswers.message"
        class="text-gray-800 whitespace-pre-line"
      ></p>
      <NuxtLink
        :to="`/classes/${showSubmittedAnswers.classSuggestId}`"
        class="text-blue-500 hover:underline"
        >Xem lớp của bạn</NuxtLink
      >
      <p class="text-gray-600 mt-2">
        Tổng điểm: {{ showSubmittedAnswers.points }}<br />
        Số câu trả lời đúng: {{ showSubmittedAnswers.correctAnswers }} /
        {{ showSubmittedAnswers.totalQuestions }}<br />
        Thời gian làm bài: {{ showSubmittedAnswers.time }}
        <NuxtLink
          :to="`/results/${showSubmittedAnswers.detail}`"
          class="text-blue-500 hover:underline"
          >Xem chi tiết kết quả</NuxtLink
        >
      </p>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  props: {
    testId: {
      type: [Number, String],
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
      showSubmittedAnswers: null,
      currentPage: 1, // Trang hiện tại của non-primary questions
      currentPagePrimary: 1, // Trang hiện tại của primary questions
      itemsPerPage: 1, // Số câu hỏi non-primary hiển thị mỗi trang
      itemsPerPagePrimary: 1,
      remainingTime: 0,
      timerInterval: null,
      isSubmitted: false,
    };
  },
  computed: {
    primaryQuestions() {
      return this.tests[0]?.questionDtos.filter((q) => q.primary).length || [];
    },
    totalQuestions() {
      return this.tests.reduce(
        (total, test) =>
          total + test.questionDtos.filter((q) => !q.primary).length,
        0
      );
    },
    totalPagesChild() {
      const parentQuestionIds = this.getCurrentQuestionIds();
      const nonPrimaryQuestions = this.tests[0]?.questionDtos.filter(
        (question) => !question.primary
      );

      const filteredQuestions = nonPrimaryQuestions.filter((question) =>
        parentQuestionIds.includes(question.parentQuestionId)
      );

      return Math.ceil(filteredQuestions.length / this.itemsPerPage);
    },
    totalPagesParent() {
      return Math.ceil(this.primaryQuestions / this.itemsPerPage);
    },
    formattedTime() {
      const minutes = Math.floor(this.remainingTime / 60);
      const seconds = this.remainingTime % 60;
      return `${minutes}:${seconds < 10 ? "0" : ""}${seconds}`;
    },
  },

  methods: {
    formatDuration(seconds) {
      const hours = Math.floor(seconds / 3600);
      const minutes = Math.floor((seconds % 3600) / 60);
      const remainingSeconds = seconds % 60;
      return `${hours.toString().padStart(2, "0")}:${minutes
        .toString()
        .padStart(2, "0")}:${remainingSeconds.toString().padStart(2, "0")}`;
    },
    // Hàm thay đổi trang
    changePage(page) {
      this.currentPage = page;
    },
    changePageParent(page) {
      this.currentPagePrimary = page;
      this.currentPage = 1;
    },

    // Hàm lấy ID câu hỏi trên trang hiện tại
    getCurrentQuestionIds() {
      const test = this.tests[0]; // Chỉ có một bài kiểm tra
      if (!test || !test.questionDtos) return [];

      const primaryQuestions = test.questionDtos.filter(
        (question) => question.primary
      );
      const start = (this.currentPagePrimary - 1) * this.itemsPerPagePrimary;
      const end = start + this.itemsPerPagePrimary;
      const currentPrimaryQuestions = primaryQuestions.slice(start, end);

      return currentPrimaryQuestions.map((question) => question.questionId);
    },
    relatedQuestionsForCurrentPage(test) {
      const PrimaryQuestions = test.questionDtos.filter(
        (question) => question.primary
      );
      const start = (this.currentPagePrimary - 1) * this.itemsPerPagePrimary;
      const end = start + this.itemsPerPagePrimary;
      return PrimaryQuestions.slice(start, end);
    },
    paginatedQuestions(test) {
      if (!test || !test.questionDtos) return [];

      const parentQuestionIds = this.getCurrentQuestionIds();

      const nonPrimaryQuestions = test.questionDtos.filter(
        (question) =>
          !question.primary &&
          parentQuestionIds.includes(question.parentQuestionId)
      );
      const start = (this.currentPage - 1) * this.itemsPerPage;
      return nonPrimaryQuestions.slice(start, start + this.itemsPerPage);
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
      console.log("requestData", requestData);
      axios
        .post("http://localhost:5082/api/studentApi", requestData)
        .then((response) => {
          console.log("Kết quả nộp bài:", response.data);
          this.showSubmittedAnswers = response.data;

          if (this.timerInterval) {
            clearInterval(this.timerInterval);
          }

          this.isSubmitted = true;
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
        console.log("Dữ liệu bài kiểm tra:", this.tests);
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
