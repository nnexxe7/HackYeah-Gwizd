<template>
  <div>
  <capacitor-google-map
  ref="mapRef"
  style="display: inline-block; width: 100vw; height: 86vh"
  >
  </capacitor-google-map>
  </div>
  Aktualna lokalizacja: {{ currentLocation.lat }}, {{ currentLocation.lng }}
  </template>

  <style>
  .gm-bundled-control.gm-bundled-control-on-bottom,
  .gm-bundled-control.gm-bundled-control-on-top {
    display: none !important;}
  .gm-fullscreen-control {
    display: none !important;}
  </style>
  
  
  <script setup lang="ts">
  import { onMounted, nextTick, ref, onUnmounted } from "vue";
  import { GoogleMap } from "@capacitor/google-maps";
  import { Geolocation, GeolocationPosition } from '@capacitor/geolocation';
  import { watch } from "vue";
  
  const currentLocation = ref({ lat: 0, lng: 0 });
  
  // props
  const props = defineProps<{
  markerData: { coordinate: any; title: string; snippet: string }[];
  }>();
  // eventy
  const emits = defineEmits<{
  (event: "onMarkerClicked", info: any): void;
  (event: "onMapClicked"): void;
  }>();
  
  const mapRef = ref<HTMLElement>();
  const markerIds = ref<string[] | undefined>();
  let newMap: GoogleMap;
  
  onMounted(async () => {
  console.log("mounted ", mapRef.value);
  await nextTick();
  await createMap();
  });
  
  // kasowanie znacznikow
  onUnmounted(() => {
  console.log("onunmounted");
  newMap.removeMarkers(markerIds?.value as string[]);
  });
  
  const addSomeMarkers = async (newMap: GoogleMap) => {
  markerIds?.value && newMap.removeMarkers(markerIds?.value as string[]);
  
  // narysuj kazdy punkt na mapie
  let markers = props.markerData.map(({ coordinate, title, snippet }) => {
  return {
  coordinate,
  title,
  snippet,
  };
  });
  
  markerIds.value = await newMap.addMarkers(markers);
  };
  
  async function createMap() {
  if (!mapRef.value) return;
  
  // pobiernaie aktualnej pozycji
  try {
    const position = await Geolocation.getCurrentPosition();
    const { latitude, longitude } = position.coords;
  
    // renderowanie mapy i aktualizowanie centrowania
    newMap = await GoogleMap.create({
      id: "my-cool-map",
      element: mapRef.value,
      apiKey: "AIzaSyDWYUDxr7VCsb-ZBSuXhatZt74hegrReKk",
      config: {
        center: {
          lat: latitude,
          lng: longitude,
        },
        zoom: 12,
      },
    });
    
  
    // dodanie markerow
    addSomeMarkers(newMap);
  
    // event nasluchiwania
    newMap.setOnMarkerClickListener((event) => {
      emits("onMarkerClicked", event);
    });
  
    newMap.setOnMapClickListener(() => {
      emits("onMapClicked");
    });
  
    // aktualizacja pozycji
    currentLocation.value = { lat: latitude, lng: longitude };
    console.log("Aktualna lokalizacja:", currentLocation.value);
  } catch (error) {
    console.error("Error getting current location:", error);
  }
  
  }
  </script>
  