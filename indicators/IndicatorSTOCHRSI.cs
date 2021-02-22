using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IndicatorSTOCHRSI : IndicatorBase, IIndicator
{

    public IndicatorSTOCHRSI()
    {
        this.indicator = this;
    }
    public string getName()
    {
        return "STOCHRSI";
    }

    public TypeIndicator getTypeIndicator()
    {
        return TypeIndicator.Normal;
    }

    public void setPeriod(int period)
    {
        this.period = period;

    }
    public double getResult()
    {
        return this.result;
    }

    public double getResult2()
    {
        return this.result2;
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
            double[] arrayresultTA = new double[arrayPriceClose.Length];

            double[] outK = new double[arrayPriceClose.Length];
            double[] outD = new double[arrayPriceClose.Length];
            TicTacTec.TA.Library.Core.StochRsi(0, arrayPriceClose.Length - 1, arrayPriceClose, 14, 3, 3, TicTacTec.TA.Library.Core.MAType.Ema, out outBegidx, out outNbElement, outK, outD);
            double stochRsiK = outK[outNbElement - 1];
            double stochRsiD = outD[outNbElement - 1];
            this.result = stochRsiK;
            this.result2 = stochRsiD;
            if (stochRsiK > 80 && stochRsiD > 80)
                return Operation.sell;
            if (stochRsiK < 20 && stochRsiD < 20)
                return Operation.buy;
            return Operation.nothing;
        }
        catch
        {
            return Operation.nothing;
        }
    }
}
