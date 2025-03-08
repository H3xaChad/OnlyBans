<script>
	import Navbar from "$lib/components/Navbar.svelte";
	import InfoPanel from "$lib/components/admin/InfoPanel.svelte";
	import TopBar from "$lib/components/TopBar.svelte";
	import { writable } from "svelte/store";

	let selectedSubtopic = writable(null);

	function updateSubtopic(subtopic) {
		selectedSubtopic.set(subtopic);
	}
</script>

<style>
    :global(body) {
        margin: 0;
        padding: 0;
    }

    .topbar-container {
        position: fixed;
        width: 100%;
        top: 0;
        left: 0;
        z-index: 1000;
    }

    .main-container {
        display: flex;
        height: calc(100vh - 50px);
        margin-top: 50px;
        overflow: hidden;
    }

    .divider {
        width: 2px;
        background: #ccc;
        position: absolute;
        top: 100px; /* Abstand zur TopBar */
        bottom: 50px; /* Abstand zur unteren Kante */
        left: 250px; /* Positioniert ihn rechts neben der Navbar (Navbar-Breite anpassen) */
    }
</style>

<div class="topbar-container">
	<TopBar />
</div>

<div class="main-container">
	<Navbar bind:selectedSubtopic={selectedSubtopic} />
	<div class="divider"></div> <!-- Trennstrich zwischen Navbar und InfoPanel -->
	<InfoPanel selectedSubtopic={$selectedSubtopic} />
</div>
