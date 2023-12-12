using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Models;

public class ViewQueue : INotifyPropertyChanged
{
    private QueueRequests<string> _queueRequests;
    private QueueResults<string> _queueResults;
    public event PropertyChangedEventHandler? PropertyChanged;

    public ViewQueue(QueueRequests<string> queueRequests, QueueResults<string> queueResults)
    {
        _queueRequests = queueRequests;
        _queueResults = queueResults;
    }
    
    public void EnqueueResult(string expression)
    {
        _queueResults.Enqueue(expression);
        OnPropertyChanged("Results");
    }

    public void DequeueResult()
    {
        _queueResults.Dequeue();
        OnPropertyChanged("Results");
    }

    public void EnqueueRequest(string expression)
    {
        _queueRequests.Enqueue(expression);
        OnPropertyChanged("Requests");
    }

    public void DequeueRequest()
    {
        _queueRequests.Dequeue();
        OnPropertyChanged("Requests");
    }

    public IEnumerable<string> Requests => _queueRequests.ReadAll();
    public IEnumerable<string> Results => _queueResults.ReadAll();

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
