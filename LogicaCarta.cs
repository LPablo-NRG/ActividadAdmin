using JuegoTutorial.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace JuegoTutorial
{
    public class LogicaCarta
    {
        private List<Carta> cartasEnPantalla;
        private List<Carta> cartasInventario = new List<Carta> { null, null, null, null, null };
        private Canvas mapa;
        private InterfazJugador interfazJugador;
        private MediaPlayer reproductor;
        private int UltimaCartaRecogida = 0;
        private Jugador.Puntuacion puntuacion;
        private bool puedeInteractuar = true;
        private int segundosTiempoGuerra = 10; //deberia ser constante?

        public LogicaCarta(List<Carta> cartasEnPantalla, Canvas mapa, InterfazJugador interfazJugador, MediaPlayer reproductor, Jugador.Puntuacion puntuacion)
        {
            this.cartasEnPantalla = cartasEnPantalla;
            this.mapa = mapa;
            this.interfazJugador = interfazJugador;
            this.reproductor = reproductor;
            this.puntuacion = puntuacion;
        }

        private void guardarCarta(Carta carta)
        {
            
            if (UltimaCartaRecogida == 0 || UltimaCartaRecogida == 5)
            {
                cartasInventario[0] = carta;
                configurarCarta(interfazJugador.listaCartas[0], carta);
                UltimaCartaRecogida = 1;
                interfazJugador.seleccionarCarta(UltimaCartaRecogida);

            } else
            {
                cartasInventario[UltimaCartaRecogida] = carta;
                configurarCarta(interfazJugador.listaCartas[UltimaCartaRecogida], carta);
                UltimaCartaRecogida = UltimaCartaRecogida + 1;
                interfazJugador.seleccionarCarta(UltimaCartaRecogida);
            }
            
        }

        public void recogerCarta(Carta carta)
        {
            if (puedeInteractuar)
            {
                puedeInteractuar = false;
                guardarCarta(carta);
                cartasEnPantalla.Remove(carta);
                ((Canvas)carta.Parent).Children.Remove(carta);
                activarTiempoDeGuerra();
            }
        }


        private async void configurarCarta(Carta cartaConfigurable, Carta cartaNueva)
        {
            reproductor.Volume = .5;
            reproductor.Open(new Uri("pack://siteoforigin:,,,/Recursos/AudioTTB1.mp3", UriKind.RelativeOrAbsolute));
            reproductor.Play();
            cartaConfigurable.establecerTipo("Misterio");
            cartaConfigurable.carta.Width = interfazJugador.Carta1.Width;
            cartaConfigurable.carta.Height = interfazJugador.Carta1.Height;
            cartaConfigurable.Opacity = 1;
            

            await Task.Delay(1000);
            cartaConfigurable.establecerTipo(cartaNueva.tipo);
        }


        public void entregarCartas()
        {
            if (cartasInventario.Count >= 3 && puedeInteractuar)
            {
                Dictionary<string, int> cartas = new Dictionary<string, int>();
                cartas.Add("Verde", 0);
                cartas.Add("Azul", 0);
                cartas.Add("Rojo", 0);

                contarCartas(cartas);

                string tipo = obtenerTipoCartasSuficientes(cartas);
                if (tipo != null)
                {
                    descartarCartas(tipo);
                    reproductor.Volume = .7;
                    reproductor.Open(new Uri("pack://siteoforigin:,,,/Recursos/EntregarCartas.wav", UriKind.RelativeOrAbsolute));
                    reproductor.Play();
                    puntuacion.puntaje++;
                    interfazJugador.tbcPuntaje.Text = puntuacion.puntaje + " pts";
                }
            }
        }

        private void contarCartas(Dictionary<string, int> cartas)
        {
            int cartasVerdes = 0, cartasAzules = 0, cartasRojas = 0;
            for (int i = 0; i < cartasInventario.Count(); i++)
            {
                if (cartasInventario[i] != null)
                {
                    switch (cartasInventario[i].tipo)
                    {
                        case "Verde":
                            cartasVerdes++;
                            break;
                        case "Azul":
                            cartasAzules++;
                            break;
                        case "Rojo":
                            cartasRojas++;
                            break;
                        default:
                            break;
                    }
                }
                
            }

            cartas["Verde"] = cartasVerdes;
            cartas["Azul"] = cartasAzules;
            cartas["Rojo"] = cartasRojas;
        }


        private string obtenerTipoCartasSuficientes(Dictionary<string, int> cartas)
        {
            foreach (string tipo in cartas.Keys)
            {
                if (cartas[tipo] >= 3)
                {
                    return tipo;
                }
            }
            return null;
        }

        private async void activarTiempoDeGuerra()
        {
            int cartasDeGuerra = 0; 
            await Task.Delay(1000);
            foreach (Carta carta in cartasInventario)
            {
                if (carta != null && carta.tipo == "Guerra")
                {
                    cartasDeGuerra++;
                }
                if (cartasDeGuerra == 3)
                {


                    interfazJugador.iniciarTiempoGuerra();
                    await Task.Delay(3000);
                    interfazJugador.tbcTiempoGuerra.Visibility = Visibility.Visible; //deberia hacer un metodo en la interfaz?
                    descartarCartas("Guerra");
                    await Task.Delay(segundosTiempoGuerra*1000);
                    interfazJugador.detenerTiempoGuerra();
                    puedeInteractuar = true;
                    return;
                }
            }

            puedeInteractuar = true;
        }

        private void descartarCartas(string tipo)
        {
            int cartasDescartadas = 0;
            for (int i = 0; i < cartasInventario.Count(); i++)
            {
                Carta carta = cartasInventario[i];

                if (carta.tipo == tipo)
                {
                    cartasInventario.Remove(carta);
                    i--;
                    cartasDescartadas++;
                }
                if (cartasDescartadas == 3)
                {
                    cartasInventario.Add(null);
                    cartasInventario.Add(null);
                    cartasInventario.Add(null);
                    actualizarInterfazCartas();
                    actualizarUltimaRecogida();
                    return;
                }
            }
        }

        private void actualizarInterfazCartas()
        {
            for (int i = 0; i < cartasInventario.Count(); i++)
            {
                if (cartasInventario[i] != null)
                {
                    interfazJugador.listaCartas[i].establecerTipo(cartasInventario[i].tipo);
                } else
                {
                    interfazJugador.listaCartas[i].establecerCartaFantasma();
                }
            }
        }

        private void actualizarUltimaRecogida()
        {
            for (int i = 0; i < cartasInventario.Count(); i++)
            {
                if (cartasInventario[i] == null)
                {
                    UltimaCartaRecogida = i;
                    interfazJugador.seleccionarCarta(UltimaCartaRecogida);
                    return;
                }
            }
            
        }
    }
}
