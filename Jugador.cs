using JuegoTutorial.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoTutorial
{
    public class Jugador
    {
        public Personaje personaje { get; set; }
        private InterfazJugador interfaz;
        private MainWindow ventanaJuego;
        public ControlMovimiento controlMovimiento {  get; set; }
        public LogicaCarta logicaCarta { get; set; }
        private Puntuacion puntuacion = new Puntuacion(0);

        public class Puntuacion
        {
            public int puntaje { get; set; }
            public Puntuacion(int puntaje)
            {
                this.puntaje = puntaje;
            }
        }

        public Jugador(Personaje personaje, InterfazJugador interfaz, MainWindow ventanaJuego)
        {
            this.personaje = personaje;
            this.interfaz = interfaz;
            this.ventanaJuego = ventanaJuego;

            
            logicaCarta = new LogicaCarta(ventanaJuego.controlCartas.cartasGeneradas,ventanaJuego.GameScreen, interfaz, ventanaJuego.reproductor, puntuacion);
            controlMovimiento = new ControlMovimiento(this, ventanaJuego.GameScreen, logicaCarta);

        }

        public void moverse()
        {
            controlMovimiento.CambiarPosicion();
        }
    }
}
