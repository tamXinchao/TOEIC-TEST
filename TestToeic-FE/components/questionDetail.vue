<template>
  <div>
    <div v-for="q in question" :key="q.questionId">
      <p>
        <span class="underline">Câu hỏi:</span>
        <span class="font-bold ml-2">{{ q.questionContent }}</span>
      </p>

      <!-- Lặp qua mảng answers và hiển thị answerContent -->
      <div v-for="(answer, answerIndex) in q.answers" :key="answer.answerId">
        <input
          type="radio"
          :id="'answer-' + answer.answerId"
          :name="'question-' + q.questionId"
          :value="answer.answerId"
          :checked="selectedAnswers[q.questionId] === answer.answerId"
          @change="selectAnswer(q.questionId, answer.answerId)"
        />
        <label :for="'answer-' + answer.answerId">
          {{ String.fromCharCode(65 + answerIndex) }}.
          <span class="ml-2">{{ answer.answerContent }}</span>
        </label>
      </div>
    </div>
  </div>
</template>

<script setup>
import { defineProps, defineEmits } from "vue";

// Nhận câu hỏi và selectedAnswers từ props
const { question, selectedAnswers } = defineProps([
  "question",
  "selectedAnswers",
]);
const emit = defineEmits(["update-selected-answer"]);

// Phát ra sự kiện khi người dùng chọn câu trả lời
const selectAnswer = (questionId, answerId) => {
  emit("update-selected-answer", questionId, answerId);
};
console.log(selectAnswer);
</script>

<style scoped></style>
