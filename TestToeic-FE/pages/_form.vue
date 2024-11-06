<template>
  <div class="form-container">
    <h2 class="text-2xl mb-4">Nhập thông tin thí sinh</h2>
    <form @submit.prevent="submitForm">
      <div class="mb-4">
        <label for="name" class="block text-gray-700 font-bold mb-2"
          >Tên thí sinh:</label
        >
        <input
          type="text"
          id="name"
          v-model="candidateName"
          class="border rounded py-2 px-3 text-gray-700 w-full"
          placeholder="Nhập tên thí sinh"
          required
        />
      </div>
      <NuxtLink to="/questions">
        <button
          type="submit"
          class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
        >
          Bắt đầu làm bài
        </button>
      </NuxtLink>
    </form>
  </div>
</template>

<script setup></script>

<style scoped>
.form-container {
  max-width: 400px;
  margin: 0 auto;
  padding: 20px;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  background-color: #f9fafb;
}
</style>
