<script lang="ts">
    export let title: string | null = null
    export let description: string | null = null
    export let userName: string | null = null
    export let imageUrl: string | null = null
</script>

<div class="bg-white p-6 rounded-2xl shadow-2xl border border-gray-200">
    <h3 class="text-xl font-bold text-gray-700">{title}</h3>
    <p class="text-gray-500 text-sm mb-3">Posted by {userName ?? 'Unknown User'}</p>
    {#if imageUrl}
        <img 
            class="w-full h-64 object-cover rounded-lg mb-3" 
            src={imageUrl} 
            alt=""
        />
    {/if}
    <p class="text-gray-600">{description}</p>
</div>
