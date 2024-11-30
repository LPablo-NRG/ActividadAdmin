using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JuegoTutorial.UserControls
{
    /// <summary>
    /// Lógica de interacción para InterfazJugador.xaml
    /// </summary>
    public partial class InterfazJugador : UserControl
    {
        public List<Carta> listaCartas { get; set; }
        private int anteriorCartaSeleccionada = 0;
        private int cartaSeleccionada = 0;

        public InterfazJugador()
        {
            InitializeComponent();
            listaCartas = new List<Carta> { };
            listaCartas.Add(Carta1);
            listaCartas.Add(Carta2);
            listaCartas.Add(Carta3);
            listaCartas.Add(Carta4);
            listaCartas.Add(Carta5);
            seleccionarCarta(0);
        }

        public void ActualizarTamanio(double anchoPantalla, double altoPantalla)
        {
            Console.WriteLine("ancho actual:" + Height);
            Console.WriteLine("ancho nuevo:" + altoPantalla);

            Width = anchoPantalla;
            Height = altoPantalla;
            Pantalla.Height = Height;
            Pantalla.Width = Width;
            rectanguloCompleto.Rect = new Rect(0, 0, anchoPantalla, altoPantalla);
            rectanguloInterior.Rect = new Rect(20, 20, anchoPantalla - 40, altoPantalla - 40);

        }

        public void iniciarTiempoGuerra()
        {
            Storyboard animacion = (Storyboard)this.Resources["inicioTiempoGuerra"]; 
            animacion.Begin();

            
        }
        public void detenerTiempoGuerra()
        {
            Storyboard animacion = (Storyboard)this.Resources["inicioTiempoGuerra"];
            animacion.Stop();
            tbcTiempoGuerra.Visibility = Visibility.Hidden;
        }

        public void seleccionarCarta(int indiceCarta) 
        { 
            if (indiceCarta == 5)
            {
                indiceCarta = 0;
            }
            anteriorCartaSeleccionada = cartaSeleccionada;
            cartaSeleccionada = indiceCarta;

            deseleccionarCarta(anteriorCartaSeleccionada);
            Storyboard animacion = (Storyboard)FindResource("objetoSeleccionado");
            animacion.Begin(listaCartas[indiceCarta]);
            

        }

        public void deseleccionarCarta(int indiceCarta)
        {
            if (indiceCarta == -1)
            {
                indiceCarta = 4;
            }
            Storyboard animacion = (Storyboard)FindResource("objetoDeseleccionado");
            animacion.Begin(listaCartas[indiceCarta]);
        }
    }
}
