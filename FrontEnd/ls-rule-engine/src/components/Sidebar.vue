<script setup>
import SidebarItem from '../components/SidebarItem.vue';
</script>
<template>
    <div class="h-[50px] bg-gray-900 flex justify-start items-center">
       <div class="px-[20px]">
          <h3 class="font-bold text-3xl">Ls Rule Engine</h3>
       </div>
    </div>
    <div class="h-[calc(100vh-50px)] bg-gray-800 py-[20px] ">
       <div class="flex flex-col justify-between px-[20px] h-full space-y-[10px]">
          <div class=" ">
            <sidebar-item navigate="/home" description="Dashboard"></sidebar-item>
            <sidebar-item navigate="/dashboard" description="Rule Management"></sidebar-item>
            <sidebar-item navigate="/RuleTesting" description="Rule Testing"></sidebar-item>
            <sidebar-item navigate="/dashboard" description="Logs/Reports"></sidebar-item>
          </div>
 
          <div class="h-[50px]">
             <router-link to="/home" class="sidebar-menu-item">
                <svg class="mr-2 w-[15px] h-[15px]  fill-current " aria-hidden="true" xmlns="http://www.w3.org/2000/svg"
                   fill="none" viewBox="0 0 20 20">
                   <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                      d="M3 8v10a1 1 0 0 0 1 1h4v-5a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1v5h4a1 1 0 0 0 1-1V8M1 10l9-9 9 9" />
                </svg>
                Settings
 
             </router-link>
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