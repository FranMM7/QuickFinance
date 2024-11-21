<script lang="ts">
import { defineComponent, onMounted, ref } from "vue";
import { useToast } from "vue-toastification";
import { editlocation, fetchlocation, location, addlocation, deletelocation } from "@/api/services/locationServices";
import { useAuthStore } from "@/stores/auth";
import { ListLoader } from "vue-content-loader";
import ErrorCard from "../error/errorCard.vue";

export default defineComponent({
    name: "LocationsList",
    components: {
        ListLoader,
        ErrorCard,
    },
    setup() {
        const toast = useToast();
        const authStore = useAuthStore();

        // State
        const loading = ref(true);
        const error = ref("");
        const list = ref<location[]>([]);
        const modalShow = ref(false);
        const modalTitle = ref("Add Location");
        const locName = ref<String>('');
        const editingLocationId = ref<number | null>(null);

        // Methods
        const openModal = (record?: location) => {
            modalTitle.value = record ? "Edit Location" : "Add Location";
            locName.value = record?.name || "";
            editingLocationId.value = record?.id || null;
            modalShow.value = true;
        };

        const closeModal = () => {
            locName.value = "";
            editingLocationId.value = null;
            modalShow.value = false;
        };

        const saveLocation = async () => {
            try {
                if (!locName.value.trim()) {
                    toast.error("Location name is required!");
                    return;
                }

                if (editingLocationId.value) {
                    // Edit existing location
                    const locationToEdit = list.value.find((loc) => loc.id === editingLocationId.value);
                    if (locationToEdit) {
                        const editRecord: location = {
                            id: locationToEdit.id,
                            createdOn: locationToEdit.createdOn,
                            updatedOn: locationToEdit.updatedOn,
                            name: locName.value,
                            state: 1,
                            userId: authStore.user?.id || ''
                        }
                        await editlocation(editRecord.id, editRecord);
                        locationToEdit.name = locName.value;
                        toast.success("Location updated successfully!");
                    }
                } else {
                    // Add new location
                    const newLocation: location = {
                        id: 0, // Generate unique ID
                        createdOn: new Date(),
                        updatedOn: new Date(),
                        name: locName.value,
                        state: 1,
                        userId: authStore.user?.id || ''
                    };
                    const response = await addlocation(newLocation)
                    newLocation.id = response.id
                    list.value.push(newLocation); // Temporary push; backend sync recommended
                    toast.success("Location added successfully!");
                }
                closeModal();
            } catch (err) {
                console.error("Error while saving location data:", err);
                toast.error("An error occurred while saving. Please try again.");
            }
        };

        const deleteRecord = (id: number) => {
            const isConfirmed = confirm("Are you sure you want to delete this category?");
            if (isConfirmed) {
                deletelocation(id);
                list.value = list.value.filter((loc) => loc.id !== id);
                toast.success("Location deleted successfully!");
            }
        };

        const loadPage = async () => {
            try {
                const userId = authStore.user?.id || "";
                const response = await fetchlocation(userId);
                list.value = response || [];
            } catch (err) {
                console.error("Error while fetching data:", err);
                error.value = "Failed to load locations. Please try again.";
            } finally {
                loading.value = false;
            }
        };

        // Lifecycle
        onMounted(() => {
            loadPage();
        });

        return {
            list,
            modalShow,
            modalTitle,
            locName,
            editingLocationId,
            openModal,
            closeModal,
            saveLocation,
            deleteRecord,
            loading,
            error,
            loadPage
        };
    },
});
</script>


<template>
    <div class="container">
        <!-- Loader and Error -->
        <div v-if="loading">
            <ListLoader />
        </div>
        <div v-else-if="error">
            <ErrorCard :message="error" />
        </div>
        <div v-else>
            <div class="row mb-2 p-2">
                <div class="col">
                    <h1>Locations:</h1>
                </div>
                <div class="col-auto">
                    <div class="btn-group">
                        <button type="button" class="btn btn-info" @click="openModal()">Add Location</button>
                        <button class="btn btn-secondary" @click="loadPage">Refresh list</button>
                    </div>
                </div>
            </div>

            <!-- Locations Table -->
            <table class="table table-striped text-center">
                <thead>
                    <tr>
                        <th>Location</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="record in list" :key="record.id">
                        <td>{{ record.name }}</td>
                        <td>
                            <div class="btn-group">
                                <button type="button" class="btn btn-primary" @click="openModal(record)">Edit</button>
                                <button type="button" class="btn btn-danger"
                                    @click="deleteRecord(record.id)">Delete</button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <!-- Modal -->
        <div v-if="modalShow" class="modal show" tabindex="-1" style="display: block;" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">{{ modalTitle }}</h5>
                        <button type="button" class="btn-close" @click="closeModal"></button>
                    </div>
                    <div class="modal-body">
                        <label for="locationName">Location:</label>
                        <input v-model="locName" type="text" class="form-control" id="locationName"
                            placeholder="Enter location name" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" @click="saveLocation">Save changes</button>
                        <button type="button" class="btn btn-secondary" @click="closeModal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>