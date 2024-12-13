<template>
  <div><EditTestDetail :TestDetail="TestDetail" /></div>
</template>

<script setup>
import axios from "axios";
const route = useRoute();
const TestDetail = ref(null);
const testId = route.params.testId;
const { data } = await axios.get(`http://localhost:5082/api/testApi/${testId}`);
TestDetail.value = data;
console.log(data);
</script>

<style scoped></style>
