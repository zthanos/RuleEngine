<script setup>
import { Logger } from 'sass';
import SchemaEditor from '../components/SchemaEditor.vue';
import UpdateModel from '../components/UpdateModel.vue';
</script>

<template>
  <div class=" space-y-2">
    <h1 class="text-3xl font-semibold ">Rule Testing</h1>
    <form class="pb-8" action="#">
      <label for="typeToApplyRule" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Type to Apply
        Rule</label>
      <div class="w-96  flex space-x-6">

        <input type="text" name="typeToApplyRule" id="typeToApplyRule" v-bind="typeToApplyRule"
          class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white"
          placeholder="name@company.com" required>
        <button type="submit" @click="getRules()"
          class=" text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm  text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
          Get Rules
        </button>
      </div>
    </form>
    <ol
      class="flex items-center w-full text-sm font-medium text-center text-gray-500 dark:text-gray-400 sm:text-base pl-64 pr-64">
      <li
        class="flex md:w-full items-center text-blue-600 dark:text-blue-500 sm:after:content-[''] after:w-full after:h-1 after:border-b after:border-gray-200 after:border-1 after:hidden sm:after:inline-block after:mx-6 xl:after:mx-10 dark:after:border-gray-700">
        <span
          class="flex items-center after:content-['/'] sm:after:hidden after:mx-2 after:text-gray-200 dark:after:text-gray-500">
          <svg class="w-3.5 h-3.5 sm:w-4 sm:h-4 me-2.5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg"
            fill="currentColor" viewBox="0 0 20 20">
            <path
              d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5Zm3.707 8.207-4 4a1 1 0 0 1-1.414 0l-2-2a1 1 0 0 1 1.414-1.414L9 10.586l3.293-3.293a1 1 0 0 1 1.414 1.414Z" />
          </svg>
          Import <span class="hidden sm:inline-flex sm:ms-2">Schema</span>
        </span>
      </li>
      <li
        class="flex cursor- md:w-full items-center after:content-[''] after:w-full after:h-1 after:border-b after:border-gray-200 after:border-1 after:hidden sm:after:inline-block after:mx-6 xl:after:mx-10 dark:after:border-gray-700">
        <span
          class="flex items-center after:content-['/'] sm:after:hidden after:mx-2 after:text-gray-200 dark:after:text-gray-500">
          <span class="me-2">2</span>
          Upadate <span class="hidden sm:inline-flex sm:ms-2">Model</span>
        </span>
      </li>
      <li class="flex items-center">
        <span class="me-2">3</span>
        Results
      </li>
    </ol>

    <div v-if="currentStep == 1">
      <SchemaEditor :schema="newschema" @submit-schema="(schema) => this.codeContent = schema"></SchemaEditor>
      <div class="flex  w-full justify-end pr-4">
        <button type="submit" @click="nextStep()"
          class=" text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
          Next Step: Update Model
        </button>
      </div>
    </div>

  </div>

  <div v-if="currentStep == 2" class="">

    <div class="flex">
      <h5 class="text-xl font-medium text-gray-900 dark:text-white">Applicable Rules </h5>
    </div>
    <div class="flex space-x-6">
      <UpdateModel :schema="JSON.parse(this.codeContent)" @submit-json="(jsonData) => this.formData = jsonData">
      </UpdateModel>
      <textarea class="w-[550px] border-0" >{{ ruleResult.ruleContent }}</textarea>
    </div>
    <div class="flex  w-full justify-end pr-4 space-x-2">
      <button type="submit" @click="previousStep()"
        class=" text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
        Previous Step: Import Schema
      </button>
      <button type="submit" @click="nextStep()"
        class=" text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
        Next Step: Execute Rule
      </button>
    </div>
  </div>

  <div v-if="currentStep == 3" class="">
    <div class="flex w-full justify-evenly space-x-8">
      <h3 class="w-full text-3xl text-gray-400 pb-4 pt-8 ">Original Values </h3>
      <h3 class="w-full text-3xl text-gray-400 pb-4 pt-8 ">Rule Applied Values </h3>

    </div>
    <div class="flex w-full justify-evenly space-x-8">
      <div id="highlight1" class="syntax-area w-full rounded-br-md h-full  ">
        <pre class="text-xs"><code ref="originalJson" class="json "  >{{ formData }}</code></pre>
      </div>
      <div id="highlight2" class="syntax-area w-full rounded-br-md h-full  ">
        <pre class="text-xs"><code ref="resultJson" class="json "  >{{ ruleResult }}</code></pre>
      </div>
    </div>

  </div>
</template>
<script>

import hljs from 'highlight.js';
import 'highlight.js/styles/default.css'; // or another style you prefer

export default {
  components: {
    SchemaEditor,
  },
  data() {
    return {
      currentStep: 1,
      schemaInput: '', // String representation of the JSON schema
      typeToApplyRule: '',
      ruleResult: {},
      loadedSchema: {},
      newschema: {}
    };
  },
  watch: {
    loadedSchema: {
      immediate: true,
      handler(newVal, oldVal) {
        this.newschema = newVal
        console.log(JSON.stringify(this.newschema))

      }
    },
  },
  methods: {
    previousStep() {
      switch (this.currentStep) {
        case 2:
          // this.initializeFormData(this.codeContent);
          this.currentStep--;
          break;
        case 3:
          this.handleSubmit()
          this.currentStep--;
          break;
      }
    },
    nextStep() {
      switch (this.currentStep) {
        case 1:
          // this.initializeFormData(this.codeContent);
          this.currentStep++;
          break;
        case 2:
          this.handleSubmit()
          this.currentStep++;
          break;
      }
    },
    highlightResponse() {

      this.$nextTick(() => {
        const originalJson = this.$refs.originalJson;
        if (originalJson) {
          hljs.highlightElement(originalJson);
        }
        const resultJson = this.$refs.resultJson;
        if (resultJson) {
          hljs.highlightElement(resultJson);
        }
      });

    },
    async getRules() {
      const response = await fetch('https://localhost:44328/api/rules?type=Buyer' + this.typeToApplyRule + '-1', {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
      });
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      let resultsRaw = await response.json();
      this.ruleResult = resultsRaw[0];
      this.loadedSchema = JSON.parse(this.ruleResult.jsonSchema.replace(/^"|"$/g, ''));
    },
    async handleSubmit() {
      try {
        const jsonData = JSON.stringify(this.formData); // Convert formData to JSON string
        const payload = {
          jsonData: jsonData,
          typeToApplyRule: "Buyer-1" // Replace with the appropriate value or variable
        };

        const response = await fetch('https://localhost:44328/execute-rule', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload) // Convert the entire payload to JSON string
        });

        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }

        this.ruleResult = await response.json(); // or response.json() if your server responds with JSON
        this.highlightResponse();
        console.log(this.result);
      } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
      }
    }

  }
};
</script>
  