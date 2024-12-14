<template>
  <div class="max-w-4xl mx-auto p-6 bg-white shadow-md rounded-lg">
    <!-- <button @click.prevent="openModal">Thêm câu hỏi</button> -->
    <!-- Tiêu đề bài kiểm tra -->
    <div class="max-w-3xl mx-auto p-6">
      <h1 class="text-4xl font-semibold text-gray-800 text-center mb-6">
        {{ testDetail.testDto.title }}
      </h1>

      <!-- Thông tin bài kiểm tra -->
      <div class="grid grid-cols-1 mt-7 sm:grid-cols-2 gap-6">
        <div class="space-y-4">
          <p class="text-lg text-gray-600">
            <span class="font-semibold">Người tạo:</span>
            {{ testDetail.testDto.userCreate }}
          </p>
          <p class="text-lg text-gray-600">
            <span class="font-semibold">Số câu hỏi:</span>
            {{ testDetail.testDto.questionDtos.length }}
          </p>
          <p class="text-lg text-gray-600">
            <span class="font-semibold">Ngày tạo:</span>
            {{ testDetail.testDto.stringDateCreateTest }}
          </p>
        </div>

        <div class="space-y-4">
          <p class="text-lg text-gray-600">
            <span class="font-semibold">Điểm bài kiểm tra:</span>
            {{ testDetail.testDto.point }}
          </p>
          <p class="text-lg text-gray-600">
            <span class="font-semibold">Thời gian làm bài:</span>
            {{ testDetail.testDto.testTimeMinutes }} phút
          </p>
        </div>
      </div>
    </div>

    <!-- Danh sách câu hỏi -->
    <div class="space-y-8">
      <div
        v-for="(group, groupIndex) in groupedQuestions"
        :key="groupIndex"
        class="p-6 bg-gray-100 rounded-lg shadow-md space-y-4"
      >
        <!-- Nhóm câu hỏi chính -->
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-xl font-bold text-blue-600">
            {{ group.primary.labelOfPrimaryQuestion }}
          </h2>
          <button
            @click="deleteGroup(groupIndex)"
            class="px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600"
          >
            Xóa phần
          </button>
        </div>
        <div class="mb-4">
          <input
            v-model="group.primary.labelOfPrimaryQuestion"
            class="w-full p-3 border rounded-lg text-gray-800 mb-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
            placeholder="Nhập nhãn của câu hỏi chính"
          />
          <textarea
            v-model="group.primary.questionContent"
            class="w-full p-3 border rounded-lg mt-2 bg-white resize-none focus:outline-none focus:ring-2 focus:ring-blue-400"
            placeholder="Nhập câu hỏi chính"
            @input="autoResize"
          ></textarea>
        </div>

        <!-- Các câu hỏi phụ -->
        <div class="space-y-6">
          <div
            v-for="(question, questionIndex) in group.questions"
            :key="question.questionId"
            class="p-4 bg-white border rounded-lg shadow-sm"
          >
            <div class="flex justify-between items-center mb-3">
              <input
                v-model="question.questionContent"
                class="flex-grow p-3 border rounded-lg text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-400"
                placeholder="Nhập câu hỏi phụ"
              />
              <input
                v-model="question.pointOfQuestion"
                type="number"
                class="w-24 p-3 border rounded-lg text-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-400"
                placeholder="Điểm"
                min="0"
              />
              <button
                @click="deleteQuestion(groupIndex, questionIndex)"
                class="px-3 py-1 bg-red-500 text-white rounded-lg hover:bg-red-600 ml-4"
              >
                Xóa
              </button>
            </div>

            <!-- Các đáp án -->
            <div class="mt-4 space-y-4">
              <div
                v-for="(answer, answerIndex) in question.answers"
                :key="answer.answerId"
                class="flex items-center space-x-4"
              >
                <input
                  v-model="answer.answerContent"
                  class="flex-grow p-3 border rounded-lg text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-400"
                  placeholder="Nhập đáp án"
                />
                <input
                  v-model="answer.explain"
                  class="flex-grow p-3 border rounded-lg text-gray-600 italic focus:outline-none focus:ring-2 focus:ring-blue-400"
                  placeholder="Giải thích (nếu có)"
                />
                <label class="flex items-center space-x-2">
                  <input
                    type="checkbox"
                    v-model="answer.correct"
                    class="form-checkbox h-5 w-5 text-green-500"
                  />
                  <span class="text-sm">Đúng</span>
                </label>
                <button
                  @click="deleteAnswer(groupIndex, questionIndex, answerIndex)"
                  class="px-3 py-1 bg-red-500 text-white rounded-lg hover:bg-red-600"
                >
                  Xóa
                </button>
              </div>
            </div>

            <!-- Nút thêm đáp án -->
            <div class="mt-4">
              <button
                @click="addAnswer(groupIndex, questionIndex)"
                class="px-4 py-2 bg-orange-500 text-white rounded-lg hover:bg-orange-600"
              >
                Thêm đáp án
              </button>
            </div>
          </div>
        </div>

        <!-- Nút thêm câu hỏi -->
        <div class="mt-4 text-right">
          <button
            @click="addQuestion(group.groupOfQuestion)"
            class="px-4 py-2 bg-green-500 text-white rounded-lg hover:bg-green-600"
          >
            Thêm câu hỏi
          </button>
        </div>
      </div>

      <!-- Nút thêm nhóm và lưu -->
      <div class="flex justify-between mt-8">
        <button
          @click="addGroup"
          class="px-4 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600"
        >
          Thêm phần
        </button>
        <button
          @click="save"
          class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
        >
          Lưu bài kiểm tra
        </button>
      </div>
    </div>

    <div
      v-if="isModalOpen"
      class="fixed inset-0 bg-gray-500 bg-opacity-75 z-50 flex justify-center items-center w-full h-full max-h-full"
    >
      <div class="relative p-3 w-full max-w-4xl h-full">
        <!-- Modal content -->
        <div
          class="relative bg-white rounded-lg shadow dark:bg-gray-800 overflow-hidden flex flex-col max-h-screen"
        >
          <!-- Modal header -->
          <div
            class="flex items-center justify-between p-4 border-b dark:border-gray-600 bg-gray-100 dark:bg-gray-700"
          >
            <h3 class="text-lg font-semibold text-gray-900 dark:text-white">
              {{ isEditMode ? "Chỉnh sửa" : "Thêm câu hỏi mới" }}
            </h3>
            <button
              type="button"
              @click="closeModal"
              class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
              aria-label="Close"
            >
              <svg
                class="w-5 h-5"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M6 18L18 6M6 6l12 12"
                />
              </svg>
            </button>
          </div>

          <!-- Modal body -->
          <div class="p-4 overflow-y-auto flex-grow">
            <form>
              <!-- Form Fields Here -->
            </form>
          </div>

          <!-- Modal footer -->
          <div
            class="flex justify-end p-4 border-t dark:border-gray-600 bg-gray-100 dark:bg-gray-700"
          >
            <button
              @click.prevent="isEditMode ? edit() : add()"
              class="px-4 py-2 text-white bg-blue-600 rounded-lg hover:bg-blue-700 focus:ring-2 focus:ring-blue-400"
            >
              {{ isEditMode ? "Lưu chỉnh sửa" : "Thêm mới" }}
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { ref } from "vue";
export default {
  props: {
    TestDetail: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      testDetail: this.TestDetail,
      textareaHeight: "auto",
      testId: this.TestDetail.testDto.id,
      isEditMode: ref(false),
      isModalOpen: ref(false),
      typeOfQuestionInput: ref(true),
      parentOfQuestionInput: ref(""),
    };
  },

  computed: {
    groupedQuestions() {
      const questions = this.testDetail.testDto.questionDtos;
      const groups = questions.reduce((acc, question) => {
        const groupId = question.groupOfQuestion;

        if (!acc[groupId]) {
          acc[groupId] = {
            groupOfQuestion: groupId,
            primary: null,
            questions: [],
          };
        }

        if (question.primary) {
          acc[groupId].primary = question;
        } else {
          acc[groupId].questions.push(question);
        }

        return acc;
      }, {});

      return Object.values(groups);
    },
    isMainQuestion() {
      return this.typeOfQuestionInput === true;
    },
  },

  methods: {
    autoResize(event) {
      const textarea = event.target;
      textarea.style.height = "auto";
      textarea.style.height = `${textarea.scrollHeight}px`;
    },
    addGroup() {
      // Tạo một phần mới
      this.testDetail.testDto.questionDtos.push({
        labelOfPrimaryQuestion: "", // Nhãn của câu hỏi chính
        groupOfQuestion: `${Math.floor(1000 + Math.random() * 9000)}`, // ID nhóm
        primary: true, // Đây là câu hỏi chính
        new: true,
        questionContent: "",
        labelOfPrimaryQuestion: "",
        multipleAnswer: false,
        pointOfQuestion: 0,
        parentQuestionId: null,
      });
    },
    addQuestion(groupOfQuestion) {
      // Thêm một câu hỏi phụ vào nhóm
      this.testDetail.testDto.questionDtos.push({
        groupOfQuestion: groupOfQuestion,
        new: true,
        primary: false,
        multipleAnswer: false,
        parentQuestionId: null,
        questionContent: "",
        answers: [],
        pointOfQuestion: 0,
      });
    },
    addAnswer(groupIndex, questionIndex) {
      const group = this.groupedQuestions[groupIndex];
      if (group && group.questions[questionIndex]) {
        group.questions[questionIndex].answers.push({
          new: true,
          answerContent: "",
          correct: false,
          explain: "",
        });
      }
    },
    deleteGroup(groupIndex) {
      const group = this.groupedQuestions[groupIndex];
      if (group) {
        const groupOfQuestion = group.groupOfQuestion;
        const isConfirm = confirm("Bạn có chắc chắn muốn xóa câu hỏi này?");
        if (!confirm) return;
        if (isConfirm) {
          if (group.primary.new) {
            console.log(group.primary.new);
            this.testDetail.testDto.questionDtos =
              this.testDetail.testDto.questionDtos.filter(
                (question) => question.groupOfQuestion !== groupOfQuestion
              );
            return;
          }
          axios
            .delete(
              `http://localhost:5082/api/questionApi/${groupOfQuestion}?testId=${this.testId}`
            )
            .then(
              (res) => {
                console.log(res.data);
                alert(res.data.message);
                this.testDetail.testDto.questionDtos =
                  this.testDetail.testDto.questionDtos.filter(
                    (question) => question.groupOfQuestion !== groupOfQuestion
                  );
              },
              (err) => {
                console.error(err);
              }
            );
        }
      }
    },
    deleteQuestion(groupIndex, questionIndex) {
      const group = this.groupedQuestions[groupIndex];
      const question = group.questions[questionIndex];
      const questionId = question.questionId;
      const isConfirm = confirm("Bạn có chắc chắn muốn xóa câu hỏi này?");
      if (isConfirm) {
        if (group.questions[questionIndex].new) {
          this.testDetail.testDto.questionDtos =
            this.testDetail.testDto.questionDtos.filter((q) => q !== question);
          // Xóa câu hỏi khỏi nhóm hiện tại
          group.questions.splice(questionIndex, 1);
          return;
        }
        axios
          .delete(
            `http://localhost:5082/api/questionApi/${questionId}?testId=${this.testId}`
          )
          .then(
            (res) => {
              console.log(res.data);
              alert(res.data.message);
              this.testDetail.testDto.questionDtos =
                this.testDetail.testDto.questionDtos.filter(
                  (q) => q.questionId !== questionId
                );
            },
            (err) => {
              console.error(err);
            }
          );
      }
    },

    deleteAnswer(groupIndex, questionIndex, answerIndex) {
      const group = this.groupedQuestions[groupIndex];
      if (group && group.questions[questionIndex]) {
        const question = group.questions[questionIndex];
        if (question.answers[answerIndex]) {
          console.log(group.questions);
          const isConfirm = confirm("Bạn có chắc chắn muốn xóa đáp án này?");
          if (isConfirm) {
            axios.delete(
              `http://localhost:5082/api/answerApi?id=${question.answers[answerIndex].answerId}`
            );
            question.answers.splice(answerIndex, 1);
          }
        }
      }
    },
    save() {
      // Gửi dữ liệu lên server

      console.log(
        JSON.parse(JSON.stringify(this.testDetail.testDto.questionDtos))
      );
      const request = {
        questionDtos: JSON.stringify(this.testDetail.testDto.questionDtos),
      };
      console.log(request);
      axios
        .post(
          `http://localhost:5082/api/questionApi?testId=${this.testId}`,
          this.testDetail.testDto.questionDtos
        )
        .then((res) => {
          console.log(res.data);
          alert(res.data.message);
        })
        .catch((err) => {
          console.error(err);

          if (err.response) {
            alert(err.response.data.message);
          } else {
            alert("Có lỗi xảy ra. Vui lòng thử lại sau.");
          }
        });
    },
    openModal(classToEdit = null) {
      this.isModalOpen = true;
    },
    closeModal() {
      this.isModalOpen = false;
    },
  },
};
</script>

<style scoped>
.container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}
button {
  cursor: pointer;
}
/* Custom Styles */
.text-gray-600 {
  color: #4a5568;
}
.text-gray-800 {
  color: #2d3748;
}
.bg-gray-100 {
  background-color: #f7fafc;
}
.bg-white {
  background-color: white;
}
.bg-blue-600 {
  background-color: #3182ce;
}
.bg-orange-500 {
  background-color: #f6ad55;
}
.bg-red-500 {
  background-color: #f56565;
}
.bg-green-500 {
  background-color: #48bb78;
}
.bg-opacity-75 {
  background-color: rgba(0, 0, 0, 0.75);
}
.shadow-md {
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}
.shadow-sm {
  box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
}
.text-sm {
  font-size: 0.875rem;
}
.text-lg {
  font-size: 1.125rem;
}
.font-semibold {
  font-weight: 600;
}
</style>
