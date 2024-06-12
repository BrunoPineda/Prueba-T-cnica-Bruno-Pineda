<template>
  <div id="app">
    <div class="container mx-auto p-4">
      <ApuestaForm @iniciarJuego="iniciarJuego" />
      <button @click="abrirModal" class="w-full mt-4 py-3 bg-yellow-500 hover:bg-yellow-600 rounded text-white text-lg font-semibold">Cargar Saldo</button>
      <RuletaJuego :resultado="resultado" />
    </div>
    <!-- Modal -->
    <div v-if="mostrarModal" class="fixed z-10 inset-0 overflow-y-auto">
      <div class="flex items-center justify-center min-h-screen">
        <div class="fixed inset-0 transition-opacity" aria-hidden="true">
          <div class="absolute inset-0 bg-gray-500 opacity-75"></div>
        </div>
        <div class="bg-white rounded-lg overflow-hidden shadow-xl transform transition-all sm:max-w-lg sm:w-full">
          <div class="bg-white p-6">
            <h3 class="text-lg leading-6 font-medium text-gray-900 mb-4">Cargar Saldo</h3>
            <form @submit.prevent="cargarSaldo">
              <div class="mb-4">
                <label for="nombre" class="block text-gray-700">Nombre:</label>
                <input type="text" v-model="nombreModal" required class="w-full p-2 border rounded" />
              </div>
              <div class="mb-4">
                <label for="monto" class="block text-gray-700">Monto:</label>
                <input type="number" v-model="montoModal" required class="w-full p-2 border rounded" />
              </div>
              <div class="flex justify-end">
                <button type="button" @click="cerrarModal" class="mr-2 py-2 px-4 bg-gray-500 hover:bg-gray-600 rounded text-white">Cancelar</button>
                <button type="submit" class="py-2 px-4 bg-yellow-500 hover:bg-yellow-600 rounded text-white">Cargar</button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import Swal from 'sweetalert2';
import ApuestaForm from './components/ApuestaForm.vue';
import RuletaJuego from './components/RuletaJuego.vue';

export default {
  name: 'App',
  components: {
    ApuestaForm,
    RuletaJuego
  },
  data() {
    return {
      resultado: null,
      mostrarModal: false,
      nombreModal: '',
      montoModal: ''
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
    async cargarSaldo() {
      try {
        const response = await axios.post('https://localhost:7138/api/Usuario', {
          nombre: this.nombreModal,
          monto: this.montoModal
        });
        Swal.fire('Saldo', `Saldo de ${this.nombreModal}: ${response.data.monto}`, 'info');
        this.cerrarModal();
      } catch (error) {
        Swal.fire('Error', 'No se pudo cargar el saldo', 'error');
      }
    },
    async mostrarSaldo(nombre) {
      try {
        const response = await axios.get(`https://localhost:7138/api/Usuario/${nombre}`);
        Swal.fire('Saldo', `Saldo de ${nombre}: ${response.data}`, 'info');
      } catch (error) {
        Swal.fire('Error', 'No se pudo cargar el saldo', 'error');
      }
    },
    abrirModal() {
      this.mostrarModal = true;
    },
    cerrarModal() {
      this.mostrarModal = false;
      this.nombreModal = '';
      this.montoModal = '';
    }
  }
};
</script>

<style>
@import 'tailwindcss/tailwind.css';

.container {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
}
</style>

