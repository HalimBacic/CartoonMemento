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

namespace CartoonMemento
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();
            this.aboutBox.FontWeight = FontWeights.Bold;
            this.aboutBox.Text = "Cartoon Memento \n Version: 1.0.0 \n By Halim Bacic";
            this.helpBox.FontWeight = FontWeights.Bold;
            this.helpBox.Text = "Welcome to CartoonMemento";
            this.helpBox.FontWeight = FontWeights.Black;
            this.helpBox.Text = "This is a simple software for photo editing. General purpose, you can put some cartoon sticker on your picture and \n save to your location." +
                "In left side boxes you can choose ....";
        }
    }
}
