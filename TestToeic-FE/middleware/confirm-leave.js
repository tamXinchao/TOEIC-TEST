export default function (context) {
  if (
    context.route.name === "yourRouteName" &&
    context.store.state.unsavedChanges
  ) {
    const answer = window.confirm(
      "Bạn có chắc muốn rời khỏi trang này? Thay đổi sẽ không được lưu lại."
    );
    if (!answer) {
      // Ngừng chuyển trang
      context.redirect(context.route.fullPath);
    }
  }
}
