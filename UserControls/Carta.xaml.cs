using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WpfAnimatedGif;

namespace JuegoTutorial.UserControls
{
    /// <summary>
    /// Lógica de interacción para Carta.xaml
    /// </summary>
    public partial class Carta : UserControl
    {
        public string tipo { set; get;}
        
        public Carta()
        {
            InitializeComponent();
            establecerCartaFantasma();
            
        }

        public Carta(string tipo)
        {
            InitializeComponent();
            this.tipo = tipo;

            establecerTipo("Basico");
        }

        public void establecerTipo(string tipo)
        {
            ImageBehavior.SetAnimatedSource(carta, null);
            string rutaImagen;
            switch (tipo)
            {
                case "Verde":
                    rutaImagen = "pack://application:,,,/JuegoTutorial;component/Recursos/CartaVerde.png";
                    break;
                case "Rojo":
                    rutaImagen = "pack://application:,,,/JuegoTutorial;component/Recursos/CartaRoja.png";
                    break;
                case "Azul":
                    rutaImagen = "pack://application:,,,/JuegoTutorial;component/Recursos/CartaAzul.png";
                    break;
                case "Guerra":
                    configurarGif("pack://application:,,,/JuegoTutorial;component/Recursos/CartaGuerra.gif");
                    return;
                case "Basico":
                    rutaImagen = "pack://application:,,,/JuegoTutorial;component/Recursos/CartaBasica.png";
                    break;
                case "Misterio":
                    configurarGif("pack://application:,,,/JuegoTutorial;component/Recursos/CartaMisterio.gif");
                    return;
                default:
                    rutaImagen = null;
                    break;
            }
            if (rutaImagen != null)
            {
                BitmapImage imagenCarta = new BitmapImage(new Uri(rutaImagen));
                carta.Source = imagenCarta;
            }
        }
        
        private void configurarGif(String rutaGif)
        {
            var gif = new BitmapImage();
            gif.BeginInit();
            gif.UriSource = new Uri(rutaGif, UriKind.RelativeOrAbsolute);
            gif.EndInit();
            ImageBehavior.SetAnimatedSource(carta, gif);
        }

        public void establecerCartaFantasma()
        {
            tipo = "Basico";
            ImageBehavior.SetAnimatedSource(carta, null);
            string rutaImagen = "pack://application:,,,/JuegoTutorial;component/Recursos/CartaBasica.png";
            BitmapImage imagenCarta = new BitmapImage(new Uri(rutaImagen));
            carta.Source = imagenCarta;

            carta.Width = 90;
            carta.Height = 120;
            Opacity = 0.5;

        }

    }
}
