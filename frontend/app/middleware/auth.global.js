export default defineNuxtRouteMiddleware(async (to) => {
  const publicRoutes = ["/auth/redirect"];
  if (publicRoutes.includes(to.path)) return;
  if (import.meta.server) return;

  const { accessToken, refreshToken, isTokenExpired, refresh, login, logout } = useAuth();

  if (accessToken.value && !isTokenExpired(accessToken.value)) return;

  if (refreshToken.value) {
    try {
      await refresh();
      return;
    } catch {
      logout();
    }
  }

  await login();
});
