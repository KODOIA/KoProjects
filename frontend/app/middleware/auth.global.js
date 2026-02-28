export default defineNuxtRouteMiddleware(async (to) => {
  const publicRoutes = ["/auth/redirect"];
  if (publicRoutes.includes(to.path)) return;
  if (import.meta.server) return;

  const { accessToken, refreshToken, isTokenExpired, decodeJwtPayload, refresh, login, logout } = useAuth();

  if (accessToken.value && !isTokenExpired(accessToken.value)) {
    const userStore = useUserStore();
    if (!userStore.id) {
      const payload = decodeJwtPayload(accessToken.value);
      if (payload) userStore.setFromTokenPayload(payload);
    }
    return;
  }

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
