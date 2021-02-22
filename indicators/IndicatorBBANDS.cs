using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IndicatorBBANDS : IndicatorBase, IIndicator
{

    public IndicatorBBANDS()
    {
        this.indicator = this;
        this.period = 20;
    }
    public string getName()
    {
        return "BBANDS";
    }
    public void setPeriod(int period)
    {
        this.period = period;
    }

    public TypeIndicator getTypeIndicator()
    {
        return TypeIndicator.Normal;
    }

    public Tendency getTendency()
    {
        return this.tendency;
    }

    public Operation GetOperation(double[] arrayPriceOpen, double[] arrayPriceClose, double[] arrayPriceLow, double[] arrayPriceHigh, double[] arrayVolume)
    {
        try
        {
            int outBegidx, outNbElement;
            double[] lineDown = new double[arrayPriceClose.Length];
            double[] lineMid = new double[arrayPriceClose.Length];
            double[] lineUp = new double[arrayPriceClose.Length];
            TicTacTec.TA.Library.Core.Bbands(0, arrayPriceClose.Length - 1, arrayPriceClose, this.period, 2, 2, TicTacTec.TA.Library.Core.MAType.Ema, out outBegidx, out outNbElement, lineUp, lineMid, lineDown);
            double priceClose = arrayPriceClose[arrayPriceClose.Length - 1];
            double _lineDown = lineDown[outNbElement - 1];
            double _lineMid = lineMid[outNbElement - 1];
            double _lineUp = lineUp[outNbElement - 1];
            this.result = _lineDown;
            this.result2 = _lineUp;
            if (priceClose > _lineUp && priceClose > _lineMid)
                return Operation.sell;
            if (_lineDown > priceClose && priceClose < _lineMid)
                return Operation.buy;
            return Operation.nothing;
        }
        catch
        {
            return Operation.nothing;
        }
    }

    public double getResult()
    {
        return this.result;
    }

    public double getResult2()
    {
        return this.result2;
    }
}
