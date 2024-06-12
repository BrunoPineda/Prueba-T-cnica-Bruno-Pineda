<template>
  <div class="bg-green-700 p-6 rounded-lg shadow-lg max-w-md mx-auto mt-10">
    <h2 class="text-yellow-400 text-2xl font-bold mb-6 text-center">Juego de la Ruleta</h2>
    <form @submit.prevent="validarFormulario" class="space-y-6">
      <div>
        <label for="nombre" class="block text-white text-lg font-semibold mb-2">Nombre:</label>
        <input type="text" v-model="nombre" required class="w-full p-3 rounded bg-white text-gray-900" placeholder="Ingresa tu nombre" />
      </div>
      <div>
        <label for="monto" class="block text-white text-lg font-semibold mb-2">Monto Inicial:</label>
        <input type="number" v-model="monto" required class="w-full p-3 rounded bg-white text-gray-900" placeholder="Ingresa el monto inicial" />
      </div>
      <div>
        <label for="tipoApuesta" class="block text-white text-lg font-semibold mb-2">Tipo de Apuesta:</label>
        <select v-model="tipoApuesta" class="w-full p-3 rounded bg-white text-gray-900">
          <option value="color">Color</option>
          <option value="pares">Pares</option>
          <option value="impares">Impares</option>
          <option value="numero">Número y Color</option>
        </select>
      </div>
      <div v-if="tipoApuesta === 'numero'">
        <label for="numero" class="block text-white text-lg font-semibold mb-2">Número (1-36):</label>
        <input type="number" v-model="numero" min="1" max="36" class="w-full p-3 rounded bg-white text-gray-900" placeholder="Ingresa un número" />
      </div>
      <div>
        <label for="color" class="block text-white text-lg font-semibold mb-2">Color:</label>
        <select v-model="color" class="w-full p-3 rounded bg-white text-gray-900">
          <option value="">Seleccione un color</option>
          <option value="rojo">Rojo</option>
          <option value="negro">Negro</option>
        </select>
      </div>
      <button type="submit" class="w-full py-3 bg-yellow-500 hover:bg-yellow-600 rounded text-white text-lg font-semibold">Iniciar Juego</button>
    </form>
    <button @click="mostrarSaldo" class="w-full mt-4 py-3 bg-yellow-500 hover:bg-yellow-600 rounded text-white text-lg font-semibold">Mostrar Saldo</button>
  </div>
</template>

<script>
import Swal from 'sweetalert2';
import axios from 'axios';

export default {
  data() {
    return {
      nombre: '',
      monto: 0,
      tipoApuesta: 'color',
      numero: null,
      color: '',
      combinacionesValidas: [
        { numero: 1, color: 'rojo' }, { numero: 3, color: 'rojo' }, { numero: 5, color: 'rojo' },
        { numero: 7, color: 'rojo' }, { numero: 9, color: 'rojo' }, { numero: 12, color: 'rojo' },
        { numero: 14, color: 'rojo' }, { numero: 16, color: 'rojo' }, { numero: 18, color: 'rojo' },
        { numero: 19, color: 'rojo' }, { numero: 21, color: 'rojo' }, { numero: 23, color: 'rojo' },
        { numero: 25, color: 'rojo' }, { numero: 27, color: 'rojo' }, { numero: 30, color: 'rojo' },
        { numero: 32, color: 'rojo' }, { numero: 34, color: 'rojo' }, { numero: 36, color: 'rojo' },
        { numero: 2, color: 'negro' }, { numero: 4, color: 'negro' }, { numero: 6, color: 'negro' },
        { numero: 8, color: 'negro' }, { numero: 10, color: 'negro' }, { numero: 11, color: 'negro' },
        { numero: 13, color: 'negro' }, { numero: 15, color: 'negro' }, { numero: 17, color: 'negro' },
        { numero: 20, color: 'negro' }, { numero: 22, color: 'negro' }, { numero: 24, color: 'negro' },
        { numero: 26, color: 'negro' }, { numero: 28, color: 'negro' }, { numero: 29, color: 'negro' },
        { numero: 31, color: 'negro' }, { numero: 33, color: 'negro' }, { numero: 35, color: 'negro' }
      ]
    };
  },
  methods: {
    validarFormulario() {
      if (!this.nombre || this.monto <= 0 || !this.color || (this.tipoApuesta === 'numero' && (!this.numero || this.numero < 1 || this.numero > 36))) {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Por favor, complete todos los campos correctamente.',
        });
      } else if (this.tipoApuesta === 'numero' && !this.combinacionesValidas.some(combinacion => combinacion.numero === this.numero && combinacion.color === this.color)) {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'La combinación de número y color no es válida.',
        });
      } else {
        this.iniciarJuego();
      }
    },
    iniciarJuego() {
      this.$emit('iniciarJuego', {
        nombre: this.nombre,
        monto: this.monto,
        tipoApuesta: this.tipoApuesta,
        numero: this.numero,
        color: this.color
      });
    },
    cargarSaldo() {
      this.$emit('cargarSaldo', this.nombre);
    },
    async mostrarSaldo() {
      const { value: nombre } = await Swal.fire({
        title: 'Visualiza tu saldo',
        input: 'text',
        inputLabel: 'Nombre',
        inputPlaceholder: 'Ingresa tu nombre',
        showCancelButton: true,
        inputValidator: (value) => {
          if (!value) {
            return 'Necesitas escribir un nombre!';
          }
        }
      });
      
      if (nombre) {
        try {
          const response = await axios.get(`https://localhost:7138/api/Usuario/${nombre}`);
          Swal.fire('Saldo', `Saldo de ${nombre}: ${response.data}`, 'info');
        } catch (error) {
          Swal.fire('Error', 'No se pudo cargar el saldo', 'error');
        }
      }
    }
  }
};
</script>

<style scoped>
form {
  margin: 20px 0;
}
button {
  margin-top: 10px;
}
</style>
