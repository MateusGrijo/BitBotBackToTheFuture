using BitBotBackToTheFuture;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;


enum TendencyMarket
{
    HIGH,
    NORMAL,
    LOW,
    VERY_LOW,
    VERY_HIGH
}

class MainClass
{


    //REAL NET
    public static string version = "0.0.2.13";
    public static string location = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + System.IO.Path.DirectorySeparatorChar;

    public static string bitmexKey = "";
    public static string bitmexSecret = "";
    public static string bitmexKeyWeb = "";
    public static string bitmexSecretWeb = "";
    public static string timeGraph = "";
    public static string statusShort = "";
    public static string statusLong = "";
    public static string pair = "";
    public static int qtdyContacts = 0;
    public static int interval = 0;
    public static int intervalOrder = 0;
    public static int intervalCapture = 0;
    public static int intervalCancelOrder = 30;
    public static int positionContracts = 0;
    public static double profit = 0;
    public static int limiteOrder = 0;
    public static double fee = 0;
    public static double stoploss = 10;
    public static double stopgain = 15;
    public static string bitmexDomain = "";
    public static string operation = "normal"; // normal - scalper - surf
    public static bool roeAutomatic = true;
    public static bool usedb = false;
    public static bool tendencyBook = false;

    public static double traillingProfit = -1;
    public static double traillingStop = -1;

    public static double stepValue = 0.5;
    public static TendencyMarket tendencyMarket = TendencyMarket.NORMAL;
    public static BitMEX.BitMEXApi bitMEXApi = null;

    public static List<IIndicator> lstIndicatorsAll = new List<IIndicator>();
    public static List<IIndicator> lstIndicatorsEntry = new List<IIndicator>();
    public static List<IIndicator> lstIndicatorsEntryCross = new List<IIndicator>();
    public static List<IIndicator> lstIndicatorsEntryDecision = new List<IIndicator>();


    public static int sizeArrayCandles = 500;
    public static double[] arrayPriceClose = new double[sizeArrayCandles];
    public static double[] arrayPriceHigh = new double[sizeArrayCandles];
    public static double[] arrayPriceLow = new double[sizeArrayCandles];
    public static double[] arrayPriceVolume = new double[sizeArrayCandles];
    public static double[] arrayPriceOpen = new double[sizeArrayCandles];
    public static DateTime[] arrayDate = new DateTime[sizeArrayCandles];


    public static Object data = new Object();

