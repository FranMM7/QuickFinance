<script lang="ts">
import { defineComponent, onMounted } from 'vue';

export default defineComponent({
    name: 'NotFound',
    setup() {
        onMounted(() => {
            // Retrieve theme colors dynamically from CSS variables
            const rootStyles = getComputedStyle(document.documentElement);
            const themeColors = [
                rootStyles.getPropertyValue('--bs-primary').trim() || 'blue',
                rootStyles.getPropertyValue('--bs-secondary').trim() || 'gray',
                rootStyles.getPropertyValue('--bs-success').trim() || 'green',
                rootStyles.getPropertyValue('--bs-danger').trim() || 'red',
                rootStyles.getPropertyValue('--bs-warning').trim() || 'orange',
                rootStyles.getPropertyValue('--bs-info').trim() || 'cyan',
            ];

            // Generate dynamic keyframes for color animation
            const colorKeyframes = themeColors
                .map((color, index) => {
                    const percentage = (index / themeColors.length) * 100;
                    return `${percentage}% { color: ${color}; }`;
                })
                .join(' ');

            const animationRule = `
        @keyframes themeColorCycle {
          ${colorKeyframes}
        }
      `;

            // Inject dynamic keyframes into the stylesheet
            const styleSheet = document.styleSheets[0];
            styleSheet.insertRule(animationRule, styleSheet.cssRules.length);
        });

        return {};
    },
});
</script>

<style>
@import '../../assets/404custom.css';
</style>


<template>
    <div class="align-self-center text-center m-lg-5">
        <h1 class="animation">404</h1>
        <h2 class="animation">UH OH! You're lost.</h2>
        <!-- Apply the fade-in animation to the paragraph -->
        <p class="fade-in">
            The page you are looking for does not exist.
            How you got here is a mystery. But you can click the button below
            to go back to the homepage.
        </p>
    </div>
    <div class="align-self-center text-center">
        <!-- Animated gradient button -->
        <router-link to="/">
            <button class="animated-gradient-btn">Go to Home</button>
        </router-link>
    </div>
</template>