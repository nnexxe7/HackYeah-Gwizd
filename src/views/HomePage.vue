<template>
  <AddTootBtnVue />
  <ion-header>
    <ion-toolbar>
      <ion-button slot="start" class="avatar-btn" fill="clear">
      <ion-avatar class="avatar">
           <img alt="Up" src="../assets/wildanimal.png" />
          </ion-avatar>
      </ion-button>
      <ion-title class="title">Dzika Orkiestra</ion-title>
      <ion-button slot="end" class="points-btn" fill="clear">
        <div></div>
        <ion-avatar class="avatar">
           <img alt="star" class="star" src="../assets/point-star.svg" />
          </ion-avatar>
          <div class="points-value">{{points}}</div>
      </ion-button>
        </ion-toolbar>
  </ion-header>
<div>
<capacitor-google-map
  ref="mapRef"
  style="display: inline-block; width: 100vw; height: 104vh"
>
</capacitor-google-map>
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

let points=ref(0)
const fetchPoints = async () => {
  try {
    const response = await fetch("http://35.158.22.100:5000/api/users/getuser?username=testuser");
    
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }

    const responseData = await response.json();
    points.value = responseData.points;
  } catch (error) {
    console.error('There was an error fetching the data:', error);
  }
};

let intervalId: any; 
onMounted(async () => {
  await fetchPoints();

  intervalId = setInterval(fetchPoints, 5000);
});

onUnmounted(() => {
  clearInterval(intervalId);
});
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
  iconUrl: "https://d20ttky6ra9b9o.cloudfront.net/currentlocation.png",
  visible: true,
  iconSize: {
    width: 30,
    height: 30
  },
};

// Usunięcie istniejącego markera i dodanie nowego z aktualnym położeniem
markerIds?.value && newMap.removeMarkers(markerIds?.value as string[]);
markerIds.value = await newMap.addMarkers([customMarker]);
getToots();
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
  iconUrl: "https://d20ttky6ra9b9o.cloudfront.net/currentlocation.png",
  visible: true,
  iconSize: {
    width: 30,
    height: 30
  },
};

markerIds.value = await newMap.addMarkers([customMarker]);
getToots();
};

function getIcon(value) {
    let result;

    switch (value) {
        case 1:
            result = "https://d20ttky6ra9b9o.cloudfront.net/wildanimal.png";
            break;
        case 2:
            result = "https://d20ttky6ra9b9o.cloudfront.net/animalactivity.png";
            break;
        case 4:
            result = "https://d20ttky6ra9b9o.cloudfront.net/lostanimal.png";
            break;
        case 5:
            result = "https://d20ttky6ra9b9o.cloudfront.net/carcass.png";
            break;
        default:
            result = "https://d20ttky6ra9b9o.cloudfront.net/wildanimal.png";
            break;
    }

    return result;
}
const customMarkers = [];

/**
 * Dodawanie do mapy oznaczonych wcześniej zwierząt
 */
// Określ URL endpointu API
  const getToots = async () => {
    const apiUrl = "http://35.158.22.100:5000/api/toots/findall";

    // Użyj funkcji fetch do wysłania zapytania GET do API
    fetch(apiUrl, {
        method: 'POST'})
      .then(response => {
        // Sprawdź, czy odpowiedź jest poprawna
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        // Jeśli odpowiedź jest poprawna, zwróć jej zawartość jako obiekt JSON
        return response.json();
      })
      .then(async data => {
        // Przetwarzaj dane (w tym przypadku wyświetl je w konsoli)
        console.log(data);

        customMarkers = [];
        for (let i = 0; i < data.length; i++) {
          const customMarker = {
              coordinate: { 
                  lat: data[i].location.latitude, 
                  lng: data[i].location.longtitude 
              },
              icon: customMarkerIcon,
              title: "Moja aktualna lokalizacja",
              snippet: "Opis mojej lokalizacji",
              iconUrl: getIcon(data[i].type),
              visible: true,
              iconSize: {
                  width: 20,
                  height: 20
              },
            metadata: {toot: data[i]}
          };
          customMarkers.push(customMarker);
}

        markerIds.value = await newMap.addMarkers(customMarkers);
        //HALO!!! DODAJ TUTAJ OBSŁUGĘ TYCH KLIKNIĘĆ
        newMap.setOnMarkerClickListener(async (event) => {
        console.log('Kliknięto marker:', event);
        if (event.metadata && event.metadata.toot) {
            console.log('Kliknięto marker o ID:', event.metadata.toot.id);
        }
    });
      })
      .catch(error => {
        // Obsłuż ewentualne błędy (w tym przypadku wyświetl je w konsoli)
        console.log('There was a problem with the fetch operation:', error.message);
      });
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
getToots();

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
.title{
  text-align: center;
}
.avatar-btn{
  width: 70px;
  margin-right: 10px;
}
.points-btn{
  width: 80px;
  display: flex;
}
.star{
  width: 20px;
  height: 20px;
  position: absolute;
  left: -26px;
  top: 3px;
  justify-content: flex-end;
}
.points-value{
  text-align: left;
}

</style>