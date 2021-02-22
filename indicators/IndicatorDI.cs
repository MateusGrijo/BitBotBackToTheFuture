using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IndicatorDI : IndicatorBase, IIndicator
{


    public IndicatorDI()
    {
        this.indicator = this;
        this.period = 14;
    }

    public string getName()
    {
        return "DI";
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
            double priceClose = arrayPriceClose[arrayPriceClose.Length - 1];

            double[] result = new double[arrayPriceClose.Length];
            TicTacTec.TA.Library.Core.MinusDI(0, arrayPriceClose.Length - 1, arrayPriceHigh, arrayPriceLow, arrayPriceClose, this.period, out outBegidx, out outNbElement, result);
            double valueMinus = result[outNbElement - 1];

            result = new double[arrayPriceClose.Length];
            TicTacTec.TA.Library.Core.PlusDI(0, arrayPriceClose.Length - 1, arrayPriceHigh, arrayPriceLow, arrayPriceClose, this.period, out outBegidx, out outNbElement, result);
            double valuePlus = result[outNbElement - 1];

            this.result = valueMinus;
            this.result2 = valuePlus;

            if (valuePlus > valueMinus)
                return Operation.buy;
            if (valueMinus > valuePlus)
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
