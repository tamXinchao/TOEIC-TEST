<template>
  <TestDetail :testId="testId" :userId="userId" />
</template>

<script>
export default {
  data() {
    return {
      testId: 4, // Dữ liệu testId (có thể lấy từ route params hoặc API)
      userId: "1a2b3c4d-5678-90ab-cdef-1234567890ab", // Dữ liệu userId
    };
  },
};
</script>
