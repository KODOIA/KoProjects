export default defineNuxtRouteMiddleware(async (to) => {
  const publicRoutes = ["/auth/redirect", "/public"];
  if (publicRoutes.includes(to.path)) return;

  const accessToken = useCookie("access_token");
  const refreshToken = useCookie("refresh_token");
});
