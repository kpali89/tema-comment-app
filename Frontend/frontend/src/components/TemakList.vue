<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import Tema from './Tema.vue'; // <<< Tema Komponens

// C# API CÍME (a saját Codespaces címed!)
const API_URL = 'https://turbo-space-broccoli-xv5445qwx9hpwrp-5178.app.github.dev/api/temak'; 

const temak = ref([]);
const loading = ref(true);
const error = ref(null);
const newTemaTitle = ref('');
const isPosting = ref(false);

// Adatok lekérése a C# API-ból
const fetchTemak = async () => {
  try {
    loading.value = true;
    const response = await axios.get(API_URL);
    temak.value = response.data;
  } catch (err) {
    error.value = err;
    console.error("API hiba:", err);
  } finally {
    loading.value = false;
  }
};

// Új Téma Küldése
const submitNewTema = async () => {
  if (!newTemaTitle.value || isPosting.value) return;

  isPosting.value = true;

  const payload = { cim: newTemaTitle.value };

  try {
    const response = await axios.post(API_URL, payload);

    // Frissítés helyett csak betesszük az újat a lista elejére a jobb UX érdekében
    temak.value.unshift(response.data); 
    newTemaTitle.value = ''; 

  } catch (err) {
    alert('Hiba történt az új téma mentésekor.');
    console.error("POST Hiba:", err);
  } finally {
    isPosting.value = false;
  }
};

// Amikor a komponens betöltődik
onMounted(fetchTemak);
</script>

<template>
  <div class="container mt-5">
    <h1 class="text-center mb-4">Téma-Komment Alkalmazás</h1>

    <div class="card bg-light mb-4">
      <div class="card-body">
        <h3 class="card-title">Új Téma Létrehozása</h3>
        <form @submit.prevent="submitNewTema" class="input-group">
          <input 
            type="text" 
            v-model="newTemaTitle" 
            placeholder="Írd be az új téma címét" 
            required 
            maxlength="100"
            class="form-control"
          />
          <button 
            type="submit" 
            :disabled="isPosting" 
            class="btn btn-primary"
          >
            {{ isPosting ? 'Mentés...' : 'Téma Küldése' }}
          </button>
        </form>
      </div>
    </div>

    <div v-if="loading" class="alert alert-info text-center">Adatok betöltése...</div>
    <div v-else-if="error" class="alert alert-danger">Hiba történt: {{ error.message }}</div>

    <div v-else>
      <Tema
        v-for="tema in temak"
        :key="tema.id"
        :tema="tema"
        @temaFrissitesSzukseges="fetchTemak"
      />
    </div>
  </div>
</template>

<style scoped>

</style>