<template>
  <div class="wrapper chat-container">
    <div class="chat-content">
      <div 
        v-for="message in messages" 
        :key="message.date.toISOString()" 
        class="message-container"
        :class="message.isAnswer ? '' : 'left'"
      >
        <template v-if="message.isAnswer">
          <div class="message-date" v-if="!$vuetify.display.mobile">{{ displayMessageDate(message.date) }}</div>
          <v-card variant="tonal">
            <v-card-text v-html="message.message"></v-card-text>
          </v-card>
        </template>
        <template v-else>
          <div class="message-date" v-if="!$vuetify.display.mobile">{{ displayMessageDate(message.date) }}</div>
          <v-card :text="message.message" variant="tonal"></v-card>
        </template>
      </div>
      <v-progress-circular
        v-if="loading"
        indeterminate
        color="purple"
      ></v-progress-circular>
      <div id="bottom" ref="chatContainer"></div>
    </div>
    <div class="chat-input">
      <v-textarea
        v-model="question"
        label="Que souhaites tu demander à Gandalf ?"
        variant="solo-filled"
        append-icon="mdi-send"
        @keyup.enter="submit"
        @click:append="submit"
      ></v-textarea>
    </div>
  </div>
</template>

<script setup>
import { useChatApi } from '../composables/useChatApi';
import { format } from 'date-fns';
import { nextTick,  ref } from 'vue';

const { ask } = useChatApi();
const messages = ref([]);
const question = ref('');
const loading = ref(false);
const chatContainer = ref(null);

async function submit() {
  messages.value.push({
    message: question.value,
    date: new Date(),
    isAnswer: false
  });

  nextTick(() => {
    chatContainer.value.scrollIntoView();
  });

  loading.value = true;
  const { data: response } = await ask(question.value);  
  loading.value = false;

  messages.value.push({
    message: response.replaceAll('\n', '<br>'),
    date: new Date(),
    isAnswer: true
  });

  question.value = '';
  nextTick(() => {
    chatContainer.value.scrollIntoView();
  });
  
}

function displayMessageDate(date) {
  return format(date, 'HH:mm');
}

</script>

<style scoped lang="scss">
.chat-content {
  flex-grow: 1;
  overflow-y: scroll;
}
.chat-container {
  display: flex;
  flex-direction: column;
  scrollbar-width: none;
}
.chat-input {
  flex-grow: 0;
}

.message-container {
  display: flex;
  margin-bottom: 1rem;
  margin-right: 4rem;

  &.left {
    flex-direction: row-reverse;
    margin-right: 0.5rem;
    margin-left: 4rem;
    .message-date {
      margin-left: 1rem;
    }
  }

  * {
    flex-grow: 0;
  }

  img {
    width: 2.5rem;
    object-fit: scale-down;
  }

  .message-date {
    align-self: center;
    margin-right: 1rem;
  }
}

#bottom {
  visibility: hidden;
  height: 1px;
}
</style>