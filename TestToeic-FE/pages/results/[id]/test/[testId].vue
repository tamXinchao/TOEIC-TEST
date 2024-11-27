<template>
  <div><ResultDetail :userId="id" :studentAnswertId="studentAnswertId" /></div>
</template>

<script setup>
const route = useRoute();
const { id, studentAnswertId } = route.params;
</script>

<style scoped></style>
