<template>
    <ion-fab vertical="bottom" horizontal="end">
        <ion-fab-button>
          <ion-icon :icon="chevronUpCircle"></ion-icon>
        </ion-fab-button>
        <ion-fab-list side="top">
          <ion-fab-button class="lostAnimal" @click="setOpen(true)">
            <ion-icon :icon="document">lost</ion-icon>
          </ion-fab-button>
          
        </ion-fab-list>
    </ion-fab>
    <ion-modal :is-open="isOpen">
      <lost-animal-form @close-modal="setOpen(false)" />
    </ion-modal>
  
</template>
<script lang="ts" setup>
 import { IonButtons, IonButton, IonModal, IonHeader, IonToolbar, IonContent, IonTitle } from '@ionic/vue';
 import { ref } from 'vue';
import LostAnimalForm from './LostAnimalForm.vue';

const isOpen = ref(false);

const setOpen = (open: boolean) => {
  isOpen.value = open;
};
  
</script>
  
  <style>
  </style>
  
  