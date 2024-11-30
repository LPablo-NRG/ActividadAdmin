using JuegoTutorial.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace JuegoTutorial
{
    public class ControlMovimiento
    {
        public Jugador jugador {  get; set; }
        private LogicaCarta logicaCarta;
        private readonly Canvas mapa; 
        private double anchoMapa;
        private double altoMapa;
        private const int VELOCIDAD_MAXIMA_HORIZONTAL = 8; //suele ser 10
        private const int VELOCIDAD_MAXIMA_VERTICAL = 8; // suele ser 7
        private bool teclaArribaPresionada, teclaAbajoPresionada, teclaIzquierdaPresionada, teclaDerechaPresionada;
        private float VelocidadX = 0, VelocidadY = 0, Friction = 0.5f, Velocidad = 1;


        public ControlMovimiento(Jugador jugador, Canvas mapa, LogicaCarta logicaCarta)
        {
            this.jugador = jugador;
            this.mapa = mapa;
            this.logicaCarta = logicaCarta;
            //logicaCarta = new ControlLogicaCarta(ventana.controlCartas.cartasGeneradas, ventana.GameScreen, jugador.interfaz ,ventana.reproductor);
            

            mapa.Loaded += (sender, e) => {
                anchoMapa = mapa.ActualWidth;
                altoMapa = mapa.ActualHeight;
            };

        }


        public void AlPresionarTecla(Key tecla)
        {
            switch(tecla)
            {
                case Key.W:
                    teclaArribaPresionada = true;
                    break;

                case Key.A:
                    teclaIzquierdaPresionada = true;
                    break;

                case Key.S:
                    teclaAbajoPresionada = true;
                    break;

                case Key.D:
                    teclaDerechaPresionada = true;
                    break;
            }
        }

        public void AlSoltarTecla(Key tecla)
        {
            switch (tecla)
            {
                case Key.W:
                    teclaArribaPresionada = false;
                    break;

                case Key.A:
                    teclaIzquierdaPresionada = false;
                    break;

                case Key.S:
                    teclaAbajoPresionada = false;
                    break;

                case Key.D:
                    teclaDerechaPresionada = false;
                    break;
            }
        }

        private void moverArriba ()
        {
            if ((ObtenerPosicionY() <= 0) && (VelocidadY > 0))
            {
                VelocidadY = 0;
                return;
            }
            else if ((ObtenerPosicionY() > 0) && (VelocidadY < VELOCIDAD_MAXIMA_VERTICAL))
            {
                VelocidadY += Velocidad;
            }
            Canvas.SetTop(jugador.personaje, Canvas.GetTop(jugador.personaje) - VelocidadY);
        }

        private void moverAbajo ()
        {
            if ((ObtenerPosicionY() >= (altoMapa - jugador.personaje.ColisionPersonaje.ActualHeight)) && (VelocidadY < 0))
            {
                VelocidadY = 0;
                return;
            }
            else if ((ObtenerPosicionY() < (altoMapa - jugador.personaje.ColisionPersonaje.ActualHeight)) && (VelocidadY > -VELOCIDAD_MAXIMA_VERTICAL))
            {
                VelocidadY -= Velocidad;
            }
            Canvas.SetTop(jugador.personaje, Canvas.GetTop(jugador.personaje) - VelocidadY);
        }

        private void moverIzquierda()
        {
            if ((ObtenerPosicionX() <= 0) && (VelocidadX < 0))
            {
                VelocidadX = 0;
            }
            else if ((ObtenerPosicionX() > 0) && (VelocidadX > -VELOCIDAD_MAXIMA_HORIZONTAL))
            {
                VelocidadX -= Velocidad;
            }
            Canvas.SetLeft(jugador.personaje, Canvas.GetLeft(jugador.personaje) + VelocidadX);
            mirarIzquierda();
        }

        private void moverDerecha()
        {
            if ((ObtenerPosicionX() >= (anchoMapa - jugador.personaje.ColisionPersonaje.ActualWidth)) && (VelocidadX > 0))
            {
                VelocidadX = 0;
            }
            else if ((ObtenerPosicionX() < (anchoMapa - jugador.personaje.ColisionPersonaje.ActualWidth)) && (VelocidadX < VELOCIDAD_MAXIMA_HORIZONTAL))
            {
                VelocidadX += Velocidad;
            }
            Canvas.SetLeft(jugador.personaje, Canvas.GetLeft(jugador.personaje) + VelocidadX);
            mirarDerecha();
        }

        public void CambiarPosicion()
        {
            if (teclaArribaPresionada)
            {
                moverArriba();
            }
            else if (teclaAbajoPresionada)
            {
                moverAbajo();
            }
            else if (VelocidadY != 0)
            {
                if (VelocidadY > 0)
                {
                    VelocidadY -= Velocidad;
                }
                else
                {
                    VelocidadY += Velocidad;
                }
                Canvas.SetTop(jugador.personaje, Canvas.GetTop(jugador.personaje) - VelocidadY);
            }
            else
            {
                VelocidadY = 0;
                Canvas.SetTop(jugador.personaje, Canvas.GetTop(jugador.personaje) - VelocidadY);
            }

            colisionConObjeto("y");

            if (teclaIzquierdaPresionada)
            {
                moverIzquierda();
            }
            else if (teclaDerechaPresionada)
            {
                moverDerecha();
            }
            else if (VelocidadX != 0)
            {
                if (VelocidadX > 0)
                {
                    VelocidadX -= Velocidad;
                }
                else
                {
                    VelocidadX += Velocidad;
                }
                Canvas.SetLeft(jugador.personaje, Canvas.GetLeft(jugador.personaje) + VelocidadX);
            }
            else
            {
                VelocidadX = 0;
                Canvas.SetLeft(jugador.personaje, Canvas.GetLeft(jugador.personaje) + VelocidadX);
            }

            colisionConObjeto("x");

            colisionConCarta();

        }
    

        private void mirarIzquierda()
        {
            var imageBrush = jugador.personaje.Jugador.Fill as ImageBrush;
            if (imageBrush?.Transform is ScaleTransform scaleTransform)
            {
                scaleTransform.ScaleX = -1; // Voltear a la izquierda
            }
        }

        private void mirarDerecha()
        {
            var imageBrush = jugador.personaje.Jugador.Fill as ImageBrush;
            if (imageBrush?.Transform is ScaleTransform scaleTransform)
            {
                scaleTransform.ScaleX = 1; // Orientación normal
            }
        }

        private double ObtenerPosicionX()
        {
            double colisionX = Canvas.GetLeft(jugador.personaje.ColisionPersonaje);

            double gridX = Canvas.GetLeft(jugador.personaje);

            double posicionFinalX = colisionX + gridX;

            return posicionFinalX;
        }

        private double ObtenerPosicionY()
        {
            double colisionY = Canvas.GetTop(jugador.personaje.ColisionPersonaje);

            double gridY = Canvas.GetTop(jugador.personaje);

            double posicionFinalY = colisionY + gridY;

            return posicionFinalY;
        }


        private void colisionConObjeto(string eje)
        {
            foreach (Rectangle objeto in mapa.Children.OfType<Rectangle>())
            {
                if ((string)objeto.Tag == "Colision")
                {
                    Rect colisionJugador = new Rect(ObtenerPosicionX(), ObtenerPosicionY(), jugador.personaje.ColisionPersonaje.Width, jugador.personaje.ColisionPersonaje.Height);
                    Rect colisionObjeto = new Rect(Canvas.GetLeft(objeto), Canvas.GetTop(objeto), objeto.Width, objeto.Height);

                    if (colisionJugador.IntersectsWith(colisionObjeto))
                    {
                        if (eje == "x")
                        {
                            Canvas.SetLeft(jugador.personaje, Canvas.GetLeft(jugador.personaje) - VelocidadX);
                            VelocidadX = 0;
                        }
                        else
                        {
                            Canvas.SetTop(jugador.personaje, Canvas.GetTop(jugador.personaje) + VelocidadY);
                            VelocidadY = 0;
                        }

                        if (objeto.Name == "Base")
                        {
                            logicaCarta.entregarCartas();
                        }
                    }
                }
            }
        }

        private void colisionConCarta()
        {
            var cartas = mapa.Children.OfType<Carta>().ToList();
            foreach (var carta in cartas)
            {
                if ((string)carta.Tag == "Colision")
                {
                    Rect colisionJugador = new Rect(ObtenerPosicionX(), ObtenerPosicionY(), jugador.personaje.ColisionPersonaje.Width, jugador.personaje.ColisionPersonaje.Height);
                    Rect colisionObjeto = new Rect(Canvas.GetLeft(carta), Canvas.GetTop(carta), carta.Width, carta.Height);

                    if (colisionJugador.IntersectsWith(colisionObjeto))
                    {
                        logicaCarta.recogerCarta(carta);
                    }
                }
            }
        }
        /*
        private void colisionConBase()
        {
            foreach(var base in mapa.)
        }*/

    }
}
