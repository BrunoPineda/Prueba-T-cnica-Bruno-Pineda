<template>
  <div class="ruleta-container relative mx-auto mt-10">
    <div id="ruleta" class="ruleta relative rounded-full"></div>
    <div class="puntero"></div>
    <div v-if="resultado" class="text-center mt-4">
      <p>Resultado: Número {{ resultado.numeroResultado }}, Color {{ resultado.colorResultado }}</p>
      <p>Premio: {{ resultado.premio }}</p>
      <p>Saldo Final: {{ resultado.saldo }}</p>
    </div>
  </div>
</template>
<script>
export default {
  name: 'RuletaJuego',
  props: {
    resultado: {
      type: Object,
      required: false // Hacer que el resultado no sea requerido
    }
  },
  watch: {
    resultado(newVal) {
      if (newVal) {
        this.girarRuleta(newVal.number);
      }
    }
  },
  methods: {
    girarRuleta(numero) {
      const numeroPosiciones = 37; // Números del 0 al 36
      const gradosPorPosicion = 360 / numeroPosiciones;
      const numeros = [0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18, 29, 7, 28, 12, 35, 3, 26];
      const colores = ["verde", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro", "rojo", "negro"];

      const obtenerAnguloParaNumero = (numero) => {
        const posicion = numeros.indexOf(numero);
        return gradosPorPosicion * posicion;
      };

      const angulo = obtenerAnguloParaNumero(numero);
      const ruleta = document.getElementById('ruleta');
 
      ruleta.style.transform = `rotate(${angulo + 360 * 5}deg)`; // 5 vueltas completas antes de detenerse en el ángulo correcto

 
      setTimeout(() => {
 
        const anguloFinal = (angulo + 360 * 5) % 360;
        const indiceFinal = Math.round(anguloFinal / gradosPorPosicion);
        const numeroFinal = numeros[indiceFinal];
        const colorFinal = colores[indiceFinal];
        console.log(`Número final: ${numeroFinal}, Color final: ${colorFinal}`);
      }, 4000); // La misma duración que la animación CSS
    }
  }
};
</script>

<style scoped>
.ruleta-container {
  position: relative;
  width: 300px;
  height: 300px;
  margin: 20px auto;
}

.ruleta {
  width: 100%;
  height: 100%;
  border: 5px solid black;
  border-radius: 50%;
  background-image: url('@/assets/ruleta.jpg'); /* Asegúrate de que la ruta sea correcta */
  background-size: cover;
  transition: transform 4s cubic-bezier(0.25, 0.1, 0.25, 1);
}

.puntero {
  position: absolute;
  top: 0;
  left: 50%;
  width: 0;
  height: 0;
  border-left: 10px solid transparent;
  border-right: 10px solid transparent;
  border-bottom: 20px solid red;
  transform: translateX(-50%);
}
</style>
