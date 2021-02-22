using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IndicatorULTOSC : IndicatorBase, IIndicator
{


    public IndicatorULTOSC()
    {
        this.indicator = this;
    }

    public string getName()
    {
        return "ULTOSC";
    }
    public TypeIndicator getTypeIndicator()
    {
        return TypeIndicator.Normal;
    }

    public void setPeriod(int period)
    {
        this.period = period;
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
            double[] result = new double[arrayPriceClose.Length];
            TicTacTec.TA.Library.Core.UltOsc(0, arrayPriceClose.Length - 1, arrayPriceHigh, arrayPriceLow, arrayPriceClose, 7, 14, 28, out outBegidx, out outNbElement, result);
            double priceClose = arrayPriceClose[arrayPriceClose.Length - 1];
            double value = result[outNbElement - 1];
            this.result = value;


            this.tendency = Tendency.nothing;
            if (result[outNbElement - 2] < result[outNbElement - 1] && result[outNbElement - 3] < result[outNbElement - 2])
                this.tendency = Tendency.high;
            if (result[outNbElement - 2] > result[outNbElement - 1] && result[outNbElement - 3] > result[outNbElement - 2])
                this.tendency = Tendency.low;


            if (value > 70)
                return Operation.sell;
            if (value < 30)
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
