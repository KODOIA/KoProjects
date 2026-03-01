<template>
    <Icon :name="`${collection}:${icon}`" :style="{ color: color }" :size="size" />
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
    },
    size: {
        type: [String, Number],
        default: '1em',
    },
})
</script>