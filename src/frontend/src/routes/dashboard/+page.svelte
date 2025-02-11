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
        
        // let user = await api.user.me().then(r => r.data)

		const res = await fetch('http://localhost:5107/api/v1/user/me', {
			credentials: 'include'
		});

		if (res.ok) {
			user = await res.json()
            console.log(`Got user: ${user}`)
		} else {
			alert('Failed to fetch user data')
			localStorage.removeItem('auth_token')
			window.location.href = '/login'
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
