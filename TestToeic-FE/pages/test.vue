<template>
  <div>
    <h1>Test</h1>
    <hr />
    {{ answerSelected }}
    <div class="flex">
      <div class="flex-1">
        <ul class="flex space-x-4">
          <li v-for="(part, index) in parts" :key="index">
            <button
              @click="getQuestions(part.questionId)"
              class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
            >
              {{ part.labelOfPrimaryQuestion }}
            </button>
          </li>
        </ul>
        <div class="mt-8">
          <ul class="space-y-4">
            <li
              v-for="(question, index) in questionInParts"
              :key="index"
              class="p-4 bg-white rounded-lg shadow-md hover:shadow-lg transition-shadow"
            >
              <div class="flex items-start">
                <span class="font-semibold text-gray-700 mr-2"
                  >{{ index + 1 }}.</span
                >
                <div class="flex-1">
                  <p class="text-gray-800">{{ question.questionContent }}</p>
                  <div class="mt-3 ml-4 space-y-2">
                    <div
                      v-for="(answer, aIndex) in question.answers"
                      :key="aIndex"
                      class="flex items-center"
                    >
                      <input
                        type="radio"
                        :name="'question-' + question.questionId"
                        :id="'answer-' + answer.answerId + '-' + aIndex"
                        :checked="isAnswerSelected(answer.answerId)"
                        @change="
                          selectAnswer(question.questionId, answer.answerId)
                        "
                        class="mr-2"
                      />
                      <label
                        :for="'answer-' + answer.answerId + '-' + aIndex"
                        class="text-gray-600"
                      >
                        {{ isAnswerSelected(answer.answerId) }}
                        {{ answer.answerContent }}
                      </label>
                    </div>
                  </div>
                </div>
              </div>
            </li>
          </ul>
        </div>
      </div>
      <div class="w-1/4">
        <h2>timer</h2>
        <ul class="flex flex-col space-y-4">
          <li v-for="(part, index) in parts" :key="index">
            <h5>
              <strong>{{ part.labelOfPrimaryQuestion }}</strong>
            </h5>
            <ul></ul>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { ref } from "vue";
const { data: tests } = await axios.get(`http://localhost:5082/api/testApi/64`);
const questions = ref([]);
questions.value = tests.testDto.questionDtos;
console.log(tests);

const answerSelected = ref([]);

const parts = ref([]);
const questionInParts = ref([]);

parts.value = questions.value.filter((question) => question.primary === true);

const getQuestions = (partId) => {
  questionInParts.value = questions.value.filter(
    (question) => question.parentQuestionId === partId
  );
};

const selectAnswer = (questionId, answerId) => {
  const existingAnswerIndex = answerSelected.value.findIndex(
    (answer) => answer.questionId === questionId
  );

  if (existingAnswerIndex !== -1) {
    answerSelected.value[existingAnswerIndex].answerId = answerId;
  } else {
    answerSelected.value.push({ questionId, answerId });
  }
};

// Update isAnswerSelected to work with new structure
const isAnswerSelected = (answerId) => {
  return answerSelected.value.some((answer) => answer.answerId === answerId);
};
</script>

<style lang="scss" scoped></style>
