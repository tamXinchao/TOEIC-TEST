<template>
  <div>
    <header class="shadow-sm bg-white">
      <nav class="container mx-auto p-4 flex justify-between">
        <NuxtLink to="/" class="font-bold">Thi đầu vào Toeic</NuxtLink>
        <ul class="flex gap-4">
          <li><NuxtLink to="/" class="text-blue-500">Home</NuxtLink></li>
          <li v-if="user && role == 'teacher'">
            <NuxtLink to="/admin">Admin</NuxtLink>
          </li>
          <li>
            <NuxtLink to="/classes">Lớp dành cho học viên</NuxtLink>
          </li>
          <li>
            <NuxtLink to="/forms">Làm bài thi thử</NuxtLink>
          </li>
        </ul>
      </nav>
    </header>

    <!-- Nội dung của trang chính -->
    <div class="container mx-auto p-4">
      <!-- Dùng slot để hiển thị nội dung của các trang con -->
      <slot />
    </div>
  </div>
</template>

<script setup>
const { $auth } = useNuxtApp();
const user = $auth.user;
const role = $auth.role;
</script>

<style scoped>
/* Thêm kiểu CSS cho các link đang active */
.router-link-exact-active {
  color: green;
  font-weight: bold;
}
</style>
