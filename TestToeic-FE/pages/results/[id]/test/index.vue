<template>
  <div><ResultList :userId="id" /></div>
</template>

<script setup>
const route = useRoute();
const { id } = route.params;
</script>

<style scoped></style>
