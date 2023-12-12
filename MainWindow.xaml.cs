using Calculator.Logic;
using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewQueue _viewQueue;
        private readonly IProccessEval _proccessEval;
        private const string DelOperator = "<-";
        private const string EqualOperator = "=";
        
        public MainWindow()
        {
            _viewQueue = new ViewQueue(new QueueRequests<string>(), new QueueResults<string>());
            _proccessEval = new ProccessEval();
            DataContext = _viewQueue;
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var btnValue = ((Button)sender).Content.ToString();

            if (btnValue.Equals(EqualOperator))
            {
                var expression = textLabel.Text;
                var tread = new Thread(async ()=> await _proccessEval.Eval(expression, _viewQueue));
                tread.Start();
               
                textLabel.Text = string.Empty;
                return;
            }

            if (btnValue.Equals(DelOperator))
            {
                if (textLabel.Text.Length < 1)
                    return;

                textLabel.Text = textLabel.Text.Remove(textLabel.Text.Length - 1);
                return;
            }
  

            textLabel.Text += btnValue;
            return;

        }
    }
}
