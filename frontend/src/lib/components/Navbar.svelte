<script>
	import { writable } from "svelte/store";

	export let sections = [
		{ title: "Rules", subtopics: ["All Rules", "Create", "Edit", "Delete"] },
		{ title: "Users", subtopics: ["All Users", "Unban"] },
		{ title: "Settings", subtopics: ["Account", "Security"] }
	];

	export let selectedSubtopic = writable(null);

	let activeSection = writable(null);

	function toggleSection(section) {
		activeSection.update(current => current === section ? null : section);
	}

	function selectSubtopic(subtopic) {
		selectedSubtopic.set(subtopic);
	}
</script>

<style>
    .navbar {
        width: 250px;
        padding: 15px;
        background: white; /* Hintergrund bleibt erhalten */
    }

    .section-button {
        display: block;
        width: 100%;
        text-align: left;
        padding: 10px;
        margin: 5px 0;
        background: none;
        border: none;
        cursor: pointer;
        font-size: 1rem;
    }

    .section-button:hover, .section-button:focus {
        background: #f0f0f0;
    }

    .subtopics {
        padding-left: 15px;
    }

    .subtopics button {
        display: block;
        width: 100%;
        text-align: left;
        background: none;
        border: none;
        cursor: pointer;
        padding: 5px;
    }

    .subtopics button:hover, .subtopics button:focus {
        background: #f0f0f0;
    }
</style>

<div class="navbar">
	{#each sections as section}
		<button class="section-button" on:click={() => toggleSection(section.title)}>
			{section.title}
		</button>
		{#if $activeSection === section.title}
			<div class="subtopics">
				{#each section.subtopics as subtopic}
					<button on:click={() => selectSubtopic(subtopic)}>
						{subtopic}
					</button>
				{/each}
			</div>
		{/if}
	{/each}
</div>
