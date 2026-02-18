<template>
  <Button
    :label="label"
    :type="type"
    :size="size"
    :severity="severity"
    :disabled="disabled"
    :loading="loading"
    :icon="icon"
    :iconPos="iconPos"
    @click="handleClick"
  />
</template>

<script setup>
const props = defineProps({
  label: {
    type: String,
    default: '',
  },
  type: {
    type: String,
    default: 'button',
    validator: (value) => ['button', 'submit', 'reset'].includes(value),
  },
  size: {
    type: String,
    default: 'small',
    validator: (value) => ['small', 'large'].includes(value),
  },
  severity: {
    type: String,
    default: 'secondary',
    validator: (value) => ['primary', 'secondary', 'success', 'info', 'warn', 'help', 'danger', 'contrast'].includes(value),
  },
  disabled: {
    type: Boolean,
    default: false,
  },
  loading: {
    type: Boolean,
    default: false,
  },
  icon: {
    type: String,
    default: '',
  },
  iconPos: {
    type: String,
    default: 'left',
    validator: (value) => ['left', 'right', 'top', 'bottom'].includes(value),
  },
});

const emit = defineEmits(['click']);

const handleClick = (event) => {
  if (!props.disabled && !props.loading) {
    emit('click', event);
  }
};
</script>
