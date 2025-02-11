<script lang="ts">
	import { onMount } from 'svelte'
    import { Api, type UserGetDto } from '$lib/api/Api'

    const api = new Api()

    let user: UserGetDto | null = null

	onMount(async () => {
		const token = localStorage.getItem('auth_token')
		if (!token) {
			window.location.href = '/login'
			return;
		}
        
        try {
        const user = await api.user.me({ credentials: 'include' });
            if (!user) throw new Error('Invalid response'); 
            console.log(`Got user:`, user)
        } catch (error) {
            console.error('Failed to fetch user data:', error);
            alert('Failed to fetch user data');
            localStorage.removeItem('auth_token');
            window.location.href = '/login';
        }
	});
</script>

<h1>Dashboard</h1>

{#if user}
    <div>
        <h2>Welcome, {user.userName}!</h2>
        <p><strong>Email:</strong> {user.email}</p>
        <p><strong>User ID:</strong> {user.id}</p>
    </div>
{:else}
	<p>Loading...</p>
{/if}
