<template>
  <div>
    Bienvenue à cette session du MTG Strasbourg
  </div>
</template>
<script setup>
import { useTitleStore } from '../stores/title';

const { setTitle } = useTitleStore();
setTitle('Accueil');
</script>
