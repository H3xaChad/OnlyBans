<script lang="ts">

    import { goto } from '$app/navigation'
    import type { LoginDto } from '$lib/api/Api'
    import { api } from '$lib/api/ApiService';

    let email = ''
    let password = ''

    async function handleLogin(event: Event) {
        event.preventDefault()
        const loginDto: LoginDto = { email, password }
        try {
            const user = await api.auth.login(loginDto).then(r => r.json())
            localStorage.setItem('auth_token', user.token)
            goto('/dashboard')
        } catch (error) {
            console.error('Login failed:', error)
            alert('Login failed')
        }
    }

    function handleOAuth() {
        window.location.href = 'http://localhost:5107/api/v1/auth/login/bosch'
    }
</script>


<div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-300 to-blue-500">
    <div class="bg-white bg-opacity-95 p-8 rounded-xl shadow-lg w-80 slide-in">
        <form on:submit={handleLogin} class="flex flex-col space-y-4">
            <input
                type="email"
                bind:value={email}
                placeholder="Email"
                required
                class="p-2 border border-gray-300 rounded focus:border-blue-400 focus:outline-none"
            >
            <input
                type="password"
                bind:value={password}
                placeholder="Password"
                required
                class="p-2 border border-gray-300 rounded focus:border-blue-400 focus:outline-none"
            >
            <button type="submit" class="p-2 bg-blue-400 text-white font-bold rounded hover:bg-blue-500 transition cursor-pointer">
                Login
            </button>
        </form>
        <button on:click={handleOAuth} class="mt-4 p-2 bg-red-400 text-white font-bold rounded hover:bg-red-500 transition w-full cursor-pointer">
            Login with OAuth
        </button>
    </div>
</div>