import { inject } from 'vue'
import { AxiosKey } from '../symbols'

export const MessageRole = {
    SYSTEM: 0,
    USER: 1,
    ASSISTANT: 2
}

export const useChatApi = () => {
  const axios = inject(AxiosKey);
  const ask = async (data) => {
    const response = await axios.post("chat/ask", data);
    return response.data;
  };

  const askFunctionCall = async (data) => {
    const response = await axios.post("chat/ask-function-call", data);
    return response.data;
  };

  const askContext = async (data) => {
    const response = await axios.post("chat/ask-context", data);
    return response.data;
  };

  const convertSystemPrompt = (prompts) => {
    return prompts.map(p => ({
      value: p,
      role: MessageRole.SYSTEM,
    }));
  };

  const convertMessages = (messages) => {
    return messages.map(m => ({
      value: m.message,
      role: m.isAnswer ? MessageRole.ASSISTANT : MessageRole.USER,
    }));
  };

  return {
    ask,
    askContext,
    askFunctionCall,
    convertSystemPrompt,
    convertMessages
  }
}