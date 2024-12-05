<template>
  <div>
    <!-- Thanh tìm kiếm và nút thêm mới -->
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
            placeholder="Tìm theo tên lớp hoặc id..."
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
    <div
      class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-6 gap-6 p-4"
    >
      <div
        v-for="classs in filteredClasses.filter(
          (classItem) => classItem.isActive || isAdmin
        )"
        :key="classs.id"
        class="form-container transition transform hover:scale-105 hover:shadow-lg bg-white border border-gray-200 rounded-lg p-4"
      >
        <NuxtLink
          :to="generateLink(classs.classId)"
          class="block text-gray-800 hover:text-blue-600"
        >
          <div
            class="text-lg font-semibold mb-2 flex justify-between items-center"
          >
            <span>{{ classs.className }}</span>
            <span class="text-gray-400 italic text-xs"
              >#{{ classs.classId }}</span
            >
          </div>

          <div class="text-xs mt-3 italic">
            <span>{{ classs.memberCount }}</span> Học viên
            <span
              v-if="route.path.includes('/admin') && classs.memberRequest > 0"
            >
              , <span class="text-yellow-500">{{ classs.memberRequest }}</span>
              yêu cầu
            </span>
          </div>
          <div class="text-xs mt-3 italic text-right">
            Trạng thái:
            <span :class="classs.isActive ? 'text-green-600' : 'text-red-500'">
              {{ classs.isActive ? "Đang mở" : "Đã đóng" }}
            </span>
          </div>
        </NuxtLink>
        <div class="flex mt-2 justify-between space-x-2" v-if="isAdmin">
          <button
            @click="openModal(classs)"
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
            @click.prevent="deleteClass(classs.classId)"
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
              {{ isEditMode ? "Chỉnh sửa" : "Thêm lớp mới" }}
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
                    Tên lớp
                  </label>
                  <span v-if="isEditMode" class="text-sm text-gray-400 ml-2"
                    >#{{ classIdInput }}</span
                  >
                </div>
                <input
                  type="text"
                  name="ClassName"
                  id="ClassName"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                  placeholder="Nhập tên lớp..."
                  required=""
                  v-model="classNameInput"
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
              Thêm lớp mới
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
import { ref, computed } from "vue";
import { useRoute } from "vue-router";
import axios from "axios";
const classNameInput = ref("");
const classIdInput = ref("");
const isActiveInput = ref(true);
const route = useRoute();
const isAdmin = route.path.includes("/admin");
const isEditMode = ref(false);
const { classes } = defineProps(["classes"]);
const localClasses = ref([...classes]);
const searchQuery = ref("");
const isModalOpen = ref(false);
const openModal = (classToEdit = null) => {
  isModalOpen.value = true;
  if (classToEdit) {
    isEditMode.value = true;
    classIdInput.value = classToEdit.classId;
    classNameInput.value = classToEdit.className;
    isActiveInput.value = classToEdit.isActive;
  } else {
    isEditMode.value = false;
  }
};

// Hàm đóng modal
const closeModal = () => {
  isModalOpen.value = false;
  isEditMode.value = false;
  classNameInput.value = "";
  isActiveInput.value = true;
};
const filteredClasses = computed(() => {
  if (!searchQuery.value) {
    return localClasses.value;
  }
  const query = searchQuery.value.toLowerCase();
  return classes.filter(
    (classs) =>
      classs.className.toLowerCase().includes(query) ||
      classs.classId.toString().toLowerCase().includes(query)
  );
});

const add = async () => {
  try {
    const response = await axios.post(`http://localhost:5082/api/classApi`, {
      ClassName: classNameInput.value,
      IsActive: isActiveInput.value,
    });
    if (response.status === 200) {
      const { message, newClass } = response.data;
      alert(message || "Đã thêm lớp mới thành công!");
      localClasses.value.push(newClass);
      closeModal();
    } else {
      alert("Đã xảy ra lỗi thêm lớp mới!");
    }
  } catch (error) {
    alert(`Lỗi: ${error.response ? error.response.data : error.message}`);
  }
};
const edit = async () => {
  try {
    const classId = classIdInput.value;
    const request = {
      ClassName: classNameInput.value,
      IsActive: isActiveInput.value,
    };
    console.log(request, classId);
    const response = await axios.put(
      `http://localhost:5082/api/classApi?id=${classId}`,
      request
    );
    console.log(request);
    if (response.status === 200) {
      const { message } = response.data;
      alert(message || "Đã sửa lớp thành công!");
      window.location.reload();
    } else {
      alert(`Đã xảy ra lỗi chỉnh sửa lớp !`);
    }
  } catch (error) {
    alert(`Lỗi: ${error.response ? error.response.data : error.message}`);
  }
};

const deleteClass = async (classId) => {
  // Yêu cầu người dùng xác nhận trước khi xóa
  const isConfirmed = confirm("Bạn có chắc chắn muốn xóa lớp này?");

  if (!isConfirmed) {
    return; // Nếu người dùng chọn "Cancel", dừng việc xóa
  }

  try {
    console.log("Class id: " + classId);
    const response = await axios.delete(
      `http://localhost:5082/api/classApi?id=${classId}` // Gọi API DELETE thay vì PUT cho hành động xóa
    );

    if (response.status === 200) {
      const { message } = response.data;
      alert(message || "Đã xóa lớp thành công!");

      localClasses.value = localClasses.value.filter(
        (classItem) => classItem.classId !== classId
      );
    } else {
      alert("Đã xảy ra lỗi khi xóa lớp!");
    }
  } catch (error) {
    alert(`Lỗi: ${error.response ? error.response.data : error.message}`);
  }
};

const generateLink = (classId) => {
  if (route.path.includes("/admin")) {
    return `/admin/class/${classId}`;
  }
  return `/classes/${classId}`;
};
</script>
