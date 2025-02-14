<script lang="ts">
    import { onMount } from 'svelte'
    import { page } from '$app/stores'
    import { api } from '$lib/api/ApiService'
    import type { PostGetDto, CommentGetDto, UserGetDto, CommentCreateDto } from '$lib/api/Api'
    import TopBar from '$lib/components/TopBar.svelte'

    let post: (PostGetDto & { imageUrl?: string, userName?: string }) | null = null
    let comments: (CommentGetDto & { userName?: string })[] = []
    let loading: boolean = true
    let error: string | null = null

    let postId: string | undefined
    $: postId = $page.params.id // Get the post ID from the URL

    let newComment: string = ''
    let submittingComment: boolean = false

    async function fetchPostData() {
        if (!postId) {
            error = 'Invalid post ID'
            return
        }

        try {
            const postResponse = await api.post.getPost(postId)
            if (!postResponse.ok) throw new Error(`Error fetching post: ${postResponse.statusText}`)

            let postData = await postResponse.json() as PostGetDto
            let imageUrl: string | undefined = undefined
            let userName = 'Unknown User'
            let likeCount = postData.likeCount

            if (postData.id) {
                const imageResponse = await api.post.getPostImage(postData.id)
                if (imageResponse.ok) {
                    const blob = await imageResponse.blob()
                    imageUrl = URL.createObjectURL(blob)
                }
            }

            if (postData.userId) {
                const userResponse = await api.user.getUser(postData.userId)
                if (userResponse.ok) {
                    const user: UserGetDto = await userResponse.json()
                    userName = user.displayName!
                }
            }

            post = { ...postData, imageUrl, userName, likeCount }
            await fetchComments()
        } catch (err) {
            error = 'Failed to load post.'
            console.error(err)
        } finally {
            loading = false
        }
    }

    async function fetchComments() {
        if (!postId) return
        try {
            const commentResponse = await api.post.getComments(postId)
            if (!commentResponse.ok) throw new Error(`Error fetching comments: ${commentResponse.statusText}`)

            const commentData = await commentResponse.json() as CommentGetDto[]
            comments = await Promise.all(
                commentData.map(async (comment) => {
                    let commenterName = 'Unknown User'
                    if (comment.userId) {
                        const userResponse = await api.user.getUser(comment.userId)
                        if (userResponse.ok) {
                            const user: UserGetDto = await userResponse.json()
                            commenterName = user.displayName!
                        }
                    }
                    return { ...comment, userName: commenterName }
                })
            )
        } catch (err) {
            console.error('Failed to fetch comments:', err)
        }
    }

    async function likePost() {
        if (!postId) return
        try {
            const response = await api.post.likePost(postId)
            if (!response.ok) throw new Error(`Error liking post: ${response.statusText}`)
            if (post) post.likeCount = (post.likeCount || 0) + 1
        } catch (err) {
            console.error('Failed to like post:', err)
        }
    }

    async function submitComment() {
        if (!postId || newComment.trim().length === 0) return
        submittingComment = true

        try {
            const commentData: CommentCreateDto = { content: newComment, postId }
            const response = await api.comment.createComment(commentData)
            if (!response.ok) throw new Error(`Error posting comment: ${response.statusText}`)
            
            newComment = ''
            await fetchComments() // Refresh comments after posting
        } catch (err) {
            console.error('Failed to post comment:', err)
        } finally {
            submittingComment = false
        }
    }

    onMount(fetchPostData)
</script>

<TopBar />

<div class="min-h-screen flex flex-col items-center justify-center bg-gradient-to-b from-blue-100 to-white py-10 overflow-auto px-4">
    {#if loading}
        <p class="text-gray-600">Loading post...</p>
    {:else if error}
        <p class="text-red-500">{error}</p>
    {:else if post}
        <div class="w-full max-w-3xl bg-white p-6 rounded-2xl shadow-2xl border border-gray-200">
            <h2 class="text-3xl font-bold text-gray-700 text-center mb-4">{post.title}</h2>
            
            {#if post.imageUrl}
                <img 
                    class="w-full h-full object-cover rounded-lg shadow-lg mb-6" 
                    src={post.imageUrl} 
                    alt=""
                />
            {/if}

            <p class="text-gray-500 text-sm text-center mb-4">Posted by {post.userName ?? 'Unknown User'}</p>

            <p class="text-gray-600 text-lg mb-4">{post.description}</p>

            <div class="flex justify-between items-center text-gray-700 mb-6">
                <button on:click={likePost} class="text-lg font-semibold hover:text-red-500 transition">
                    ❤️ {post.likeCount} Likes
                </button>
                <p class="text-sm">{comments.length} Comments</p>
            </div>

            <div class="border-t border-gray-300 pt-4">
                <h3 class="text-xl font-semibold text-gray-700 mb-4">Comments</h3>
                
                {#if comments.length === 0}
                    <p class="text-gray-500">No comments yet.</p>
                {:else}
                    <div class="space-y-4">
                        {#each comments as comment}
                            <div class="bg-gray-100 p-4 rounded-lg shadow-sm">
                                <p class="text-sm font-semibold text-gray-700">{comment.userName}</p>
                                <p class="text-gray-600">{comment.content}</p>
                                <p class="text-xs text-gray-500">{new Date(comment.createdAt!).toLocaleString()}</p>
                            </div>
                        {/each}
                    </div>
                {/if}

                <!-- Comment Input -->
                <div class="mt-6">
                    <textarea 
                        bind:value={newComment}
                        placeholder="Write a comment..." 
                        class="w-full p-3 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                    ></textarea>
                    <button 
                        on:click={submitComment} 
                        class="mt-2 px-4 py-2 bg-blue-500 text-white rounded-lg shadow hover:bg-blue-600 transition"
                        disabled={submittingComment}
                    >
                        {submittingComment ? 'Posting...' : 'Post Comment'}
                    </button>
                </div>
            </div>
        </div>
    {/if}
</div>
