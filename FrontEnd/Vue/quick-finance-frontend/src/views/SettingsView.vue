<script lang="ts">
import LocationsList from '@/components/Settings/locationsList.vue';
import Settings from '@/components/Settings/settings.vue';
import { defineComponent, onMounted } from 'vue';

export default defineComponent({
  name: 'SettingsView',
  components: {
    Settings,
    LocationsList
  },
  setup() {
    return {}
  }
});
</script>

<template>
  <div class="container mt-4">
    <ul class="nav nav-tabs nav-fill" role="tablist">
      <li class="nav-item" role="presentation">
        <a href="#settings" class="nav-link active" data-bs-toggle="tab" aria-selected="true">My Settings</a>
      </li>
      <li class="nav-item" role="presentation">
        <a href="#locations" class="nav-link" data-bs-toggle="tab" aria-selected="true">My Locations</a>
      </li>
    </ul>

    <div class="tab-content" id="tabContent">
      <div class="tab-pane fade show active" id="settings" role="tabpanel">
        <h1>Settings</h1>
        <settings/>
      </div>
      <div class="tab-pane fade" id="locations" role="tabpanel">
        <h1>Locations:</h1>
        <LocationsList/>
      </div>
    </div>

  </div>
</template>
