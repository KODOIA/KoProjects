import Aura from "@primeuix/themes/aura";

export default defineNuxtConfig({
  css: ['./app/assets/css/tailwind.css'],
  modules: [
    "@primevue/nuxt-module",
    "@nuxtjs/tailwindcss",
    "@pinia/nuxt",
    "@nuxtjs/i18n",
    "@nuxt/icon",
  ],
  primevue: {
    options: {
      theme: {
        preset: Aura,
      },
    },
    usePrimeVue: true
  },
  i18n: {
    defaultLocale: 'en',
    locales: [
      { code: 'en', name: 'English', file: 'en.json' }
    ]
  },
  compatibilityDate: "2025-07-15",
  devtools: { enabled: false },
});