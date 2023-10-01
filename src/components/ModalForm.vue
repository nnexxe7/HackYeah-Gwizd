<template>
  <ion-header>
          <ion-toolbar>
            <ion-title>{{formTitle}}</ion-title>
            <ion-buttons slot="end">
              <ion-button @click="closeModal()">Close</ion-button>
            </ion-buttons>
          </ion-toolbar>
        </ion-header>
        <ion-content class="ion-padding">
          <form @submit.prevent="toot" ref="form">
              <ion-list class="inputs-list">
                <ion-item>
                    <ion-label>Upload Photo</ion-label>
                    <ion-button @click="captureImage">Capture Image</ion-button>
                </ion-item>
                <div v-if="photoUrl" class="photo">
                    <img :src="photoUrl" alt="Captured Image" class="photo-img" />
                </div>
                  <ion-item>
                      <ion-select class="input-select" name="relatedAnimal" label="Animal type" fill="outline" label-placement="floating">
                          <ion-select-option value=1>Cat</ion-select-option>
                          <ion-select-option value=2>Dog</ion-select-option>
                          <ion-select-option value=3>Fox</ion-select-option>
                          <ion-select-option value=4>Hedgehog</ion-select-option>
                          <ion-select-option value=5>Boar</ion-select-option>
                          <ion-select-option value=6>Deer</ion-select-option>
                          <ion-select-option value=7>Snake</ion-select-option>
                      </ion-select>
                  </ion-item>
                  <ion-item v-if="tootType=='animalActivity'">
                      <ion-select class="input-select" label="Activity type" fill="outline" label-placement="floating">
                          <ion-select-option value=1>Trace</ion-select-option>
                          <ion-select-option value=2>Damage</ion-select-option>
                          <ion-select-option value=3>Nest</ion-select-option>
                          <ion-select-option value=4>Other</ion-select-option>
                      </ion-select>
                  </ion-item>
                  <ion-item class="description">
                      <ion-input label="Description" name="description" labelPlacement="floating" fill="outline"></ion-input>
                  </ion-item>
                  <ion-checkbox class="isDangerous" name="isDangerous" value="true" >Is dangerous?</ion-checkbox>
                  
              </ion-list>
              <ion-button type="submit" class="submit-btn">Toot!</ion-button>
          </form>
        </ion-content>
  </template>
<script lang="ts">
import { PropType, defineComponent, ref, computed, onUnmounted } from 'vue';
import { Global } from './../global'
import { Camera, CameraResultType } from '@capacitor/camera';

export default defineComponent({
name: 'LostAnimalForm',
props: {
  tootType: {
      type: String as PropType<string>,
    required: true
  }
},
setup(props,{emit}) {
  const form = ref(null);
  const photoUrl = ref('');  // do przechowywania URL zdjęcia

const captureImage = async () => {
  try {
    const image = await Camera.getPhoto({
      quality: 90,
      allowEditing: false,
      resultType: CameraResultType.Uri
    });
    
    photoUrl.value = image.webPath;
  } catch (error) {
    console.error("Error capturing image:", error);
  }
};
  const typeMapping = {
    wildAnimal: 1,
    animalActivity: 2,
    humanActivity: 3,
    lostAnimal: 4,
    Carcass: 5
  };

  const tootTypeNumeric = computed(() => typeMapping[props.tootType]);

  const formTitle = computed(() => {
      switch(props.tootType) {
        case 'lostAnimal': return 'Lost Animal';
        case 'wildAnimal': return 'Wild Animal';
        case 'animalActivity': return 'Animal Activity';
        case 'Carcass': return 'Carcass';
        default: return '';
      }
  });
  
  const toot = async () => {
    try {
      const formData = new FormData(form.value);
      console.log(formData.get('isDangerous'))
      // Create data object from formData
      const data = {
        type: tootTypeNumeric.value,
        relatedAnimal: Number(formData.get('relatedAnimal')),
        description: formData.get('description'),
        isDangerous: formData.get('isDangerous') == 'true',
        location: {
      "latitude": Global.MarketLat, 
      "longtitude": Global.MarketLng
  },
        submittedBy: 'testuser'
      };

      const response = await fetch('http://35.158.22.100:5000/api/toots/submit', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
      });

      if (!response.ok) {
        throw new Error('Network response was not ok');
      }

      const responseData = await response.json();
      console.log(responseData);
      closeModal();

    } catch (error) {
      console.error('There was an error sending the data:', error);
    }
  };

  const closeModal = () => {
    emit('close-modal');
  };

  return {
      form, // Ensure you're returning the ref
      formTitle,
      closeModal,
      toot,
      captureImage,
  photoUrl
  }
} 
});
</script>
<style lang="css">
  .inputs-list{
      padding-top: 20px;
  }
  .input-select{
      padding-top: 20px;
  }
  .input-fill-outline{
      margin-top: 20px;
      padding: 20px;
  }
  .isDangerous{
      padding-top: 20px;
      padding-left: 18px;
  }
  .description .label-text-wrapper{
      padding-left: 15px;
  }
  .submit-btn{
      display: flex;
      align-items: center;
      margin-top: 30px;
  }
  .photo{
    display: flex;
    justify-content: center;
  }
 
      
</style>