export const useAuth = () => {
  const config = useRuntimeConfig();

  const accessToken = useCookie('access_token', { maxAge: 60 * 60 });
  const refreshToken = useCookie('refresh_token', { maxAge: 60 * 60 * 24 * 30 });
  const pkceVerifier = useCookie('pkce_verifier', { maxAge: 60 * 5 });

  const generateCodeVerifier = () => {
    const array = new Uint8Array(32);
    crypto.getRandomValues(array);
    return btoa(String.fromCharCode(...array))
      .replace(/\+/g, '-')
      .replace(/\//g, '_')
      .replace(/=+$/, '');
  };

  const generateCodeChallenge = async (verifier) => {
    const encoder = new TextEncoder();
    const data = encoder.encode(verifier);
    const digest = await crypto.subtle.digest('SHA-256', data);
    return btoa(String.fromCharCode(...new Uint8Array(digest)))
      .replace(/\+/g, '-')
      .replace(/\//g, '_')
      .replace(/=+$/, '');
  };

  const decodeJwtPayload = (token) => {
    try {
      const payload = token.split('.')[1];
      const padded = payload + '='.repeat((4 - (payload.length % 4)) % 4);
      const decoded = atob(padded.replace(/-/g, '+').replace(/_/g, '/'));
      return JSON.parse(decoded);
    } catch {
      return null;
    }
  };

  const isTokenExpired = (token) => {
    const payload = decodeJwtPayload(token);
    if (!payload?.exp) return true;
    return Date.now() >= payload.exp * 1000;
  };

  const login = async () => {
    const verifier = generateCodeVerifier();
    const challenge = await generateCodeChallenge(verifier);

    pkceVerifier.value = verifier;

    const params = new URLSearchParams({
      response_type: 'code',
      client_id: config.public.keycloakClientId,
      redirect_uri: config.public.keycloakRedirectUri,
      scope: 'openid profile email organization',
      code_challenge: challenge,
      code_challenge_method: 'S256'
    });

    await navigateTo(`${config.public.keycloakAuthUrl}?${params.toString()}`, {
      external: true
    });
  };

  const handleCallback = async (code) => {
    const verifier = pkceVerifier.value;
    if (!verifier) throw new Error('PKCE verifier not found. Please try logging in again.');

    const userStore = useUserStore();
    const response = await userStore.exchangeCode(code, verifier);

    accessToken.value = response.accessToken;
    refreshToken.value = response.refreshToken;

    const payload = decodeJwtPayload(response.accessToken);
    if (payload) userStore.setFromTokenPayload(payload);

    pkceVerifier.value = null;

    return response;
  };

  const refresh = async () => {
    if (!refreshToken.value) throw new Error('No refresh token available.');

    const userStore = useUserStore();
    const response = await userStore.refreshToken(refreshToken.value);

    accessToken.value = response.accessToken;
    refreshToken.value = response.refreshToken;
    const payload = decodeJwtPayload(response.accessToken);

    console.log(payload)
    if (payload) userStore.setFromTokenPayload(payload);

    return response;
  };

  const logout = () => {
    accessToken.value = null;
    refreshToken.value = null;
    const userStore = useUserStore();
    userStore.clear();
  };

  const isAuthenticated = computed(() => {
    return !!accessToken.value && !isTokenExpired(accessToken.value);
  });

  return {
    accessToken,
    refreshToken,
    isAuthenticated,
    login,
    handleCallback,
    refresh,
    logout,
    isTokenExpired,
    decodeJwtPayload
  };
};
