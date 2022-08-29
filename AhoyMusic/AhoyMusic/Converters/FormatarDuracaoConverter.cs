using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AhoyMusic.Converters
{
    public class FormatarDuracaoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valorFormatado;
            if(value != null)
            {
                valorFormatado = FormataPosicao((double)value);
            }
            else
            {
                valorFormatado = String.Empty;
            }
            return valorFormatado;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static string FormataPosicao(double valor)
        {
            int minutos = (int)valor / 60;
            int segundos = (int)valor % 60;

            if(segundos < 10)
                return String.Format("{0}:0{1}", minutos, segundos);
            else
                return String.Format("{0}:{1}", minutos, segundos);
        }
    }
}
