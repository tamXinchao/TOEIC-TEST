<template>
  <div>
    <section class="container mx-auto p-6 font-mono">
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
                <th class="px-4 py-3">Hành động</th>
              </tr>
            </thead>
            <tbody class="bg-white">
              <tr
                class="text-gray-700"
                v-for="(Schedule, index) in schedules"
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
                            : "Lớp: " + Schedule.className
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
                <td class="px-4 py-3 text-xs border">
                  <NuxtLink :to="'#'">
                    <span
                      class="mx-2 px-2 py-1 font-semibold leading-tight text-green-700 bg-green-100 rounded-sm"
                    >
                      Chỉnh sửa
                    </span>
                  </NuxtLink>
                  <NuxtLink :to="'#'">
                    <span
                      class="px-2 py-1 font-semibold leading-tight text-yellow-700 bg-green-100 rounded-sm"
                    >
                      Xóa
                    </span>
                  </NuxtLink>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
const { schedules, index } = defineProps(["schedules", "index"]);
</script>

<style lang="scss" scoped></style>
