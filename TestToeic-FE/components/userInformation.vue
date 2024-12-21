<template>
  <div class="max-w-3xl mx-auto p-6 bg-white shadow-lg rounded-lg">
    <h1 class="text-2xl font-semibold text-gray-800 mb-4">
      Thông tin người dùng
    </h1>

    <div class="space-y-4">
      <div class="flex justify-between">
        <span class="font-medium text-gray-600">Tên:</span>
        <span class="text-gray-800">{{ userInfo.userName }}</span>
      </div>

      <div class="flex justify-between">
        <span class="font-medium text-gray-600">Email:</span>
        <span class="text-gray-800">{{ userInfo.normalizedEmail }}</span>
      </div>

      <div class="flex justify-between">
        <span class="font-medium text-gray-600">Số điện thoại:</span>
        <span class="text-gray-800">{{ userInfo.phoneNumber }}</span>
      </div>
    </div>
  </div>
</template>

<script setup>
const { $auth } = useNuxtApp();
console.log($auth.userInfo.value);
const userInfo = $auth.userInfo.value;
</script>

<style scoped></style>
