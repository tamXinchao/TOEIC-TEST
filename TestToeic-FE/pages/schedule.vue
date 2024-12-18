<template>
  <div>
    <Schedule :schedules="schedules" :tests="tests"></Schedule>
  </div>
  <div>
    <UserInformation></UserInformation>
  </div>
</template>

<script setup>
import axios from "axios";
definePageMeta({
  layout: "admin",
});
const { data: schedules } = await axios.get(
  "http://localhost:5082/api/ScheduleApi/tam"
);
const { data: tests } = await axios.get(
  "http://localhost:5082/api/testApi/list"
);
</script>

<style scoped></style>
