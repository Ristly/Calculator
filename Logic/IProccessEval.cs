using Calculator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calculator.Logic;

public interface IProccessEval
{
    public int Timer { get; set; }
    public string Eval(string expression);
}