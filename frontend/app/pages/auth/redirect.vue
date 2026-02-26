<template>
  <div v-if="error" class="p-8">
    <p class="text-red-500">Authentication failed: {{ error }}</p>
    <button class="mt-4" @click="retry">Try again</button>
  </div>
  <p v-else>Redirecting...</p>
</template>

<script setup>
const route = useRoute();
const { handleCallback } = useAuth();

const error = ref(null);

const retry = () => navigateTo('/');

onMounted(async () => {
  const code = route.query.code;

  if (!code) {
    error.value = 'No authorization code received from Keycloak.';
    return;
  }

  try {
    await handleCallback(code);
    await navigateTo('/');
  } catch (err) {
    error.value = err?.message ?? 'Unknown error during token exchange.';
  }
});
</script>