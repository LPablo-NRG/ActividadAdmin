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

namespace JuegoTutorial.UserControls
{
    /// <summary>
    /// Lógica de interacción para Personaje.xaml
    /// </summary>
    public partial class Personaje : UserControl
    {
        public Personaje()
        {
            InitializeComponent();
        }

        public Personaje (String aspecto)
        {
            InitializeComponent();

            string rutaImagen;
            switch (aspecto)
            {
                case "Alexis":
                    rutaImagen = "pack://application:,,,/JuegoTutorial;component/Recursos/Alexis.png";
                    break;
                case "Jarly":
                    rutaImagen = "pack://application:,,,/JuegoTutorial;component/Recursos/Jarly.png";
                    break;
                case "Juan":
                    rutaImagen = "pack://application:,,,/JuegoTutorial;component/Recursos/Juan.png";
                    break;
                case "Max":
                    rutaImagen = "pack://application:,,,/JuegoTutorial;component/Recursos/Max.png";
                    break;
                case "Pelon":
                    rutaImagen = "pack://application:,,,/JuegoTutorial;component/Recursos/Pelon.png";
                    break;
                default:
                    rutaImagen = null;
                    break;
            }
            if (rutaImagen != null)
            {
                BitmapImage imagenCarta = new BitmapImage(new Uri(rutaImagen));
                Aspecto.ImageSource = imagenCarta;
            }
        }
    }
}
