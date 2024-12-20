<template>
  <div>
    <ClassList :classes="classes" />
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import { useRoute } from "vue-router";
const { $auth } = useNuxtApp();

// Define a reactive variable for the classes
const classes = ref([]);
const route = useRoute();
const levelId = route.params.levelId;

// Fetch classes when component is mounted

if (!$auth.user.value) {
  const { data } = await axios.get("http://localhost:5082/api/classApi");
  classes.value = data; // Set the classes data if no user is logged in
} else {
  const { data } = await axios.get(
    `http://localhost:5082/api/classApi/getByLevel/${$auth.user.value}?levelId=${levelId}`
  );
  classes.value = data; // Set the classes data for authenticated user
}
</script>

<style scoped></style>
