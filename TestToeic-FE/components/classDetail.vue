<template>
  <div class="max-w-4xl mx-auto p-6 bg-white rounded-lg shadow-md">
    <h1
      class="text-3xl font-semibold text-gray-800 mb-4 flex justify-between items-center"
    >
      {{ classes.nameofClass || "Chưa có tên lớp" }}
      <button
        v-if="
          !isAdminPath &&
          !classes.hasJoin &&
          classes.message !== 'Bạn đã gửi yêu cầu tham gia vui lòng đợi'
        "
        @click="joinClass(classes.idOfClass)"
        class="bg-green-500 text-white text-sm px-4 py-2 rounded hover:bg-green-600"
      >
        Tham gia lớp
      </button>
      <span
        v-if="classes.message == 'Bạn đã gửi yêu cầu tham gia vui lòng đợi'"
        class="bg-yellow-500 text-white text-sm px-4 py-2 rounded hover:bg-yellow-600"
      >
        Vui lòng đợi
      </span>

      <button
        v-if="!isAdminPath && classes.hasJoin"
        @click="leaveClass(classes.idOfClass)"
        class="bg-red-500 text-white text-sm px-4 py-2 rounded hover:bg-red-600"
      >
        Rời lớp
      </button>
    </h1>

    <!-- Check if classes and the Message are defined -->
    <div
      v-if="classes && classes.message"
      class="text-gray-600 p-4 bg-yellow-100 rounded-md"
    >
      <p>{{ classes.message }}</p>
    </div>
    <div class="max-w-4xl mx-auto mb-6 flex items-center space-x-4">
      <form
        class="flex-1"
        v-if="classes && classes.testOfClasses && classes.testOfClasses.length"
      >
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
            placeholder="Tìm theo tên bài kiểm tra hoặc id của bài test..."
          />
        </div>
      </form>
      <button
        v-if="isAdminPath"
        @click="openModal(null)"
        class="text-white bg-indigo-600 hover:bg-indigo-700 focus:ring-4 focus:outline-none focus:ring-indigo-300 font-medium rounded-lg text-sm px-6 py-3"
      >
        Thêm mới
      </button>
    </div>
    <!-- Check if there are tests available -->
    <div
      v-if="classes && classes.testOfClasses && classes.testOfClasses.length"
      class="space-y-4"
    >
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        <div
          v-for="test in filteredTest"
          :key="test.id"
          class="relative bg-gray-100 p-4 rounded-lg shadow-sm hover:bg-gray-200 transition"
        >
          <!-- Nút sao chép, chỉ hiển thị nếu là /admin -->
          <button
            v-if="isAdminPath"
            @click="copyTestLink(test.id, test.classId)"
            class="absolute top-2 right-2 bg-blue-500 text-white text-sm px-2 py-1 rounded hover:bg-blue-600"
          >
            Sao chép
          </button>

          <h3 class="text-lg font-semibold text-gray-800">
            {{ test.title || "Chưa có tiêu đề" }}
          </h3>
          <p class="text-gray-500 text-sm">
            <span class="font-bold">Số lượng câu hỏi:</span>
            {{ test.questionDtos?.length || 0 }}
          </p>
          <p class="text-gray-500 text-sm">
            <span class="font-bold">Thời gian làm bài:</span>
            {{ test.testTimeMinutes || "Chưa có thông tin" }} phút
          </p>
          <p class="text-gray-500 text-sm">
            <span class="font-bold me-2">Trạng thái:</span>
            <span :class="test.isActive ? 'text-green-500' : 'text-red-500'">
              {{
                test.isActive !== undefined
                  ? test.isActive
                    ? "Đang mở"
                    : "Đã đóng"
                  : "Không xác định"
              }}
            </span>
          </p>

          <!-- Display sticker if available -->
          <div v-if="test.stickers && test.stickers.length > 0" class="mt-4">
            <div class="flex flex-wrap space-x-2">
              <div
                v-for="sticker in test.stickers"
                :key="sticker.stickerId"
                class="flex items-center"
              >
                <NuxtLink :to="'#'">
                  <p
                    class="text-sm text-gray-600 bg-gray-200 my-1 px-3 py-1 rounded-full inline-flex items-center transition-all duration-300 ease-in-out hover:bg-blue-200 hover:text-white"
                  >
                    <span class="font-semibold text-gray-700">#</span>
                    {{ sticker.stickerName }}
                  </p>
                </NuxtLink>
              </div>
            </div>
          </div>
          <div class="flex mt-2 justify-end space-x-2" v-if="isAdminPath">
            <!-- Nút chỉnh sửa -->
            <button
              @click="openModal(test)"
              class="p-2 bg-green-600 hover:bg-green-700 text-white text-sm flex items-center justify-center font-medium rounded-full"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-4 w-4"
                viewBox="0 0 24 24"
                fill="currentColor"
              >
                <path
                  d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z"
                />
              </svg>
            </button>
            <!-- Nút xóa -->
            <button
              @click.prevent="deleteTest(test.id)"
              class="p-2 bg-red-600 hover:bg-red-700 flex items-center justify-center text-white text-sm font-medium rounded-full"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-4 w-4"
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
    </div>
    <div
      v-if="isModalOpen"
      class="fixed inset-0 bg-gray-500 bg-opacity-75 z-50 flex justify-center items-center w-full h-full"
    >
      <div class="relative p-4 w-full max-w-md max-h-full">
        <div class="relative bg-white rounded-lg shadow dark:bg-gray-700">
          <div
            class="flex items-center justify-between p-4 md:p-5 border-b rounded-t dark:border-gray-600"
          >
            <h3 class="text-lg font-semibold text-gray-900 dark:text-white">
              {{ isEditMode ? "Chỉnh sửa" : "Thêm bài kiểm tra" }}
            </h3>
            <button
              type="button"
              @click="closeModal"
              class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
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
            </button>
          </div>

          <form class="p-4 md:p-5">
            <div class="grid gap-4 mb-4 grid-cols-2">
              <div class="col-span-2">
                <div class="flex items-center justify-between">
                  <!-- Sử dụng flex để căn chỉnh nhãn và span -->
                  <label
                    for="name"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >
                    Tên bài kiểm tra
                  </label>
                  <span v-if="isEditMode" class="text-sm text-gray-400 ml-2"
                    >#{{ testIdInput }}</span
                  >
                </div>
                <input
                  type="text"
                  name="TestName"
                  id="TestName"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                  placeholder="Nhập tên bài kiểm tra..."
                  required=""
                  v-model="testNameInput"
                />
              </div>
              <div class="col-span-2">
                <div class="flex items-center justify-between">
                  <!-- Sử dụng flex để căn chỉnh nhãn và span -->
                  <label
                    for="name"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >
                    Thời gian bài kiểm tra
                  </label>
                  <span class="text-sm text-gray-400 ml-2"
                    >Ví dụ : 10 -> 10 phút</span
                  >
                </div>
                <div class="relative">
                  <input
                    type="text"
                    name="TestTimeMinutes"
                    id="TestTimeMinutes"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                    placeholder="Nhập thời gian làm bài kiểm tra..."
                    required
                    v-model="testTimeMinutesInput"
                  />
                  <span
                    class="absolute right-2 top-1/2 transform -translate-y-1/2 text-white"
                    >phút</span
                  >
                </div>
              </div>
              <div class="col-span-2">
                <div class="flex items-center justify-between">
                  <!-- Sử dụng flex để căn chỉnh nhãn và span -->
                  <label
                    for="name"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >
                    Điểm của bài kiểm tra
                  </label>
                </div>
                <input
                  type="text"
                  name="TestPoint"
                  id="TestPoint"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                  placeholder="Nhập điểm của bài kiểm tra..."
                  required=""
                  v-model="testPointInput"
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
              <div class="col-span-2">
                <div>
                  <!-- Dropdown -->
                  <div class="relative inline-block" ref="dropdown">
                    <button
                      @click.prevent="toggleDropdown"
                      class="bg-green-500 text-white px-4 py-2 rounded shadow hover:bg-green-600"
                    >
                      Chọn nhãn dán ({{ selectedIds.length }})
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
                            {{ option.label }} ({{ option.stickerUsed }} lần sử
                            dụng)
                            <!-- Hiển thị label -->
                          </label>
                        </li>
                      </ul>
                    </div>
                  </div>

                  <!-- Hiển thị danh sách đã chọn -->
                  <div class="mt-4">
                    <h3 class="font-bold text-white">Nhãn đã chọn:</h3>
                    <!-- Hiển thị nhãn đã chọn theo dạng ngang -->
                    <div class="flex flex-wrap gap-2">
                      <span
                        v-for="selected in selectedIds"
                        :key="selected"
                        class="bg-blue-200 text-blue-700 px-3 py-1 rounded-full"
                      >
                        #{{
                          options.find((option) => option.id === selected)
                            ?.label || selected
                        }}
                        <!-- Hiển thị label dựa trên id đã chọn -->
                      </span>
                    </div>
                  </div>
                </div>
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
              Thêm bài kiểm tra mới
            </button>

            <button
              v-if="isEditMode"
              @click.prevent="edit"
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
              Lưu chỉnh sửa
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRoute } from "vue-router";
import { ref, computed, onMounted, onBeforeUnmount } from "vue";
import axios from "axios";
const isEditMode = ref(false);
const searchQuery = ref("");
const isActiveInput = ref(true);
const isModalOpen = ref(false);
const idEditMode = ref(false);
const selectedIds = ref([]);
const dropdownOpen = ref(false);
const searchTerm = ref("");
const dropdown = ref(null);
const testNameInput = ref("");
const testTimeMinutesInput = ref("");
const testPointInput = ref("");
const testIdInput = ref("");

