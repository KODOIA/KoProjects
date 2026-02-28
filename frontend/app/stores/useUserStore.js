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
