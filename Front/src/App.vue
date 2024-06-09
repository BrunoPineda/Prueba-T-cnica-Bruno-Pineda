<template>
  <div>
    <apuesta-form @iniciarJuego="iniciarJuego" @cargarSaldo="cargarSaldo"></apuesta-form>
    <ruleta-juego :resultado="resultado"></ruleta-juego>
    <div v-if="resultado" class="mt-6 text-center">
      <p class="text-xl font-bold">Resultado: Número {{ resultado.numeroResultado }}, Color {{ resultado.colorResultado }}</p>
      <p class="text-xl font-bold">Premio: {{ resultado.premio }}</p>
      <p class="text-xl font-bold">Saldo Final: {{ resultado.saldoFinal }}</p>
    </div>
  </div>
</template>

<script>
import ApuestaForm from './components/ApuestaForm.vue';
import RuletaJuego from './components/RuletaJuego.vue';
import axios from 'axios';
import Swal from 'sweetalert2';

export default {
  components: {
    ApuestaForm,
    RuletaJuego
  },
  data() {
    return {
      resultado: null
    };
  },
  methods: {
    async iniciarJuego(apuesta) {
      try {
        const response = await axios.post('https://localhost:7138/api/Usuario/apostar', apuesta);
        this.resultado = response.data;
      } catch (error) {
        Swal.fire('Error', error.response.data, 'error');
      }
    },
    async cargarSaldo(nombre) {
      try {
        const response = await axios.get(`https://localhost:7138/api/Usuario/${nombre}`);
        Swal.fire('Saldo', `Saldo de ${nombre}: ${response.data}`, 'info');
      } catch (error) {
        Swal.fire('Error', 'No se pudo cargar el saldo', 'error');
      }
    }
  }
};
</script>
