<script lang="ts">
    import { goto } from '$app/navigation'
    import type { LoginDto } from '$lib/api/Api'
    import { api, apiLoginUrl } from '$lib/api/ApiService'
	import Input from '$lib/components/Input.svelte';

    let email = ''
    let password = ''

    async function handleLogin(event: Event) {
        event.preventDefault()
        const loginDto: LoginDto = { email, password }
        try {
            const user = await api.auth.login(loginDto).then(r => r.json())
            if (!user) throw new Error('Got no user data')
            localStorage.setItem('auth_token', user.token)
            goto('/feed')
        } catch (error) {
            console.error('Login failed:', error)
            alert('Login failed')
        }
    }

    function handleOAuth(provider: string) {
        const returnUrl = encodeURIComponent(`${window.location.origin}/feed`)
        const authUrl = `${apiLoginUrl}/${provider}?returnUrl=${returnUrl}`
        window.location.href = authUrl
    }
</script>

<div class="min-h-screen flex items-center justify-center bg-gradient-to-b from-blue-100 to-white">
    <div class="bg-white p-10 rounded-2xl shadow-2xl w-96 slide-in border border-gray-200">
        <h2 class="text-2xl font-bold text-gray-700 text-center mb-6">Login</h2>
        
        <form on:submit={handleLogin} class="flex flex-col space-y-4">
            <Input bind:value={email} type="email" placeholder="Email"/>
            <Input bind:value={password} type="password" placeholder="Password"/>
            <button type="submit" class="cursor-pointer p-3 bg-blue-500 text-white font-semibold rounded-lg hover:bg-blue-600 transition">
                Login
            </button>
        </form>

        <div class="text-center text-gray-500 my-4">or</div>

        <button on:click={() => handleOAuth('bosch')} class=" cursor-pointer p-3 bg-gray-700 text-white font-semibold rounded-lg hover:bg-gray-800 transition w-full">
            Login with OAuth
        </button>
    </div>
</div>