using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Logic;

public class ProccessEval : IProccessEval
{
    private readonly DataTable _evalService;
    public ProccessEval() 
    {
        _evalService = new DataTable();
    }
    public async Task Eval(string expression, ViewQueue viewQueue, int secTime = 3)
    {
        viewQueue.EnqueueRequest(expression);
        var timer = Task.Delay(secTime*1000);
        var result = _evalService.Compute(expression, null).ToString();
        Task.WaitAll(timer);
        viewQueue.DequeueRequest();
        viewQueue.EnqueueResult(result);
    }
}