    public static void Main(string[] args)
    {
        try
        {
            //Config
            Console.Title = "Loading...";

            Console.ForegroundColor = ConsoleColor.White;

            log("Deleron - Back to the future - v" + version + " - Bitmex version");
            log("by Matheus Grijo ", ConsoleColor.Green);
            log(" ======= HALL OF FAME BOTMEX  ======= ");
            log(" - Lucas Sousa", ConsoleColor.Magenta);
            log(" - Carlos Morato", ConsoleColor.Magenta);
            log(" - Luis Felipe Alves", ConsoleColor.Magenta);
            log(" ======= END HALL OF FAME BOTMEX  ======= ");

            log("http://botmex.ninja/");
            log("GITHUB http://github.com/matheusgrijo", ConsoleColor.Blue);
            log(" ******* DONATE ********* ");
            log("BTC 39DWjHHGXJh9q82ZrxkA8fiZoE37wL8jgh");
            log("BCH qqzwkd4klrfafwvl7ru7p7wpyt5z3sjk6y909xq0qk");
            log("ETH 0x3017E79f460023435ccD285Ff30Bd10834D20777");
            log("ETC 0x088E7E67af94293DB55D61c7B55E2B098d2258D9");
            log("LTC MVT8fxU4WBzdfH5XgvRPWkp7pE4UyzG9G5");
            log("Load config...");
            log("Considere DOAR para o projeto!", ConsoleColor.Green);
            log("Vamos aguardar 10 min para voce doar ;) ... ", ConsoleColor.Blue);
            log("ATENCAO, PARA FACILITAR A DOACAO DAQUI A 30 SEGUNDOS VAMOS ABRIR UMA PAGINA PARA VOCE!", ConsoleColor.Green);

            //System.Diagnostics.Process.Start("https://www.blockchain.com/btc/payment_request?address=1AnttTLGhzJsX7T96SutWS4N9wPYuBThu8&amount_local=30&currency=USD&nosavecurrency=true");

            String jsonConfig = System.IO.File.ReadAllText(location + "key.txt");
            JContainer jCointaner = (JContainer)JsonConvert.DeserializeObject(jsonConfig, (typeof(JContainer)));





            //usedb = jCointaner["usedb"].ToString() == "enable";

            bitmexKey = jCointaner["key"].ToString();
            bitmexSecret = jCointaner["secret"].ToString();
            bitmexKeyWeb = jCointaner["webserverKey"].ToString();
            bitmexSecretWeb = jCointaner["webserverSecret"].ToString();
            bitmexDomain = jCointaner["domain"].ToString();
            statusShort = jCointaner["short"].ToString();
            statusLong = jCointaner["long"].ToString();
            pair = jCointaner["pair"].ToString();
            //ClassDB.strConn = jCointaner["dbcon"].ToString();
            //ClassDB.dbquery = jCointaner["dbquery"].ToString();
            timeGraph = jCointaner["timeGraph"].ToString();
            qtdyContacts = int.Parse(jCointaner["contract"].ToString());
            interval = int.Parse(jCointaner["interval"].ToString());
            //intervalCancelOrder = int.Parse(jCointaner["intervalCancelOrder"].ToString());
            intervalOrder = int.Parse(jCointaner["intervalOrder"].ToString());
            //intervalCapture = int.Parse(jCointaner["webserverIntervalCapture"].ToString());
            profit = double.Parse(jCointaner["profit"].ToString());
            fee = double.Parse(jCointaner["fee"].ToString());
            //stoploss = double.Parse(jCointaner["stoploss"].ToString());
            //stepValue = double.Parse(jCointaner["stepvalue"].ToString());
            //stopgain = double.Parse(jCointaner["stopgain"].ToString());
            //roeAutomatic = jCointaner["roe"].ToString() == "automatic";
            //tendencyBook = jCointaner["tendencyBook"].ToString() == "enable";
            //operation = jCointaner["operation"].ToString();
            //limiteOrder = int.Parse(jCointaner["limiteOrder"].ToString());

            bitMEXApi = new BitMEX.BitMEXApi(bitmexKey, bitmexSecret, bitmexDomain);









            //FINAL

            if (jCointaner["webserver"].ToString() == "enable")
            {
                WebServer ws = new WebServer(WebServer.SendResponse, jCointaner["webserverConfig"].ToString());
                ws.Run();
                System.Threading.Thread tCapture = new Thread(Database.captureDataJob);
                tCapture.Start();
                System.Threading.Thread.Sleep(1000);
                OperatingSystem os = Environment.OSVersion;
                PlatformID pid = os.Platform;
                if(pid != PlatformID.Unix)
                {
                    System.Diagnostics.Process.Start(jCointaner["webserverConfig"].ToString());
                }
            }



            log("Total open orders: " + bitMEXApi.GetOpenOrders(pair).Count);

            log("");
            log("Wallet: " + bitMEXApi.GetWallet());

            lstIndicatorsAll.Add(new IndicatorADX());
            lstIndicatorsAll.Add(new IndicatorMFI());
            lstIndicatorsAll.Add(new IndicatorBBANDS());
            lstIndicatorsAll.Add(new IndicatorCCI());
            lstIndicatorsAll.Add(new IndicatorCMO());
            lstIndicatorsAll.Add(new IndicatorDI());
            lstIndicatorsAll.Add(new IndicatorDM());
            lstIndicatorsAll.Add(new IndicatorMA());
            lstIndicatorsAll.Add(new IndicatorMACD());
            lstIndicatorsAll.Add(new IndicatorMOM());
            lstIndicatorsAll.Add(new IndicatorPPO());
            lstIndicatorsAll.Add(new IndicatorROC());
            lstIndicatorsAll.Add(new IndicatorRSI());
            lstIndicatorsAll.Add(new IndicatorSAR());
            lstIndicatorsAll.Add(new IndicatorSTOCH());
            lstIndicatorsAll.Add(new IndicatorSTOCHRSI());
            lstIndicatorsAll.Add(new IndicatorTRIX());
            lstIndicatorsAll.Add(new IndicatorULTOSC());
            lstIndicatorsAll.Add(new IndicatorWILLR());
            lstIndicatorsAll.Add(new IndicatorCAROL());
            lstIndicatorsAll.Add(new IndicatorX());

            foreach (var item in jCointaner["indicatorsEntry"])
            {
                foreach (var item2 in lstIndicatorsAll)
                {
                    if (item["name"].ToString().Trim().ToUpper() == item2.getName().Trim().ToUpper())
                    {
                        item2.setPeriod(int.Parse((item["period"].ToString().Trim().ToUpper())));
                        lstIndicatorsEntry.Add(item2);
                    }
                }
            }

            foreach (var item in jCointaner["indicatorsEntryCross"])
            {
                foreach (var item2 in lstIndicatorsAll)
                {
                    if (item["name"].ToString().Trim().ToUpper() == item2.getName().Trim().ToUpper())
                    {
                        item2.setPeriod(int.Parse((item["period"].ToString().Trim().ToUpper())));
                        lstIndicatorsEntryCross.Add(item2);
                    }
                }
            }

            foreach (var item in jCointaner["indicatorsEntryDecision"])
            {
                foreach (var item2 in lstIndicatorsAll)
                {
                    if (item["name"].ToString().Trim().ToUpper() == item2.getName().Trim().ToUpper())
                    {
                        item2.setPeriod(int.Parse((item["period"].ToString().Trim().ToUpper())));
                        lstIndicatorsEntryDecision.Add(item2);
                    }
                }
            }





            bool automaticTendency = statusLong == "automatic";


            if (operation == "manual")
            {
                while (true)
                    System.Threading.Thread.Sleep(60000);
            }


            //LOOP 
            while (true)
            {

                try
                {



                    positionContracts = getPosition(); // FIX CARLOS MORATO                                            
                    double positionPrice = 0;

                    if (positionContracts != 0)
                        positionPrice = getPositionPrice();

                    log("positionContracts " + positionContracts);
                    log("positionPrice " + positionPrice);



                    if (operation == "scalper")
                    {
                        bool _stop = false;
                        if (positionContracts < 0)
                        {
                            double priceActual = getPriceActual("Buy");
                            double perc = ((priceActual * 100) / positionPrice) - 100;
                            log("perc" + perc);
                            if (perc > 0)
                                if (perc > stoploss)
                                    _stop = true;
                        }

                        if (positionContracts > 0)
                        {
                            double priceActual = getPriceActual("Sell");
                            double perc = ((priceActual * 100) / positionPrice) - 100;
                            log("perc" + perc);
                            if (perc < 0)
                                if (Math.Abs(perc) > stoploss)
                                    _stop = true;
                        }


                        if (_stop)
                        {
                            //Stop loss
                            log(bitMEXApi.CancelAllOpenOrders(pair));
                            String side = "Buy";
                            if (positionContracts > 0)
                                side = "Sell";



                            if (side == "Sell")
                                log(bitMEXApi.PostOrderPostOnly(pair, side, getPriceActual("Sell") - 10, Math.Abs(positionContracts), true));
                            if (side == "Buy")
                                log(bitMEXApi.PostOrderPostOnly(pair, side, getPriceActual("Sell") + 10, Math.Abs(positionContracts), true));
                            log("[STOP LOSS] " + pair + " " + side + " " + positionContracts);
                        }




                        bool _stopgain = false;
                        if (positionContracts < 0)
                        {
                            double priceActual = getPriceActual("Buy");
                            double perc = ((priceActual * 100) / positionPrice) - 100;
                            log("perc" + perc);
                            if (perc < 0)
                            {

                                if (traillingProfit > Math.Abs(perc))
                                    if (Math.Abs(perc) > stopgain)
                                        _stopgain = true;

                                if (Math.Abs(perc) > traillingProfit)
                                    traillingProfit = Math.Abs(perc);

                                if (Math.Abs(perc) > stopgain)
                                    _stopgain = true;
                            }
                        }

                        if (positionContracts > 0)
                        {
                            double priceActual = getPriceActual("Sell");
                            double perc = ((priceActual * 100) / positionPrice) - 100;
                            log("perc" + perc);
                            if (perc > 0)
                            {
                                if (traillingProfit > Math.Abs(perc))
                                    if (Math.Abs(perc) > stopgain)
                                        _stopgain = true;

                                if (Math.Abs(perc) > traillingProfit)
                                    traillingProfit = Math.Abs(perc);

                                if (Math.Abs(perc) > stopgain)
                                    _stopgain = true;
                            }
                        }


                        if (_stopgain)
                        {
                            //Stop loss
                            log(bitMEXApi.CancelAllOpenOrders(pair));
                            String side = "Buy";
                            if (positionContracts > 0)
                                side = "Sell";
                            if (side == "Sell")
                                log(bitMEXApi.PostOrderPostOnly(pair, side, getPriceActual("Sell") - 10, Math.Abs(positionContracts), true));
                            if (side == "Buy")
                                log(bitMEXApi.PostOrderPostOnly(pair, side, getPriceActual("Sell") + 10, Math.Abs(positionContracts), true));
                            log("[STOP GAIN] " + pair + " " + side + " " + positionContracts);
                        }

                    }



                    //SEARCH POSITION AND MAKE ORDER
                    //By Carlos Morato



                    //SEARCH POSITION AND MAKE ORDER
                    //By Carlos Morato
                    #region "Fix position not orders

                    //if (operation != "surf" && roeAutomatic && Math.Abs(positionContracts) != Math.Abs(getOpenOrderQty()))
                    //    bitMEXApi.CancelAllOpenOrders(pair);

                    fixOrdersPosition();

                    #endregion

                    //CANCEL ORDER WITHOUT POSITION
                    //By Carlos Morato




                    if (automaticTendency)
                        verifyTendency();

                    //GET CANDLES
                    //bool nothing = false;
                    //if (getCandles())
                    //{

                    //    if (statusLong == "enable")
                    //    {
                    //        log("");
                    //        log("==========================================================");
                    //        log(" ==================== Verify LONG OPERATION =============", ConsoleColor.Green);
                    //        log("==========================================================");
                    //        /////VERIFY OPERATION LONG
                    //        string operation = "buy";
                    //        //VERIFY INDICATORS ENTRY
                    //        foreach (var item in lstIndicatorsEntry)
                    //        {
                    //            Operation operationBuy = item.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                    //            log("Indicator: " + item.getName());
                    //            log("Result1: " + item.getResult());
                    //            log("Result2: " + item.getResult2());
                    //            log("Date: " + arrayDate[arrayPriceOpen.Length - 1]);
                    //            log("Operation: " + operationBuy.ToString());
                    //            log("");
                    //            if (operationBuy != Operation.buy)
                    //            {
                    //                operation = "nothing";
                    //                nothing = true;
                    //                break;
                    //            }
                    //        }

                    //        //VERIFY INDICATORS CROSS
                    //        if (operation == "buy")
                    //        {
                    //            //Prepare to long                        
                    //            while (true)
                    //            {
                    //                log("wait operation long...");
                    //                getCandles();
                    //                foreach (var item in lstIndicatorsEntryCross)
                    //                {
                    //                    Operation operationBuy = item.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                    //                    log("Indicator Cross: " + item.getName());
                    //                    log("Result1: " + item.getResult());
                    //                    log("Result2: " + item.getResult2());
                    //                    log("Operation: " + operationBuy.ToString());
                    //                    log("");

                    //                    if (item.getTypeIndicator() == TypeIndicator.Cross)
                    //                    {
                    //                        if (operationBuy == Operation.buy)
                    //                        {
                    //                            operation = "long";
                    //                            break;
                    //                        }
                    //                    }
                    //                    else if (operationBuy != Operation.buy)
                    //                    {
                    //                        operation = "long";
                    //                        break;
                    //                    }
                    //                }
                    //                if (lstIndicatorsEntryCross.Count == 0)
                    //                    operation = "long";
                    //                if (operation != "buy")
                    //                    break;

                    //                log("wait " + interval + "ms");
                    //                Thread.Sleep(interval);


                    //            }
                    //        }

                    //        //VERIFY INDICATORS DECISION
                    //        if (operation == "long" && lstIndicatorsEntryDecision.Count > 0)
                    //        {
                    //            operation = "decision";
                    //            foreach (var item in lstIndicatorsEntryDecision)
                    //            {
                    //                Operation operationBuy = item.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                    //                log("Indicator Decision: " + item.getName());
                    //                log("Result1: " + item.getResult());
                    //                log("Result2: " + item.getResult2());
                    //                log("Operation: " + operationBuy.ToString());
                    //                log("");



                    //                if (getValue("indicatorsEntryDecision", item.getName(), "decision") == "enable" && getValue("indicatorsEntryDecision", item.getName(), "tendency") == "enable")

                    //                {
                    //                    int decisionPoint = int.Parse(getValue("indicatorsEntryDecision", item.getName(), "decisionPoint"));
                    //                    if (item.getResult() >= decisionPoint && item.getTendency() == Tendency.high)
                    //                    {
                    //                        operation = "long";
                    //                        break;
                    //                    }
                    //                }


                    //                if (getValue("indicatorsEntryDecision", item.getName(), "decision") == "enable")

                    //                {
                    //                    int decisionPoint = int.Parse(getValue("indicatorsEntryDecision", item.getName(), "decisionPoint"));
                    //                    if (item.getResult() >= decisionPoint)
                    //                    {
                    //                        operation = "long";
                    //                        break;
                    //                    }
                    //                }

                    //                if (getValue("indicatorsEntryDecision", item.getName(), "tendency") == "enable")

                    //                {
                    //                    if (item.getTendency() == Tendency.high)
                    //                    {
                    //                        operation = "long";
                    //                        break;
                    //                    }
                    //                }

                    //            }
                    //        }


                    //        //EXECUTE OPERATION
                    //        if (operation == "long")
                    //            makeOrder("Buy");

                    //        ////////////FINAL VERIFY OPERATION LONG//////////////////
                    //    }



                    //    if (statusShort == "enable")

                    //    {
                    //        //////////////////////////////////////////////////////////////
                    //        log("");
                    //        log("==========================================================");
                    //        log(" ==================== Verify SHORT OPERATION =============", ConsoleColor.Red);
                    //        log("==========================================================");
                    //        /////VERIFY OPERATION LONG
                    //        string operation = "sell";
                    //        //VERIFY INDICATORS ENTRY
                    //        foreach (var item in lstIndicatorsEntry)
                    //        {
                    //            Operation operationBuy = item.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                    //            log("Indicator: " + item.getName());
                    //            log("Result1: " + item.getResult());
                    //            log("Result2: " + item.getResult2());
                    //            log("Operation: " + operationBuy.ToString());
                    //            log("");
                    //            if (operationBuy != Operation.sell)
                    //            {
                    //                operation = "nothing";
                    //                nothing = true;
                    //                break;
                    //            }
                    //        }

                    //        //VERIFY INDICATORS CROSS
                    //        if (operation == "sell")
                    //        {
                    //            //Prepare to long                        
                    //            while (true)
                    //            {
                    //                log("wait operation short...");
                    //                getCandles();
                    //                foreach (var item in lstIndicatorsEntryCross)
                    //                {
                    //                    Operation operationBuy = item.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                    //                    log("Indicator Cross: " + item.getName());
                    //                    log("Result1: " + item.getResult());
                    //                    log("Result2: " + item.getResult2());
                    //                    log("Operation: " + operationBuy.ToString());
                    //                    log("");

                    //                    if (item.getTypeIndicator() == TypeIndicator.Cross)
                    //                    {
                    //                        if (operationBuy == Operation.sell)
                    //                        {
                    //                            operation = "short";
                    //                            break;
                    //                        }
                    //                    }
                    //                    else if (operationBuy != Operation.sell)
                    //                    {
                    //                        operation = "short";
                    //                        break;
                    //                    }
                    //                }
                    //                if (lstIndicatorsEntryCross.Count == 0)
                    //                    operation = "short";
                    //                if (operation != "sell")
                    //                    break;

                    //                log("wait " + interval + "ms");
                    //                Thread.Sleep(interval);


                    //            }
                    //        }

                    //        //VERIFY INDICATORS DECISION
                    //        if (operation == "short" && lstIndicatorsEntryDecision.Count > 0)
                    //        {
                    //            operation = "decision";
                    //            foreach (var item in lstIndicatorsEntryDecision)
                    //            {
                    //                Operation operationBuy = item.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                    //                log("Indicator Decision: " + item.getName());
                    //                log("Result1: " + item.getResult());
                    //                log("Result2: " + item.getResult2());
                    //                log("Operation: " + operationBuy.ToString());
                    //                log("");



                    //                if (getValue("indicatorsEntryDecision", item.getName(), "decision") == "enable" && getValue("indicatorsEntryDecision", item.getName(), "tendency") == "enable")

                    //                {
                    //                    int decisionPoint = int.Parse(getValue("indicatorsEntryDecision", item.getName(), "decisionPoint"));
                    //                    if (item.getResult() <= decisionPoint && item.getTendency() == Tendency.low)
                    //                    {
                    //                        operation = "short";
                    //                        break;
                    //                    }
                    //                }


                    //                if (getValue("indicatorsEntryDecision", item.getName(), "decision") == "enable")

                    //                {
                    //                    int decisionPoint = int.Parse(getValue("indicatorsEntryDecision", item.getName(), "decisionPoint"));
                    //                    if (item.getResult() <= decisionPoint)
                    //                    {
                    //                        operation = "short";
                    //                        break;
                    //                    }
                    //                }

                    //                if (getValue("indicatorsEntryDecision", item.getName(), "tendency") == "enable")

                    //                {
                    //                    if (item.getTendency() == Tendency.low)
                    //                    {
                    //                        operation = "short";
                    //                        break;
                    //                    }
                    //                }

                    //            }
                    //        }


                    //        //EXECUTE OPERATION
                    //        if (operation == "short")
                    //            makeOrder("Sell");

                    //        ////////////FINAL VERIFY OPERATION LONG//////////////////
                    //    }

                    //}



                    //condicao
                    if (getCandles())
                        if (operation == "scalper")
                        {

                            IndicatorCAROL carol = new IndicatorCAROL();
                            Operation _operation = Operation.nothing;
                            _operation = carol.GetOperation(arrayPriceOpen, arrayPriceClose, arrayPriceLow, arrayPriceHigh, arrayPriceVolume);
                            if (_operation != Operation.nothing)
                            {

                                if (Math.Abs(getPosition()) == 0 && Math.Abs(getOpenOrderQty()) == 0)
                                {


                                    List<double> lst = arrayPriceClose.OfType<double>().ToList();

                                    log("Min: " + lst.Min());
                                    log("Avg: " + lst.Average());
                                    log("Max: " + lst.Max());

                                    double percaux = 0.3;
                                    double min5 = ((lst.Min() * percaux) / 100) + (lst.Min());
                                    double max5 = (lst.Max()) - ((lst.Max() * percaux) / 100);

                                    log("Min5: " + min5);
                                    log("Max5: " + max5);
                                    if (arrayPriceClose[499] > min5 && arrayPriceClose[499] < max5)
                                    {

                                        if (_operation == Operation.buy)
                                        {
                                            makeOrder("Buy", true);
                                            for (int i = 0; i < 20; i++)
                                            {
                                                if (Math.Abs(getOpenOrderQty()) == 0)
                                                {
                                                    bitMEXApi.CancelAllOpenOrders(pair);
                                                    break;
                                                }
                                                if (Math.Abs(getPosition()) > 0)
                                                {
                                                    fixOrdersPosition(false);
                                                    break;
                                                }
                                                Thread.Sleep(5000);
                                            }
                                            if (Math.Abs(getPosition()) == 0)
                                                bitMEXApi.CancelAllOpenOrders(pair);
                                        }
                                        else if (_operation == Operation.sell)
                                        {
                                            makeOrder("Sell", true);
                                            for (int i = 0; i < 20; i++)
                                            {                                                
                                                if (Math.Abs(getOpenOrderQty()) == 0)
                                                {
                                                    bitMEXApi.CancelAllOpenOrders(pair);
                                                    break;
                                                }
                                                if (Math.Abs(getPosition()) > 0)
                                                {
                                                    fixOrdersPosition(false);
                                                    break;
                                                }
                                                Thread.Sleep(5000);
                                            }
                                            if (Math.Abs(getPosition()) == 0)
                                                bitMEXApi.CancelAllOpenOrders(pair);

                                        }


                                    }
                                }

                            }
                        }




                    log("wait " + interval + "ms", ConsoleColor.Blue);
                    Thread.Sleep(interval);


                }
                catch (Exception ex)
                {
                    log("while true::" + ex.Message + ex.StackTrace);
                }


            }

        }
        catch (Exception ex)
        {
            log("ERROR FATAL::" + ex.Message + ex.StackTrace);
        }
    }



