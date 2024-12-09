<template>
  <div class="flex">
    <!-- Component MemberList nằm bên trái -->
    <div class="flex-1 p-4">
      <MemberList :members="members" />
    </div>
    <!-- Component ClassDetail nằm bên phải -->
    <div class="flex-2 p-4">
      <ClassDetail :classes="classes" :stickers="stickers" />
    </div>
  </div>
</template>

<script setup>
definePageMeta({ layout: "admin" });
import { ref, onMounted } from "vue";
import axios from "axios";

const classes = ref({});
const members = ref([]);
const stickers = ref([]);

const route = useRoute();
const classId = route.params.classId;

onMounted(async () => {
  try {
    // Lấy thông tin lớp
    const { data } = await axios.get(
      `http://localhost:5082/api/TestApi/listByClass?id=${classId}`
    );
    classes.value = data;
    // Lấy danh sách thành viên
    const { data: memberData } = await axios.get(
      `http://localhost:5082/api/MemberOfClassApi/getByClass?classId=${classId}`
    );
    members.value = memberData;
    const { data: stickerData } = await axios.get(
      `http://localhost:5082/api/StickerApi`
    );
    stickers.value = stickerData;
    console.log(data); // Gán dữ liệu cho members
  } catch (stickerData) {
    console.error("Error fetching data:", error);
  }
});
</script>

<style scoped>
/* Đảm bảo rằng các component con sẽ căn lề và có đủ không gian */
.flex {
  display: flex;
  gap: 10px; /* Khoảng cách giữa MemberList và ClassDetail */
}

.flex-1 {
  flex: 1; /* Điều chỉnh tỷ lệ không gian cho phần tử bên trái */
}

.flex-2 {
  flex: 2; /* Điều chỉnh tỷ lệ không gian cho phần tử bên phải */
}
</style>
