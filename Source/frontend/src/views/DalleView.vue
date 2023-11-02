<template>
  <div class="wrapper">
    <v-form class="form" @submit.prevent="genererImage">
      <v-container>
        <v-row>
          <v-col
            cols="12"
          >
            <v-text-field
              v-model="prompt"
              label="prompt"
              required
            ></v-text-field>
          </v-col>

          <v-col
            cols="12"
          >
          <v-select
            v-model="size"
            :items="items"
            item-title="text"
            item-value="value"
            label="Taille de l'image"
          ></v-select>
          </v-col>
        </v-row>
      </v-container>
      <v-btn color="primary" type="submit">Générer</v-btn>
    </v-form>

    <img v-if="url" :src="url" alt="Image générée"/>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useImageApi } from '../composables/useImageApi'

const prompt = ref("");
const size = ref(1024);
const url = ref(null)
const items = [{
  text: "256x256",
  value: 256,
},{
  text: "512x512",
  value: 512,
},{
  text: "1024x1024",
  value: 1024,
},];

const { generateImage } = useImageApi();

const genererImage = async() => {
  const data = {
    prompt: prompt.value,
    size: size.value,
  };

  url.value = await generateImage(data);
};
</script>
<style lang="scss" scoped>
.form {
  width: 100%;
}
</style>
