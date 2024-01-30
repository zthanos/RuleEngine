
<template>
    
  <div class="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full" 
       v-show="show">
    <div class="relative top-20 mx-auto p-5 border w-1/3 shadow-lg rounded-md bg-white">
      <div class="mt-3 text-center">
        <h3 class="text-lg leading-6 font-medium text-gray-900">JSON Schema Input</h3>
        <div class="mt-2 px-7 py-3">
          <textarea class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            placeholder="Enter JSON schema here" v-model="localGistContent" @input="updateCode"></textarea>
        </div>
        <div class="items-center px-4 py-3">
          <button class="px-4 py-2 bg-blue-500 text-white text-base font-medium rounded-md w-full shadow-sm hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-300"
                  @click="emitSchema">
            Submit
          </button>
        </div>
      </div>
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
      localGistContent: '',
    };
  },
    methods: {
        emitSchema() {
      this.$emit('submit-schema', this.localGistContent);
      this.$emit('close');
    },
      updateCode() {
        this.$nextTick(() => {
          const codeBlock = this.$refs.codeBlock;
          if (codeBlock) {
            hljs.highlightElement(codeBlock);
          }
        });
      },
      
    },
    watch: {
      gistContent() {
        this.updateCode();
      }
    }
  };
  </script>
  