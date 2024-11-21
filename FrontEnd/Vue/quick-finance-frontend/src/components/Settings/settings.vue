<script lang="ts">
import { getSettings, Settings, updateSettings } from '@/api/services/generalService';
import { useAuthStore } from '@/stores/auth';
import { defineComponent, onMounted, ref, watch } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';

export default defineComponent({
    name: 'settings',
    components: {},
    setup() {
        const store = useAuthStore()
        const router = useRouter()
        const toast = useToast()

        //data
        const language = ref("ENG"); // Default language
        const currency = ref<keyof typeof currencyMap>("USD-DOLLAR"); // Strongly typed
        const currencySymbol = ref("$"); // Default currency symbol

        // Map of currency to symbol
        const currencyMap: Record<string, string> = {
            "USD-DOLLAR": "$",
            EURO: "€",
            "HND-LEMPIRA": "L.",
        };

        // Update symbol automatically when currency changes
        watch(currency, (newValue) => {
            currencySymbol.value = currencyMap[newValue] || ""; // Safe access with TypeScript.
        });

        const loadPage = async () => {
            try {
                const userId = store.user?.id || ''
                if (userId === '') throw new Error('Unable to obtain user info')

                const response = await getSettings(userId)

                if (response) {
                    // Deserialize jsonValue to a JavaScript object
                    const settings = response; // Assuming response is of type Settings
                    const jsonSettings = JSON.parse(settings.jsonValue); // This will give you the parsed object

                    // Now you can access the properties of the JSON object
                    language.value = jsonSettings.Language || "ENG"; // Default to "ENG" if not available
                    currency.value = jsonSettings.Currency || "USD-DOLLAR"; // Default to "USD-DOLLAR" if not available
                    currencySymbol.value = jsonSettings.CurrencySymbol || "$"; // Default to "$" if not available
                }
            } catch (error) {
                console.error("Failed to load settings:", error);
            }
        }

        const submitForm = async () => {
            try {
                const userId = store.user?.id || ''

                const newSettings: Settings[] = [{
                    userId: userId,
                    settingsName: "Cultural information",
                    jsonValue: JSON.stringify({
                        Language: language.value,
                        Currency: currency.value,
                        CurrencySymbol: currencySymbol.value,
                    }),
                }];

                await updateSettings(newSettings)
                    .then((result) => {
                        console.log("Settings saved successfully, response:", result);
                    })
                    .catch((error) => {
                        console.error("Failed to save settings:", error);
                    });

                toast.success("Settings has been save!");
            } catch (error) {
                console.error("Failed to save settings:", error);
            }
        }

        onMounted(async () => {
            loadPage()
        });

        return {
            submitForm,
            language,
            currency,
            currencySymbol
        }
    }
})
</script>

<template>
    <form @submit.prevent="submitForm">
        <div class="container">

            <div class="row m-0 p-1">
                <label class="form-label">Default Language</label>
                <select class="form-select" v-model="language">
                    <option value="ENG">English</option>
                    <option value="SPA">Español</option>
                </select>
            </div>

            <div class="row m-0 p-1">
                <label class="form-label">Default Currency</label>
                <select class="form-select" v-model="currency">
                    <option value="USD-DOLLAR">USD - Dollar ($)</option>
                    <option value="EURO">EU - Euro (€)</option>
                    <option value="HND-LEMPIRA">HND - Lempira (L.)</option>
                </select>
            </div>

            <div class="row p-1">
                <div class="col text-end">
                    <button type="submit" class="btn btn-primary">Save Settings</button>
                </div>
            </div>
        </div>
    </form>
</template>