<template>
  <div>
    <!-- Dropdown -->
    <div class="relative inline-block" ref="dropdown">
      <button
        @click="toggleDropdown"
        class="bg-blue-500 text-white px-4 py-2 rounded shadow hover:bg-blue-600"
      >
        Select Options ({{ selectedIds.length }})
      </button>
      <div
        v-if="dropdownOpen"
        class="absolute bg-white border border-gray-300 rounded shadow-lg mt-2 w-72 z-10"
      >
        <!-- Input tìm kiếm -->
        <input
          type="text"
          v-model="searchTerm"
          placeholder="Search..."
          class="w-full px-3 py-2 border-b border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        <!-- Danh sách checkbox -->
        <ul class="max-h-48 overflow-y-auto">
          <li
            v-for="option in filteredOptions"
            :key="option.id"
            class="px-3 py-2 hover:bg-gray-100"
          >
            <label class="flex items-center">
              <input
                type="checkbox"
                :value="option.id"
                v-model="selectedIds"
                class="mr-2"
              />
              {{ option.label }}
            </label>
          </li>
        </ul>
      </div>
    </div>

    <!-- Hiển thị danh sách đã chọn -->
    <div class="mt-4">
      <h3 class="font-bold">Selected IDs:</h3>
      <pre class="bg-gray-100 p-2 rounded">{{ selectedIds }}</pre>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from "vue";

// Dữ liệu ban đầu (ID và Label)
const options = ref(
  Array.from({ length: 100 }, (_, i) => ({
    id: i + 1,
    label: `Option ${i + 1}`,
  }))
);
const selectedIds = ref([]); // Chỉ lưu ID
const dropdownOpen = ref(false); // Trạng thái mở dropdown
const searchTerm = ref(""); // Tìm kiếm
const dropdown = ref(null); // Tham chiếu tới dropdown

// Lọc dữ liệu dựa trên tìm kiếm
const filteredOptions = computed(() =>
  options.value.filter((option) =>
    option.label.toLowerCase().includes(searchTerm.value.toLowerCase())
  )
);

// Toggle trạng thái mở/đóng dropdown
const toggleDropdown = () => {
  dropdownOpen.value = !dropdownOpen.value;
};

// Đóng dropdown khi nhấp ra ngoài
const closeDropdown = (event) => {
  if (dropdown.value && !dropdown.value.contains(event.target)) {
    dropdownOpen.value = false;
  }
};

// Thêm và gỡ bỏ sự kiện click toàn cục
onMounted(() => {
  document.addEventListener("click", closeDropdown);
});

onBeforeUnmount(() => {
  document.removeEventListener("click", closeDropdown);
});
</script>
