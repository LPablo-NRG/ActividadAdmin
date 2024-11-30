using JuegoTutorial.UserControls;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace JuegoTutorial
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ControlMovimiento controlMovimiento;
        public ControlCartas controlCartas {  get; set; }
        private LogicaCarta controlLogicaCarta;

        private const int TIEMPO_DE_INTERVALO_EN_MILISEGUNDOS = 16;
        private DispatcherTimer GameTimer = new DispatcherTimer();

        public MediaPlayer reproductor {  get; set; }
        private Jugador jugador;

        public MainWindow()
        {
            InitializeComponent();
            this.SizeChanged += cambiarTamanio; //Candidato a ser eliminado 

            reproductor = new MediaPlayer();
            reproductor.Volume = 1;
            reproductor.MediaFailed += (s, e) =>
            {
                Console.WriteLine("Error al reproducir el audio: " + e.ErrorException.Message);
            };

            controlCartas = new ControlCartas(GameScreen, AreaCartas);

            configurarJugador();

            GameScreen.Focus();
            GameTimer.Interval = TimeSpan.FromMilliseconds(TIEMPO_DE_INTERVALO_EN_MILISEGUNDOS);
            GameTimer.Tick += tick;
            GameTimer.Start();

            
            SizeToContent = SizeToContent.WidthAndHeight; 
            
        }

        private void configurarJugador()
        {
            //Añadiendo al jugador de manera manual
            Personaje personaje = new Personaje("Pelon");
            personaje.Name = "jugador";
            Canvas.SetLeft(personaje, 300);
            Canvas.SetTop(personaje, 300);
            GameScreen.Children.Add(personaje);

            InterfazJugador interfazJugador = new InterfazJugador();
            interfazJugador.Name = "interfazJugador";
            Canvas.SetLeft(interfazJugador, 0);
            Canvas.SetTop(interfazJugador, 0);
            GameScreen.Children.Add(interfazJugador);


            jugador = new Jugador(personaje, interfazJugador, this);
            
        }

        private void tick(Object sender, EventArgs e)
        {
            jugador.moverse();
        }

        private void presionarTecla(object sender, KeyEventArgs e)
        {
            jugador.controlMovimiento.AlPresionarTecla(e.Key);
        }

        private void soltarTecla(object sender, KeyEventArgs e)
        {
            jugador.controlMovimiento.AlSoltarTecla(e.Key);
        }

        private void Button_Click_Alexis(object sender, RoutedEventArgs e)
        {
            CambiarImagen("Alexis");
        }

        private void Button_Click_Jarly(object sender, RoutedEventArgs e)
        {
            CambiarImagen("Jarly");
        }

        private void Button_Click_Pelon(object sender, RoutedEventArgs e)
        {
            CambiarImagen("Pelon");
            
        }

        private void CambiarImagen(String skinNombre)
        {
            var nuevaImagen = new BitmapImage();
            nuevaImagen.BeginInit();
            nuevaImagen.UriSource = new Uri("pack://application:,,,/Recursos/" + skinNombre + ".png");
            nuevaImagen.EndInit();
            jugador.personaje.Aspecto.ImageSource = nuevaImagen;


        }

        private void Button_Click_GeneradorCartas(object sender, RoutedEventArgs e)
        {
            if (!controlCartas.GeneradorCartasIniciado)
            {
                controlCartas.IniciarGeneradorCartas();
                BotonGeneradorCartas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF90EE90"));
            } else
            {
                controlCartas.PararGeneradorCartas();
                BotonGeneradorCartas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF08080"));
            }
        }

        private void cambiarTamanio(object sender, SizeChangedEventArgs e)
        {
            //desactivado ya que no creo que sea justo cambiar la 
            //InterfazJugador.ActualizarTamanio(ActualWidth-16, ActualHeight-39);
            
        }

        

    }
}
