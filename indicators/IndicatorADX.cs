using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IndicatorADX : IndicatorBase, IIndicator
{


    public IndicatorADX()
    {
        this.indicator = this;
        this.period = 14;
    }

    public string getName()
    {
        return "ADX";
    }


    public void setPeriod(int period)
    {
        this.period = period;
    }

    public Operation GetOperation(double[] arrayPriceOpen, double[] arrayPriceClose, double[] arrayPriceLow, double[] arrayPriceHigh, double[] arrayVolume)
    {
        try
        {
            int outBegidx, outNbElement;
            double[] result = new double[arrayPriceClose.Length];
            TicTacTec.TA.Library.Core.Adx(0, arrayPriceClose.Length - 1, arrayPriceHigh, arrayPriceLow, arrayPriceClose, this.period, out outBegidx, out outNbElement, result);
            double priceClose = arrayPriceClose[arrayPriceClose.Length - 1];
            double value = result[outNbElement - 1];
            this.result = value;
            if (value > 25)
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

    public TypeIndicator getTypeIndicator()
    {
        return TypeIndicator.Normal;
    }

    public Tendency getTendency()
    {
        return this.tendency;
    }
}