const { classes, stickers } = defineProps({
  classes: Object,
  stickers: Array,
});

const localTests = ref(classes.testOfClasses || []);

console.log(localTests);
const route = useRoute();
const options = computed(() => {
  if (!stickers) return [];
  return stickers.map((sticker) => ({
    id: sticker.stickerId,
    label: sticker.stickerName,
    stickerUsed: sticker.stickerOfTestCount,
  }));
});
const filteredOptions = computed(() =>
  options.value.filter((option) =>
    option.label.toLowerCase().includes(searchTerm.value.toLowerCase())
  )
);
const filteredTest = computed(() => {
  if (!searchQuery) {
    return localTests;
  }
  const query = searchQuery.value.toLowerCase();
  return classes.testOfClasses.filter(
    (tests) =>
      tests.title.toLowerCase().includes(query) ||
      tests.id.toString().toLowerCase().includes(query)
  );
});
const toggleDropdown = () => {
  dropdownOpen.value = !dropdownOpen.value;
};
const closeDropdown = (event) => {
  if (dropdown.value && !dropdown.value.contains(event.target)) {
    dropdownOpen.value = false;
  }
};
onMounted(() => {
  document.addEventListener("click", closeDropdown);
});
onBeforeUnmount(() => {
  document.removeEventListener("click", closeDropdown);
});
const isAdminPath = route.path.startsWith("/admin");

