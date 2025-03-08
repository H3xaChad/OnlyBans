<script>
	import { writable } from "svelte/store";

	export let onRuleCreated; // Callback-Funktion nach erfolgreicher Erstellung

	let newRuleText = writable("");
	let newRuleCategory = writable("title");
	let errorMessage = writable("");

	async function createRule() {
		if ($newRuleText.trim() === "") {
			errorMessage.set("Der Regel-Text darf nicht leer sein.");
			return;
		}

		// Hole den Token aus localStorage (wenn du JWT benutzt)
		let token = localStorage.getItem("authToken"); // Stelle sicher, dass der Token vorhanden ist

		if (!token) {
			errorMessage.set("Fehler: Kein Authentifizierungs-Token gefunden. Bitte einloggen.");
			return;
		}

		let requestBody = {
			text: $newRuleText,
			ruleCategory: $newRuleCategory === "title" ? "titleRule" : "contentRule" // Korrekte Enum-Werte senden
		};

		try {
			let response = await fetch("http://localhost:5107/api/v1/rule", {
				method: "POST",
				headers: {
					"Content-Type": "application/json",
					"Authorization": `Bearer ${token}` // Token mitsenden!
				},
				body: JSON.stringify(requestBody)
			});

			if (!response.ok) {
				let errorData = await response.json();
				throw new Error(errorData.message || `Fehler: ${response.status} ${response.statusText}`);
			}

			// Regel erfolgreich erstellt
			newRuleText.set("");
			newRuleCategory.set("title");
			onRuleCreated(); // API neu abrufen
		} catch (error) {
			console.error("Fehler beim Erstellen der Regel:", error);
			errorMessage.set(error.message);
		}
	}
</script>

<style>
    .create-rule-form {
        margin-top: 20px;
        padding: 15px;
        border-top: 2px solid #ccc;
    }
    input, select, button {
        display: block;
        width: 100%;
        margin: 10px 0;
        padding: 8px;
        font-size: 1rem;
    }
    button {
        background: #007bff;
        color: white;
        border: none;
        cursor: pointer;
    }
    button:hover {
        background: #0056b3;
    }
</style>

<div class="create-rule-form">
	<h3>Neue Regel erstellen</h3>
	{#if $errorMessage}
		<p style="color: red;">{$errorMessage}</p>
	{/if}
	<label for="new-rule-text">Regel-Text:</label>
	<input id="new-rule-text" type="text" bind:value={$newRuleText} placeholder="Gib hier die Regel ein..." />

	<label for="new-rule-category">Kategorie:</label>
	<select id="new-rule-category" bind:value={$newRuleCategory}>
		<option value="title">Title Rule</option>
		<option value="content">Content Rule</option>
	</select>

	<button on:click={createRule}>Regel erstellen</button>
</div>
