<script>
    import { goto } from '$app/navigation';
    let email = '';
    let password = '';
    
    async function login() {
        const response = await fetch('/api/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });

        if (response.ok) {
            goto('/dashboard'); // Redirect after successful login
        } else {
            alert('Login failed!');
        }
    }

    function loginWithOAuth() {
        window.location.href = '/auth/oauth'; // Redirect to OAuth provider
    }
</script>

<div class="flex min-h-screen items-center justify-center bg-gray-100">
    <div class="w-full max-w-md bg-white p-8 rounded-lg shadow-lg">
        <h2 class="text-2xl font-semibold text-gray-700 text-center mb-6">Login</h2>

        <form on:submit|preventDefault={login} class="space-y-4">
            <div>
                <label class="block text-gray-600 text-sm font-medium">Email</label>
                <input type="email" bind:value={email} required
                    class="mt-1 w-full px-4 py-2 border rounded-lg focus:ring focus:ring-blue-200 focus:outline-none">
            </div>

            <div>
                <label class="block text-gray-600 text-sm font-medium">Password</label>
                <input type="password" bind:value={password} required
                    class="mt-1 w-full px-4 py-2 border rounded-lg focus:ring focus:ring-blue-200 focus:outline-none">
            </div>

            <button type="submit"
                class="w-full bg-blue-500 hover:bg-blue-600 text-white font-semibold py-2 px-4 rounded-lg transition duration-300">
                Login
            </button>
        </form>

        <div class="text-center my-4 text-gray-500">OR</div>

        <button on:click={loginWithOAuth}
            class="w-full bg-gray-700 hover:bg-gray-800 text-white font-semibold py-2 px-4 rounded-lg transition duration-300">
            Login with OAuth
        </button>

        <p class="text-sm text-gray-500 mt-4 text-center">
            Don't have an account? <a href="/register" class="text-blue-500 hover:underline">Sign up</a>
        </p>
    </div>
</div>
