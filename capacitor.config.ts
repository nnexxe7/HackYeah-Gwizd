import { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'gwizd.hackyeah',
  appName: 'gwizd-hackyeah',
  webDir: 'dist',
  "plugins": {
    "GoogleMaps": {
      "mapsApiKey": "AIzaSyDWYUDxr7VCsb-ZBSuXhatZt74hegrReKk",
      "Geolocation": {
      "photoGallery": true,
      "position": true},
    }
  },
  server: {
    androidScheme: 'https'
  }
};

export default config;
