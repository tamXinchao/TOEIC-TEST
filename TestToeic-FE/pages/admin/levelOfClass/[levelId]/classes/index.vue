<template>
  <div>
    <ClassList :classes="classes" />
  </div>
</template>

<script setup>
definePageMeta({
  layout: "admin",
  middleware: "auth",
});
import axios from "axios";
import { useRoute } from "vue-router";

// Define a reactive variable for the classes

const route = useRoute();
const levelId = route.params.levelId;

const { data: classes } = await axios.get(
  `http://localhost:5082/api/classApi/getByLevel?levelId=${levelId}`
);
</script>

<style scoped></style>
