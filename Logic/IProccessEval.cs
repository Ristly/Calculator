using Calculator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calculator.Logic;

public interface IProccessEval
{
    public Task Eval(string expression, ViewQueue viewQueue, int secTime = 3);
}