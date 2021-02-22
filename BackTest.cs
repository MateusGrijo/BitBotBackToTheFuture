using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitBotBackToTheFuture
{
    public class BackTest
    {

        private static string symbol = "XBTUSD";
        private static int sizeArrayCandles = 500;
        private static  string size = "5m";


        public static double[] SubArray(double[] data, int index, int length)
        {
            double[] result = new double[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static void run()
        {

            MainClass.arrayPriceClose = new double[sizeArrayCandles];
            MainClass.arrayPriceHigh = new double[sizeArrayCandles];
            MainClass.arrayPriceLow = new double[sizeArrayCandles];
            MainClass.arrayPriceVolume = new double[sizeArrayCandles];
            MainClass.arrayPriceOpen = new double[sizeArrayCandles];
            MainClass.arrayDate = new DateTime[sizeArrayCandles];

            List<BitMEX.Candle> lstCandle = MainClass.bitMEXApi.GetCandleHistory(symbol, sizeArrayCandles, size);

            int i = 0;
            foreach (var candle in lstCandle)
            {
                MainClass.arrayPriceClose[i] = (double)candle.close;
                MainClass.arrayPriceHigh[i] = (double)candle.high;
                MainClass.arrayPriceLow[i] = (double)candle.low;
                MainClass.arrayPriceVolume[i] = (double)candle.volume;
                MainClass.arrayPriceOpen[i] = (double)candle.open;
                MainClass.arrayDate[i] = (DateTime)candle.TimeStamp;
                i++;
            }

            Array.Reverse(MainClass.arrayPriceClose);
            Array.Reverse(MainClass.arrayPriceHigh);
            Array.Reverse(MainClass.arrayPriceLow);
            Array.Reverse(MainClass.arrayPriceVolume);
            Array.Reverse(MainClass.arrayPriceOpen);
            Array.Reverse(MainClass.arrayDate);




            for ( i = 10; i < sizeArrayCandles; i++)
            {

                IndicatorSAR sar = new IndicatorSAR();
                IndicatorCCI cci = new IndicatorCCI();
                Operation result = sar.GetOperation(SubArray(MainClass.arrayPriceOpen, 0, i), SubArray(MainClass.arrayPriceClose, 0, i), SubArray(MainClass.arrayPriceLow, 0, i), SubArray(MainClass.arrayPriceHigh, 0, i), SubArray(MainClass.arrayPriceVolume, 0, i));
                Operation resultCCI = cci.GetOperation(SubArray(MainClass.arrayPriceOpen, 0, i), SubArray(MainClass.arrayPriceClose, 0, i), SubArray(MainClass.arrayPriceLow, 0, i), SubArray(MainClass.arrayPriceHigh, 0, i), SubArray(MainClass.arrayPriceVolume, 0, i));

                MainClass.log(MainClass.arrayDate[i].ToString() + " - " + MainClass.arrayPriceClose[i].ToString() + " - " + result.ToString());
                MainClass.log("CCI - " + cci.result);
                if (result == Operation.buy)
                {
                    //MainClass.log("Buy");
                    //MainClass.log("Open " + MainClass.arrayPriceOpen[i].ToString());
                    //MainClass.log("Close " + MainClass.arrayPriceClose[i].ToString());
                    //MainClass.log("Low " + MainClass.arrayPriceLow[i].ToString());
                    //MainClass.log("High " + MainClass.arrayPriceHigh[i].ToString());
                    //MainClass.log("Volume " + MainClass.arrayPriceVolume[i].ToString());
                }
                else if (result == Operation.sell)
                {

                    //MainClass.log("Sell");
                    //MainClass.log("Open " + MainClass.arrayPriceOpen[i].ToString());
                    //MainClass.log("Close " + MainClass.arrayPriceClose[i].ToString());
                    //MainClass.log("Low " + MainClass.arrayPriceLow[i].ToString());
                    //MainClass.log("High " + MainClass.arrayPriceHigh[i].ToString());
                    //MainClass.log("Volume " + MainClass.arrayPriceVolume[i].ToString());

                }
                else if (result == Operation.nothing)
                {

                }


            }


        }

    }
}
