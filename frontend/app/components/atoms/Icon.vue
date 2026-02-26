<template>
    <Icon :name="`${collection}:${icon}`" :style="{ color: color }" />
</template>
<script setup>
defineProps({
    icon: {
        type: String,
        required: true,
    },
    collection: {
        type: String,
        default: 'hugeicons',
    },
    color: {
        type: String,
        default: 'black',
    }
})
</script>