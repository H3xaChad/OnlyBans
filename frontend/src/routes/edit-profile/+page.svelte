<script lang="ts">
    import { onMount } from 'svelte'
    import { goto } from '$app/navigation'
    import { api } from '$lib/api/ApiService'
    import type { UserGetMyDto, UserUpdateDto } from '$lib/api/Api'
    import Topbar from '$lib/components/TopBar.svelte'
    import Input from '$lib/components/Input.svelte'

    let user: UserGetMyDto | null = null
    let loading: boolean = true
    let error: string | null = null

    let userName = ''
    let displayName = ''
    let email = ''
    let phoneNumber = ''
    let password = ''

    let currentAvatarUrl = ''
    let newAvatarUrl = ''
    let newAvatarFile: File | null = null

    onMount(async () => {
        try {
            const response = await api.user.me()
            if (!response.ok) {
                throw new Error(`Error fetching user: ${response.statusText}`)
            }
            user = await response.json() as UserGetMyDto
            if (!user) throw new Error('User data is empty')

            // For local users, prefill form fields
            if (!user.isOAuthUser) {
                userName = user.userName ?? ''
                displayName = user.displayName ?? ''
                email = user.email ?? ''
            }
        } catch (err) {
            console.error('Failed to fetch user data:', err)
            goto('/login')
        } finally {
            loading = false
        }
    })

    // Trigger the hidden file input when the avatar button is clicked
    function handleAvatarClick() {
        document.getElementById('avatarInput')?.click()
    }

    function handleAvatarChange(event: Event) {
        const target = event.target as HTMLInputElement
        if (target.files && target.files.length > 0) {
            newAvatarFile = target.files[0]
            newAvatarUrl = URL.createObjectURL(newAvatarFile)
        }
    }

    async function handleSave() {
        try {
            // If a new avatar is selected, update it first via the dedicated endpoint
            if (newAvatarFile) {
                const avatarResponse = await api.user.updateMyAvatar({ image: newAvatarFile })
                if (!avatarResponse.ok) {
                    throw new Error(`Error updating avatar: ${avatarResponse.statusText}`)
                }
            }
            // Update the profile info (excluding avatar) via JSON
            const updatePayload: UserUpdateDto = {
                userName,
                displayName,
                email,
                phoneNumber,
                password
            }
            const updateResponse = await api.user.updateUser(updatePayload)
            if (!updateResponse.ok) {
                throw new Error(`Error updating profile: ${updateResponse.statusText}`)
            }
            goto('/profile')
        } catch (err) {
            console.error('Failed to update profile:', err)
            error = 'Failed to update profile'
        }
    }
</script>

<Topbar />

<div class="pt-20 max-w-4xl mx-auto px-4">
    {#if loading}
        <p class="text-center text-gray-700 mt-8">Loading profile...</p>
    {:else if user}
        <h1 class="text-3xl font-bold mb-6">Edit Profile</h1>
        {#if user.isOAuthUser}
            <!-- OAuth users see a read-only view -->
            <div class="bg-gray-100 p-6 rounded-lg">
                <div class="flex items-center mb-4">
                    <img src={currentAvatarUrl} alt="" class="w-24 h-24 rounded-full object-cover mr-4" />
                    <div>
                        <p class="font-bold text-xl">{user.userName}</p>
                        <p class="text-gray-600">Managed by your organisation</p>
                    </div>
                </div>
                <div class="space-y-2">
                    <p><strong>Email:</strong> {user.email}</p>
                    <p><strong>User Name:</strong> {user.userName}</p>
                    <p><strong>Display Name:</strong> {user.displayName}</p>
                </div>
                <p class="mt-4 text-gray-500 text-sm">
                    Your profile information is managed by your organisation and cannot be changed.
                </p>
            </div>
        {:else}
            <!-- Local users get an editable form -->
            <div class="bg-white shadow-md rounded-lg p-6">
                <div class="flex flex-col items-center mb-6">
                    <button type="button" on:click={handleAvatarClick} class="cursor-pointer focus:outline-none" aria-label="Change profile picture">
                        {#if newAvatarUrl}
                            <img src={newAvatarUrl} alt="" class="w-24 h-24 rounded-full object-cover" />
                        {:else if currentAvatarUrl}
                            <img src={currentAvatarUrl} alt="" class="w-24 h-24 rounded-full object-cover" />
                        {:else}
                            <div class="w-24 h-24 rounded-full bg-gray-300"></div>
                        {/if}
                    </button>
                    <input
                        type="file"
                        id="avatarInput"
                        class="hidden"
                        accept="image/*"
                        on:change={handleAvatarChange}
                    />
                </div>
                <form on:submit|preventDefault={handleSave} class="space-y-4">
                    <div>
                        <label for="username" class="block text-gray-700">User Name</label>
                        <Input id="username" type="text" bind:value={userName} placeholder="User Name" />
                    </div>
                    <div>
                        <label for="displayName" class="block text-gray-700">Display Name</label>
                        <Input id="displayName" type="text" bind:value={displayName} placeholder="Display Name" />
                    </div>
                    <div>
                        <label for="email" class="block text-gray-700">Email</label>
                        <Input id="email" type="email" bind:value={email} placeholder="Email" />
                    </div>
                    <div>
                        <label for="phoneNumber" class="block text-gray-700">Phone Number</label>
                        <Input id="phoneNumber" type="tel" bind:value={phoneNumber} placeholder="Phone Number" />
                    </div>
                    <div>
                        <label for="password" class="block text-gray-700">Password</label>
                        <Input id="password" type="password" bind:value={password} placeholder="New Password" />
                    </div>
                    {#if error}
                        <p class="text-red-500">{error}</p>
                    {/if}
                    <div class="flex space-x-4">
                        <button type="submit" class="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600">
                            Save Changes
                        </button>
                        <button
                            type="button"
                            on:click={() => goto('/profile')}
                            class="px-4 py-2 border rounded-md hover:bg-gray-100"
                        >
                            Cancel
                        </button>
                    </div>
                </form>
            </div>
        {/if}
    {/if}
</div>
