<script setup lang="ts">
import { DocumentEditor } from "@onlyoffice/document-editor-vue";

const route=useRoute()

const config = {
  document: {
    fileType: "docx",
    key: "Khirz6zTPdfd7",
    title: "Example Document Title.docx",
    url: "http://180.76.195.252:3344/docs/word.docx"
  },
  documentType: "word",
  editorConfig: {
    callbackUrl: "https://example.com/url-to-callback.ashx"
  }
}

const onDocumentReady = () => {
  console.log("Document is loaded");
}

onMounted(()=>{
console.log('route',route);

})

</script>

<template>
  <div class="h-screen">
    <DocumentEditor
     id="docEditor"
     documentServerUrl="http://192.168.1.4:9001"
     :config="config"
     :events_onDocumentReady="onDocumentReady" 
    />
  </div>
  
</template>
