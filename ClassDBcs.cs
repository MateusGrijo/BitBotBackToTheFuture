using System;
using System.Data;
using System.Configuration;

/// <summary>
/// Summary description for ClassDB
/// </summary>
public class ClassDB
{

    public static String strConn = @"";
    public static String dbquery = @"";



    public ClassDB()
    {

    }


    static MySql.Data.MySqlClient.MySqlConnection createConnection()
    {
        if (MainClass.usedb)
        {
            try
            {

                MySql.Data.MySqlClient.MySqlConnection conn = null;
                conn = new MySql.Data.MySqlClient.MySqlConnection(strConn);
                int count = 0;
                int maxCount = 5;
                while (conn.State != ConnectionState.Open)
                {
                    try
                    {
                        conn.Open();
                    }
                    catch
                    {
                        System.Threading.Thread.Sleep(300);
                    }
                    count++;
                    if (count >= maxCount)
                        break;
                }
                return conn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("createConnection" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }
        else
            return null;
    }


    public static int execS(String sql)
    {
        if (MainClass.usedb)
        {
            int lines = 0;
            MySql.Data.MySqlClient.MySqlCommand cmd = null;
            MySql.Data.MySqlClient.MySqlConnection conn = null;
            try
            {
                conn = createConnection();
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.CommandTimeout = (60 * 1000) * 3;
                lines = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine("execS" + ex.Message + ex.StackTrace);
                return -1;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                    conn = null;
                }
            }
            return lines;
        }
        else
            return 0;
    }


    








}
