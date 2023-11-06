<template>
  <div>
    Working...
  </div>
</template>
<script setup>
import { useTitleStore } from '../stores/title';

const { setTitle } = useTitleStore();
setTitle('Accueil');
</script>
