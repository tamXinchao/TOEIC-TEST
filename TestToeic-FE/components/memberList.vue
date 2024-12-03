<template>
  <div class="max-w-full mx-auto p-8 bg-white rounded-lg shadow-md">
    <h2 class="mb-4 text-2xl font-semibold leading-tight">Thành viên</h2>
    <div class="overflow-x-auto">
      <table class="min-w-full text-xs">
        <colgroup>
          <col />
          <col />
          <col />
          <col />
          <col class="w-24" />
        </colgroup>
        <thead class="dark:bg-gray-300">
          <tr class="text-left">
            <th class="p-3">#</th>
            <th class="p-3">Id</th>
            <th class="p-3">Tên</th>
            <th class="p-3 text-left whitespace-nowrap">Ngày tham gia</th>
            <th class="p-3">Trạng thái</th>
          </tr>
        </thead>
        <tbody>
          <tr
            class="border-b border-opacity-20 dark:border-gray-300 dark:bg-gray-50"
            v-for="(member, index) in members"
            :key="member.memberOfClassId"
          >
            <td class="p-3">
              <p>
                <span class="font-bold">#{{ index + 1 }}</span>
              </p>
            </td>
            <td class="p-3">
              <p>{{ member.memberOfClassId }}</p>
            </td>
            <td class="p-3">
              <p>{{ member.memberName }}</p>
            </td>
            <td class="p-3 text-right">
              <p class="dark:text-gray-600">
                {{ member.getFormattedJoinDate }}
              </p>
            </td>
            <td class="p-3 text-right flex space-x-2 whitespace-nowrap">
              <!-- Sử dụng flex để căn chỉnh các phần tử con -->
              <span
                v-if="member.isActive"
                class="px-3 py-1 font-semibold rounded-md whitespace-nowrap dark:bg-green-600 dark:text-gray-50"
              >
                Đã tham gia
              </span>
              <button
                v-if="!member.isActive"
                @click="acceptMember(member.memberId, member.classId)"
                class="px-3 py-1 font-semibold rounded-md dark:bg-green-600 dark:text-gray-50 hover:bg-green-700"
              >
                Duyệt
              </button>
              <button
                v-if="!member.isActive"
                @click="rejectMember(member.memberId, member.classId)"
                class="px-3 py-1 font-semibold rounded-md dark:bg-red-600 dark:text-gray-50 hover:bg-red-700"
              >
                Từ chối
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";

const { members } = defineProps(["members"]);

const acceptMember = async (memberId, classId) => {
  try {
    const response = await axios.put(
      `http://localhost:5082/api/MemberOfClassApi/acceptMember/${memberId}?classId=${classId}`
    );
    console.log(response.data.Message);

    // Cập nhật trạng thái của thành viên trong danh sách
    const memberIndex = members.findIndex(
      (member) => member.memberId === memberId
    );
    if (memberIndex !== -1) {
      members[memberIndex].isActive = true;
    }
  } catch (error) {
    console.error("Lỗi khi duyệt thành viên:", error);
  }
};

const rejectMember = async (memberId, classId) => {
  try {
    const response = await axios.delete(
      `http://localhost:5082/api/MemberOfClassApi/rejectMember/${memberId}?classId=${classId}` // Thay thế classId theo đúng logic
    );
    console.log(response.data.Message);

    // Xoá thành viên khỏi danh sách
    const memberIndex = members.findIndex((m) => m.memberId === memberId);
    if (memberIndex !== -1) {
      members.splice(memberIndex, 1); // Xoá thành viên khỏi danh sách
    }
  } catch (error) {
    console.error("Lỗi khi từ chối thành viên:", error);
  }
};
</script>

<style scoped>
/* Không cần thêm nhiều kiểu, Tailwind đã đủ mạnh mẽ để tạo kiểu tốt */
</style>
