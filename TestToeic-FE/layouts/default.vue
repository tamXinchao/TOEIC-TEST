<template>
  <div>
    <header class="shadow-sm bg-white">
      <nav class="container mx-auto p-4 flex justify-between items-center">
        <!-- Logo hoặc tiêu đề -->
        <NuxtLink to="/" class="font-bold text-xl">Thi đầu vào Toeic</NuxtLink>

        <!-- Các menu điều hướng -->
        <ul class="flex gap-6 items-center">
          <li><NuxtLink to="/" class="hover:text-blue-500">Home</NuxtLink></li>
          <li v-if="user && role === 'teacher'">
            <NuxtLink to="/admin" class="hover:text-blue-500">Admin</NuxtLink>
          </li>
          <li v-if="user">
            <NuxtLink to="/dashboard" class="hover:text-blue-500"
              >Khóa học của bạn</NuxtLink
            >
          </li>
          <li>
            <NuxtLink to="/levelOfClass" class="hover:text-blue-500"
              >Lớp học</NuxtLink
            >
          </li>
          <li>
            <NuxtLink to="/forms" class="hover:text-blue-500"
              >Đề thi online</NuxtLink
            >
          </li>

          <!-- Dropdown User -->
          <li class="relative" ref="dropdown">
            <div
              @click="toggleDropdown"
              class="cursor-pointer flex items-center space-x-3"
            >
              <!-- Username -->
              <span
                v-if="user"
                class="font-semibold text-gray-800 hover:text-blue-500 transition"
              >
                {{ username }}
              </span>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                stroke-width="1.5"
                stroke="currentColor"
                class="size-6"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  d="M17.982 18.725A7.488 7.488 0 0 0 12 15.75a7.488 7.488 0 0 0-5.982 2.975m11.963 0a9 9 0 1 0-11.963 0m11.963 0A8.966 8.966 0 0 1 12 21a8.966 8.966 0 0 1-5.982-2.275M15 9.75a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"
                />
              </svg>
            </div>

            <!-- Dropdown Menu -->
            <ul
              v-if="isDropdownOpen"
              class="absolute right-0 mt-2 bg-white border rounded-md shadow-md w-48 text-sm"
              @click.outside="closeDropdown"
            >
              <li v-if="!user">
                <NuxtLink to="/login" class="block px-4 py-2 hover:bg-gray-100">
                  Đăng nhập
                </NuxtLink>
              </li>
              <li v-if="!user">
                <NuxtLink
                  to="/register"
                  class="block px-4 py-2 hover:bg-gray-100"
                >
                  Đăng ký
                </NuxtLink>
              </li>
              <li v-if="user">
                <button
                  @click="logout"
                  class="w-full text-left px-4 py-2 hover:bg-gray-100"
                >
                  Đăng xuất
                </button>
              </li>
            </ul>
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
import { ref, onMounted, onUnmounted } from "vue";

const { $auth } = useNuxtApp();
const user = $auth.user;
const role = $auth.role;
const username = $auth._username || "Guest";

const isDropdownOpen = ref(false);
const dropdown = ref(null);

const toggleDropdown = () => {
  isDropdownOpen.value = !isDropdownOpen.value;
};

const handleClickOutside = (event) => {
  if (dropdown.value && !dropdown.value.contains(event.target)) {
    isDropdownOpen.value = false;
  }
};

onMounted(() => {
  document.addEventListener("click", handleClickOutside);
});

onUnmounted(() => {
  document.removeEventListener("click", handleClickOutside);
});

const logout = () => {
  $auth.logout();
};
</script>

<style scoped>
.router-link-exact-active {
  color: green;
  font-weight: bold;
}
</style>
