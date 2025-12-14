<script setup>
import { defineProps, defineEmits } from 'vue';
import KommentForm from './KommentForm.vue'; // Ez már létezik

const props = defineProps({
  tema: {
    type: Object,
    required: true
  }
});

const emit = defineEmits(['temaFrissitesSzukseges']);

// Ez a metódus fut le, amikor a KommentForm sikeresen elküldte a kommentet.
const frissitesSzukseges = () => {
  // Esemény küldése a szülő (TemakList.vue) felé
  emit('temaFrissitesSzukseges');
};

// Dátum formázó segédfüggvény (amit korábban az App.vue-ban használtunk)
const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('hu-HU', {
        year: 'numeric', month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit'
    });
};
</script>

<template>
  <div class="card mb-3 shadow-sm">
    <div class="card-body">
      <h2 class="card-title">{{ tema.cim }}</h2>
      <span class="badge bg-secondary mb-3">{{ tema.kommentek.length }} komment</span>

      <div v-if="tema.kommentek.length > 0" class="mt-3">
        <h5 class="border-bottom pb-2">Hozzászólások:</h5>
        <div v-for="komment in tema.kommentek" :key="komment.id" class="p-2 border-start border-3 mb-2" style="border-color: #3498db !important;">
          <p class="mb-0">
            <strong class="text-primary">{{ komment.felhasznaloNev }}</strong> 
            <small class="text-muted">({{ formatDate(komment.datum) }})</small>
          </p>
          <p class="mb-0">{{ komment.szoveg }}</p>
        </div>
      </div>
      <p v-else class="text-muted fst-italic">Nincsenek hozzászólások ehhez a témához.</p>

      <div class="mt-4">
        <KommentForm 
          :temaId="tema.id" 
          @kommentHozzaadva="frissitesSzukseges" 
        />
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>