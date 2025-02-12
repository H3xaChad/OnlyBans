<script lang="ts">

    import { goto } from '$app/navigation';
    import type { UserCreateDto } from '$lib/api/Api';
    import { api } from '$lib/api/ApiService';

    let userName = '';
    let displayName = '';
    let email = '';
    let phoneNumber = '';
    let birthDate = '';
    let password = '';

    async function handleRegister(event: Event) {
        event.preventDefault();
        const userCreateDto: UserCreateDto = { userName, displayName, email, phoneNumber, birthDate, password };
        try {
            await api.auth.register(userCreateDto);
            alert('Registration successful! Please log in.');
            goto('/login');
        } catch (error) {
            console.error('Registration failed:', error);
            alert('Registration failed');
        }
    }
</script>

<div class="min-h-screen flex items-center justify-center bg-gradient-to-b from-blue-100 to-white">
    <div class="bg-white p-10 rounded-2xl shadow-2xl w-96 slide-in border border-gray-200">
        <h2 class="text-2xl font-bold text-gray-700 text-center mb-6">Register</h2>
        
        <form on:submit={handleRegister} class="flex flex-col space-y-4">
            <input type="text" bind:value={userName} placeholder="Username" required class="p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-300 focus:outline-none">
            <input type="text" bind:value={displayName} placeholder="Display Name" required class="p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-300 focus:outline-none">
            <input type="email" bind:value={email} placeholder="Email" required class="p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-300 focus:outline-none">
            <input type="tel" bind:value={phoneNumber} placeholder="Phone Number" required class="p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-300 focus:outline-none">
            <input type="date" bind:value={birthDate} required class="p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-300 focus:outline-none">
            <input type="password" bind:value={password} placeholder="Password" required class="p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-300 focus:outline-none">
            <button type="submit" class="cursor-pointer p-3 bg-blue-500 text-white font-semibold rounded-lg hover:bg-blue-600 transition">
                Register
            </button>
        </form>
    </div>
</div>