
<template>
    <div
        class="w-full max-w-sm p-4 bg-white border border-gray-200 rounded-lg shadow sm:p-6 md:p-8 dark:bg-gray-800 dark:border-gray-700">
        <form class="space-y-3" v-if="Object.keys(formData).length" @submit.prevent="handleSubmit">
            <h5 class="text-xl font-medium text-gray-900 dark:text-white">Update model</h5>
            <div class="" v-for="(value, key) in formData" :key="key">
                <div class="flex items-start ">
                    <label class="w-64" :for="key">{{ key }} </label>
                    <input
                        class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                        :id="key" :value="value" :type="getPropertyType(key)"
                        @input="handleInput(key, $event.target.value)">
                    <div></div>
                </div>

            </div>
            <button
                class="mt-6 w-full text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                type="submit">Update</button>
        </form>

    </div>
</template>
<script>

export default {
    props: {
        schema: {
            type: Object,
            required: true
        }
    },

    data() {
        return {
            formData: {}
        };
    },
    created() {
        this.initializeFormData();
    },
    methods: {
        getPropertyType(key) {
            const typeRaw = this.schema.properties[key].type;
            let type = Array.isArray(typeRaw) ? typeRaw[0] : typeRaw;
            switch (typeRaw){
                case 'integer': type = 'number'; break;
                
            }
            return type;
        },
        initializeFormData(schema) {
            if (this.schema && typeof this.schema === 'object' && this.schema.properties) {
                Object.keys(this.schema.properties).forEach(key => {
                    const prop = this.schema.properties[key];
                    this.formData[key] = prop.default || ''; // Use default value or empty string
                });
            }
        },
        async handleSubmit() {
            console.log("Form Data:", this.formData);
            // const jsonData = JSON.stringify(this.formData);
            this.$emit('submit-json', this.formData);

        },
        handleInput(key, value) {
            const type = this.getPropertyType(key);
            switch (type) {
                case 'number':
                    this.formData[key] = value ? Number(value) : null; // Convert to Number if value is not empty
                    break;
                case 'boolean':
                    this.formData[key] = value === 'true'; // Convert to Boolean
                    break;
                default:
                    this.formData[key] = value; // For string and other types
            }
        },

    }
};
</script>
  