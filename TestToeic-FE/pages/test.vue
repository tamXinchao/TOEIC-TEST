<template>
  <div class="p-6 bg-gray-50 rounded-lg shadow-md max-w-4xl mx-auto">
    <!-- Select lớp -->
    <div class="mb-6">
      <label
        for="class-select"
        class="block text-lg font-semibold text-gray-800 mb-2"
      >
        Chọn lớp:
      </label>
      <select
        id="class-select"
        v-model="classIdModal"
        class="block w-full px-4 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 bg-white"
      >
        <option disabled value="">Chọn một lớp</option>
        <option
          v-for="classs in classesModal"
          :key="classs.classId"
          :value="classs.classId"
        >
          {{ classs.className }} #{{ classs.classId }}
        </option>
      </select>
    </div>

    <!-- Hiển thị danh sách bài test -->
    <div class="space-y-4">
      <h2 class="text-xl font-semibold text-gray-800 mb-4">
        Danh sách bài test:
      </h2>
      <div
        v-for="test in testsModal.testOfClasses"
        :key="test.id"
        class="flex items-center space-x-4 border-b border-gray-300 py-2"
      >
        <input
          type="checkbox"
          :value="{
            id: test.id,
            classId: classId,
            UserCreate: $auth.user.value,
            title: test.title,
          }"
          v-model="selectedTestsModal"
          class="h-5 w-5 text-blue-600 border-gray-300 rounded focus:ring-2 focus:ring-blue-500"
        />
        <label class="text-gray-700 font-medium">
          {{ test.title }} #{{ test.id }}
        </label>
      </div>
    </div>

    <!-- Hiển thị danh sách đã chọn và nút xóa -->
    <div v-if="selectedTestsModal.length" class="mt-6">
      <h3 class="text-lg font-medium text-gray-800 mb-2">
        Danh sách bài test đã chọn:
      </h3>
      <ul class="list-disc list-inside text-gray-700">
        <li
          v-for="(selected, index) in selectedTestsModal"
          :key="selected.id"
          class="flex items-center space-x-2 py-2"
        >
          <span class="text-gray-800"
            >{{ selected.UserCreate }} #{{ selected.id }}</span
          >

          <button
            @click="removeSelectedTest(index)"
            class="text-red-500 hover:text-red-700 font-bold transition duration-300"
          >
            Xóa
          </button>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { ref, onMounted, watch } from "vue";

const testsModal = ref([]);
const selectedTestsModal = ref([]);
const classIdModal = ref(""); // Giá trị mặc định là chuỗi rỗng
const classesModal = ref([]);
const { $auth } = useNuxtApp();
const userId = $auth.user.value;

onMounted(async () => {
  // Lấy danh sách các lớp
  const responseClass = await axios.get("http://localhost:5082/api/classApi");
  if (responseClass.status === 200) {
    classesModal.value = responseClass.data;
  }
});

watch(classIdModal, async (newClassId) => {
  // Lấy danh sách bài test khi thay đổi lớp
  if (newClassId) {
    const responseTest = await axios.get(
      `http://localhost:5082/api/TestApi/listByClass?id=${newClassId}`
    );

    if (responseTest.status === 200) {
      testsModal.value = responseTest.data;
    }
  } else {
    tests.value = []; // Xóa danh sách test nếu không có lớp được chọn
  }
});

// Hàm xóa bài test khỏi danh sách đã chọn
const removeSelectedTest = (index) => {
  selectedTestsModal.value.splice(index, 1);
};
</script>

<style scoped>
/* Có thể thêm các class Tailwind tùy chỉnh ở đây */
</style>
