<template>
    <ion-fab vertical="bottom" horizontal="end">
        <ion-fab-button>
          <ion-icon :icon="chevronUpCircle"></ion-icon>
        </ion-fab-button>
        <ion-fab-list side="top">
          <ion-fab-button class="lostAnimal" @click="setTootType('lostAnimal')">
            <ion-icon :icon="document">lost</ion-icon>
          </ion-fab-button>
          <ion-fab-button class="wildAnimal" @click="setTootType('wildAnimal')">
            <ion-icon :icon="document">lost</ion-icon>
          </ion-fab-button>
          <ion-fab-button class="animalActivity" @click="setTootType('animalActivity')">
            <ion-icon :icon="document">lost</ion-icon>
          </ion-fab-button>
          <ion-fab-button class="Carcass" @click="setTootType('Carcass')">
            <ion-icon :icon="document">lost</ion-icon>
          </ion-fab-button>
          
        </ion-fab-list>
    </ion-fab>
    <ion-modal :is-open="isOpen">
      <modal-form :toot-type="tootType" @close-modal="isOpen=false" />
    </ion-modal>
  
</template>
<script lang="ts" setup>
 import { IonButtons, IonButton, IonModal, IonHeader, IonToolbar, IonContent, IonTitle } from '@ionic/vue';
 import { ref } from 'vue';
import ModalForm from './ModalForm.vue';


const isOpen = ref(false);
const tootType=ref('')

const setTootType = (type: string) => {
  
  tootType.value=type;
  isOpen.value = true;
};
  
</script>
  
  <style>
  .ion-fab-button {
  z-index: 99999;}
  </style>
  
  