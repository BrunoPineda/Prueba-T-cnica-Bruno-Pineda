<template>
  <div class="vista-home text-center p-6 bg-gray-800 rounded-lg shadow-lg max-w-lg mx-auto mt-10">
    <ApuestaForm @iniciarJuego="iniciarJuego" @cargarSaldo="cargarSaldo" />
    <RuletaJuego :resultado="resultado" />
    <div v-if="mensaje" class="mensaje text-yellow-300 mt-4 text-xl font-semibold">{{ mensaje }}</div>
    <button @click="guardarPartida" v-if="resultado && !isSpinning" class="mt-4 px-4 py-3 bg-yellow-500 hover:bg-yellow-600 rounded text-white text-lg font-semibold">Guardar Partida</button>
  </div>
</template>

<script>
import ApuestaForm from '../components/ApuestaForm.vue';
import RuletaJuego from '../components/RuletaJuego.vue';
import axios from 'axios';

export default {
  name: 'VistaHome',
  components: {
    ApuestaForm,
    RuletaJuego
  },
  data() {
    return {
      resultado: null, // Inicializar como null
      mensaje: '',
      nombre: '',
      monto: 0,
      isSpinning: false,
      numero: null,
      color: ''
    };
  },
  methods: {
    async iniciarJuego({ nombre, monto, numero, color }) {
      this.nombre = nombre;
      this.monto = monto;
      this.numero = numero;
      this.color = color;
      try {
        await axios.post('https://localhost:7138/api/Usuario', {
          nombre,
          monto
        });
        const response = await axios.get('https://localhost:7138/api/Random');
        this.resultado = response.data;
        this.isSpinning = true;

        setTimeout(() => {
          this.isSpinning = false;
          
          // Lógica para determinar si el jugador gana o pierde
          let respuesta;
          let premio = 0;

          if (this.resultado.number === this.numero && this.resultado.color.toLowerCase() === this.color.toLowerCase()) {
            respuesta = '¡Has ganado el triple de tu apuesta!';
            premio = monto * 3;
          } else if (this.resultado.number % 2 === this.numero % 2 && this.resultado.color.toLowerCase() === this.color.toLowerCase()) {
            respuesta = '¡Has ganado el monto de tu apuesta!';
            premio = monto;
          } else if (this.resultado.color.toLowerCase() === this.color.toLowerCase()) {
            respuesta = '¡Has ganado la mitad de tu apuesta!';
            premio = monto / 2;
          } else {
            respuesta = 'Has perdido.';
            premio = -monto;
          }

          this.monto += premio;
          this.mensaje = `Resultado: Número ${this.resultado.number}, Color ${this.resultado.color}. ${respuesta}`;
        }, 4000); // La misma duración que la animación CSS
      } catch (error) {
        console.error('Error al iniciar el juego:', error);
        this.mensaje = 'Error al iniciar el juego';
      }
    },
    async cargarSaldo(nombre) {
      try {
        const response = await axios.get(`https://localhost:7138/api/Usuario/${nombre}`);
        this.monto = response.data;
        this.mensaje = `Saldo cargado: ${this.monto}`;
        this.resultado = null; // Asegúrate de que resultado esté en null
      } catch (error) {
        console.error(error);
        this.mensaje = 'Error al cargar el saldo';
      }
    },
    async guardarPartida() {
      try {
        await axios.post('https://localhost:7138/api/Usuario', {
          nombre: this.nombre,
          monto: this.monto
        });
        this.mensaje = 'Partida guardada correctamente';
      } catch (error) {
        console.error(error);
        this.mensaje = 'Error al guardar la partida';
      }
    }
  }
};
</script>

<style scoped>
.vista-home {
  text-align: center;
}
.mensaje {
  color: red;
}
</style>
