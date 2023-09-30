<template>
    <ion-header>
            <ion-toolbar>
              <ion-title>Modal</ion-title>
              <ion-buttons slot="end">
                <ion-button @click="closeModal()">Close</ion-button>
              </ion-buttons>
            </ion-toolbar>
          </ion-header>
          <ion-content class="ion-padding">
            <p v-if="tootType=='wildAnimal'">
              Lorem ipsum dolor sit amet consectetur adipisicing elit. Magni illum quidem recusandae ducimus quos
              reprehenderit. Veniam, molestias quos, dolorum consequuntur nisi deserunt omnis id illo sit cum qui. Eaque,
              dicta.
            </p>
          </ion-content>
    </template>
<script lang="ts">
import { PropType, defineComponent } from 'vue';

export default defineComponent({
  name: 'LostAnimalForm',
  props: {
    tootType: {
        type: String as PropType<string>,
      required: true
    }
  },
  setup(props) {
    return props.tootType
  },
  methods: {
    closeModal() {
        console.log('close modal')
      this.$emit('close-modal');
    }
  }
});
</script>