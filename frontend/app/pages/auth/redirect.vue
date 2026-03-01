<template>
  <div class="min-h-screen flex items-center justify-center">
    <VisualsAnimatedGradient />

    <Card style="width: 25rem; overflow: hidden; position: relative; z-index: 1;">
      <template #title>
        <span v-if="error">{{ t('authRedirectTitleError') }}</span>
        <span v-else>{{ t('authRedirectTitleLoading') }}</span>
      </template>
      <template #subtitle>
        <span v-if="error">{{ t('authRedirectSubtitleError') }}</span>
        <span v-else>{{ t('authRedirectSubtitleLoading') }}</span>
      </template>
      <template #content>
        <div v-if="error" class="flex flex-col items-center gap-3 py-2">
          <VisualsIcon icon="alert-diamond" size="2em" color="red" />
          <p class="text-red-500 text-sm text-center m-0">{{ error }}</p>
        </div>
        <div v-else class="flex flex-col items-center gap-3 py-2">
          <ProgressSpinner style="width: 2.5rem; height: 2.5rem" strokeWidth="4" />
          <p class="text-sm text-center m-0 text-surface-500">
            {{ t('authRedirectBody') }}
          </p>
        </div>
      </template>
      <template v-if="error" #footer>
        <div class="flex gap-4 mt-1">
          <InputButton
            :label="t('authRedirectRetry')"
            class="w-full"
            @click="retry"
          />
        </div>
      </template>
    </Card>
  </div>
</template>

<script setup>
const { t } = useI18n();
const route = useRoute();
const { handleCallback } = useAuth();

const error = ref(null);

const retry = () => navigateTo("/");

onMounted(async () => {
  const code = route.query.code;

  if (!code) {
    error.value = "No authorization code received from Keycloak.";
    return;
  }

  try {
    await handleCallback(code);
    await navigateTo('/');
  } catch (err) {
    error.value = err?.message ?? "Unknown error during token exchange.";
  }
});
</script>
