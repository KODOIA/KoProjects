<template>
  <div class="p-8 max-w-2xl mx-auto">
    <h1 class="text-3xl font-bold mb-6">Formular-Beispiel</h1>

    <div class="mb-8 p-6 border rounded-lg">
      <h2 class="text-xl font-semibold mb-4">Kontaktformular</h2>
      
      <AtomsForm 
        ref="contactForm"
        @submit="handleContactSubmit"
        @error="handleContactError"
        v-slot="{ form: $form }"
      >
        <AtomsInputText
          name="firstName"
          placeholder="Vorname"
          :rules="[rules.required, rules.max64Characters]"
          :form-context="$form"
          prepend-icon="arrow-left-up-linear"
          @click:prepend-icon="test1"
        />

        <AtomsInputText
          name="lastName"
          placeholder="Nachname"
          :rules="[rules.required, rules.max64Characters]"
          :form-context="$form"
          append-icon="arrow-right-down-linear"
          @click:append-icon="test2"
        />

        <AtomsInputText
          name="email"
          type="email"
          placeholder="E-Mail"
          :rules="[rules.required, rules.email]"
          :form-context="$form"
        />

        <AtomsInputText
          name="phone"
          placeholder="Telefon (optional)"
          :rules="[rules.phone]"
          :form-context="$form"
        />

        <AtomsButton 
          type="submit" 
          label="Absenden" 
          severity="primary"
        />
      </AtomsForm>

      <ClientOnly>
        <div v-if="contactSubmitted" class="mt-4 p-4 bg-green-100 text-green-800 rounded">
          ✓ Formular erfolgreich abgesendet!
        </div>
      </ClientOnly>
    </div>
  </div>
</template>

<script setup>
const { rules } = useFormRules();

// Kontaktformular
const contactSubmitted = ref(false);

const handleContactSubmit = (values) => {
  console.log('Kontaktformular abgesendet:', values);
  contactSubmitted.value = true;
  
  // Verstecke Erfolgsmeldung nach 3 Sekunden
  setTimeout(() => {
    contactSubmitted.value = false;
  }, 3000);
};

const test1 = () => {
  console.log('Prepend-Icon geklickt');
};

const test2 = () => {
  console.log('Append-Icon geklickt');
};

const handleContactError = (errors) => {
  console.log('Kontaktformular Fehler:', errors);
};

// Registrierungsformular
const registrationSubmitted = ref(false);

const handleRegistrationSubmit = (values) => {
  console.log('Registrierung abgesendet:', values);
  registrationSubmitted.value = true;
  
  setTimeout(() => {
    registrationSubmitted.value = false;
  }, 3000);
};

// Erweitertes Formular
const advancedSubmitted = ref(false);

const handleAdvancedSubmit = (values) => {
  console.log('Erweitertes Formular abgesendet:', values);
  advancedSubmitted.value = true;
  
  setTimeout(() => {
    advancedSubmitted.value = false;
  }, 3000);
};
</script>
