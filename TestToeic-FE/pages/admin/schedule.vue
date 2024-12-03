<template>
  <div>
    <Schedule :schedules="schedules" :index="index"></Schedule>
  </div>
</template>

<script setup>
import axios from "axios";
definePageMeta({
  layout: "admin",
});
const { data: schedules } = await axios.get(
  "http://localhost:5082/api/ScheduleApi"
);
</script>

<style scoped></style>
