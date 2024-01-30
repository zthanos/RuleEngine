<script setup>
import Sidebar from '../components/Sidebar.vue';
</script>
<template>
    <div class="w-screen h-screen flex">
        <div class="w-[400px] bg-gray-200 text-white" v-show="showSide">
            <div class="h-[50px] bg-gray-900 flex justify-start items-center">
                <div class="px-[20px]">
                    <h3 class="font-bold text-3xl">Scrum Dashboard</h3>
                </div>
            </div>
            <div class="h-[calc(100vh-50px)] bg-gray-800 py-[20px] ">
                <div class="flex flex-col justify-between px-[20px] h-full space-y-[10px]">
                    <div class=" ">
                        <router-link to="/home" class="sidebar-menu-item">
                            <svg class="mr-2 w-[15px] h-[15px]  fill-current " aria-hidden="true"
                                xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                                <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="M3 8v10a1 1 0 0 0 1 1h4v-5a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1v5h4a1 1 0 0 0 1-1V8M1 10l9-9 9 9" />
                            </svg>
                            Home

                        </router-link>
                        <router-link to="/home" class="sidebar-menu-item">
                            <svg class="mr-2 w-[15px] h-[15px]  fill-current " aria-hidden="true"
                                xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                                <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="M3 8v10a1 1 0 0 0 1 1h4v-5a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1v5h4a1 1 0 0 0 1-1V8M1 10l9-9 9 9" />
                            </svg>
                            Profile

                        </router-link>
                        <router-link to="/home" class="sidebar-menu-item">
                            <svg class="mr-2 w-[15px] h-[15px]  fill-current " aria-hidden="true"
                                xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                                <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="M3 8v10a1 1 0 0 0 1 1h4v-5a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1v5h4a1 1 0 0 0 1-1V8M1 10l9-9 9 9" />
                            </svg>
                            Download

                        </router-link>
                    </div>


                    <div class="h-[50px]">
                        <router-link to="/home" class="sidebar-menu-item">
                            <svg class="mr-2 w-[15px] h-[15px]  fill-current " aria-hidden="true"
                                xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                                <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="M3 8v10a1 1 0 0 0 1 1h4v-5a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1v5h4a1 1 0 0 0 1-1V8M1 10l9-9 9 9" />
                            </svg>
                            Settings

                        </router-link>
                    </div>
                </div>
            </div>


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
                    <input type="text" id="voice-search"
                        class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg">
                </div>
                <div class="w-[200px]"></div>
            </div>
            <div class="h-[calc(100vh-50px)] bg-white">
                <router-view></router-view>
            </div>

        </div>
    </div>
</template>


