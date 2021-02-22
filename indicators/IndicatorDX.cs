using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IndicatorDX : IndicatorBase, IIndicator
{


    public IndicatorDX()
    {
        this.indicator = this;
        this.period = 14;
    }

    public string getName()
    {
        return "DX";
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
            double[] result = new double[arrayPriceClose.Length];
            TicTacTec.TA.Library.Core.Dx(0, arrayPriceClose.Length - 1, arrayPriceHigh, arrayPriceLow, arrayPriceClose, this.period, out outBegidx, out outNbElement, result);
            double priceClose = arrayPriceClose[arrayPriceClose.Length - 1];
            double value = result[outNbElement - 1];

            this.tendency = Tendency.nothing;
            if (result[outNbElement - 2] < result[outNbElement - 1] && result[outNbElement - 3] < result[outNbElement - 2])
                this.tendency = Tendency.high;
            if (result[outNbElement - 2] > result[outNbElement - 1] && result[outNbElement - 3] > result[outNbElement - 2])
                this.tendency = Tendency.low;


            this.result = value;
            if (value > 0)
                return Operation.buy;
            if (value < 0)
                return Operation.sell;
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
