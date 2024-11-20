<template>
  <div class="bg-gray-50 min-h-screen">
    <!-- Header -->
    <header class="shadow-sm bg-white">
      <nav class="container mx-auto p-4 flex justify-between items-center">
        <NuxtLink to="/" class="font-bold">Thi đầu vào Toeic</NuxtLink>
        <ul class="flex gap-6">
          <li>
            <NuxtLink
              to="/"
              class="text-blue-500 hover:text-blue-700 transition duration-200"
              >Home</NuxtLink
            >
          </li>
          <li>
            <NuxtLink
              to="/tests"
              class="text-gray-700 hover:text-gray-900 transition duration-200"
              >Làm bài thi thử</NuxtLink
            >
          </li>
        </ul>
      </nav>
    </header>
    <div class="container mx-auto p-4">
      <!-- Dùng slot để hiển thị nội dung của các trang con -->
      <slot />
    </div>
  </div>
</template>

<script setup></script>

<style scoped>
.router-link-exact-active {
  color: green;
  font-weight: bold;
}
</style>