    static bool existsOrderOpenById(string id)
    {
        List<BitMEX.Order> lst = bitMEXApi.GetOpenOrders(pair);
        foreach (var item in lst)
        {
            if (item.OrderId.ToUpper().Trim() == id.ToUpper().Trim())
                return true;
        }
        return false;
    }


    static void makeOrder(string side, bool nothing = false)
    {
        bool execute = false;
        try
        {


            log(" wait 5s Make order " + side);
            double price = 0;
            String json = "";

            if (nothing)
            {
                price = Math.Abs(getPriceActual(side));
                json = bitMEXApi.PostOrderPostOnly(pair, side, price, Math.Abs(qtdyContacts) / 2);
                return;
            }

            if (side == "Sell" && statusShort == "enable" && Math.Abs(limiteOrder) > Math.Abs(bitMEXApi.GetOpenOrders(pair).Count))
            {


                if (tendencyBook)
                    if (getTendencyOrderBook() != Tendency.low)
                        return;


                if (operation == "surf")
                {
                    price = Math.Abs(getPriceActual(side)) - 10;
                    json = bitMEXApi.PostOrderPostOnly(pair, side, price, Math.Abs(getPosition()) + Math.Abs(qtdyContacts), true);
                }
                else
                {
                    price = Math.Abs(getPriceActual(side)) - 10;
                    json = bitMEXApi.PostOrderPostOnly(pair, side, price, Math.Abs(qtdyContacts));
                }

                log(json);

                if (json.ToLower().IndexOf("error") >= 0 || json.ToLower().IndexOf("canceled") >= 0)
                    return;

                JContainer jCointaner = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));


                for (int i = 0; i < intervalCancelOrder; i++)
                {
                    if (!existsOrderOpenById(jCointaner["orderID"].ToString()) && !existsOrderOpenById(jCointaner["orderID"].ToString()))
                    {
                        if (operation == "scalper")
                        {
                            price = price - stepValue;
                        }
                        else
                        {
                            price -= (price * profit) / 100;
                            price = Math.Abs(Math.Floor(price));
                        }

                        if (operation != "surf")
                        {

                            //json = "error";
                            //while (json.ToLower().IndexOf("error") >= 0)
                            //{
                            //    //json = bitMEXApi.PostOrderPostOnly(pair, "Buy", price, Math.Abs(qtdyContacts));
                            //    //if (json.ToLower().IndexOf("error") >= 0)
                            //        Thread.Sleep(800);
                            //}

                        }



                        traillingProfit = -1;
                        traillingStop = -1;

                        log(json);
                        execute = true;
                        break;
                    }
                    log("wait order limit " + i + " of " + intervalCancelOrder + "...");
                    if (operation != "scalper")
                        Thread.Sleep(800);
                }



                if (!execute)
                {
                    bitMEXApi.DeleteOrders(jCointaner["orderID"].ToString());
                    while (existsOrderOpenById(jCointaner["orderID"].ToString()))
                        bitMEXApi.DeleteOrders(jCointaner["orderID"].ToString());
                    log("Cancel order ID " + jCointaner["orderID"].ToString());
                }

            }

