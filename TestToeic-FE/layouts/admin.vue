<template>
  <div>
    <header class="shadow-sm bg-white">
      <nav class="container mx-auto p-4 flex justify-between">
        <NuxtLink to="/" class="font-bold">Thi đầu vào Toeic</NuxtLink>
        <ul class="flex gap-4">
          <li>
            <NuxtLink to="/admin" class="text-blue-500">Admin</NuxtLink>
          </li>
          <li>
            <NuxtLink to="/admin/class">Class</NuxtLink>
          </li>
          <li>
            <NuxtLink to="/admin/schedule">Schedules</NuxtLink>
          </li>
        </ul>
      </nav>
    </header>

    <!-- Nội dung của trang chính -->
    <div class="container mx-auto p-4">
      <slot />
    </div>
  </div>
</template>

<script setup>
definePageMeta({
  middleware: "auth",
});
</script>
<style scoped>
.router-link-active {
  color: green;
  font-weight: bold;
}
</style>
