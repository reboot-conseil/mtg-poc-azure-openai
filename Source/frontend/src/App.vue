<template>
  <v-app id="inspire">
    <v-navigation-drawer class="bg-grey-lighten-5" permanent>
      <v-list nav class="nav">
        <v-list-item></v-list-item>
        <router-link :to="{ name: 'home' }">
          <v-list-item class="nav-item" prepend-icon="mdi-home" title="Accueil" density="comfortable"></v-list-item>
        </router-link>
        <router-link :to="{ name: 'dalle' }">
          <v-list-item class="nav-item" prepend-icon="mdi-image-edit" title="Dall-E" density="comfortable"></v-list-item>
        </router-link>
        <router-link :to="{ name: 'chat' }">
          <v-list-item class="nav-item" prepend-icon="mdi-chat" title="Base documentaire - Chat GPT" density="comfortable"></v-list-item>
        </router-link>
        
        <router-link :to="{ name: 'chat-context' }">
          <v-list-item class="nav-item" prepend-icon="mdi-account-voice" title="Conversation avec Chat GPT" density="comfortable"></v-list-item>
        </router-link>
        <router-link :to="{ name: 'chat-function-call' }">
          <v-list-item class="nav-item" prepend-icon="mdi-account-voice" title="Appel externe - Chat GPT" density="comfortable"></v-list-item>
        </router-link>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar>
      <v-app-bar-title>Démo Azure OpenAi {{  title ? `- ${title}` : '' }}</v-app-bar-title>
    </v-app-bar>

    <v-main class="main">
      <router-view></router-view>
    </v-main>
  </v-app>
</template>
<script setup>
import { storeToRefs } from 'pinia';
import { useTitleStore } from './stores/title';

const { title } = storeToRefs(useTitleStore());
</script>

<style lang="scss">
.main {
  display: flex;
  align-content: center;
  justify-content: center;
  width: 100%;
  min-height: 300px;
}

.nav {
  padding: 0 !important;
}

.nav-item {
  color: black !important;
  width: 100%;
  border-radius: 0 !important;
  margin-bottom: 0.5rem;
  font-size: larger;
  background-color: #E3F2FD !important;
}

.router-link-active {
  .nav-item { 
    color: white !important;
    background-color: #0D47A1 !important;
  }
}
</style>
