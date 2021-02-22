using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IndicatorCAROL : IndicatorBase, IIndicator
{

    public IndicatorCAROL()
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
        return "CAROL";
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
        Operation op1 = GetOperationDetail(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayVolume);


        double diffCandle = Math.Abs((((arrayPriceClose[arrayPriceClose.Length - 1] * 100) / arrayPriceClose[arrayPriceClose.Length - 2]) - 100));

        Console.WriteLine("diffCandle: " + diffCandle + "%");

        if (diffCandle < 0.2)
            return op1;


        return Operation.nothing;

    }

    public Operation GetOperationDetail(double[] arrayPriceOpen, double[] arrayPriceClose, double[] arrayPriceLow, double[] arrayPriceHigh, double[] arrayVolume)
    {
        try
        {
            IndicatorMACD macd = new IndicatorMACD();
            Operation operationMACD = macd.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayVolume);

            IndicatorCCI cci = new IndicatorCCI();
            Operation operationCCI = cci.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayVolume);

            IndicatorRSI rsi = new IndicatorRSI();
            Operation operationRSI = rsi.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayVolume);

            MainClass.log("CCI " + cci.result);
            MainClass.log("RSI " + rsi.result);

            if (cci.result > 0 && operationMACD == Operation.buy && rsi.result > 50 && cci.getTendency() == Tendency.high && rsi.getTendency() == Tendency.high)
            //if (operationMACD == Operation.buy)
            {
                double[] arrayresultMA = new double[arrayPriceClose.Length];
                int outBegidx, outNbElement;
                TicTacTec.TA.Library.Core.MovingAverage(0, arrayPriceClose.Length - 1, arrayPriceClose, 100, TicTacTec.TA.Library.Core.MAType.Ema, out outBegidx, out outNbElement, arrayresultMA);
                if (arrayPriceClose[arrayPriceClose.Length - 1] > arrayresultMA[outNbElement - 1])
                    return Operation.buy;
            }
            if (cci.result < 0 && operationMACD == Operation.sell && rsi.result < 50 && cci.getTendency() == Tendency.low && rsi.getTendency() == Tendency.low)
            //if (operationMACD == Operation.sell)
            {
                double[] arrayresultMA = new double[arrayPriceClose.Length];
                int outBegidx, outNbElement;
                TicTacTec.TA.Library.Core.MovingAverage(0, arrayPriceClose.Length - 1, arrayPriceClose, 100, TicTacTec.TA.Library.Core.MAType.Ema, out outBegidx, out outNbElement, arrayresultMA);
                if (arrayPriceClose[arrayPriceClose.Length - 1] < arrayresultMA[outNbElement - 1])
                    return Operation.sell;
            }

            return Operation.nothing;



        }
        catch
        {
            return Operation.nothing;
        }
    }
}
