<script lang="ts">
	import { writable } from "svelte/store";

	/**
	 * @type {string}
	 */
	export let selectedSubtopic; // Vom Parent übergebene Auswahl aus der Navbar
	let rules = writable([]); // Speichert alle Regeln
	let filteredRules = writable([]); // Speichert die gefilterten Regeln
	let loading = writable(false);
	let errorMessage = writable("");
	let filterCategory = writable("all"); // Standard: Alle Regeln anzeigen

	async function fetchData() {
		if (selectedSubtopic !== "All Rules") {
			// Wenn nicht "All Rules" gewählt wurde, leere die Listen
			rules.set([]);
			filteredRules.set([]);
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

			// Prüfen, ob die API ein Array zurückgibt
			if (Array.isArray(data)) {
				rules.set(data); // Setzt die Regel-Liste
				applyFilter(data); // Filter direkt nach Laden der Daten anwenden
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

	// Funktion zum Anwenden des Filters
	function applyFilter(data = $rules) {
		const category = $filterCategory;
		if (category === "all") {
			filteredRules.set(data); // Alle Regeln anzeigen
		} else {
			const categoryValue = category === "title" ? 0 : 1;
			filteredRules.set(data.filter(rule => rule.ruleCategory === categoryValue));
		}
	}

	// Reagiere auf Änderungen am Filter
	$: applyFilter();

	// API-Abfrage ausführen, wenn "All Rules" gewählt wurde
	$: if (selectedSubtopic === "All Rules") fetchData();
</script>

<style>
    .info-panel {
        flex-grow: 1;
        padding: 15px;
        background: white;
        overflow-y: auto;
    }
    .rule-card {
        border: 1px solid #ccc;
        padding: 10px;
        margin-bottom: 10px;
        border-radius: 5px;
        background: #f9f9f9;
    }
    .rule-category {
        font-size: 0.9em;
        color: #555;
    }
    .filter-container {
        margin-bottom: 15px;
    }
    select {
        padding: 5px;
        font-size: 1rem;
    }
</style>

<!-- NUR anzeigen, wenn "All Rules" in der Navbar gewählt wurde -->
{#if selectedSubtopic === "All Rules"}
	<div class="info-panel">
		<h2>Informationen</h2>

		<!-- Filter-Option -->
		<div class="filter-container">
			<label for="category-filter">Filter:</label>
			<select id="category-filter" bind:value={$filterCategory} on:change={() => applyFilter()}>
				<option value="all">Alle</option>
				<option value="title">Title Rule</option>
				<option value="content">Content Rule</option>
			</select>
		</div>

		{#if $loading}
			<p>Daten werden geladen...</p>
		{:else if $errorMessage}
			<p style="color: red;">{$errorMessage}</p>
		{:else if $filteredRules.length === 0}
			<p>Keine passenden Regeln gefunden.</p>
		{:else}
			{#each $filteredRules as rule}
				<div class="rule-card">
					<strong>{rule.text}</strong>
					<div class="rule-category">
						Kategorie: {rule.ruleCategory === 0 ? "Title Rule" : "Content Rule"}
					</div>
				</div>
			{/each}
		{/if}
	</div>
{/if}
