<template>
  <v-container fluid class="wrapper pb-0">
    <v-row class="chat-row">
      <v-col cols="12" md="4" class="py-0">
        <div class="chat-container">
          <div class="system-container">
            <div 
              v-for="(message, index) in systemPrompts" 
              :key="message.date" 
              class="system-prompt"
            >
              <v-card variant="tonal" class="card">
                <v-card-text>
                  <span>
                    {{ message }}
                  </span>
                  <v-btn 
                    color="error"
                    class="left"
                    density="compact"
                    icon="mdi-trash-can-outline"
                    @click="removeSystemPrompt(index)"
                  ></v-btn>
                </v-card-text>
                
              </v-card>
            </div>
          </div>
          <div class="chat-input">
            <v-textarea
              v-model="systemInput"
              label="Prompt système"
              variant="solo-filled"
              append-icon="mdi-plus"
              @keyup.enter="addSystemPrompt"
              @click:append="addSystemPrompt"
            ></v-textarea>
          </div>
        </div>
      </v-col>
      <v-divider vertical color="black"></v-divider>
      <v-col cols="12" md="8" class="py-0">
        <div class="chat-container">
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
              color="blue"
            ></v-progress-circular>
            <div id="bottom" ref="chatContainer"></div>
          </div>
          <div class="chat-input">
            <v-textarea
              v-model="question"
              label="Que souhaites tu demander ?"
              variant="solo-filled"
              append-icon="mdi-send"
              @keyup.enter="submit"
              @click:append="submit"
            ></v-textarea>
          </div>
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
import { useChatApi } from '../composables/useChatApi';
import { format } from 'date-fns';
import { nextTick,  ref } from 'vue';
import { useTitleStore } from '../stores/title';

const { setTitle } = useTitleStore();
setTitle('GPT - Function call');

const { askFunctionCall } = useChatApi();
const messages = ref([]);
const systemPrompts = ref([]);
const systemInput = ref('');
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
  const formValues = {
    systemPrompts: systemPrompts.value,
    question: question.value,
  };
  question.value = '';
  const response = await askFunctionCall(formValues);
  loading.value = false;

  messages.value.push({
    message: response.replaceAll('\n', '<br>'),
    date: new Date(),
    isAnswer: true
  });
  nextTick(() => {
    chatContainer.value.scrollIntoView();
  });
  
}

function displayMessageDate(date) {
  return format(date, 'HH:mm');
}

function addSystemPrompt() {
  systemPrompts.value.push(systemInput.value);
  systemInput.value = '';
}

function removeSystemPrompt(index) {
  systemPrompts.value.pop(index);
}

</script>

<style scoped lang="scss">
.chat-content {
  flex-grow: 1;
  overflow-y: scroll;
}
.chat-container {
  display: flex;
  height: 100%;
  flex-direction: column;
  scrollbar-width: none;
}
.chat-input {
  flex-grow: 0;
}

.chat-row {
  height: 100%;
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

.system-container {
  flex-grow: 1;
  overflow-y: scroll;
  margin-left: 1rem;
  .system-prompt {
    display: flex;
    margin-bottom: 0.25rem;
    
    * {
      flex-grow: 0;
    }

    .card {
      width: 100%;

      .left {
        float: right;
      }
    }
  }
}

#bottom {
  visibility: hidden;
  height: 1px;
}
</style>