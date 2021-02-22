using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IndicatorSAR : IndicatorBase, IIndicator
{

    public IndicatorSAR()
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
        return "SAR";
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

    public double[] arrayresultTA;

    public Operation GetOperation(double[] arrayPriceOpen, double[] arrayPriceClose, double[] arrayPriceLow, double[] arrayPriceHigh, double[] arrayVolume)
    {
        try
        {
            int outBegidx, outNbElement;            
            arrayresultTA = new double[arrayPriceClose.Length];
            TicTacTec.TA.Library.Core.Sar(0, arrayPriceClose.Length - 1, arrayPriceHigh, arrayPriceLow, 0.02, 0.2, out outBegidx, out outNbElement, arrayresultTA);            
            double value = arrayresultTA[outNbElement - 1];
            double lastValue = arrayresultTA[outNbElement - 2];
            double priceClose = arrayPriceClose[arrayPriceClose.Length - 1];
            this.result = value;
            this.result2 = lastValue;
            if (value < priceClose && lastValue > arrayPriceClose[arrayPriceClose.Length - 2])
                return Operation.buy;
            if (value > priceClose && lastValue < arrayPriceClose[arrayPriceClose.Length - 2])
                return Operation.sell;
            return Operation.nothing;
        }
        catch
        {
            return Operation.nothing;
        }
    }
}
