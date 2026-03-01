export const useUserStore = defineStore('user', {
  state: () => ({
    id: null,
    name: null,
    givenName: null,
    familyName: null,
    preferredUsername: null,
    email: null,
  }),
  actions: {
    async exchangeCode(code, codeVerifier) {
      const config = useRuntimeConfig();
      return await $fetch(`${config.public.apiBaseUrl}/api/token`, {
        method: 'POST',
        body: { grantType: 'authorization_code', code, codeVerifier },
        headers: { 'Content-Type': 'application/json' }
      });
    },
    async refreshToken(token) {
      const config = useRuntimeConfig();
      return await $fetch(`${config.public.apiBaseUrl}/api/token`, {
        method: 'POST',
        body: { grantType: 'refresh_token', refreshToken: token }
      });
    },
    setFromTokenPayload(payload) {
      this.id = payload.sub ?? null;
      this.name = payload.name ?? null;
      this.givenName = payload.given_name ?? null;
      this.familyName = payload.family_name ?? null;
      this.preferredUsername = payload.preferred_username ?? null;
      this.email = payload.email ?? null;
    },
    clear() {
      this.$reset();
    },
  },
});
