<script setup>
import Sidebar from './components/Sidebar.vue';
import Navbar from './components/Navbar.vue';
import Home from './components/Home.vue'
</script>

<template>
  <div class="w-screen h-screen flex">
    <div class="w-[400px] bg-gray-200 text-white" v-show="showSide">
      <Sidebar />


    </div>
    <div class="w-full h-full bg-gray-400">
      <div
        class="h-[50px] bg-gray-100 flex justify-between items-center shadow-sm px-[20px] w-full py-[10px] x-10 border-b">
        <div class="cursor-pointer w-[30ox]" @click="toggleSidebar">
          <svg class="w-[20px] h-[20px]" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none"
            viewBox="0 0 17 14">
            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
              d="M1 1h15M1 7h15M1 13h15" />
          </svg>
        </div>
        <div class="w-[calc(100% - 30px)] flex">
          <input type="text" id="voice-search" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg">
        </div>
        <div class="w-[200px]"></div>
      </div>
      <div class="h-[calc(100vh-50px)] bg-white p-4">
        <router-view></router-view>
      </div>

    </div>
  </div>
</template>


<script>

export default {
  data() {
    return {
      showSide: true
    }
  },
  methods: {
    toggleSidebar() {
      console.log('toggleSidebar called');
      this.showSide = !this.showSide;
    }

  }
};
</script>