// Hàm gửi yêu cầu POST để sao chép bài kiểm tra
const copyTestLink = async (testId, classId) => {
  try {
    // Định nghĩa payload
    const clone = {
      Id: testId,
      UserCreate: "5a8f41cb-b4f4-435a-6991-63e7be71b6d4", // Thay bằng ID người dùng thật nếu cần
      ClassId: classId,
    };

    // Gửi yêu cầu POST tới API và chờ phản hồi
    const response = await axios.post(
      `http://localhost:5082/api/TestApi`,
      clone
    );

    // Xử lý kết quả từ API
    if (response.status === 200) {
      const { message } = response.data; // Lấy message từ API
      alert(message || "Đã sao chép liên kết bài kiểm tra thành công!");
      window.location.reload();
    } else {
      alert("Đã xảy ra lỗi khi sao chép liên kết!");
    }
  } catch (error) {
    console.error("Lỗi khi gửi yêu cầu sao chép liên kết:", error);
    alert("Không thể sao chép liên kết. Vui lòng thử lại!");
  }
};
const joinClass = async (classId) => {
  try {
    // Định nghĩa payload
    const memberInfo = {
      MemberId: "1a2b3c4d-5678-90ab-cdef-1334567890ab",
      ClassId: classId,
    };

    // Gửi yêu cầu POST tới API và chờ phản hồi
    const response = await axios.post(
      `http://localhost:5082/api/MemberOfClassApi/joinClass`,
      memberInfo
    );

    // Xử lý kết quả từ API
    if (response.status === 200) {
      const { message } = response.data; // Lấy message từ API
      alert(message || "Bạn đã gửi yêu cầu tham gia. Vui lòng đợi duyệt!");
      window.location.reload();
    } else {
      alert("Đã xảy ra lỗi khi gửi yêu cầu!");
    }
  } catch (error) {
    console.error("Lỗi khi gửi yêu cầu:", error);
    alert("Không thể gửi yêu cầu. Vui lòng thử lại!");
  }
};
const leaveClass = async (classId) => {
  try {
    // Định nghĩa payload
    const memberInfo = {
      MemberId: "1a2b3c4d-5678-90ab-cdef-1334567890ab",
      ClassId: classId,
    };

    // Gửi yêu cầu POST tới API và chờ phản hồi
    const response = await axios.delete(
      `http://localhost:5082/api/MemberOfClassApi/leaveClass/${memberInfo.MemberId}?classId=${memberInfo.ClassId}` // Thay thế classId theo đúng logic,
    );

    // Xử lý kết quả từ API
    if (response.status === 200) {
      const { message } = response.data; // Lấy message từ API
      alert(message || "Bạn đã rời lớp thành công!");
      window.location.reload();
    } else {
      alert("Đã xảy ra lỗi khi bạn đang cố rời lớp!");
    }
  } catch (error) {
    console.error("Lỗi khi rời lớp:", error);
    alert("Không thể rời lớp. Vui lòng thử lại!");
  }
};