            if (side == "Buy" && statusLong == "enable" && Math.Abs(limiteOrder) > Math.Abs(bitMEXApi.GetOpenOrders(pair).Count))
            {

                if (tendencyBook)
                    if (getTendencyOrderBook() != Tendency.high)
                        return;


                price = 0;
                json = "";
                if (operation == "surf")
                {
                    price = Math.Abs(getPriceActual(side)) + 10;
                    json = bitMEXApi.PostOrderPostOnly(pair, side, price, Math.Abs(getPosition()) + Math.Abs(qtdyContacts), true);
                }
                else
                {
                    price = Math.Abs(getPriceActual(side)) + 10;
                    json = bitMEXApi.PostOrderPostOnly(pair, side, price, Math.Abs(qtdyContacts));
                }

                log(json);
                if (json.ToLower().IndexOf("error") >= 0 || json.ToLower().IndexOf("canceled") >= 0)
                    return;


                JContainer jCointaner = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));

                log("wait total...");
                for (int i = 0; i < intervalCancelOrder; i++)
                {
                    if (!existsOrderOpenById(jCointaner["orderID"].ToString()) && !existsOrderOpenById(jCointaner["orderID"].ToString()))
                    {

                        if (operation == "scalper")
                        {
                            price = price + stepValue;
                        }
                        else
                        {
                            price += (price * profit) / 100;
                            price = Math.Abs(Math.Floor(price));
                        }

                        if (operation != "surf")
                        {
                            //json = "error";
                            //while (json.ToLower().IndexOf("error") >= 0)
                            //{
                            //    json = bitMEXApi.PostOrderPostOnly(pair, "Sell", price, Math.Abs(qtdyContacts));
                            //    if(json.ToLower().IndexOf("error") >= 0)
                            //        Thread.Sleep(800);
                            //}
                        }
                        traillingProfit = -1;
                        traillingStop = -1;
                        log(json);
                        execute = true;
                        break;
                    }
                    log("wait order limit " + i + " of " + intervalCancelOrder + "...");
                    if (operation != "scalper")
                        Thread.Sleep(800);
                }


                if (!execute)
                {
                    bitMEXApi.DeleteOrders(jCointaner["orderID"].ToString());
                    while (existsOrderOpenById(jCointaner["orderID"].ToString()))
                        bitMEXApi.DeleteOrders(jCointaner["orderID"].ToString());
                    log("Cancel order ID " + jCointaner["orderID"].ToString());
                }


            }
        }
        catch (Exception ex)
        {
            log("makeOrder()::" + ex.Message + ex.StackTrace);
        }

        if (execute)
        {

            log("wait " + intervalOrder + "ms", ConsoleColor.Blue);
            Thread.Sleep(intervalOrder);


        }
    }

    static void verifyTendency()
    {
        try
        {
            String json = Http.get("https://api.binance.com/api/v1/ticker/24hr?symbol=BTCUSDT");
            JContainer j = (Newtonsoft.Json.Linq.JContainer)JsonConvert.DeserializeObject(json);

            tendencyMarket = TendencyMarket.NORMAL;
            decimal priceChangePercent = decimal.Parse(j["priceChangePercent"].ToString().Replace(".", ","));
            if (priceChangePercent < -1.0m)
                tendencyMarket = TendencyMarket.LOW;
            if (priceChangePercent > -1.0m && priceChangePercent < 1.5m)
                tendencyMarket = TendencyMarket.NORMAL;
            if (priceChangePercent > 1.5m)
                tendencyMarket = TendencyMarket.HIGH;
            if (priceChangePercent < -2)
                tendencyMarket = TendencyMarket.VERY_LOW;
            if (priceChangePercent > 3.5m)
                tendencyMarket = TendencyMarket.VERY_HIGH;


            if (tendencyMarket == TendencyMarket.VERY_HIGH || tendencyMarket == TendencyMarket.HIGH)
            {
                statusShort = "disable";
                statusLong = "enable";
            }
            else if (tendencyMarket == TendencyMarket.NORMAL)
            {
                statusShort = "enable";
                statusLong = "enable";
            }
            else if (tendencyMarket == TendencyMarket.LOW || tendencyMarket == TendencyMarket.VERY_LOW)
            {
                statusShort = "enable";
                statusLong = "disable";
            }

            //if (tendencyMarket == TendencyMarket.VERY_HIGH || tendencyMarket == TendencyMarket.VERY_LOW)
            //  timeGraph = "1m";
            //else
            //  timeGraph = "5m";
        }
        catch (Exception ex)
        {

        }
    }


    static double getPriceActual(string type)
    {
        List<BitMEX.OrderBook> listBook = bitMEXApi.GetOrderBook(pair, 1);
        foreach (var item in listBook)
        {
            if (item.Side.ToUpper() == type.ToUpper())
                return item.Price;
        }

        return 0;
    }

    static bool getCandles()
    {
        try
        {
            arrayPriceClose = new double[sizeArrayCandles];
            arrayPriceHigh = new double[sizeArrayCandles];
            arrayPriceLow = new double[sizeArrayCandles];
            arrayPriceVolume = new double[sizeArrayCandles];
            arrayPriceOpen = new double[sizeArrayCandles];
            arrayDate = new DateTime[sizeArrayCandles];
            List<BitMEX.Candle> lstCandle = bitMEXApi.GetCandleHistory(pair, sizeArrayCandles, timeGraph);
            int i = 0;
            foreach (var candle in lstCandle)
            {
                arrayPriceClose[i] = (double)candle.close;
                arrayPriceHigh[i] = (double)candle.high;
                arrayPriceLow[i] = (double)candle.low;
                arrayPriceVolume[i] = (double)candle.volume;
                arrayPriceOpen[i] = (double)candle.open;
                arrayDate[i] = (DateTime)candle.TimeStamp;
                i++;
            }

            Array.Reverse(arrayPriceClose);
            Array.Reverse(arrayPriceHigh);
            Array.Reverse(arrayPriceLow);
            Array.Reverse(arrayPriceVolume);
            Array.Reverse(arrayPriceOpen);
            Array.Reverse(arrayDate);


            Console.Title = DateTime.Now.ToString() + " - " + pair + " - $ " + arrayPriceClose[499].ToString() + " v" + version + " - " + bitmexDomain + " | " + tendencyMarket + "| operation " + operation;
            return true;
        }
        catch (Exception ex)
        {
            log("GETCANDLES::" + ex.Message + ex.StackTrace);
            //log("wait " + intervalOrder + "ms");
            //Thread.Sleep(intervalOrder);
            return false;
        }

    }

    //By Lucas Sousa modify MatheusGrijo
    static int getPosition()
    {
        try
        {
            log("getPosition...");
            List<BitMEX.Position> OpenPositions = bitMEXApi.GetOpenPositions(pair);
            int _qtdContacts = 0;
            foreach (var Position in OpenPositions)
                _qtdContacts += (int)Position.CurrentQty;
            log("getPosition: " + _qtdContacts);
            return _qtdContacts;
        }
        catch (Exception ex)
        {
            log("getPosition::" + ex.Message + ex.StackTrace);
            throw new Exception("Error getPosition");
        }
    }

    static double getRoe()
    {
        try
        {
            log("getRoe...");
            List<BitMEX.Position> OpenPositions = bitMEXApi.GetOpenPositions(pair);
            double _roe = 0;
            foreach (var Position in OpenPositions)
                _roe += Position.percentual();
            log("getRoe: " + _roe);
            return _roe;
        }
        catch (Exception ex)
        {
            log("getRoe::" + ex.Message + ex.StackTrace);
            throw new Exception("Error getRoe");
        }
    }

    //GetOpenOrderQty
    //by Carlos Morato
    static int getOpenOrderQty()
    {
        try
        {
            List<BitMEX.Order> OpenOrderQty = bitMEXApi.GetOpenOrders(pair);
            int _contactsQty = 0;
            foreach (var Order in OpenOrderQty)
                if (Order.Side == "Sell")
                    _contactsQty += (int)Order.OrderQty * (-1);
                else
                    _contactsQty += (int)Order.OrderQty;
            return _contactsQty;
        }
        catch (Exception ex)
        {
            log("getOpenOrderQty::" + ex.Message + ex.StackTrace);
            return 0;
        }
    }


    //GetPositionPrice
    //by Carlos Morato
    static double getPositionPrice()
    {
        try
        {
            List<BitMEX.Position> OpenPositionsPrice = bitMEXApi.GetOpenPositions(pair);
            double _priceContacts = 0;
            foreach (var Position in OpenPositionsPrice)
                _priceContacts = (double)Position.AvgEntryPrice;
            return _priceContacts;
        }
        catch (Exception ex)
        {
            log("getPositionPrice::" + ex.Message + ex.StackTrace);
            return 0;
        }
    }

    static string getValue(String nameList, String nameIndicator, String nameParameter)
    {
        String jsonConfig = System.IO.File.ReadAllText(location + "key.txt");
        JContainer jCointaner = (JContainer)JsonConvert.DeserializeObject(jsonConfig, (typeof(JContainer)));
        foreach (var item in jCointaner[nameList])
            if (item["name"].ToString().Trim().ToUpper() == nameIndicator.ToUpper().Trim())
                return item[nameParameter].ToString().Trim();
        return null;
    }

    public static void log(string value, ConsoleColor color = ConsoleColor.White)
    {
        try
        {

            value = "[" + DateTime.Now.ToString() + "] - " + value;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;

            System.IO.StreamWriter w = new StreamWriter(location + DateTime.Now.ToString("yyyyMMdd") + "_log.txt", true);
            w.WriteLine(value);
            w.Close();
            w.Dispose();

        }
        catch { }
    }



    public static Tendency getTendencyOrderBook()
    {
        try
        {
            List<BitMEX.OrderBook> lstOrderBook = bitMEXApi.GetOrderBook(pair, 100);
            int totalBuy = 0;
            int totalSell = 0;
            foreach (var item in lstOrderBook)
            {
                if (item.Side == "Buy")
                    totalBuy += Math.Abs(item.Size);
                if (item.Side == "Sell")
                    totalSell += Math.Abs(item.Size);
            }


            log("totalBuy " + totalBuy);
            log("totalSell " + totalSell);

            if (totalBuy > totalSell)
                return Tendency.high;
            else if (totalBuy < totalSell)
                return Tendency.low;
            else
                return Tendency.nothing;
        }
        catch
        {
            return Tendency.nothing;
        }
    }




    public static void fixOrdersPosition(bool force = true)
    {
        positionContracts = getPosition(); // FIX CARLOS MORATO                                            
        if (operation != "surf" && roeAutomatic && (Math.Abs(getOpenOrderQty()) < Math.Abs(positionContracts)))
        {
            log("Get Position " + positionContracts);


            int qntContacts = (Math.Abs(positionContracts) - Math.Abs(getOpenOrderQty()));


            if (positionContracts > 0)
            {

                string side = "Sell";
                double priceContacts = Math.Abs(getPositionPrice());
                double actualPrice = Math.Abs(getPriceActual(side));
                double priceContactsProfit = Math.Abs(Math.Floor(priceContacts + (priceContacts * (profit + fee) / 100)));


                if (actualPrice > priceContactsProfit)
                {
                    double price = priceContacts + stepValue;
                    String json = bitMEXApi.PostOrderPostOnly(pair, side, price, Math.Abs(qntContacts), force);
                    JContainer jCointaner2 = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                    log(json);

                }
                else
                {
                    double price = priceContacts + stepValue;
                    String json = bitMEXApi.PostOrderPostOnly(pair, side, price, Math.Abs(qntContacts), force);
                    JContainer jCointaner2 = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                    log(json);
                }
            }

            if (positionContracts < 0)
            {

                string side = "Buy";
                double priceContacts = Math.Abs(getPositionPrice());
                double actualPrice = Math.Abs(getPriceActual(side));
                double priceContactsProfit = Math.Abs(Math.Floor(priceContacts - (priceContacts * (profit + fee) / 100)));


                if (actualPrice < priceContactsProfit)
                {
                    double price = priceContacts - stepValue;
                    String json = bitMEXApi.PostOrderPostOnly(pair, side, price, Math.Abs(qntContacts), force);
                    JContainer jCointaner2 = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                    log(json);

                }
                else
                {
                    double price = priceContacts - stepValue;
                    String json = bitMEXApi.PostOrderPostOnly(pair, side, price, Math.Abs(qntContacts), force);
                    JContainer jCointaner2 = (JContainer)JsonConvert.DeserializeObject(json, (typeof(JContainer)));
                    log(json);

                }
            }

        }
    }

    public static void tests()
    {

        //    BackTest.run();



        return;
    }


}





