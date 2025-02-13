<script lang="ts">
    import { onMount } from 'svelte'
    import { api } from '$lib/api/ApiService'
    import type { PostGetDto } from '$lib/api/Api'
    import Post from '$lib/components/Post.svelte'

    let posts: (PostGetDto & { imageUrl?: string, userName?: string })[] = []
    let loading: boolean = true
    let error: string | null = null

    onMount(async () => {
        try {
            const response = await api.post.getAllPosts()
            if (!response.ok) {
                throw new Error(`Error fetching posts: ${response.statusText}`)
            }
            const data = await response.json() as PostGetDto[]
            posts = await Promise.all(
                data.map(async (post) => {
                    let imageUrl: string | undefined = undefined
                    if (post.id) {
                        const imageResponse = await api.post.getPostImage(post.id)
                        if (!imageResponse.ok) {
                            throw new Error(`Error fetching image for post ${post.id}`)
                        }
                        const blob = await imageResponse.blob()
                        imageUrl = URL.createObjectURL(blob)
                    }
                    let userName = 'Unknown User'
                    if (post.userId) {
                        const userResponse = await api.user.getUser(post.userId)
                        if (userResponse.ok) {
                            const user = await userResponse.json()
                            userName = user.displayName
                        }
                    }
                    return {
                        ...post,
                        imageUrl,
                        userName
                    }
                })
            )
        } catch (err) {
            error = 'Failed to load posts.'
            console.error(err)
        } finally {
            loading = false
        }
    })
</script>

<div class="min-h-screen flex flex-col items-center justify-center bg-gradient-to-b from-blue-100 to-white py-10 overflow-auto">
    <h2 class="text-2xl font-bold text-gray-700 text-center mb-6">Feed</h2>

    {#if loading}
        <p class="text-gray-600">Loading posts...</p>
    {:else if error}
        <p class="text-red-500">{error}</p>
    {:else if posts.length === 0}
        <p class="text-gray-600">No posts available.</p>
    {:else}
        <div class="w-full max-w-2xl space-y-6">
            {#each posts as post}
                <Post
                    title={post.title} 
                    description={post.description} 
                    userName={post.userName} 
                    imageUrl={post.imageUrl} 
                />
            {/each}
        </div>
    {/if}
</div>