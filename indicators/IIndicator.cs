using System;

public enum Operation
{
    buy,
    sell,
    nothing
};

public enum TypeIndicator
{
    Normal,
    Cross
};

public enum Tendency
{
    high,
    low,
    nothing
};

public interface IIndicator
{
    String getName();
    TypeIndicator getTypeIndicator();
    Tendency getTendency();
    void setPeriod(int period);
    double getResult();
    double getResult2();
    Operation GetOperation(double[] arrayPriceOpen, double[] arrayPriceClose, double[] arrayPriceLow, double[] arrayPriceHigh, double[] arrayVolume);
}
