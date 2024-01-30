<template>
    <div class="w-full">

        <div class="flex w-full justify-end ml-[1px]  pr-4 bg-black">
            <button @click="toggleView()" class="pt-1 pb-2">
                <div v-if="!showCodeBlock">
                    <svg class="w-5 h-5 text-gray-600 dark:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg"
                        fill="none" viewBox="0 0 24 24">
                        <path stroke="currentColor" stroke-linecap="round" stroke-width="2"
                            d="m21 21-3.5-3.5M17 10a7 7 0 1 1-14 0 7 7 0 0 1 14 0Z" />
                    </svg>
                </div>
                <div v-else>
                    <svg class="w-6 h-6 text-gray-600 dark:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg"
                        fill="none" viewBox="0 0 24 24">
                        <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                            d="m14.3 4.8 2.9 2.9M7 7H4a1 1 0 0 0-1 1v10c0 .6.4 1 1 1h11c.6 0 1-.4 1-1v-4.5m2.4-10a2 2 0 0 1 0 3l-6.8 6.8L8 14l.7-3.6 6.9-6.8a2 2 0 0 1 2.8 0Z" />
                    </svg>
                </div>
                <!-- {{ showCodeBlock ? 'Edit' : 'Preview' }} -->
            </button>
        </div>
        <div class=" h-full" w-full>
            <div class="flex h-full" v-if="!showCodeBlock">
                <textarea ref="lineNumbers" id="line-numbers" class="line-numbers rounded-bl-md h-full"
                    readonly>{{ lineNumbers }}</textarea>
                <textarea id="gist-textarea" class="textarea w-full  rounded-br-md" placeholder="Enter JSON schema here"
                    spellcheck="false" autocomplete="off" v-model="codeContent" @input="updateLineNumbers"
                    @scroll="syncScroll" @keydown="handleTab"></textarea>
            </div>

            <div v-else class="flex w-full h-full overflow-y-scroll">
                <textarea ref="lineNumbers" id="line-numbers" readonly
                    class="syntax-numbers rounded-bl-md">{{ lineNumbers }}</textarea>
                <div id="highlight" class="syntax-area w-full rounded-br-md h-full  ">
                    <pre
                        class="text-xs"><code ref="codeBlock" class="json " @scroll="syncScroll" >{{ codeContent }}</code></pre>
                </div>
            </div>
        </div>
    </div>
</template>
  
<script>
import hljs from 'highlight.js';
import 'highlight.js/styles/default.css'; // or another style you prefer

export default {
    data() {
        return {
            showCodeBlock: false,
            codeContent: '',
            lineNumbers: '1.',
        };
    },
    methods: {
        emitSchema() {
            this.$emit('submit-schema', this.codeContent);
        },
        toggleView() {
            this.showCodeBlock = !this.showCodeBlock;
            if (this.showCodeBlock) {
                this.$nextTick(() => {
                    const codeBlock = this.$refs.codeBlock;
                    if (codeBlock) {
                        hljs.highlightElement(codeBlock);
                    }
                });
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
        }
    },
    watch: {
        codeContent(newValue) {
            this.updateLineNumbers();
            this.emitSchema() 
            //this.updateCode();
        }
    }
};
</script>
  