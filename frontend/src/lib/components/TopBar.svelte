<script lang="ts">
    import { onMount } from 'svelte'
    import { api } from '$lib/api/ApiService'

    let avatarUrl: string = ''

    onMount(async () => {
        try {
            const response = await api.user.getMyAvatar()
            if (response.ok) {
                const blob = await response.blob()
                avatarUrl = URL.createObjectURL(blob)
            }
        } catch (err) {
            console.error('Failed to load avatar', err)
        }
    })
</script>

<nav class="fixed top-0 left-0 right-0 z-50 bg-white shadow-md">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between h-16 items-center">
            <div class="flex items-center space-x-10">
                <a href="/" class="text-2xl font-bold bg-gradient-to-r from-indigo-500 to-purple-500 text-transparent bg-clip-text">
                    OnlyBans
                </a>
                <a href="/feed" class="text-gray-700 hover:text-gray-900 px-3 py-2 rounded-md text-sm font-medium">
                    Feed
                </a>
                <a href="/admin" class="text-gray-700 hover:text-gray-900 px-3 py-2 rounded-md text-sm font-medium">
                    Admin
                </a>
            </div>
            <div class="flex items-center">
                <a href="/profile">
                    {#if avatarUrl}
                        <img class="h-10 w-10 rounded-full" src={avatarUrl} alt="Profile avatar">
                    {:else}
                        <div class="h-10 w-10 rounded-full bg-gray-300"></div>
                    {/if}
                </a>
            </div>
        </div>
    </div>
</nav>
