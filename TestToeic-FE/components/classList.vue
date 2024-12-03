<template>
  <div
    class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-6 gap-6 p-4"
  >
    <div
      class="form-container transition transform hover:scale-105 hover:shadow-lg bg-white border border-gray-200 rounded-lg p-4"
      v-for="classs in classes"
      :key="classs.id"
    >
      <NuxtLink
        :to="generateLink(classs.classId)"
        class="block text-gray-800 hover:text-blue-600"
      >
        <div class="text-lg font-semibold mb-2">
          <span class="font-bold"></span> {{ classs.className }}
        </div>
        <div class="text-xs mt-3 italic">
          <span>
            {{ classs.memberCount }}
          </span>
          <span> Học viên</span>

          <!-- Hiển thị số yêu cầu chỉ khi đường dẫn là /admin -->
          <span v-if="route.path.includes('/admin') && classs.memberRequest > 0"
            >,
          </span>
          <span
            v-if="route.path.includes('/admin') && classs.memberRequest > 0"
            class="text-yellow-500"
          >
            {{ classs.memberRequest }}
          </span>
          <span
            v-if="route.path.includes('/admin') && classs.memberRequest > 0"
            class="text-yellow-500"
          >
            yêu cầu
          </span>
        </div>

        <div class="text-xs mt-3 italic text-right">
          <span>Trạng thái: </span>
          <span :class="classs.isActive ? 'text-green-600' : 'text-red-500'">
            {{ classs.isActive ? "Đang mở" : "Đã đóng" }}
          </span>
        </div>
      </NuxtLink>
    </div>
  </div>
</template>

<script setup>
import { useRoute } from "vue-router";

const route = useRoute();
const { classes } = defineProps(["classes"]);

const generateLink = (classId) => {
  // Kiểm tra xem đường dẫn hiện tại có chứa '/admin'
  if (route.path.includes("/admin")) {
    return `/admin/class/${classId}`;
  }
  return `/classes/${classId}`;
};
</script>

<style scoped>
.form-container {
  padding: 20px;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  background-color: #f9fafb;
  transition: all 0.3s ease-in-out;
}

.form-container:hover {
  transform: scale(1.03);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
</style>
