import { inject } from 'vue'
import { AxiosKey } from '../symbols'

export const useChatApi = () => {
  const axios = inject(AxiosKey);
  const generateImage = async (data) => {
    const response = await axios.post("image/generate", data);
    return response.data;
  }

  return {
    generateImage
  }
}