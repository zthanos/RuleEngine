<template>
    <router-link :to="navigate" class="sidebar-menu-item">
      <svg class="mr-2 w-[15px] h-[15px] fill-current" aria-hidden="true" xmlns="http://www.w3.org/2000/svg"
           fill="none" viewBox="0 0 20 20">
        <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
              d="M3 8v10a1 1 0 0 0 1 1h4v-5a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1v5h4a1 1 0 0 0 1-1V8M1 10l9-9 9 9" />
      </svg>
      {{ description }}
    </router-link>
  </template>
  
  <script>
  export default {
    name: "SidebarItem", // Multi-word component name
    props: {
      description: {
        type: String,
        required: true
      },
      navigate: {
        type: String,
        required: true
      }
    }
  }
  </script>
  

  