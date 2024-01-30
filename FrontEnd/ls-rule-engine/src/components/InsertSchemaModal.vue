
<template>
  <!-- Main modal -->
      <!-- Modal content -->
      <div class="relative bg-white rounded-lg shadow dark:bg-gray-700">
        <!-- Modal header -->
        <div class="flex items-center justify-between p-4 md:p-5 border-b rounded-t dark:border-gray-600">
          <h3 class="text-xl font-semibold text-gray-900 dark:text-white">
            Import Json Schema
          </h3>
          <button type="button"
            class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
            data-modal-hide="default-modal">
            <svg class="w-3 h-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
              <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6" />
            </svg>
            <span class="sr-only">Close modal</span>
          </button>
        </div>
        <!-- Modal body -->
        <div class="space-y-1 h-[500px]">
          <textarea class="w-full h-full" id="code-textarea" placeholder="Enter JSON schema here" spellcheck="false"
            autocomplete="off" v-model="codeContent" @input="updateCode" @scroll="syncScroll"
            @keydown="handleTab"></textarea>

        </div>
        <!-- Modal footer -->
        <div class="flex items-center p-4 md:p-5 border-t border-gray-200 rounded-b dark:border-gray-600">
          <button data-modal-hide="default-modal" type="button" @click="$emit('submit-schema', this.codeContent)"
            class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">I
            accept</button>
          <button data-modal-hide="default-modal" type="button"
            class="ms-3 text-gray-500 bg-white hover:bg-gray-100 focus:ring-4 focus:outline-none focus:ring-blue-300 rounded-lg border border-gray-200 text-sm font-medium px-5 py-2.5 hover:text-gray-900 focus:z-10 dark:bg-gray-700 dark:text-gray-300 dark:border-gray-500 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-600">Decline</button>
        </div>
      </div>
</template>
  
<script>
import hljs from 'highlight.js';
import 'highlight.js/styles/default.css';

export default {
  props: {
    show: Boolean
  },
  data() {
    return {
      codeContent: '',
    };
  },
  methods: {
  //   updateCode() {
  //     this.$nextTick(() => {
  //       const codeBlock = this.$refs.codeBlock;
  //       if (codeBlock) {
  //         hljs.highlightElement(codeBlock);
  //       }
  //     });
  //   },

  // },
  // watch: {
  //   gistContent() {
  //     this.updateCode();
  //   }
  }
};
</script>
  