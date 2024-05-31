using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AhorcdoTEAM1
{
    public partial class personaje3 : ContentPage
    {

        private int num1, num2, resultadoCorrecto;
        private int intentos = 0;
        private int puntaje = 0;
        private const int maxIntentos = 6;
        private string nivelSeleccionado;
        private int nivelTiempo;
        private Random random = new Random();
        private System.Timers.Timer timer;
        private string imagenPredeterminada = "base.png";

        public personaje3(string nivel)
        {
            InitializeComponent();

            nivelSeleccionado = nivel;
            NivelLabel.Text = $"Nivel seleccionado: {nivelSeleccionado}";

            if (nivelSeleccionado != "Nivel 1")
            {
                ObtenerTiempoNivel(nivelSeleccionado);
                tiempoLabel.Text = nivelTiempo.ToString();
                timer = new System.Timers.Timer(1000);
                timer.Elapsed += tiempo;
            }
            else
            {
                tiempoLabel.Text = "Ilimitado";
            }

            imagen.Source = "base.png";
        }

        private void ObtenerTiempoNivel(string nivel)
        {
            switch (nivel)
            {
                case "Nivel 2":
                    nivelTiempo = 20;
                    break;
                case "Nivel 3":
                    nivelTiempo = 15;
                    break;
                case "Nivel 4":
                    nivelTiempo = 10;
                    break;
                default:
                    nivelTiempo = 0;
                    break;
            }
        }

        private void tiempo(object sender, System.Timers.ElapsedEventArgs e)
        {
            nivelTiempo--;

            Device.BeginInvokeOnMainThread(() =>
            {
                tiempoLabel.Text = nivelTiempo.ToString();

                if (nivelTiempo <= 0)
                {
                    timer.Stop();
                    timer.Dispose();
                    operacionAleatoria();

                    ReiniciarTiempo();

                    DisplayAlert("Tiempo agotado", "Se ha agotado el tiempo", "OK");
                    intentos++;
                    MostrarImagen(intentos);
                    if (intentos >= maxIntentos)
                    {
                        DisplayAlert("Alert", "Has alcanzado el máximo de intentos", "OK");
                        button1.IsEnabled = true;
                        timer.Stop();
                        return;
                    }
                }
            });
        }



        private void ReiniciarTiempo()
        {
            timer?.Stop();
            timer?.Dispose();

            ObtenerTiempoNivel(nivelSeleccionado);
            tiempoLabel.Text = nivelTiempo.ToString();

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += tiempo;
            timer.Start();
        }



        private void Button1_Click(object sender, EventArgs e)
        {


            resultado.IsEnabled = true;
            button1.IsEnabled = false;
            intentos = 0;
            puntaje = 0;

            imagen.Source = "base.png";


            puntajeLabel.Text = $"Puntaje: {puntaje}";
            LimpiarImagen();

            operacionAleatoria();

            if (nivelSeleccionado != "Nivel 1")
            {
                timer.Start();
            }
        }

        private void operacionAleatoria()
        {
            num1 = random.Next(1, 11);
            num2 = random.Next(1, 11);

            numero1.Text = num1.ToString();
            numero1.IsEnabled = false;

            numero2.Text = num2.ToString();
            numero2.IsEnabled = false;

            resultado.Text = "";
            resultadoCorrecto = num1 * num2;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (intentos >= maxIntentos)
            {
                DisplayAlert("Alert", "Has alcanzado el máximo de intentos", "OK");
                button1.IsEnabled = true;
                timer.Stop();
                return;
            }

            if (int.TryParse(resultado.Text, out int resultadoIngresado))
            {
                if (resultadoIngresado == resultadoCorrecto)
                {
                    DisplayAlert("Correcto", "¡El resultado es correcto!", "OK");

                    puntaje++;
                    puntajeLabel.Text = $"Puntaje: {puntaje}";

                    if (nivelSeleccionado != "Nivel 1")
                    {
                        ReiniciarTiempo();
                    }

                    if (intentos >= 1)
                    {
                        intentos--;
                        MostrarImagen(intentos);
                    }
                    else
                    {
                        intentos = 0;
                    }
                }
                else
                {
                    DisplayAlert("Incorrecto", $"El resultado es {resultadoCorrecto}.", "OK");
                    if (nivelSeleccionado != "Nivel 1")
                    {
                        ReiniciarTiempo();
                    }
                    intentos++;
                    MostrarImagen(intentos);
                }

                if (intentos >= maxIntentos)
                {
                    DisplayAlert("Alert", "¡Se acabaron los intentos!", "OK");
                    resultado.IsEnabled = false;
                    button1.IsEnabled = true;
                }
                else
                {
                    operacionAleatoria();
                }
            }
        }

        private void MostrarImagen(int intentos)
        {
            string nombreImagen;
            switch (intentos)
            {
                case 1:
                    nombreImagen = "basecangrejo1.png";
                    break;
                case 2:
                    nombreImagen = "basecangrejo2.png";
                    break;
                case 3:
                    nombreImagen = "basecangrejo3.png";
                    break;
                case 4:
                    nombreImagen = "basecangrejo4.png";
                    break;
                case 5:
                    nombreImagen = "basecangrejo5.png";
                    break;
                case 6:
                    nombreImagen = "basecangrejo6.png";
                    break;
                default:
                    nombreImagen = "base.png";
                    break;
            }
            imagen.Source = ImageSource.FromFile(nombreImagen);
        }

        private void LimpiarImagen()
        {
            if (imagen.Source != null && imagen.Source.ToString() != $"File: {imagenPredeterminada}")
            {
                imagen.Source = null;
            }
        }
    }
}
