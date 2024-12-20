<template>
  <div><LevelOfClass :levelOfClasses="levelOfClasses" /></div>
</template>

<script setup>
import axios from "axios";
const { data: levelOfClasses } = await axios.get(
  `http://localhost:5082/api/LevelOfClassApi`
);
</script>

<style scoped></style>
