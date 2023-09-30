<template>
    <ion-fab vertical="bottom" horizontal="end">
        <ion-fab-button>
          Add
        </ion-fab-button>
        <ion-fab-list side="top">
          <ion-fab-button class="lostAnimal custom-fab" @click="setTootType('lostAnimal')">
            <ion-label>Lost Animal</ion-label>
            <ion-avatar class="avatar">
              <img alt="lost animal" src="../assets/lostanimal.png" />
            </ion-avatar>
          </ion-fab-button>

          <ion-fab-button class="Carcass custom-fab" @click="setTootType('Carcass')">
            <ion-label>Carcass</ion-label>
            <ion-avatar class="avatar">
              <img alt="Up" src="../assets/carcass.png" />
            </ion-avatar>
          </ion-fab-button>
          
          <ion-fab-button class="animalActivity custom-fab" @click="setTootType('animalActivity')">
            <ion-label>Animal Activity</ion-label>
            <ion-avatar class="avatar">
              <img alt="Up" src="../assets/animalactivity.png" />
            </ion-avatar>
          </ion-fab-button>

          <ion-fab-button class="wildAnimal custom-fab" @click="setTootType('wildAnimal')">
            <ion-label>Wild Animal</ion-label>
            <ion-avatar class="avatar">
              <img alt="Up" src="../assets/wildanimal.png" />
            </ion-avatar>
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
  
<<<<<<< HEAD
  <style>
  </style>
  
  
=======
<style>

  .custom-fab{
  margin-left: -130px !important;
  height: 40px;
  width: 170px;
  margin-top: 5px;
  --border-radius: 20px !important;

}

.custom-fab > *:not(:last-child) {
  margin-right: 10px;
}
.avatar{
  position:absolute;
  right: 9px;
  height: 32px;
  width: 32px;
}

ion-label{
  position:absolute;
  right:40px;
}

</style>
>>>>>>> 6e78ff6a9861771d3a27c3cd7256fd9b477e19a0
