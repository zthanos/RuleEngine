
<template>
    <form v-if="Object.keys(formData).length" @submit.prevent="handleSubmit">
        <div v-for="(value, key) in formData" :key="key">
            <label :for="key">{{ key }} - {{ getPropertyType(key) }}</label>
            <input
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                :id="key" :value="value" :type="getPropertyType(key)" @input="handleInput(key, $event.target.value)">
            <div></div>
        </div>
        <button type="submit">Submit</button>
    </form>
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
            const type = this.schema.properties[key].type;
            return Array.isArray(type) ? type[0] : type;
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
  