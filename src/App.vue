<template>
  <ion-app>
    <ion-router-outlet />
  </ion-app>
</template>

<style>
ion-content {
  --background: transparent;
}

body {
  background: transparent;
}</style>

<script setup lang="ts">
import { IonApp, IonRouterOutlet } from '@ionic/vue';
</script>
