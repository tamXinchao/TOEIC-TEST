<template>
  <div>
    <section class="container mx-auto p-6 font-mono">
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
              placeholder="Tìm theo tên bài kiểm tra hoặc id của bài test..."
            />
          </div>
        </form>
        <button
          v-if="isAdmin"
          @click="openModal(null)"
          class="text-white bg-indigo-600 hover:bg-indigo-700 focus:ring-4 focus:outline-none focus:ring-indigo-300 font-medium rounded-lg text-sm px-6 py-3"
        >
          Thêm mới
        </button>
      </div>
      <div class="w-full mb-8 overflow-hidden rounded-lg shadow-lg">
        <div class="w-full overflow-x-auto">
          <table class="w-full">
            <thead>
              <tr
                class="text-md font-semibold tracking-wide text-left text-gray-900 bg-gray-100 uppercase border-b border-gray-600"
              >
                <th class="px-4 py-3">#</th>
                <th class="px-4 py-3">Id</th>
                <th class="px-4 py-3">Tên</th>
                <th class="px-4 py-3">Ngày bắt đầu</th>
                <th class="px-4 py-3">Ngày kết thúc</th>
                <th class="px-4 py-3">Kết thúc sau</th>
                <th v-if="isAdmin" class="px-4 py-3">Hành động</th>
              </tr>
            </thead>
            <tbody class="bg-white">
              <tr
                class="text-gray-700"
                v-for="(Schedule, index) in filteredClasses"
                :key="Schedule.id"
              >
                <td class="px-4 py-3 border">{{ index + 1 }}</td>
                <td class="px-4 py-3 text-ms font-semibold border">
                  #{{ Schedule.testId }}
                </td>
                <td class="px-4 py-3 text-xs border">
                  <div class="flex items-center text-sm">
                    <div>
                      <p class="font-semibold text-black">
                        {{ Schedule.testName }}
                      </p>
                      <p
                        class="text-xs"
                        :class="
                          Schedule.className === 'Chưa có lớp'
                            ? 'text-yellow-600'
                            : 'text-gray-600'
                        "
                      >
                        {{
                          Schedule.className === "Chưa có lớp"
                            ? "Chưa có lớp"
                            : "Lớp: " +
                              Schedule.className +
                              " #" +
                              Schedule.classId
                        }}
                      </p>
                    </div>
                  </div>
                </td>
                <td class="px-4 py-3 text-sm border">
                  {{ Schedule.stringDayOpenTest }}
                </td>
                <td class="px-4 py-3 text-sm border">
                  {{ Schedule.stringDayCloseTest }}
                </td>
                <td
                  class="px-4 py-3 text-sm border"
                  :class="
                    Schedule.timeRemaining == 'Đã kết thúc'
                      ? 'text-yellow-600'
                      : 'text-green-600'
                  "
                >
                  {{ Schedule.timeRemaining }}
                </td>
                <td v-if="isAdmin" class="px-4 py-3 text-xs border">
                  <button @click="openModal(Schedule)">
                    <span
                      class="mx-2 px-2 py-1 font-semibold leading-tight text-green-700 bg-green-100 rounded-sm"
                    >
                      Chỉnh sửa
                    </span>
                  </button>
                  <button @click="deleteSchedule(Schedule.scheduleId)">
                    <span
                      class="px-2 py-1 font-semibold leading-tight text-yellow-700 bg-green-100 rounded-sm"
                    >
                      Xóa
                    </span>
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </section>
    <!-- Modal content -->
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
              {{ isEditMode ? "Chỉnh sửa" : "Thêm lịch" }}
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
                  <label
                    for="TestName"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >
                    Tên bài kiểm tra
                  </label>
                  <span
                    v-if="selectedTestId"
                    class="text-gray-400 italic text-xs"
                  >
                    {{ selectedTestId }}
                  </span>
                </div>
                <input type="hidden" v-model="scheduleId" />
                <input
                  type="text"
                  name="TestName"
                  id="TestName"
                  list="testNamesList"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                  placeholder="Chọn bài kiểm tra..."
                  required
                  v-model="testNameInput"
                  @input="updateSelectedTestId"
                  :disabled="isEditMode"
                />
                <datalist id="testNamesList">
                  <option
                    v-for="test in testNames"
                    :key="test.testId"
                    :value="`${test.testName} (#${test.testId})`"
                    :data-id="test.testId"
                  ></option>
                </datalist>
              </div>

              <div class="col-span-2">
                <label
                  for="DayOpenTest"
                  class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >Thời gian bắt đầu</label
                >
                <input
                  type="datetime-local"
                  id="DayOpenTest"
                  v-model="dayOpenTest"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-500 focus:border-primary-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                  required
                />
              </div>

              <div class="col-span-2">
                <label
                  for="DayCloseTest"
                  class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >Thời gian kết thúc</label
                >
                <input
                  type="datetime-local"
                  id="DayCloseTest"
                  v-model="dayCloseTest"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-500 focus:border-primary-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                  required
                />
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
              Thêm lịch mới
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
              Sửa lịch
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
const { schedules, tests } = defineProps(["schedules", "tests"]);
const route = useRoute();
const searchQuery = ref("");
const isModalOpen = ref(false);
const isEditMode = ref(false);
const testNameInput = ref("");
const dayOpenTest = ref("");
const dayCloseTest = ref("");
const localSchedules = ref([...schedules]);
const scheduleId = ref("");
const testNames = ref([]);
const selectedTestId = ref("");
const isAdmin = route.path.includes("/admin");
import axios from "axios";

