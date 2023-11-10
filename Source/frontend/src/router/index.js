import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import DalleView from '../views/DalleView.vue'
import ChatView from '../views/ChatView.vue'
import ChatContextView from '../views/ChatContextView.vue'
import ChatFunctionCallView from '../views/ChatFunctionCallView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/dalle',
      name: 'dalle',
      component: DalleView
    },
    {
      path: '/chat',
      name: 'chat',
      component: ChatView
    },
    {
      path: '/chat/context',
      name: 'chat-context',
      component: ChatContextView
    },
    {
      path: '/chat/function-call',
      name: 'chat-function-call',
      component: ChatFunctionCallView
    },
  ]
})

export default router
