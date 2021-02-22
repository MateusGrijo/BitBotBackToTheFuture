using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IndicatorMFI : IndicatorBase, IIndicator
{

    public IndicatorMFI()
    {
        this.indicator = this;
        this.period = 20;
    }

    public string getName()
    {
        return "MFI";
    }

    public void setPeriod(int period)
    {
        this.period = period;
    }
    public TypeIndicator getTypeIndicator()
    {
        return TypeIndicator.Normal;
    }

    public double getResult()
    {
        return this.result;
    }

    public Tendency getTendency()
    {
        return this.tendency;

    }

    public double getResult2()
    {
        return this.result2;
    }

    public Operation GetOperation(double[] arrayPriceOpen, double[] arrayPriceClose, double[] arrayPriceLow, double[] arrayPriceHigh, double[] arrayVolume)
    {
        try
        {
            int outBegidx, outNbElement;
            double[] arrayresultTA = new double[arrayPriceClose.Length];
            arrayresultTA = new double[arrayPriceClose.Length];
            TicTacTec.TA.Library.Core.Mfi(0, arrayPriceClose.Length - 1, arrayPriceHigh, arrayPriceLow, arrayPriceClose, arrayVolume, this.period, out outBegidx, out outNbElement, arrayresultTA);
            double value = arrayresultTA[outNbElement - 1];

            this.tendency = Tendency.nothing;
            if (arrayresultTA[outNbElement - 2] < arrayresultTA[outNbElement - 1] && arrayresultTA[outNbElement - 3] < arrayresultTA[outNbElement - 2])
                this.tendency = Tendency.high;
            if (arrayresultTA[outNbElement - 2] > arrayresultTA[outNbElement - 1] && arrayresultTA[outNbElement - 3] > arrayresultTA[outNbElement - 2])
                this.tendency = Tendency.low;

            this.result = value;
            if (value == 0)
                return Operation.nothing;
            if (value > 80)
                return Operation.sell;
            if (value < 20)
                return Operation.buy;
            return Operation.nothing;
        }
        catch
        {
            return Operation.nothing;
        }
    }
}