// Hàm để cập nhật selectedTestId khi testNameInput thay đổi

const getTestNames = () => {
  return tests.map((test) => ({
    testId: test.id,
    testName: test.title,
  }));
};
console.log(tests);
testNames.value = getTestNames();
const updateSelectedTestId = () => {
  // Lấy giá trị nhập vào và loại bỏ dấu ngoặc và #Id
  const cleanInput = testNameInput.value
    .trim()
    .replace(/#\d+|\(|\)/g, "")
    .trim();

  // Tìm các bài kiểm tra có testName khớp với giá trị người dùng nhập
  const matchingTests = testNames.value.filter(
    (test) => test.testName === cleanInput
  );

  // Kiểm tra nếu có các bài kiểm tra trùng tên
  if (matchingTests.length > 0) {
    // Lấy ID từ phần nhập vào
    const testIdMatch = testNameInput.value.match(/#(\d+)/);
    if (testIdMatch) {
      // Tìm bài kiểm tra có testId khớp
      const selectedTest = matchingTests.find(
        (test) => test.testId === parseInt(testIdMatch[1])
      );

      if (selectedTest) {
        // Gán selectedTestId nếu tên và ID đều khớp
        selectedTestId.value = "#" + selectedTest.testId;
        testNameInput.value = cleanInput;
      } else {
        selectedTestId.value = "Không tìm thấy bài kiểm tra";
      }
    } else {
      selectedTestId.value = "Không tìm thấy bài kiểm tra";
    }
  } else {
    selectedTestId.value = "Không tìm thấy bài kiểm tra";
  }
};

console.log(testNames);
const openModal = (scheduleToEdit = null) => {
  isModalOpen.value = true;
  if (scheduleToEdit) {
    isEditMode.value = true;
    testNameInput.value = scheduleToEdit.testName;
    selectedTestId.value = "#" + scheduleToEdit.testId;
    dayOpenTest.value = scheduleToEdit.dayOpenTest.slice(0, 16);
    dayCloseTest.value = scheduleToEdit.dayCloseTest.slice(0, 16);
    scheduleId.value = scheduleToEdit.scheduleId;
  } else {
    isEditMode.value = false;
    dayOpenTest.value = new Date(new Date().getTime() + 7 * 60 * 60 * 1000)
      .toISOString()
      .slice(0, 16);
  }
};

const closeModal = () => {
  isModalOpen.value = false;
  isEditMode.value = false;
  testNameInput.value = "";
  dayOpenTest.value = "";
  dayCloseTest.value = "";
  selectedTestId.value = "";
};
console.log(schedules);
const filteredClasses = computed(() => {
  if (!searchQuery.value) {
    return localSchedules.value;
  }
  const query = searchQuery.value.toLowerCase();
  return schedules.filter(
    (schedule) =>
      schedule.testName.toLowerCase().includes(query) ||
      schedule.testId.toString().toLowerCase().includes(query)
  );
});
const add = async () => {
  try {
    // Kiểm tra xem selectedTestId có giá trị hợp lệ không
    if (!selectedTestId.value || typeof selectedTestId.value !== "string") {
      alert("Bài kiểm tra không hợp lệ");
      return;
    }

    // Loại bỏ dấu "#" nếu có trong selectedTestId
    selectedTestId.value = selectedTestId.value.replace("#", "");

    // Kiểm tra ngày mở bài kiểm tra
    if (!dayOpenTest.value) {
      alert("Ngày mở bài kiểm tra không hợp lệ");
      return;
    }
    const dayOpenTestVietnamTime = new Date(
      new Date(dayOpenTest.value).getTime() + 7 * 60 * 60 * 1000
    );
    const currentTimeForDayOpenTest =
      dayOpenTestVietnamTime.toISOString().split(".")[0] + "Z";

    // Kiểm tra ngày đóng bài kiểm tra
    if (!dayCloseTest.value) {
      alert("Ngày đóng bài kiểm tra không hợp lệ");
      return;
    }
    const dayCloseTestVietnamTime = new Date(
      new Date(dayCloseTest.value).getTime() + 7 * 60 * 60 * 1000
    );
    const currentTimeForDayCloseTest =
      dayCloseTestVietnamTime.toISOString().split(".")[0] + "Z";

    // Kiểm tra nếu ngày đóng bài kiểm tra phải sau ngày mở
    if (
      new Date(currentTimeForDayCloseTest) <=
      new Date(currentTimeForDayOpenTest)
    ) {
      alert("Ngày đóng bài kiểm tra phải sau ngày mở");
      return;
    }

    // Tạo request
    const request = {
      TestId: selectedTestId.value,
      DayOpenTest: currentTimeForDayOpenTest,
      DayCloseTest: currentTimeForDayCloseTest,
    };
    const response = await axios.post(
      `http://localhost:5082/api/scheduleApi`,
      request
    );
    if (response.status === 200) {
      const { message, newSchedule } = response.data;
      alert(message || "Đã lịch mới thành công!");
      localSchedules.value.push(newSchedule);
      closeModal();
    } else {
      alert("Đã xảy ra lỗi thêm lịch mới!");
    }

    console.log(request);
  } catch (error) {
    // Bắt lỗi và hiển thị thông báo
    alert(`Lỗi: ${error.response ? error.response.data : error.message}`);
  }
};
const edit = async () => {
  try {
    // Kiểm tra xem selectedTestId có giá trị hợp lệ không
    if (!selectedTestId.value || typeof selectedTestId.value !== "string") {
      alert("Bài kiểm tra không hợp lệ");
      return;
    }

    // Loại bỏ dấu "#" nếu có trong selectedTestId
    selectedTestId.value = selectedTestId.value.replace("#", "");

    // Kiểm tra ngày mở bài kiểm tra
    if (!dayOpenTest.value) {
      alert("Ngày mở bài kiểm tra không hợp lệ");
      return;
    }
    const dayOpenTestVietnamTime = new Date(
      new Date(dayOpenTest.value).getTime() + 7 * 60 * 60 * 1000
    );
    const currentTimeForDayOpenTest =
      dayOpenTestVietnamTime.toISOString().split(".")[0] + "Z";

    // Kiểm tra ngày đóng bài kiểm tra
    if (!dayCloseTest.value) {
      alert("Ngày đóng bài kiểm tra không hợp lệ");
      return;
    }
    const dayCloseTestVietnamTime = new Date(
      new Date(dayCloseTest.value).getTime() + 7 * 60 * 60 * 1000
    );
    const currentTimeForDayCloseTest =
      dayCloseTestVietnamTime.toISOString().split(".")[0] + "Z";

    // Kiểm tra nếu ngày đóng bài kiểm tra phải sau ngày mở
    if (
      new Date(currentTimeForDayCloseTest) <=
      new Date(currentTimeForDayOpenTest)
    ) {
      alert("Ngày đóng bài kiểm tra phải sau ngày mở");
      return;
    }

    // Tạo request
    const request = {
      TestId: selectedTestId.value,
      DayOpenTest: currentTimeForDayOpenTest,
      DayCloseTest: currentTimeForDayCloseTest,
    };
    console.log(request);
    const response = await axios.put(
      `http://localhost:5082/api/scheduleApi?id=${scheduleId.value}`,
      request
    );
    console.log(request);
    if (response.status === 200) {
      const { message } = response.data;
      alert(message || "Đã sửa lịch thành công!");
      window.location.reload();
    } else {
      alert(`Đã xảy ra lỗi chỉnh sửa lịch !`);
    }
  } catch (error) {
    // Bắt lỗi và hiển thị thông báo
    alert(`Lỗi: ${error.response ? error.response.data : error.message}`);
  }
};
const deleteSchedule = async (scheduleIdDelete) => {
  // Sử dụng setTimeout để không chặn luồng chính
  setTimeout(async () => {
    const isConfirmed = confirm("Bạn có chắc chắn muốn xóa lịch này?");
    if (!isConfirmed) {
      return; // Người dùng hủy, dừng xử lý
    }

    try {
      console.log("Class id: " + scheduleIdDelete);

      // Gửi yêu cầu DELETE đến API
      const response = await axios.delete(
        `http://localhost:5082/api/scheduleApi?id=${scheduleIdDelete}`
      );

      // Kiểm tra phản hồi từ API
      if (response.status === 200) {
        const { message } = response.data;
        alert(message || "Đã xóa lịch thành công!");

        // Cập nhật danh sách localSchedules
        localSchedules.value = localSchedules.value.filter(
          (scheduleItem) => scheduleItem.scheduleId !== scheduleIdDelete
        );
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
  }, 0); // Đẩy xử lý vào hàng đợi sự kiện
};
</script>

<style lang="scss" scoped></style>
