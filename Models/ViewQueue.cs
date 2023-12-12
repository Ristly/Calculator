using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Models;

public class ViewQueue : INotifyPropertyChanged
{
    private SyncQueue _queueRequests;
    private SyncQueue _queueResults;
    public event PropertyChangedEventHandler? PropertyChanged;

    public ViewQueue(SyncQueue queueRequests, SyncQueue queueResults)
    {
        _queueRequests = queueRequests;
        _queueResults = queueResults;
    }
    
    public void EnqueueResult(string expression)
    {
        _queueResults.Enqueue(expression);
        OnPropertyChanged("Results");
    }

    public string DequeueResult()
    {
        var elem = _queueResults.Dequeue();
        OnPropertyChanged("Results");
        return elem;
    }

    public void EnqueueRequest(string expression)
    {
        _queueRequests.Enqueue(expression);
        OnPropertyChanged("Requests");
    }

    public string DequeueRequest()
    {
       var elem = _queueRequests.Dequeue();
        OnPropertyChanged("Requests");
        return elem;
    }

    public IEnumerable<string> Requests => _queueRequests.ReadAll();
    public IEnumerable<string> Results => _queueResults.ReadAll();

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
