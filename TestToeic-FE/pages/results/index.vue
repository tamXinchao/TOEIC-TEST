<template>
  <div class="max-w-4xl mx-auto bg-white p-6 rounded-lg shadow-lg">
    <h2 class="text-2xl font-semibold mb-4 text-center">
      Nhập tài khoản của bản
    </h2>

    <form @submit.prevent="submitForm">
      <table class="min-w-full table-auto">
        <thead>
          <tr class="bg-gray-200">
            <th class="px-4 py-2 text-left">Tên Trường</th>
            <th class="px-4 py-2 text-left">Thông Tin</th>
          </tr>
        </thead>
        <tbody>
          <tr class="border-b">
            <td class="px-4 py-2 font-medium">Tài khoản</td>
            <td class="px-4 py-2">
              <input
                v-model="name"
                type="text"
                class="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Nhập họ và tên"
              />
            </td>
          </tr>
          <tr>
            <td class="px-4 py-2"></td>
            <td class="px-4 py-2 text-right">
              <button
                type="submit"
                class="px-6 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                Lưu Thông Tin
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </form>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      name: "",
    };
  },
  methods: {
    async submitForm() {
      try {
        const response = await axios.get(
          "http://localhost:5082/api/userApi/getByUsername?username=" +
            this.name
        );
        if (response.status === 200) {
          console.log(response.data.id);
          const userId = response.data.id;
          this.$router.push(`/forms/${userId}/test`);
          // Redirect to the new route with the ID included in the URL
        } else {
          alert("Unexpected response: " + response.data);
          console.log(response.data);
        }
      } catch (error) {
        if (error.response) {
          // Server returned an error response
          alert(`${error.response.data.message || error.response.statusText}`);
          console.error("Error response data:", error.response.data);
        } else if (error.request) {
          // Request was made but no response received
          alert("Error: No response from server. Please try again later.");
          console.error("Error request:", error.request);
        } else {
          // Something happened in setting up the request
          alert("Error: " + error.message);
          console.error("Error message:", error.message);
        }
      }
    },
  },
};
</script>

<style lang="scss" scoped></style>
