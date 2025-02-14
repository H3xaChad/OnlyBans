<script lang="ts">
    import { onMount } from 'svelte'
    import { api } from '$lib/api/ApiService'
    import type { UserGetMyDto, PostGetDto } from '$lib/api/Api'
    import { goto } from '$app/navigation'
	import Topbar from '$lib/components/TopBar.svelte'

    let user: UserGetMyDto | null = null
    let posts: (PostGetDto & { imageUrl?: string })[] = []
    let profileAvatarUrl: string = ''
    let loading: boolean = true
    let postsLoading: boolean = true
    let error: string | null = null

    onMount(async () => {
        try {
            const userResponse = await api.user.me()
            if (!userResponse.ok) {
                throw new Error(`Error fetching user: ${userResponse.statusText}`)
            }
            user = await userResponse.json() as UserGetMyDto
            if (!user) throw new Error('User data is empty')

            const avatarResponse = await api.user.getMyAvatar()
            if (avatarResponse.ok) {
                const blob = await avatarResponse.blob()
                profileAvatarUrl = URL.createObjectURL(blob)
            }

            const postsResponse = await api.post.getMyPosts()
            if (postsResponse.ok) {
                const postsData = await postsResponse.json() as PostGetDto[]
                posts = await Promise.all(
                    postsData.map(async (post) => {
                        let imageUrl: string | undefined = undefined
                        if (post.id) {
                            const imageResponse = await api.post.getPostImage(post.id)
                            if (imageResponse.ok) {
                                const blob = await imageResponse.blob()
                                imageUrl = URL.createObjectURL(blob)
                            }
                        }
                        return { ...post, imageUrl }
                    })
                )
            }
        } catch (err) {
            console.error('Failed to fetch data:', err)
            goto('/login')
        } finally {
            loading = false
            postsLoading = false
        }
    })

    function handleEditProfile() {
        goto('/edit-profile')
    }

    function handleLogout() {
        api.auth.logout()
        goto('/login')
    }
</script>

<Topbar/>

<div class="pt-20 max-w-5xl mx-auto px-4">
    {#if loading}
        <p class="text-center text-gray-700 mt-8">Loading profile...</p>
    {:else if user}
        <!-- Profile Header -->
        <div class="mb-8">
            <div class="flex flex-col md:flex-row items-center md:items-start md:space-x-8">
                <div class="mb-4 md:mb-0">
                    {#if profileAvatarUrl}
                        <img src={profileAvatarUrl} alt="Profile avatar" class="w-32 h-32 rounded-full object-cover" />
                    {:else}
                        <div class="w-32 h-32 rounded-full bg-gray-300"></div>
                    {/if}
                </div>
                <div class="flex-1">
                    <div class="flex items-center space-x-4">
                        <h2 class="text-3xl font-bold">{user.userName}</h2>
                        <button on:click={handleEditProfile} class="cursor-pointer px-4 py-2 border rounded-md text-sm font-medium text-gray-700 hover:bg-gray-100">
                            Edit Profile
                        </button>
                        <button on:click={handleLogout} class="cursor-pointer px-4 py-2 border rounded-md text-sm font-medium text-gray-700 hover:bg-gray-100">
                            Logout
                        </button>
                    </div>
                    <div class="mt-4">
                        <p><strong>Email:</strong> {user.email}</p>
                        <p><strong>Display Name:</strong> {user.displayName}</p>
                    </div>
                </div>
            </div>
        </div>

        <hr class="mb-8" />

        <!-- Posts Grid -->
        <div>
            <h3 class="text-2xl font-bold mb-4">My Posts</h3>
            {#if postsLoading}
                <p>Loading posts...</p>
            {:else if posts.length === 0}
                <p>No posts found.</p>
            {:else}
                <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
                    {#each posts as post}
                        <div class="relative group">
                            {#if post.imageUrl}
                                <img src={post.imageUrl} alt={post.title || 'User post'} class="w-full h-64 object-cover rounded-lg" />
                            {:else}
                                <div class="w-full h-64 bg-gray-200 rounded-lg"></div>
                            {/if}
                            <div class="absolute inset-0 bg-black bg-opacity-25 opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center rounded-lg">
                                <span class="text-white text-lg font-bold">{post.title}</span>
                            </div>
                        </div>
                    {/each}
                </div>
            {/if}
        </div>
    {/if}
</div>
