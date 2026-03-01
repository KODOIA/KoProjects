<template>
  <Form
    v-slot="$form"
    :resolver="customResolver"
    @submit="handleSubmit"
    :class="formClass"
  >
    <slot :form="$form" />
  </Form>
</template>

<script setup>
const props = defineProps({
  formClass: {
    type: String,
    default: 'flex flex-col gap-4',
  },
});

const emit = defineEmits(['submit', 'error']);
const fieldRules = ref({});
const formValues = ref({});
const registerField = (fieldName, rules) => {
  fieldRules.value[fieldName] = rules || [];
};

const customResolver = ({ values }) => {
  const errors = {};
  formValues.value = values;

  for (const [fieldName, rules] of Object.entries(fieldRules.value)) {
    const value = values[fieldName];
    
    for (const rule of rules) {
      const result = rule(value, values);
      
      if (typeof result === 'string') {
        errors[fieldName] = [{ message: result }];
        break;
      }
    }
  }

  return {
    values,
    errors,
  };
};

const handleSubmit = (event) => {
  if (event.valid) {
    emit('submit', event.values);
  } else {
    emit('error', event.errors);
  }
};

provide('registerField', registerField);
provide('formValues', formValues);
</script>
