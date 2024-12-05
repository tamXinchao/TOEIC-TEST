<template>
  <div><ClassList :classes="classes" /></div>
</template>

<script setup>
definePageMeta({ layout: "admin" });
import axios from "axios";

const { data: classes } = await axios.get(`http://localhost:5082/api/classApi`);
console.log(classes);
</script>

<style scoped></style>
