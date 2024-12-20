<template>
  <div>
    <Schedule :schedules="schedules" :tests="tests"></Schedule>
  </div>
</template>

<script setup>
import axios from "axios";
definePageMeta({
  layout: "admin",
  middleware: "auth",
});
const { data: schedules } = await axios.get(
  "http://localhost:5082/api/ScheduleApi"
);
const { data: tests } = await axios.get(
  "http://localhost:5082/api/testApi/list"
);
</script>

<style scoped></style>
