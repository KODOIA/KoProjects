<template>
  <div class="flex flex-col gap-1">
    <InputText
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
    default: 'text',
  },
  size: {
    type: String,
    default: 'small',
  },
  placeholder: {
    type: String,
    default: '',
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
});

const registerField = inject('registerField', null);

onMounted(() => {
  if (registerField && props.rules.length > 0) {
    registerField(props.name, props.rules);
  }
});
</script>
