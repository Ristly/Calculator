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
        private const string ClearOperator = "C";
        
        public MainWindow()
        {

            _viewQueue = new ViewQueue(new SyncQueue(), new SyncQueue());
            _proccessEval = new ProccessEval();
            DataContext = _viewQueue;
            InitializeComponent();

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            TaskFactory taskFactory = new TaskFactory(token);
            for(var i = 0; i<2; i++)
            {
                taskFactory.StartNew(async () =>
                {
                    while (true)
                    {
                        var expression = _viewQueue.DequeueRequest();
                        if (expression is null)
                        {
                            await Task.Delay(1000);
                            continue;
                        }

                        var res = _proccessEval.Eval(expression);
                        _viewQueue.EnqueueResult(res);
                    }

                });
            }

           

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var btnValue = ((Button)sender).Content.ToString();

            if (btnValue.Equals(EqualOperator))
            {
                if (string.IsNullOrEmpty(textLabel.Text.Trim()))
                {
                    MessageBox.Show("Отсутсвтует выражения для рассчёта");
                    return;
                }

                if(!int.TryParse(evalTimeText.Text.Trim(),out var time))
                {
                    MessageBox.Show("Неверно указно время вычисления");
                    evalTimeText.Text = "0";
                    return;
                }
                _proccessEval.Timer = time;
                var expression = textLabel.Text;
                _viewQueue.EnqueueRequest(expression);
               
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

            if (btnValue.Equals(ClearOperator))
            {
                textLabel.Text = "";
                return;
            }

            textLabel.Text += btnValue;
            return;

        }
    }
}
