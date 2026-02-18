<template>
  <InputText
    type="text"
    :size="size"
    :placeholder="placeholder"
    :disabled="disabled"
    :name="name"
  />
</template>

<script setup>
defineProps({
  size: {
    type: String,
    required: false,
    default: "small",
  },
  placeholder: {
    type: String,
    required: false,
    default: "Placeholder",
  },
  disabled: {
    type: Boolean,
    required: false,
    default: false,
  },
  name: {
    type: String,
    required: false,
  },
});
</script>
