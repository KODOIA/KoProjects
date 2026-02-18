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
          :formContext="$form"
        />

        <AtomsInputText
          name="lastName"
          placeholder="Nachname"
          :rules="[rules.required, rules.max64Characters]"
          :formContext="$form"
        />

        <AtomsInputText
          name="email"
          type="email"
          placeholder="E-Mail"
          :rules="[rules.required, rules.email]"
          :formContext="$form"
        />

        <AtomsInputText
          name="phone"
          placeholder="Telefon (optional)"
          :rules="[rules.phone]"
          :formContext="$form"
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

    <div class="mb-8 p-6 border rounded-lg">
      <h2 class="text-xl font-semibold mb-4">Registrierung</h2>
      
      <AtomsForm 
        ref="registrationForm"
        @submit="handleRegistrationSubmit"
        v-slot="{ form: $form }"
      >
        <AtomsInputText
          name="username"
          placeholder="Benutzername"
          :rules="[rules.required, rules.minLength(3), rules.maxLength(20), rules.alphaNumeric]"
          :formContext="$form"
        />

        <AtomsInputText
          name="email"
          type="email"
          placeholder="E-Mail"
          :rules="[rules.required, rules.email]"
          :formContext="$form"
        />

        <AtomsInputText
          name="password"
          type="password"
          placeholder="Passwort"
          :rules="[rules.required, rules.strongPassword]"
          :formContext="$form"
        />

        <AtomsInputText
          name="confirmPassword"
          type="password"
          placeholder="Passwort bestätigen"
          :rules="[rules.required, rules.sameAsField('password', 'Passwort')]"
          :formContext="$form"
        />

        <AtomsButton 
          type="submit" 
          label="Registrieren" 
          severity="success"
        />
      </AtomsForm>

      <ClientOnly>
        <div v-if="registrationSubmitted" class="mt-4 p-4 bg-green-100 text-green-800 rounded">
          ✓ Registrierung erfolgreich!
        </div>
      </ClientOnly>
    </div>

    <div class="p-6 border rounded-lg">
      <h2 class="text-xl font-semibold mb-4">Weitere Validierungsbeispiele</h2>
      
      <AtomsForm 
        ref="advancedForm"
        @submit="handleAdvancedSubmit"
        v-slot="{ form: $form }"
      >
        <AtomsInputText
          name="onlyLetters"
          placeholder="Nur Buchstaben"
          :rules="[rules.required, rules.alpha]"
          :formContext="$form"
        />

        <AtomsInputText
          name="onlyNumbers"
          placeholder="Nur Zahlen"
          :rules="[rules.required, rules.numeric]"
          :formContext="$form"
        />

        <AtomsInputText
          name="age"
          type="number"
          placeholder="Alter (18-120)"
          :rules="[rules.required, rules.numeric, rules.minValue(18), rules.maxValue(120)]"
          :formContext="$form"
        />

        <AtomsInputText
          name="website"
          placeholder="Website (optional)"
          :rules="[rules.url]"
          :formContext="$form"
        />

        <AtomsButton 
          type="submit" 
          label="Validieren" 
          severity="info"
        />
      </AtomsForm>

      <ClientOnly>
        <div v-if="advancedSubmitted" class="mt-4 p-4 bg-green-100 text-green-800 rounded">
          ✓ Alle Validierungen bestanden!
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
