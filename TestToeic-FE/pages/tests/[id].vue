<template>
  <div>
    <TestDetail :test="test" />
  </div>
</template>

<script setup>
definePageMeta({
  layout: "tests",
});
import axios from "axios";
const { id } = useRoute().params;
console.log("Lấy Id: " + id);
const { data: test } = axios.get(
  `http://localhost:5082/api/testApi/list?id=${id}`
);
</script>

<style scoped></style>
