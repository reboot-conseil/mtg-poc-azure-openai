import { inject } from 'vue'
import { AxiosKey } from '../symbols'

export const useChatApi = () => {
  const axios = inject(AxiosKey);
  const ask = async (data) => {
    const response = await axios.post("chat/ask", data);
    return response.data;
  }

  return {
    ask
  }
}