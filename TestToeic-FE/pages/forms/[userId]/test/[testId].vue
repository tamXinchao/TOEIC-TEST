<template>
  <TestDetail :testId="testId" :userId="userId" />
</template>

<script>
export default {
  data() {
    return {
      testId: this.$route.params.testId, // Lấy testId từ URL
      userId: this.$route.params.userId,
    };
  },
  watch: {
    // Theo dõi sự thay đổi của route params
    "$route.params": function (newParams) {
      this.testId = newParams.testId;
      this.userId = newParams.userId;
    },
  },
};
</script>
