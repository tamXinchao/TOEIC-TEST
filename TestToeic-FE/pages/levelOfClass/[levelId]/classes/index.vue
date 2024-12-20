<template>
  <div>
    <ClassList :classes="classes" />
  </div>
</template>

<script setup>
import axios from "axios";
import { useRoute } from "vue-router";

const route = useRoute();
const levelId = route.params.levelId;

const { data: classes } = await axios.get(
  `http://localhost:5082/api/classApi/getByLevel?levelId=${levelId}`
);
</script>

<style scoped></style>
