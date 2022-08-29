using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace AhoyMusic.Converters
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource imgSource;
            if(value != null)
            {
                byte[] byteImageData = value as byte[];
                imgSource = ImageSource.FromStream(() => new MemoryStream(byteImageData));
            }
            else
            {
                imgSource = null;
            }
            return imgSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
