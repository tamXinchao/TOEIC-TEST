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
        <!-- Hiển thị câu hỏi phân trang -->
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
  data() {
    return {
      tests: [], // Mảng chứa dữ liệu các bài kiểm tra
      selectedAnswers: {}, // Đối tượng lưu trữ câu trả lời đã chọn
      submittedAnswers: [], // Mảng lưu id câu hỏi và câu trả lời đã chọn
      currentPage: 1, // Trang hiện tại
      itemsPerPage: 1, // Số câu hỏi mỗi trang
      remainingTime: 0, // Thời gian còn lại tính bằng giây
      timerInterval: null, // Biến lưu khoảng thời gian
    };
  },
  computed: {
    // Tổng số câu hỏi trong tất cả bài kiểm tra
    totalQuestions() {
      return this.tests.reduce(
        (total, test) => total + test.questionDtos.length,
        0
      );
    },
    // Tổng số trang câu hỏi
    totalPages() {
      return Math.ceil(this.totalQuestions / this.itemsPerPage);
    },
    // Thời gian định dạng phút:giây
    formattedTime() {
      const minutes = Math.floor(this.remainingTime / 60);
      const seconds = this.remainingTime % 60;
      return `${minutes}:${seconds < 10 ? "0" : ""}${seconds}`;
    },
  },
  methods: {
    // Dữ liệu phân trang cho câu hỏi trong mỗi bài kiểm tra
    paginatedQuestions(test) {
      const allQuestions = test.questionDtos;
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return allQuestions.slice(start, end);
    },
    // Khởi động đếm ngược
    startTimer(durationInMinutes) {
      this.remainingTime = durationInMinutes * 60;

      // Đặt khoảng thời gian đếm ngược mỗi giây
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
      const hours = Math.floor(seconds / 3600); // Tính số giờ
      const minutes = Math.floor((seconds % 3600) / 60); // Tính số phút
      const remainingSeconds = seconds % 60; // Tính số giây còn lại

      // Đảm bảo định dạng là 2 chữ số cho mỗi phần
      return `${hours.toString().padStart(2, "0")}:${minutes
        .toString()
        .padStart(2, "0")}:${remainingSeconds.toString().padStart(2, "0")}`;
    },
    // Phương thức nộp bài
    submitTest() {
      const testId = this.tests[0].id;

      // Lấy thời gian hiện tại và chuyển sang dạng ISO với hậu tố Z (UTC)
      const now = new Date();

      // Lấy thời gian hiện tại theo múi giờ Việt Nam (UTC+7)
      const vietnamTime = new Date(now.getTime() + 7 * 60 * 60 * 1000);

      // Chuyển sang định dạng ISO mà không có phần microseconds
      const currentTime = vietnamTime.toISOString().split(".")[0] + "Z";

      console.log(currentTime); // Kết quả sẽ là: "2024-11-25T11:02:29"

      const StudentName = "1a2b3c4d-5678-90ab-cdef-1234567890ab";

      // Tính toán thời gian đã sử dụng làm bài
      const duration = this.tests[0].testTimeMinutes * 60 - this.remainingTime;
      const formattedDuration = this.formatDuration(duration); // Giả sử formatDuration đã có sẵn

      // Mảng câu trả lời đã chọn
      const questions = this.tests[0].questionDtos.map((question) => {
        const selectedAnswerId =
          this.selectedAnswers[question.questionId] || null; // Nếu chưa chọn đáp án thì gán là null
        return {
          questionId: question.questionId,
          answerId: selectedAnswerId, // Nếu chưa chọn, sẽ là null
        };
      });

      // Cấu trúc dữ liệu gửi lên API
      const requestData = {
        testId: testId,
        applicationUserId: StudentName,
        completion: currentTime, // Đảm bảo là thời gian ISO với hậu tố Z
        duration: formattedDuration, // Đảm bảo là định dạng "00:30:00"
        answerOfStudentDtos: questions, // Câu hỏi và câu trả lời
      };

      console.log("Các câu trả lời đã chọn:", requestData);

      // Gửi dữ liệu lên API
      axios
        .post("http://localhost:5082/api/studentApi", requestData)
        .then((response) => {
          console.log("Kết quả nộp bài:", response.data);
        })
        .catch((error) => {
          console.error("Có lỗi xảy ra khi nộp bài:", error);
        });
    },
  },
  async mounted() {
    try {
      // Gọi API để lấy dữ liệu bài kiểm tra
      const response = await axios.get(`http://localhost:5082/api/testApi/4`);

      // Kiểm tra nếu response.data có tồn tại
      if (response.data && Array.isArray(response.data)) {
        this.tests = response.data; // Gán dữ liệu vào this.tests (tất cả bài kiểm tra)
        console.log("Dữ liệu câu hỏi:", this.tests);
        // Bắt đầu đồng hồ đếm ngược với thời gian của bài kiểm tra đầu tiên
        if (this.tests.length > 0) {
          this.startTimer(this.tests[0].testTimeMinutes);
        }
      } else {
        console.log("Không có dữ liệu câu hỏi trong API response.");
      }
    } catch (error) {
      console.error("Có lỗi xảy ra khi gọi API:", error); // Xử lý lỗi nếu có
    }
  },
  beforeDestroy() {
    // Hủy bộ đếm thời gian khi rời khỏi trang
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
