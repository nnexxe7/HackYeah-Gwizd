<template>
    <div>
      <div ref="map" class="map"></div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, onMounted } from 'vue';
  import { Plugins } from '@capacitor/core';
  
  const { Geolocation, GoogleMaps } = Plugins;
  
  const mapRef = ref<HTMLDivElement | null>(null);
  let map: google.maps.Map | null = null;
  
  onMounted(() => {
  Geolocation.getCurrentPosition().then((position: GeolocationPosition) => {
    const initialLocation = {
      lat: position.coords.latitude,
      lng: position.coords.longitude,
    };

    map = new google.maps.Map(mapRef.value!, {
      center: initialLocation,
      zoom: 14,
    });

    const marker = new google.maps.Marker({
      position: initialLocation,
      map,
      title: 'Your Location',
    });
  });
});

  </script>
  
  <style scoped>
  .map {
    width: 100%;
    height: 300px;
  }
  </style>
  