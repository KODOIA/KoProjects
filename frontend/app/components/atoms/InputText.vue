<template>
  <div class="flex flex-col gap-1">
    <IconField v-if="prependIcon || appendIcon">
      <InputIcon 
        v-if="prependIcon"
        :class="{ 'cursor-pointer': hasPrependIconClick }"
        :style="{ pointerEvents: hasPrependIconClick ? 'auto' : 'none', zIndex: 10 }"
        @click="handlePrependIconClick"
      >
        <AtomsIcon :icon="prependIcon" :color="prependIconColor" />
      </InputIcon>

      <InputText
        :name="name"
        :type="type"
        :size="size"
        :placeholder="placeholder"
        :disabled="disabled"
        fluid
      />

      <InputIcon 
        v-if="appendIcon"
        :class="{ 'cursor-pointer': hasAppendIconClick }"
        :style="{ pointerEvents: hasAppendIconClick ? 'auto' : 'none', zIndex: 10 }"
        @click="handleAppendIconClick"
      >
        <AtomsIcon :icon="appendIcon" :color="appendIconColor" />
      </InputIcon>
    </IconField>

    <InputText
      v-else
      :name="name"
      :type="type"
      :size="size"
      :placeholder="placeholder"
      :disabled="disabled"
      fluid
    />

    <Message
      v-if="formContext && formContext[name]?.invalid"
      severity="error"
      size="small"
      variant="simple"
    >
      {{ formContext[name].error?.message }}
    </Message>
  </div>
</template>

<script setup>
const props = defineProps({
  name: {
    type: String,
    required: true,
  },
  type: {
    type: String,
    default: "text",
  },
  size: {
    type: String,
    default: "small",
  },
  placeholder: {
    type: String,
    default: "",
  },
  disabled: {
    type: Boolean,
    default: false,
  },
  rules: {
    type: Array,
    default: () => [],
  },
  formContext: {
    type: Object,
    default: null,
  },
  prependIcon: {
    type: String,
    default: null,
  },
  appendIcon: {
    type: String,
    default: null,
  },
  prependIconColor: {
    type: String,
  },
  appendIconColor: {
    type: String,
  },
});

const emit = defineEmits(['click:prepend-icon', 'click:append-icon']);

const registerField = inject("registerField", null);
const instance = getCurrentInstance();

const hasPrependIconClick = computed(() => {
  return !!instance?.vnode?.props?.['onClick:prependIcon'];
});

const hasAppendIconClick = computed(() => {
  return !!instance?.vnode?.props?.['onClick:appendIcon'];
});

const handlePrependIconClick = (event) => {
  event.stopPropagation();
  emit('click:prepend-icon', event);
};

const handleAppendIconClick = (event) => {
  event.stopPropagation();
  emit('click:append-icon', event);
};

onMounted(() => {
  if (registerField && props.rules.length > 0) {
    registerField(props.name, props.rules);
  }
});
</script>
