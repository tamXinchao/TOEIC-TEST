<template>
  <div><ClassDetail :classes="classes" /></div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";

const classes = ref({
  testOfClasses: [], // Initializing as an empty array to avoid undefined errors
});

const route = useRoute();
const classId = route.params.id;
const { $auth } = useNuxtApp();
onMounted(async () => {
  try {
    const { data } = await axios.get(
      `http://localhost:5082/api/TestApi/${$auth.user.value}/listByClass?id=${classId}`
    );
    classes.value = data; // Populate classes with the API response
    console.log(classes);
  } catch (error) {
    console.error("Error fetching data:", error);
  }
});
</script>

<style scoped></style>
