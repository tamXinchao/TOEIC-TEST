<template>
  <div class="max-w-4xl mx-auto p-6 bg-white rounded-lg shadow-md">
    <h1 class="text-3xl font-semibold text-gray-800 mb-4">Bài kiểm tra</h1>

    <!-- Check if classes and the Message are defined -->
    <div
      v-if="classes && classes.message"
      class="text-gray-600 p-4 bg-yellow-100 rounded-md"
    >
      <p>{{ classes.message }}</p>
    </div>

    <!-- Check if there are tests available -->
    <div
      v-if="classes && classes.testOfClasses && classes.testOfClasses.length"
      class="space-y-4"
    >
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        <div
          v-for="test in classes.testOfClasses"
          :key="test.testOfClassId"
          class="bg-gray-100 p-4 rounded-lg shadow-sm hover:bg-gray-200 transition"
        >
          <h3 class="text-lg font-semibold text-gray-800">
            {{ test.title || "Chưa có tiêu đề" }}
          </h3>
          <p class="text-gray-500 text-sm">
            <span class="font-bold">Số lượng câu hỏi:</span>
            {{ test.questionDtos?.length || 0 }}
          </p>
          <p class="text-gray-500 text-sm">
            <span class="font-bold">Thời gian làm bài:</span>
            {{ test.testTimeMinutes || "Chưa có thông tin" }} phút
          </p>
          <p class="text-gray-500 text-sm">
            <span class="font-bold me-2">Trạng thái:</span>
            <span :class="test.isActive ? 'text-green-500' : 'text-red-500'">
              {{
                test.isActive !== undefined
                  ? test.isActive
                    ? "Đang mở"
                    : "Đã đóng"
                  : "Không xác định"
              }}
            </span>
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
const { classes } = defineProps(["classes"]);
</script>

<style scoped>
/* Scoped styling if needed */
</style>
