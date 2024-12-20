<template>
  <div>
    <div>
      <p>Tên: {{ userInfo.userName }}</p>
      <p>Email: {{ userInfo.normalizedEmail }}</p>
      <p>Số điện thoại: {{ userInfo.phoneNumber }}</p>
    </div>
  </div>
</template>

<script setup>
const { $auth } = useNuxtApp();
console.log($auth.userInfo.value);
const userInfo = $auth.userInfo.value;
</script>

<style scoped></style>
