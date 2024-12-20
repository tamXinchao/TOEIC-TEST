export default defineNuxtRouteMiddleware(async (to, from) => {
  const { $auth } = useNuxtApp();
  console.log("Checking auth...");
  const isAuthenticated = await $auth.checkAuth();
  const isTeacher = $auth.role.value;
  console.log(3);

  console.log("Is user authenticated:", isAuthenticated);
  console.log("User role:", isTeacher);
  console.log(3);

  if (!isAuthenticated) {
    return navigateTo("/login");
  }
  if (isTeacher != "teacher") {
    return navigateTo("/");
  }
});
