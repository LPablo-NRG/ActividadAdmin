using JuegoTutorial.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace JuegoTutorial
{
    public class ControlCartas
    {
        private const int TIEMPO_DE_INTERVALO_EN_SEGUNDOS = 1;
        private const int MAX_NUM_CARTAS = 10;

        public bool GeneradorCartasIniciado = false;

        public List<Carta> cartasGeneradas {  get; set; }

        private DispatcherTimer temporizadorCarta = new DispatcherTimer();

        private readonly Canvas Mapa;
        private readonly Canvas areaCartas;
        private static readonly Random random = new Random();

        public ControlCartas(Canvas Mapa, Canvas areaCartas)
        {
            this.Mapa = Mapa;
            this.areaCartas = areaCartas;
            cartasGeneradas = new List<Carta>();
            temporizadorCarta.Interval = TimeSpan.FromSeconds(TIEMPO_DE_INTERVALO_EN_SEGUNDOS);
            temporizadorCarta.Tick += generadorTick;
        }

        public void GenerarCarta()
        {
            if (cartasGeneradas.Count < MAX_NUM_CARTAS)
            {
                string tipo = tipoAleatorio();
                var carta = new Carta(tipo);
                carta.Tag = "Colision";

                double posicionX = random.Next(0, (int)areaCartas.ActualWidth - (int)carta.Width);
                double posicionY = random.Next(0, (int)areaCartas.ActualHeight - (int)carta.Height); 


                Canvas.SetLeft(carta, posicionX + ((Mapa.ActualWidth - areaCartas.ActualWidth) / 2));
                Canvas.SetTop(carta, posicionY + ((Mapa.ActualHeight - areaCartas.ActualHeight) / 2));
                Mapa.Children.Add(carta);
                cartasGeneradas.Add(carta);
            }
            
        }

        private string tipoAleatorio()
        {
            string[] tipos = { "Verde", "Azul", "Rojo", "Guerra" };
            int indice = random.Next(tipos.Length);
            return tipos[indice];
        }

        public void IniciarGeneradorCartas()
        {
            temporizadorCarta.Start();
            GeneradorCartasIniciado = true;
            Console.WriteLine("Generador de cartas iniciado...");
        }

        public void PararGeneradorCartas()
        {
            temporizadorCarta.Stop();
            GeneradorCartasIniciado = false;
            Console.WriteLine("Generador de cartas detenido...");
        }

        private void generadorTick(Object sender, EventArgs e)
        {
            GenerarCarta();
            Console.WriteLine("Tick generador...");
        }

    }
}
