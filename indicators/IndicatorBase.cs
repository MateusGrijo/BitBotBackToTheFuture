using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class parameterSetData
{
    public DateTime begin;
    public DateTime end;
    public String pair;
}
public class IndicatorBase
{
    public IIndicator indicator;

    public DataTable loadData(String pair, DateTime begin, DateTime end)
    {

        return null;
    }

    public Tendency tendency = Tendency.nothing;
    public int period = 14;
    public double result = 0;
    public double result2 = 0;
    public DataTable dtResult;
    public void createSchemaResult()
    {
        dtResult = new DataTable("Table");
        dtResult.Columns.Add("date");
        dtResult.Columns.Add("pair");
        dtResult.Columns.Add("price");
        dtResult.Columns.Add("operation");
        dtResult.Columns.Add("value");
    }

    static string myExeDir = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();
    public void saveResult(String indicator, String pair)
    {
        //DataSet ds = new DataSet();
        //ds.Tables.Add(this.dtResult);
        //ds.WriteXml(myExeDir + "\\" + indicator + "_" + pair + ".xml");
    }

    public void loadResult(String indicator, String pair)
    {
        //if (this.dtResult == null)
        //{
        //    DataSet ds = new DataSet();
        //    ds.ReadXml(myExeDir + "\\" + indicator + "_" + pair + ".xml");
        //    this.dtResult = ds.Tables[0];
        //}
    }


    public double[] arrayPriceClose;
    public double[] arrayPriceHigh;
    public double[] arrayPriceLow;
    public double[] arrayPriceOpen;
    public double[] arrayVolume;

    public void loadArray(DataRow[] rows)
    {
        arrayPriceClose = new double[rows.Length];
        arrayPriceHigh = new double[rows.Length];
        arrayPriceLow = new double[rows.Length];
        arrayPriceOpen = new double[rows.Length];
        arrayVolume = new double[rows.Length];
        int i = 0;
        foreach (DataRow row in rows)
        {
            arrayPriceClose[i] = double.Parse(row["close"].ToString());
            arrayPriceHigh[i] = double.Parse(row["high"].ToString());
            arrayPriceLow[i] = double.Parse(row["low"].ToString());
            arrayPriceOpen[i] = double.Parse(row["open"].ToString());
            arrayVolume[i] = double.Parse(row["volume"].ToString());
            i++;
        }
    }

    public decimal lastPriceClose;
    public Operation GetOperation(DateTime date, String pair)
    {
        try
        {
            this.loadResult(indicator.getName(), pair);
            DataRow[] rows = this.dtResult.Select("date = '" + date.ToString("dd/MM/yyyy HH:mm:ss") + "' and pair = '" + pair + "' ");
            if (rows.Length > 0)
            {
                lastPriceClose = decimal.Parse(rows[0]["price"].ToString().Replace(".", ","));
                if (rows[0]["operation"].ToString() == "buy")
                    return Operation.buy;
                if (rows[0]["operation"].ToString() == "sell")
                    return Operation.sell;


            }

        }
        catch
        {

        }
        return Operation.nothing;
    }

    public void setData(Object obj)
    {
        try
        {
            parameterSetData parameter = (parameterSetData)obj;
            String pair = parameter.pair;
            DateTime begin = parameter.begin;
            DateTime end = parameter.end;
            DateTime endFinal = end;
            end = begin.AddMinutes(2000);
            DataTable dt = this.loadData(pair, begin, endFinal);
            this.createSchemaResult();
            while (true)
            {
                try
                {
                    Int64 unixbegin = (Int64)(begin.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    Int64 unixend = (Int64)(end.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    this.loadArray(dt.Select("date >= " + unixbegin + " and date <= " + unixend));
                    Operation operation = indicator.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayVolume);
                    dtResult.Rows.Add(end, pair, arrayPriceClose[arrayPriceClose.Length - 1], operation.ToString(), "");

                }
                catch (Exception ex)
                {
                    //Logger.log(ex.Message + ex.StackTrace, "");
                }
                begin = begin.AddMinutes(5);
                end = end.AddMinutes(5);
                if (end >= endFinal)
                    break;
            }
            this.saveResult(indicator.getName(), pair);
            this.dtResult = null;
        }
        catch
        {

        }
    }
}

