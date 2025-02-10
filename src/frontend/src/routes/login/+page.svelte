<script lang="ts">
	import { goto } from '$app/navigation';
	let email = '';
	let password = '';

	async function handleLogin(event: Event) {
		event.preventDefault();
		const res = await fetch('http://localhost:5107/api/v1/auth/login', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({ email, password })
		});

		if (res.ok) {
			const data = await res.json();
			localStorage.setItem('auth_token', data.token);
			goto('/dashboard');
		} else {
			alert('Login failed');
		}
	}

	function handleOAuth() {
		window.location.href = 'http://localhost:5107/api/v1/auth/bosch';
	}
</script>

<form on:submit={handleLogin}>
	<input type="email" bind:value={email} placeholder="Email" required>
	<input type="password" bind:value={password} placeholder="Password" required>
	<button type="submit">Login</button>
</form>

<button on:click={handleOAuth}>Login with OAuth</button>
