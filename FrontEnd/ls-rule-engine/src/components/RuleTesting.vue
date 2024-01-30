<script setup>
import InsertSchemaModal from '../components/InsertSchemaModal.vue';
</script>
<template>
  <div id="code-wrapper" class="flex w-full">
    <div class="w-full space-y-2">
      <h1 class="text-2xl font-semibold">Rule Testing</h1>
      <!-- <div class="flex">
        <textarea id="line-numbers" class="line-numbers rounded-bl-md" readonly>{{ lineNumbers }}</textarea>
        <textarea 
          id="gist-textarea" 
          class="textarea w-full rounded-br-md" 
          placeholder="Enter JSON schema here"
          spellcheck="false" 
          autocomplete="off" 
          v-model="codeContent" 
          @input="updateCode"
          @scroll="syncScroll" 
          @keydown="handleTab">
        </textarea>
        
      </div> -->
    </div>

  </div>
  <!-- Modal toggle -->
  <div class="flex justify-end mb-2">
    <button data-modal-target="default-modal" data-modal-toggle="default-modal"
      class="px-3 py-2 text-xs font-medium text-center text-white bg-blue-700 rounded-lg hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
      type="button">
      Input JSchema
    </button>
  </div>


  <!-- Modal -->
  <div id="default-modal" tabindex="-1" aria-hidden="true"
    class="hidden overflow-y-auto overflow-x-hidden fixed top-0 right-0 left-0 z-50 justify-center items-center w-full md:inset-0 h-[calc(100%-1rem)] max-h-full">
    <div class="relative p-4 w-full max-w-2xl max-h-full">
      <insert-schema-modal @close-modal="showModal = false" @submit-schema="(schema) => handleSubmittedSchema(schema)">
      </insert-schema-modal>
    </div>
  </div>
  <div class="flex w-full h-[600px] overflow-y-scroll" >
    <textarea id="line-numbers" readonly class="syntax-numbers rounded-bl-md">{{ lineNumbers }}</textarea>
    <div id="highlight" class="syntax-area w-full rounded-br-md h-full  ">
      <pre class="text-xs"><code ref="codeBlock" class="json " @scroll="syncScroll" >{{ codeContent }}</code></pre>
    </div>
  </div>


  <div>
    <form v-if="Object.keys(formData).length" @submit.prevent="handleSubmit">
      <div v-for="(value, key) in formData" :key="key">
        <label :for="key">{{ key }}</label>
        <input  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
        :id="key" :value="value" :type="type" @input="handleInput(key, $event.target.value)">
        <div>{{ this.formData[key].type  }}</div>
      </div>
      <button type="submit">Submit</button>
    </form>

    <!-- <div class="flex flex-col w-full">
      <form @submit.prevent="handleSubmit">
        <div v-for="property in jsonProperties" class="flex space-x-4">
          <div class="mb-5">
            <label :for="property.name" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">{{
              property.name
            }} - {{ property.description }}</label>
            <input :type="property.type" :id="property.name" v-model="property.value"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
              placeholder="">
          </div>
        </div>
        <button type="submit">Submit</button>
      </form>

    </div> -->
  </div>
</template>
  
<script>

import hljs from 'highlight.js';
import 'highlight.js/styles/default.css'; // or another style you prefer

export default {
  components: {
    InsertSchemaModal,
  },
  data() {
    return {
      codeContent: '',
      lineNumbers: '1.',
      schemaInput: '', // String representation of the JSON schema
      schema: {}, // Parsed JSON schema
      formData: {}, // Data object for user input
      jsonProperties: []
    };
  },
  watch: {
    codeContent(newValue) {
      try {
        this.initializeFormData(newValue);
        this.updateLineNumbers();
        this.updateCode();
      } catch (e) {
        alert("Invalid JSON schema");
      }
    },
  },
  methods: {
    handleSubmittedSchema(schema) {
      this.codeContent = schema;
      this.showModal = false;
      this.updateLineNumbers();
      this.updateCode();
      // this.highlightCode();
    },
    initializeFormData(content) {
      try {
        this.schema = JSON.parse(content);
        this.formData = {};
       
       

        if (this.schema && typeof this.schema === 'object' && this.schema.properties) {
          Object.keys(this.schema.properties).forEach(key => {
            const prop = this.schema.properties[key];
            // Set a default or placeholder value based on the type
            if (prop.type === 'string') {
              this.formData[key] = ''; // default for string
            } else if (prop.type === 'number') {
              this.formData[key] = 0; // default for number
            } else if (prop.type === 'boolean') {
              this.formData[key] = false; // default for boolean
            } else if (prop.type === 'object') {
              this.formData[key] = {}; // default for object
            }   if (prop.type === 'string') {
              this.formData[key] = ''; // default for string
            }  else if (prop.type === 'integer') {
              this.formData[key] = 0; // default for string
              this.formData[key].type = 'number'
            } else {
              this.formData[key] = '';
            }
            // ... handle other types as needed
          });
        }
        // for (const key in this.schema.properties) {
        //   console.log(key)
        // }

        // for (const key in this.schema.properties) {
        //   // console.log( this.schema.properties[key]);
        //   // console.log( this.schema.properties[key].type);
        //   this.jsonProperties.push({ name: key, type: this.schema.properties[key].type, value: '', description: this.schema.properties[key].description });
        //   this.$set(this.formData, key, this.jsonProperties[key].default || '');

          //this.formData.push('key');
        // }
        // console.log(this.jsonProperties);
      } catch (e) {
        alert("Invalid JSON schema");
      }
    },
    handleInput(key, value) {
      this.formData[key] = value;
    },
    async handleSubmit() {
      console.log("Form Data:", this.formData);
      const jsonData = JSON.stringify(this.formData);
      const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          jsonData: jsonData,
          typeToApplyRule: "Buyer-1" // Replace with actual type
        })
      };
      try {
        const response = await fetch('https://localhost:44328/execute-rule', requestOptions);
        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const data = await response.text(); // or response.json() if your server responds with JSON
        console.log(data);
      } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
      }
    },
    updateLineNumbers() {
      const lines = this.codeContent.split("\n").length;
      this.lineNumbers = Array.from({ length: lines }, (v, k) => k + 1).join('.\n') + '.';
    },
    syncScroll(event) {
      const lineNumberText = this.$refs.lineNumbers;
      lineNumberText.scrollTop = event.target.scrollTop;
    },
    handleTab(event) {
      if (event.key === "Tab") {
        event.preventDefault();
        const start = event.target.selectionStart;
        const end = event.target.selectionEnd;
        this.codeContent = this.codeContent.substring(0, start) + "\t" + this.codeContent.substring(end);
        this.$nextTick(() => {
          event.target.selectionStart = event.target.selectionEnd = start + 1;
        });
      }
    },
    clearTextareas() {
      this.codeContent = '';
      this.lineNumbers = '1.';
    },
    trimCodeBlock(codeBlock) {
      const lines = codeBlock.textContent.split("\n")
      if (lines.length > 2) {
        lines.shift();
        lines.pop();
      }
      codeBlock.textContent = lines.join("\n")
      return codeBlock
    },
    updateCode() {
      this.$nextTick(() => {
        const codeBlock = this.$refs.codeBlock;
        if (codeBlock) {
          hljs.highlightElement(codeBlock);
        }
      });
      this.updateLineNumbers();
    }
  }
};
</script>
  