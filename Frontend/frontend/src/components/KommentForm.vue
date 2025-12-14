<script setup>
import { ref, defineProps, defineEmits } from 'vue';
import axios from 'axios';

// -------------------------------------------------------------------
// PROPS (Bemeneti adatok a szülőtől)
// -------------------------------------------------------------------

// A szülő (Tema.vue) komponens átadja a Tema ID-jét, amihez a kommentet fűzzük.
const props = defineProps({
  temaId: {
    type: Number,
    required: true
  }
});

// -------------------------------------------------------------------
// EMITS (Kimeneti események a szülőnek)
// -------------------------------------------------------------------

// Esemény, amit akkor adunk ki, ha sikeresen elküldtük a kommentet.
// Ezt fogja a szülő kezelni a témák frissítéséhez.
const emit = defineEmits(['kommentHozzaadva']);

// -------------------------------------------------------------------
// STATE (Komponens belső állapota)
// -------------------------------------------------------------------

const felhasznaloNev = ref('');
const szoveg = ref('');
const isHiba = ref(false);
const isSikeres = ref(false);

// -------------------------------------------------------------------
// LOGIKA
// -------------------------------------------------------------------

const postKomment = async () => {
  isHiba.value = false;
  isSikeres.value = false;

  // Alapvető validáció
  if (felhasznaloNev.value.trim() === '' || szoveg.value.trim() === '') {
    isHiba.value = true;
    return;
  }

  const kommentData = {
    temaId: props.temaId,
    felhasznaloNev: felhasznaloNev.value,
    szoveg: szoveg.value,
  };

  try {
    // API hívás a C# backend új végpontjára
    await axios.post('https://turbo-space-broccoli-xv5445qwx9hpwrp-5178.app.github.dev/api/Temak/Komment', kommentData);
    
    isSikeres.value = true;
    
    // Töröljük az űrlap tartalmát
    szoveg.value = '';
    
    // Értesítsük a szülő komponenst, hogy frissítheti a témát
    emit('kommentHozzaadva');

  } catch (error) {
    console.error('Hiba a komment elküldésekor:', error);
    isHiba.value = true;
  }
};
</script>

<template>
  <div class="p-3 border rounded bg-white">
    <h6 class="mb-3">Új hozzászólás írása</h6>

    <form @submit.prevent="postKomment">
      <div class="mb-3">
        <label for="felhasznaloNev" class="form-label">Név:</label>
        <input 
          type="text" 
          id="felhasznaloNev" 
          v-model="felhasznaloNev" 
          required 
          maxlength="50"
          placeholder="Pl. Gipsz Jakab"
          class="form-control"
        />
      </div>

      <div class="mb-3">
        <label for="szoveg" class="form-label">Komment:</label>
        <textarea 
          id="szoveg" 
          v-model="szoveg" 
          required 
          maxlength="500"
          class="form-control"
        ></textarea>
      </div>

      <div v-if="isHiba" class="alert alert-danger" role="alert">
        Hiba történt, vagy minden mező kitöltése kötelező!
      </div>
      <div v-if="isSikeres" class="alert alert-success" role="alert">
        A hozzászólás sikeresen elküldve!
      </div>

      <button type="submit" class="btn btn-success w-100">
        Komment elküldése
      </button>
    </form>
  </div>
</template>

<style scoped>

</style>