using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.OracleClient;

namespace SkinPass1Scheduling
{
    public class SKINPASS1DataBase
    {
        #region Var
        public static string ConncetionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=172.25.1.101)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=dev)));Persist Security Info=True;User ID=apps;Password=appsdev;Unicode=True";
        #endregion

        #region Constructor
        public SKINPASS1DataBase()
        {
        }
        #endregion

        #region ExecuteCommand

        public int ExecuteCommand(string command, CommandType type)
        {
            int i;
            using (OracleConnection conn = new OracleConnection(ConncetionString))
            {
                using (OracleCommand ocmd = new OracleCommand(command, conn))
                {
                    ocmd.CommandType = type;
                    conn.Open();
                    i = ocmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return i;
        }


        public int ExecuteCommand(string command, CommandType type, OracleParameter[] param)
        {
            int i;
            using (OracleConnection conn = new OracleConnection(ConncetionString))
            {
                using (OracleCommand ocmd = new OracleCommand(command, conn))
                {
                    ocmd.CommandType = type;
                    foreach (OracleParameter pr in param)
                    {
                        if (pr.Value == null)
                            pr.Value = DBNull.Value;
                        ocmd.Parameters.Add(pr);
                    }
                    conn.Open();
                    i = ocmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return i;
        }

        #endregion

        #region ExeceuteScalar

        public void ExecuteScalar(ref object Scalar, string command, CommandType type, OracleParameter[] param)
        {
            using (OracleConnection conn = new OracleConnection(ConncetionString))
            {
                using (OracleCommand ocmd = new OracleCommand(command, conn))
                {
                    ocmd.CommandType = type;
                    ocmd.CommandText = command;
                    foreach (OracleParameter pr in param)
                    {
                        if (pr.Value == null)
                            pr.Value = DBNull.Value;
                        ocmd.Parameters.Add(pr);
                    }
                    conn.Open();
                    Scalar = ocmd.ExecuteScalar();
                    conn.Close();
                }
            }
        }

        public void ExecuteScalar(ref object Scalar, string command, CommandType type)
        {
            using (OracleConnection conn = new OracleConnection(ConncetionString))
            {
                using (OracleCommand ocmd = new OracleCommand(command, conn))
                {
                    ocmd.CommandType = type;
                    ocmd.CommandText = command;
                    conn.Open();
                    Scalar = ocmd.ExecuteScalar();
                    conn.Close();
                }
            }
        }

        #endregion

        #region GetDataTable
        public void GetDataTable(ref DataTable DT, string selectCommand, CommandType type, OracleParameter[] param)
        {
            // DT.Rows.Clear();
            using (OracleConnection conn = new OracleConnection(ConncetionString))
            {
                using (OracleDataAdapter DA = new OracleDataAdapter(selectCommand, conn))
                {
                    DA.SelectCommand.CommandType = type;
                    foreach (OracleParameter pr in param)
                    {
                        if (pr.Value == null)
                            pr.Value = DBNull.Value;

                        DA.SelectCommand.Parameters.Add(pr);
                    }
                    conn.Open();

                    DA.Fill(DT);

                    conn.Close();
                }
            }

        }

        public void GetDataTable(ref DataTable DT, string selectCommand, CommandType type)
        {
            // DT.Rows.Clear();
            using (OracleConnection conn = new OracleConnection(ConncetionString))
            {
                using (OracleDataAdapter DA = new OracleDataAdapter(selectCommand, conn))
                {
                    DA.SelectCommand.CommandType = type;
                    conn.Open();
                    DA.Fill(DT);
                    conn.Close();
                }
            }

        }
        public DataTable GetDataTable(string selectCommand, CommandType type, OracleParameter[] param)
        {
            DataTable DT = new DataTable();
            using (OracleConnection conn = new OracleConnection(ConncetionString))
            {
                using (OracleDataAdapter DA = new OracleDataAdapter(selectCommand, conn))
                {
                    DA.SelectCommand.CommandType = type;
                    foreach (OracleParameter pr in param)
                    {
                        if (pr.Value == null)
                            pr.Value = DBNull.Value;

                        DA.SelectCommand.Parameters.Add(pr);
                    }
                    conn.Open();

                    DA.Fill(DT);

                    conn.Close();
                }
            }
            return DT;
        }

        public DataTable 
            GetDataTable(string selectCommand, CommandType type)
        {
            // DT.Rows.Clear();
            DataTable DT = new DataTable();
            using (OracleConnection conn = new OracleConnection(ConncetionString))
            {
                using (OracleDataAdapter DA = new OracleDataAdapter(selectCommand, conn))
                {
                    DA.SelectCommand.CommandType = type;
                    conn.Open();
                    DA.Fill(DT);
                    conn.Close();
                }
            }
            return DT;
        }
        #endregion

        #region GetDataSet

        public void GetDataSet(ref DataSet DS, string selectCommand)
        {
            using (OracleConnection conn = new OracleConnection(ConncetionString))
            {
                using (OracleDataAdapter da = new OracleDataAdapter(selectCommand, conn))
                {
                    conn.Open();
                    da.Fill(DS);
                    conn.Close();
                }
            }
        }

        public void GetDataSet(ref DataSet DS, string selectCommand, CommandType type, OracleParameter[] param)
        {
            using (OracleConnection conn = new OracleConnection(ConncetionString))
            {
                using (OracleDataAdapter DA = new OracleDataAdapter(selectCommand, conn))
                {
                    DA.SelectCommand.CommandType = type;
                    foreach (OracleParameter pr in param)
                    {
                        if (pr.Value == null)
                            pr.Value = DBNull.Value;

                        DA.SelectCommand.Parameters.Add(pr);
                    }
                    conn.Open();
                    DA.Fill(DS);
                    conn.Close();
                }
            }
        }
        #endregion
    }
}
