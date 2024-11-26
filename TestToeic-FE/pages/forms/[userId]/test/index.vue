<template>
  <div><TestList :tests="tests"></TestList></div>
</template>

<script setup>
import axios from "axios";

const { data: tests } = await axios.get(
  `http://localhost:5082/api/testApi/list`
);
</script>

<style lang="scss" scoped></style>
