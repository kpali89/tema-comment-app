<template>
  <div class="container">
    <h1>Téma-Komment Alkalmazás</h1>
    <div class="new-tema-form">
      <h3>Új Téma Létrehozása</h3>
      <form @submit.prevent="submitNewTema">
        <input 
          type="text" 
          v-model="newTemaTitle" 
          placeholder="Írd be az új téma címét" 
          required 
          maxlength="100"
        />
        <button type="submit" :disabled="isPosting">{{ isPosting ? 'Mentés...' : 'Téma Küldése' }}</button>
      </form>
    </div>
    <div v-if="loading" class="loading">Adatok betöltése...</div>
    <div v-else-if="error" class="error">Hiba történt: {{ error.message }}</div>

    <div v-else>
      <div v-for="tema in temak" :key="tema.id" class="tema-card">
        <h2>{{ tema.cim }}</h2>
        <div class="komment-szamlalo">{{ tema.kommentek.length }} komment</div>
        
        <div v-if="tema.kommentek.length > 0" class="komment-lista">
          <h3>Kommentek:</h3>
          <div v-for="komment in tema.kommentek" :key="komment.id" class="komment">
            <p><strong>{{ komment.felhasznaloNev }}</strong> ({{ formatDate(komment.datum) }})</p>
            <p>{{ komment.szoveg }}</p>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';

// ----------------------------------------------------
// KONSTANS: C# API CÍME
// ----------------------------------------------------
// A Codespaces futtatásakor ez a port a publikus címen lesz elérhető.
// A C# backendet a 5178-as porton indítottad el.
const API_URL = 'https://turbo-space-broccoli-xv5445qwx9hpwrp-5178.app.github.dev/api/temak'; 
// NOTE: Ezt a Codespaces-ben futtatáskor a VS Code automatikusan Proxy-zza!
// ----------------------------------------------------


const temak = ref([]);
const loading = ref(true);
const error = ref(null);

const newTemaTitle = ref('');
const isPosting = ref(false); // Blokkolja a gombot a mentés alatt

const submitNewTema = async () => {
  if (!newTemaTitle.value || isPosting.value) return;

  isPosting.value = true;
  
  // A minimalista JSON adat, amire a C# API-nak szüksége van:
  const payload = {
    cim: newTemaTitle.value
  };

  try {
    // POST kérés küldése a C# API-nak
    const response = await axios.post(API_URL, payload);
    
    // Az API visszaadja a teljes, új Témát (ID-vel, Kommentek listával együtt).
    // Ezt szúrjuk be a Vue adatok elejére:
    temak.value.unshift(response.data); 
    
    newTemaTitle.value = ''; // Űrlap ürítése
    
  } catch (err) {
    alert('Hiba történt az új téma mentésekor.');
    console.error("POST Hiba:", err);
  } finally {
    isPosting.value = false;
  }
};

// Dátum formázó segédfüggvény
const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString('hu-HU', {
    year: 'numeric', month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit'
  });
};

// Adatok lekérése a C# API-ból
const fetchTemak = async () => {
  try {
    const response = await axios.get(API_URL);
    temak.value = response.data;
  } catch (err) {
    error.value = err;
    console.error("API hiba:", err);
  } finally {
    loading.value = false;
  }
};

// Amikor a komponens betöltődik, indítsa el a lekérdezést
onMounted(fetchTemak);
</script>

<style scoped>
.container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
  font-family: sans-serif;
}
.tema-card {
  border: 1px solid #ccc;
  padding: 15px;
  margin-bottom: 20px;
  border-radius: 8px;
  background-color: #f9f9f9;
}
.komment-szamlalo {
  font-size: 0.9em;
  color: #555;
  margin-bottom: 10px;
}
.komment-lista {
  margin-top: 15px;
  padding-left: 10px;
  border-left: 2px solid #3498db;
}
.komment {
  margin-bottom: 10px;
  padding: 5px;
  border-bottom: 1px dotted #eee;
}
.loading, .error {
  padding: 20px;
  text-align: center;
  font-weight: bold;
}
</style>