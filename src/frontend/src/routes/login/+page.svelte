<script lang="ts">

	import { goto } from '$app/navigation';
	import { Api } from '$lib/api/Api';
    import type { LoginDto } from '$lib/api/Api';

    const api = new Api()

	let email = '';
	let password = '';

	async function handleLogin(event: Event) {
        event.preventDefault();

        const loginDto: LoginDto = { email, password };

        try {
            const res = await api.auth.login(loginDto).then(r => r.json());
            localStorage.setItem('auth_token', res.token);
            goto('/dashboard');
        } catch (error) {
            console.error('Login failed:', error);
            alert('Login failed');
        }
    }

	function handleOAuth() {
		window.location.href = 'http://localhost:5107/api/v1/auth/login/bosch';
	}
</script>

<form on:submit={handleLogin}>
	<input type="email" bind:value={email} placeholder="Email" required>
	<input type="password" bind:value={password} placeholder="Password" required>
	<button type="submit">Login</button>
</form>

<button on:click={handleOAuth}>Login with OAuth</button>
