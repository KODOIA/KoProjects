
export const useFormRules = () => {
  const { t } = useI18n();
  
  const rules = {
    required: (value) => {
      if (!value || value.toString().trim() === '') {
        return t('required');
      }
      return true;
    },
    email: (value) => {
      if (!value) return true;
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(value)) {
        return t('email');
      }
      return true;
    },
    minLength: (min) => (value) => {
      if (!value) return true;
      if (value.length < min) {
        return t('minLength', { min });
      }
      return true;
    },
    maxLength: (max) => (value) => {
      if (!value) return true;
      if (value.length > max) {
        return t('maxLength', { max });
      }
      return true;
    },
    max64Characters: (value) => {
      if (!value) return true;
      if (value.length > 64) {
        return t('max64Characters');
      }
      return true;
    },
    max128Characters: (value) => {
      if (!value) return true;
      if (value.length > 128) {
        return t('max128Characters');
      }
      return true;
    },
    max255Characters: (value) => {
      if (!value) return true;
      if (value.length > 255) {
        return t('max255Characters');
      }
      return true;
    },
    numeric: (value) => {
      if (!value) return true;
      if (!/^\d+$/.test(value)) {
        return t('numeric');
      }
      return true;
    },
    alpha: (value) => {
      if (!value) return true;
      if (!/^[a-zA-ZäöüÄÖÜß\s]+$/.test(value)) {
        return t('alpha');
      }
      return true;
    },
    alphaNumeric: (value) => {
      if (!value) return true;
      if (!/^[a-zA-Z0-9äöüÄÖÜß\s]+$/.test(value)) {
        return t('alphaNumeric');
      }
      return true;
    },
    phone: (value) => {
      if (!value) return true;
      const phoneRegex = /^[\d\s\+\-\(\)]+$/;
      if (!phoneRegex.test(value)) {
        return t('phone');
      }
      return true;
    },
    url: (value) => {
      if (!value) return true;
      try {
        new URL(value);
        return true;
      } catch {
        return t('url');
      }
    },
    strongPassword: (value) => {
      if (!value) return true;
      if (value.length < 8) {
        return t('passwordMinLength');
      }
      if (!/[A-Z]/.test(value)) {
        return t('passwordUppercase');
      }
      if (!/[a-z]/.test(value)) {
        return t('passwordLowercase');
      }
      if (!/\d/.test(value)) {
        return t('passwordNumber');
      }
      return true;
    },
    sameAs: (otherValue, fieldName = 'das andere Feld') => (value) => {
      if (value !== otherValue) {
        return t('sameAs', { fieldName });
      }
      return true;
    },
    sameAsField: (otherFieldName, fieldLabel = 'das andere Feld') => (value, allValues) => {
      if (value !== allValues?.[otherFieldName]) {
        return t('sameAs', { fieldName: fieldLabel });
      }
      return true;
    },
    minValue: (min) => (value) => {
      if (!value) return true;
      if (Number(value) < min) {
        return t('minValue', { min });
      }
      return true;
    },
    maxValue: (max) => (value) => {
      if (!value) return true;
      if (Number(value) > max) {
        return t('maxValue', { max });
      }
      return true;
    },
  };

  return {
    rules,
  };
};
