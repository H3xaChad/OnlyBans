<script>
	import { writable } from "svelte/store";
	import AllRules from '$lib/components/admin/AllRules.svelte';
	import CreateRule from '$lib/components/admin/CreateRule.svelte';

	export let selectedSubtopic;
	let rules = writable([]);
	let filterCategory = writable("all");
	let loading = writable(false);
	let errorMessage = writable("");

	async function fetchData() {
		if (selectedSubtopic !== "All Rules" && selectedSubtopic !== "Create") {
			rules.set([]);
			errorMessage.set("");
			return;
		}

		loading.set(true);
		errorMessage.set("");

		try {
			let response = await fetch("http://localhost:5107/api/v1/rule");

			if (!response.ok) {
				throw new Error(`Fehler: ${response.status} ${response.statusText}`);
			}

			let data = await response.json();

			if (Array.isArray(data)) {
				rules.set(data);
			} else {
				errorMessage.set("Fehler: Die API hat kein Array zurückgegeben.");
			}
		} catch (error) {
			console.error("API-Fehler:", error);
			errorMessage.set(`Fehler beim Laden der Daten: ${error.message}`);
		} finally {
			loading.set(false);
		}
	}

	// API-Abfrage ausführen, wenn "All Rules" oder "Create" gewählt wurde
	$: if (selectedSubtopic === "All Rules" || selectedSubtopic === "Create") fetchData();
</script>

<style>
    .info-panel {
        flex-grow: 1;
        padding: 15px;
        background: white;
        overflow-y: auto;
    }
</style>

{#if selectedSubtopic === "All Rules" || selectedSubtopic === "Create"}
	<div class="info-panel">
		<h2>Informationen</h2>

		{#if $loading}
			<p>Daten werden geladen...</p>
		{:else if $errorMessage}
			<p style="color: red;">{$errorMessage}</p>
		{:else}
			<AllRules rules={rules} filterCategory={filterCategory} />
		{/if}

		<!-- Falls "Create" gewählt ist, auch das Formular anzeigen -->
		{#if selectedSubtopic === "Create"}
			<CreateRule onRuleCreated={fetchData} />
		{/if}
	</div>
{/if}