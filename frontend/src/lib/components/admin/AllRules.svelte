<script lang="ts">
	import { writable } from "svelte/store";

	export let rules = writable([]);
	export let filterCategory = writable("all");

	function applyFilter(data) {
		const category = $filterCategory;
		if (category === "all") return data;
		const categoryValue = category === "title" ? 0 : 1;
		return data.filter(rule => rule.ruleCategory === categoryValue);
	}
</script>

<style>
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
</style>

<div>
	<!-- Filter -->
	<div>
		<label for="category-filter">Filter:</label>
		<select id="category-filter" bind:value={$filterCategory}>
			<option value="all">Alle</option>
			<option value="title">Title Rule</option>
			<option value="content">Content Rule</option>
		</select>
	</div>

	<!-- Regeln anzeigen -->
	{#each applyFilter($rules) as rule}
		<div class="rule-card">
			<strong>{rule.text}</strong>
			<div class="rule-category">
				Kategorie: {rule.ruleCategory === 0 ? "Title Rule" : "Content Rule"}
			</div>
		</div>
	{/each}
</div>
