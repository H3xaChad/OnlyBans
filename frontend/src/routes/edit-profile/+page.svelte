<script lang="ts">
    
    import { onMount } from 'svelte';
    import type { UserGetDto } from '$lib/api/Api';
    import { api } from '$lib/api/ApiService';
	import { goto } from '$app/navigation';

    let user: UserGetDto | null = null;

    onMount(async () => {
    try {
        user = await api.user.me().then(r => r.json())
        if (!user) throw new Error('User data is empty');
        console.log('User data:', user);
    } catch (error) {
        console.error('Failed to fetch user data:', error);
        goto("/login");
    }
});

</script>

<h1>Profile Information</h1>

{#if user}
    <div>
        <h2>Welcome, {user.userName}!</h2>
        <p><strong>User ID:</strong> {user.id}</p>
        <p><strong>Email:</strong> {user.email}</p>
        <p><strong>Display Name:</strong> {user.displayName}</p>
    </div>
{:else}
    <p>Loading...</p>
{/if}