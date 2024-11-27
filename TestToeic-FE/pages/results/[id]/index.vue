<template>
  <div>
    <!-- Chỉ hiển thị component ResultDetail nếu result có dữ liệu -->
    <ResultDetail v-if="result" :result="result" />
    <!-- Nếu không có dữ liệu, có thể hiển thị thông báo lỗi hoặc loading -->
    <div v-else>Loading...</div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import { useRoute } from "vue-router"; // Đảm bảo đã import đúng component

// Khai báo result dưới dạng ref
const result = ref(null);

// Lấy id từ route params
const route = useRoute();
const { id } = route.params;

// Gửi yêu cầu API khi component được mounted
onMounted(() => {
  axios
    .get(`http://localhost:5082/api/StudentApi?studentPointId=${id}`)
    .then((response) => {
      if (response.status === 200) {
        result.value = response.data;
        console.log(result);
      } else {
        alert("Unexpected response: " + response.data);
        console.log(response.data);
      }
    })
    .catch((error) => {
      if (error.response) {
        alert(`${error.response.data.message || error.response.statusText}`);
        console.error("Error response data:", error.response.data);
      } else if (error.request) {
        alert("Error: No response from server. Please try again later.");
        console.error("Error request:", error.request);
      } else {
        alert("Error: " + error.message);
        console.error("Error message:", error.message);
      }
    });
});
</script>

<style scoped></style>