const openModal = (test) => {
  isModalOpen.value = true;
  if (test) {
    isEditMode.value = true;
    testIdInput.value = test.id;
    testNameInput.value = `${test.title}`;
    testTimeMinutesInput.value = test.testTimeMinutes;
    testPointInput.value = test.point;
    selectedIds.value = test.stickers.map((sticker) => sticker.stickerId);
    isActiveInput.value = test.isActive;
  } else {
    isEditMode.value = false;
    testIdInput.value = "";
    testNameInput.value = "";
    testPointInput.value = "";
    testTimeMinutesInput.value = "";
    selectedIds.value = [];
    isActiveInput.value = true;
  }
};
const closeModal = () => {
  isModalOpen.value = false;
  idEditMode.value = false;
  testIdInput.value = "";
  testNameInput.value = "";
  testPointInput.value = "";
  testTimeMinutesInput.value = "";
  isActiveInput.value = true;
};

const add = async () => {
  try {
    const formattedData = selectedIds.value.map((id) => ({ stickerId: id }));
    // Tạo request
    const request = {
      UserCreate: "f4a25ce5-bae1-471c-b506-a0a218cf32a6",
      TestName: testNameInput.value,
      TestTimeMinutes: testTimeMinutesInput.value,
      Point: testPointInput.value,
      StickersOfTests: formattedData,
      ClassId: classes.idOfClass,
      IsActive: isActiveInput.value,
    };

    const response = await axios.post(
      `http://localhost:5082/api/TestApi/create`,
      request
    );
    if (response.status === 200) {
      const { message, newTest } = response.data;
      alert(message || "Đã thêm lớp mới thành công!");
      localTests.value.push(newTest);
      closeModal();
    } else {
      alert("Đã xảy ra lỗi thêm lớp mới!");
    }
  } catch (error) {
    // Bắt lỗi và hiển thị thông báo
    alert(`Lỗi: ${error.response ? error.response.data : error.message}`);
  }
};

const edit = async () => {
  try {
    // Tạo dữ liệu yêu cầu với thông tin đã thay đổi
    const formattedData = selectedIds.value.map((id) => ({ stickerId: id }));

    const request = {
      UserCreate: "f4a25ce5-bae1-471c-b506-a0a218cf32a6", // User đang chỉnh sửa
      TestName: testNameInput.value,
      TestTimeMinutes: testTimeMinutesInput.value,
      Point: testPointInput.value,
      StickersOfTests: formattedData,
      ClassId: classes.idOfClass,
      IsActive: isActiveInput.value,
    };

    // Thực hiện PUT request để cập nhật bài kiểm tra
    const response = await axios.put(
      `http://localhost:5082/api/TestApi/update/${testIdInput.value}`, // Sử dụng ID của bài kiểm tra cần cập nhật
      request
    );

    if (response.status === 200) {
      const { message, updatedTest } = response.data;
      alert(message || "Đã cập nhật bài kiểm tra thành công!");

      // Cập nhật lại dữ liệu trong localTests (hoặc danh sách đang hiển thị)
      const index = classes.testOfClasses.findIndex(
        (test) => test.id === updatedTest.id
      );
      if (index !== -1) {
        classes.testOfClasses[index] = updatedTest;
      }

      closeModal(); // Đóng modal sau khi cập nhật thành công
    } else {
      alert("Đã xảy ra lỗi khi cập nhật bài kiểm tra!");
    }
  } catch (error) {
    // Bắt lỗi và hiển thị thông báo
    alert(`Lỗi: ${error.response ? error.response.data : error.message}`);
  }
};

const deleteTest = async (TestIdDelete) => {
  setTimeout(async () => {
    const isConfirmed = confirm("Bạn có chắc chắn muốn xóa bài kiểm tra này?");
    if (!isConfirmed) {
      return; // Người dùng hủy, dừng xử lý
    }

    try {
      // Gửi yêu cầu DELETE đến API
      const response = await axios.delete(
        `http://localhost:5082/api/testApi?id=${TestIdDelete}`
      );

      // Kiểm tra phản hồi từ API
      if (response.status === 200) {
        const { message } = response.data;
        alert(message || "Đã xóa lịch thành công!");

        classes.testOfClasses = classes.testOfClasses.filter(
          (testItem) => testItem.id !== TestIdDelete
        );
        console.log(localTests.value);
      } else {
        alert("Đã xảy ra lỗi khi xóa lịch!");
      }
    } catch (error) {
      const errorMessage =
        error.response?.data ||
        error.message ||
        "Đã xảy ra lỗi không xác định.";
      alert(`Lỗi: ${errorMessage}`);
    }
  }, 0);
};
</script>

<style scoped>
/* Scoped styling if needed */
button {
  transition: all 0.3s ease;
}
</style>
