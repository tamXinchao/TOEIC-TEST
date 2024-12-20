<template>
  <div
    class="p-6 bg-white rounded-lg shadow-md hover:shadow-lg transition-shadow"
  >
    <!-- Thông tin tổng quan -->
    <div class="mb-4">
      <h2 class="text-xl font-bold text-gray-800">
        Đề bài kiểm tra: {{ test.title }}
        <span class="text-gray-500">#{{ test.id }}</span>
      </h2>
    </div>

    <!-- Số câu hỏi -->
    <div class="mb-4 flex items-center text-lg">
      <span class="font-semibold text-gray-800 mr-2">Số câu hỏi:</span>
      <span class="text-blue-500">
        {{ test.questionDtos.filter((question) => !question.primary).length }}
      </span>
    </div>

    <!-- Số phần -->
    <div class="mb-4 flex items-center text-lg">
      <span class="font-semibold text-gray-800 mr-2">Gồm:</span>
      <span class="text-blue-500">
        {{ test.questionDtos.filter((question) => question.primary).length }}
        phần
      </span>
    </div>

    <!-- Thời gian làm bài -->
    <div class="mb-4 flex items-center text-lg">
      <span class="font-semibold text-gray-800 mr-2">Thời gian:</span>
      <span class="text-blue-500">{{ test.testTimeMinutes }} phút</span>
    </div>

    <!-- Ngày tạo bài thi -->
    <div class="mb-4 flex items-center text-lg">
      <span class="font-semibold text-gray-800 mr-2">Ngày tạo bài thi:</span>
      <span class="text-blue-500">{{ test.stringDateCreateTest }}</span>
    </div>

    <!-- Nhãn của bài thi -->
    <div class="mb-4">
      <div class="text-lg font-semibold text-gray-800 mb-2">
        Nhãn của bài thi:
      </div>
      <div class="flex flex-wrap gap-2">
        <div
          v-for="sticker in test.stickers"
          :key="sticker.stickerId"
          class="bg-gray-200 text-gray-700 px-3 py-1 rounded-full hover:bg-blue-500 hover:text-white transition"
        >
          #{{ sticker.stickerName }}
        </div>
      </div>
    </div>
    <div class="mt-6 text-center">
      <NuxtLink
        :to="path"
        class="bg-green-500 text-white px-4 py-2 rounded-lg hover:bg-green-600 transition-colors"
      >
        Bắt đầu làm bài thi
      </NuxtLink>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
const { $auth } = useNuxtApp();
$auth.checkAuth();
const test = ref(null);
const route = useRoute();
const path = ref(null);
if ($auth.checkAuth()) {
  path.value = `/forms/${$auth.user.value}/test/${route.params.id}`;
} else {
  path.value = "/forms/";
}

const response = await axios.get(
  `http://localhost:5082/api/testApi/${route.params.id}`
);
test.value = response.data.testDto;
</script>

<style lang="scss" scoped></style>
