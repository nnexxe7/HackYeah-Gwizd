<template>
  <AddTootBtnVue />
<div>
<capacitor-google-map
  ref="mapRef"
  style="display: inline-block; width: 100vw; height: 104vh"
>
</capacitor-google-map>
</div>
<div class="strip">
<ion-avatar class="avatar">
  <img alt="Up" src="../assets/wildanimal.png" />
</ion-avatar>
<div class="points">PUNKTY</div>
</div>
<AddTootBtnVue />

</template>

<style>

</style>

<script setup lang="ts">
import {
onMounted,
nextTick,
ref,
onUnmounted,
defineProps,
defineEmits,
} from "vue";
import { GoogleMap } from "@capacitor/google-maps";
import { Geolocation } from "@capacitor/geolocation";
import AddTootBtnVue from "@/components/AddTootBtn.vue";
import { Global } from './../global'


const currentLocation = ref({ lat: 0, lng: 0 });

// Props
const props = defineProps<{
markerData: { coordinate: any; title: string; snippet: string }[];
}>();

// Emits
const emits = defineEmits<{
(event: "onMarkerClicked", info: any): void;
(event: "onMapClicked"): void;
}>();

const customMarker = ref<{ coordinate: any; title: string; snippet: string } | null>(null);


const mapRef = ref<HTMLElement>();
const markerIds = ref<string[] | undefined>();
let newMap: GoogleMap;

// Define custom marker icon
const customMarkerIcon = {
url: "@/assets/marker.svg",
size: {
width: 50,
height: 50,
},
};

// Funkcja do aktualizacji położenia markera w czasie rzeczywistym
const updateMarkerPosition = async () => {
try {
const position = await Geolocation.getCurrentPosition();
const { latitude, longitude } = position.coords;
currentLocation.value = { lat: latitude, lng: longitude };

// Aktualizacja położenia markera
const customMarker = {
  coordinate: { lat: latitude, lng: longitude },
  icon: customMarkerIcon,
  title: "Moja aktualna lokalizacja",
  snippet: "Opis mojej lokalizacji",
  visible: true,
};

// Usunięcie istniejącego markera i dodanie nowego z aktualnym położeniem
markerIds?.value && newMap.removeMarkers(markerIds?.value as string[]);
markerIds.value = await newMap.addMarkers([customMarker]);
} catch (error) {
console.error("Error getting current location:", error);
}
};

onMounted(async () => {
console.log("mounted ", mapRef.value);
await nextTick();
await createMap();
updateMarkerPosition(); // Aktualizuj marker na początku

// aktualizowanie lokacji
setInterval(updateMarkerPosition, 5000);
});

// Kasowanie znaczników
onUnmounted(() => {
console.log("onunmounted");
newMap.removeMarkers(markerIds?.value as string[]);
});

/**
* Dodawanie znaczników do mapy przy użyciu rekwizytu przekazanego do komponentu
* @param newMap
*/
const addSomeMarkers = async (newMap: GoogleMap) => {
markerIds?.value && newMap.removeMarkers(markerIds?.value as string[]);

// Pobierz aktualną lokalizację
const position = await Geolocation.getCurrentPosition();
const { latitude, longitude } = position.coords;

// Stwórz marker z niestandardową ikoną
const customMarker = {
coordinate: { lat: latitude, lng: longitude },
icon: customMarkerIcon,
title: "Moja aktualna lokalizacja",
snippet: "Opis mojej lokalizacji",
visible: true,
};

markerIds.value = await newMap.addMarkers([customMarker]);
};

/**
* Tworzenie mapy
*/
async function createMap() {
if (!mapRef.value) return;

// Pobieranie aktualnej pozycji
try {
const position = await Geolocation.getCurrentPosition();
const { latitude, longitude } = position.coords;

// Renderowanie mapy i aktualizacja centrowania
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
    disableDefaultUI: true,
  },
});

// Dodawanie markerów
addSomeMarkers(newMap);

const currentMarker = ref<string | null>(null);
// Nasłuchiwanie zdarzeń
newMap.setOnMapClickListener(async (event) => {
  const { latitude, longitude } = event;

  // Usuń poprzedni marker, jeśli istnieje
  if (currentMarker.value) {
    newMap.removeMarker(currentMarker.value);
  }

  // Stwórz marker z niestandardową ikoną na podstawie klikniętej lokalizacji
  const customMarker = {
    coordinate: { lat: latitude, lng: longitude },
    title: "Nowy marker",
    snippet: "Opis nowego markera",
  };

  Global.MarketLat = customMarker.coordinate.lat
  Global.MarketLng = customMarker.coordinate.lng

  // Dodaj nowy marker do mapy i zaktualizuj currentMarker
  const markerId = await newMap.addMarker(customMarker);
  currentMarker.value = markerId;

  console.log("Stworzono nowy marker na pozycji:", event);
});

// Aktualizacja pozycji
currentLocation.value = { lat: latitude, lng: longitude };
console.log("Aktualna lokalizacja:", currentLocation.value);
} catch (error) {
console.error("Error getting current location:", error);
}

}
</script>
<style>
.strip{
height: 40px;
position: absolute;
top: 0px;
width: 100%;
background-color: #fff;
z-index: 10;
display: flex;
justify-content: space-between;
}

</style>
