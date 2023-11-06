import { ref } from 'vue'
import { defineStore } from 'pinia'

export const useTitleStore = defineStore('title', () => {
  const title = ref('');
  function setTitle(value) {
    title.value = value
  }

  return { title, setTitle }
})
