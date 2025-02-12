<script lang="ts">
    export let type = "text";
    export let value = "";
    export let placeholder = "";
</script>

<input
    bind:value
    type={type}
    placeholder={placeholder}
    required
    class="p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-300 focus:outline-none"
/>