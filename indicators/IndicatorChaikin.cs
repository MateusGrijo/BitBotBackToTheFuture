using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IndicatorChaikin : IndicatorBase, IIndicator
{


    public IndicatorChaikin()
    {
        this.indicator = this;
    }
    public void setPeriod(int period)
    {
        this.period = period;
    }
    public TypeIndicator getTypeIndicator()
    {
        return TypeIndicator.Normal;
    }

    public string getName()
    {
        return "CHAIKIN";
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


            double[] arrayresultTA = new double[arrayPriceClose.Length];
            int outBegidx, outNbElement;
            arrayresultTA = new double[arrayPriceClose.Length];
            TicTacTec.TA.Library.Core.AdOsc(0, arrayPriceClose.Length - 1, arrayPriceHigh, arrayPriceLow, arrayPriceClose, arrayVolume, 3, 10, out outBegidx, out outNbElement, arrayresultTA);
            double chaikin = arrayresultTA[outNbElement - 1];
            this.result = chaikin;
            if (chaikin < 0)
                return Operation.sell;
            if (chaikin > 0)
                return Operation.buy;

            return Operation.nothing;
        }
        catch
        {
            return Operation.nothing;
        }
    }
}
