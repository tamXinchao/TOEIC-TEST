<template>
  <div class="p-4 bg-gray-50 min-h-screen">
    <h1 class="text-2xl font-semibold text-gray-800 mb-6">
      Danh sách khóa học
    </h1>
    <div class="max-w-4xl mx-auto mb-6 flex items-center space-x-4">
      <form class="flex-1">
        <label
          for="default-search"
          class="mb-2 text-sm font-medium text-gray-900 sr-only"
          >Tìm kiếm</label
        >
        <div class="relative">
          <div
            class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none"
          >
            <svg
              class="w-4 h-4 text-gray-600"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 20 20"
            >
              <path
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"
              />
            </svg>
          </div>
          <input
            type="text"
            v-model="searchQuery"
            id="default-search"
            class="block w-full p-4 ps-10 text-sm text-gray-800 border border-gray-300 rounded-lg bg-white focus:ring-indigo-500 focus:border-indigo-500"
            placeholder="Tìm theo tên khóa học hoặc id..."
          />
        </div>
      </form>
      <button
        @click="openModal(null)"
        v-if="isAdmin"
        class="text-white bg-indigo-600 hover:bg-indigo-700 focus:ring-4 focus:outline-none focus:ring-indigo-300 font-medium rounded-lg text-sm px-6 py-3"
      >
        Thêm mới
      </button>
    </div>
    <!-- Danh sách lớp -->
    <div class="lg:grid-cols-4 xl:grid-cols-5 gap-8 p-4">
      <div
        v-for="level in filteredClasses"
        :key="level.levelOfClassId"
        class="form-container transition transform hover:scale-105 my-2 hover:shadow-lg bg-white border border-gray-200 rounded-lg p-6"
      >
        <NuxtLink
          :to="generateLink(level.levelOfClassId)"
          class="block text-gray-800 hover:text-blue-600"
        >
          <div
            class="text-lg font-semibold mb-2 flex justify-between items-center"
          >
            <span>{{ level.leveName }}</span>
            <span class="text-gray-400 italic text-xs"
              >#{{ level.levelOfClassId }}</span
            >
          </div>

          <div class="text-xs mt-3 italic">
            <!-- Số lượng lớp -->
            <span class="font-semibold text-gray-800">{{
              level.classCount
            }}</span>
            lớp học
          </div>

          <div class="text-xs mt-3 italic text-right">
            Trạng thái:
            <span :class="level.isActive ? 'text-green-600' : 'text-red-500'">
              {{ level.isActive ? "Đang mở" : "Đã đóng" }}
            </span>
          </div>

          <div
            v-if="isAdmin"
            :class="{
              'text-green-500': level.isActive,
              'text-red-500': !level.isActive,
            }"
            class="font-semibold mt-2"
          >
            {{ level.isActive ? "Hoạt động" : "Đã đóng" }}
          </div>
        </NuxtLink>
        <div class="flex mt-2 justify-between space-x-2" v-if="isAdmin">
          <button
            @click="openModal(level)"
            class="flex-1 inline-flex items-center px-4 py-2 bg-green-600 hover:bg-green-700 text-white text-sm font-medium rounded-md"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="h-3 w-3 flex-1"
              viewBox="0 0 24 24"
              fill="currentColor"
            >
              <path
                d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z"
              />
            </svg>
          </button>
          <button
            @click.prevent="deleteClass(level.levelOfClassId)"
            class="flex-1 inline-flex items-center px-4 py-2 bg-red-600 hover:bg-red-700 text-white text-sm font-medium rounded-md"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="h-3 w-3 flex-1"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
              />
            </svg>
          </button>
        </div>
      </div>
    </div>
    <!-- Main modal -->
    <!-- Modal -->
    <div
      v-if="isModalOpen"
      class="fixed inset-0 bg-gray-500 bg-opacity-75 z-50 flex justify-center items-center w-full h-full"
    >
      <div class="relative p-4 w-full max-w-md max-h-full">
        <!-- Modal content -->
        <div class="relative bg-white rounded-lg shadow dark:bg-gray-700">
          <!-- Modal header -->
          <div
            class="flex items-center justify-between p-4 md:p-5 border-b rounded-t dark:border-gray-600"
          >
            <h3 class="text-lg font-semibold text-gray-900 dark:text-white">
              {{ isEditMode ? "Chỉnh sửa" : "Thêm khóa học mới" }}
            </h3>
            <button
              type="button"
              @click="closeModal"
              class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
              data-modal-toggle="crud-modal"
            >
              <svg
                class="w-3 h-3"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 14 14"
              >
                <path
                  stroke="currentColor"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
                />
              </svg>
              <span class="sr-only">Close modal</span>
            </button>
          </div>
          <!-- Modal body -->
          <form class="p-4 md:p-5">
            <div class="grid gap-4 mb-4 grid-cols-2">
              <div class="col-span-2">
                <div class="flex items-center justify-between">
                  <!-- Sử dụng flex để căn chỉnh nhãn và span -->
                  <label
                    for="name"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >
                    Tên khóa học
                  </label>
                  <span v-if="isEditMode" class="text-sm text-gray-400 ml-2"
                    >#{{ levelIdInput }}</span
                  >
                </div>
                <input
                  type="text"
                  name="LevelName"
                  id="LevelName"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                  placeholder="Nhập tên khóa học..."
                  required=""
                  v-model="levelNameInput"
                />
              </div>

              <div class="col-span-2 sm:col-span-1">
                <label
                  for="Active"
                  class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >Trạng thái</label
                >
                <select
                  id="Active"
                  v-model="isActiveInput"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-500 focus:border-primary-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                >
                  <option :value="true">Hoạt động</option>
                  <option :value="false">Không hoạt động</option>
                </select>
              </div>
            </div>
            <button
              v-if="!isEditMode"
              @click.prevent="add"
              class="text-white inline-flex items-center bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
            >
              <svg
                class="me-1 -ms-1 w-5 h-5"
                fill="currentColor"
                viewBox="0 0 20 20"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  fill-rule="evenodd"
                  d="M10 5a1 1 0 011 1v3h3a1 1 0 110 2h-3v3a1 1 0 11-2 0v-3H6a1 1 0 110-2h3V6a1 1 0 011-1z"
                  clip-rule="evenodd"
                ></path>
              </svg>
              Thêm khóa học mới
            </button>
            <button
              v-if="isEditMode"
              @click.prevent="edit"
              class="text-white inline-flex items-center bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
            >
              Lưu chỉnh sửa
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
const { levelOfClasses } = defineProps(["levelOfClasses"]);
const localLevelOfClasses = ref(
  Array.isArray(levelOfClasses) ? [...levelOfClasses] : []
);
console.log(localLevelOfClasses);
const levelNameInput = ref("");
const isActiveInput = ref(true);
const levelIdInput = ref("");
const searchQuery = ref("");
const isEditMode = ref(false);
const isModalOpen = ref(false);
const route = useRoute();
const isAdmin = route.path.startsWith("/admin");
const generateLink = (classId) => {
  if (route.path.startsWith("/admin")) {
    return `/admin/levelOfClass/${classId}/classes`;
  }
  if (route.path.startsWith("/levelOfClass")) {
    return `/levelOfClass/${classId}/classes`;
  }
  return `/level/${classId}/classes`;
};
const openModal = (classToEdit = null) => {
  isModalOpen.value = true;
  if (classToEdit) {
    isEditMode.value = true;
    levelIdInput.value = classToEdit.levelOfClassId;
    levelNameInput.value = classToEdit.leveName;
    isActiveInput.value = classToEdit.isActive;
  } else {
    isEditMode.value = false;
  }
};
const closeModal = () => {
  isModalOpen.value = false;
  isEditMode.value = false;
  levelNameInput.value = "";
  isActiveInput.value = true;
};
const filteredClasses = computed(() => {
  if (!searchQuery.value) {
    return localLevelOfClasses.value;
  }
  const query = searchQuery.value.toLowerCase();
  return levelOfClasses.filter(
    (level) =>
      level.levelOfClassId.toString().toLowerCase().includes(query) || // Convert levelOfClassId to string before using toLowerCase
      level.leveName.toString().toLowerCase().includes(query)
  );
});
</script>

<style scoped>
/* Custom styles if needed */
</style>
