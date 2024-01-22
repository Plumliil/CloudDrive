<script setup lang="ts">
import { DocumentEditor } from "@onlyoffice/document-editor-vue";

const config = {
  document: {
    fileType: "docx",
    key: "Khirz6zTPdfd7",
    title: "Example Document Title.docx",
    url: "https://example.com/url-to-example-document.docx"
  },
  documentType: "word",
  editorConfig: {
    callbackUrl: "https://example.com/url-to-callback.ashx"
  }
}

const onDocumentReady = () => {
  console.log("Document is loaded");
}

</script>

<template>
  <DocumentEditor id="docEditor" documentServerUrl="http://documentserver/" :config="config"
    :events_onDocumentReady="onDocumentReady" />
</template>
