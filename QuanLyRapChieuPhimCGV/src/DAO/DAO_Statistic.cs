using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Statistic
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Statistic()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public decimal getRevenueInDay(DateTime date)
        {
            decimal result = 0;
            try
            {
                cnn.Open();
                string query = $@"DECLARE @DOANHTHU_PHIM DECIMAL;
                                    DECLARE @DOANHTHU_DOAN DECIMAL;
                                    SELECT @DOANHTHU_PHIM = SUM(TONGTIEN)
                                    FROM VE
                                    WHERE
	                                    DAY(NGAYLAP) = DAY('{date}') AND
	                                    MONTH(NGAYLAP) = MONTH('{date}') AND
	                                    YEAR(NGAYLAP) = YEAR('{date}')

                                    IF(@DOANHTHU_PHIM IS NULL)
	                                    BEGIN
		                                    SET @DOANHTHU_PHIM = 0
	                                    END

                                    SELECT @DOANHTHU_DOAN = SUM(TONGTIEN)
                                    FROM HOADON
                                    WHERE
	                                    DAY(NGAYHD) = DAY('{date}') AND
	                                    MONTH(NGAYHD) = MONTH('{date}') AND
	                                    YEAR(NGAYHD) = YEAR('{date}')

                                    IF(@DOANHTHU_DOAN IS NULL)
	                                    BEGIN
		                                    SET @DOANHTHU_DOAN = 0
	                                    END

                                    SELECT @DOANHTHU_PHIM + @DOANHTHU_DOAN";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    result = reader.GetDecimal(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return result;
        }

        public decimal getRevenueInMonth(DateTime date)
        {
            decimal result = 0;
            try
            {
                cnn.Open();
                string query = $@"DECLARE @DOANHTHU_PHIM DECIMAL;
                                    DECLARE @DOANHTHU_DOAN DECIMAL;
                                    SELECT @DOANHTHU_PHIM = SUM(TONGTIEN)
                                    FROM VE
                                    WHERE
	                                    MONTH(NGAYLAP) = MONTH('{date}') AND
	                                    YEAR(NGAYLAP) = YEAR('{date}')

                                    IF(@DOANHTHU_PHIM IS NULL)
	                                    BEGIN
		                                    SET @DOANHTHU_PHIM = 0
	                                    END

                                    SELECT @DOANHTHU_DOAN = SUM(TONGTIEN)
                                    FROM HOADON
                                    WHERE
	                                    MONTH(NGAYHD) = MONTH('{date}') AND
	                                    YEAR(NGAYHD) = YEAR('{date}')

                                    IF(@DOANHTHU_DOAN IS NULL)
	                                    BEGIN
		                                    SET @DOANHTHU_DOAN = 0
	                                    END

                                    SELECT @DOANHTHU_PHIM + @DOANHTHU_DOAN";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    result = reader.GetDecimal(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return result;
        }

        public decimal getRevenueInYear(DateTime date)
        {
            decimal result = 0;
            try
            {
                cnn.Open();
                string query = $@"DECLARE @DOANHTHU_PHIM DECIMAL;
                                    DECLARE @DOANHTHU_DOAN DECIMAL;
                                    SELECT @DOANHTHU_PHIM = SUM(TONGTIEN)
                                    FROM VE
                                    WHERE
	                                    YEAR(NGAYLAP) = YEAR('{date}')

                                    IF(@DOANHTHU_PHIM IS NULL)
	                                    BEGIN
		                                    SET @DOANHTHU_PHIM = 0
	                                    END

                                    SELECT @DOANHTHU_DOAN = SUM(TONGTIEN)
                                    FROM HOADON
                                    WHERE
	                                    YEAR(NGAYHD) = YEAR('{date}')

                                    IF(@DOANHTHU_DOAN IS NULL)
	                                    BEGIN
		                                    SET @DOANHTHU_DOAN = 0
	                                    END

                                    SELECT @DOANHTHU_PHIM + @DOANHTHU_DOAN";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    result = reader.GetDecimal(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return result;
        }

        public DataTable getRevenueDaysOfWeek(DateTime date)
        {
            DataTable table = new DataTable();
            table.Columns.Add("key", typeof(string));
            table.Columns.Add("value", typeof(decimal));
            try
            {
                cnn.Open();
                string query = $@"DECLARE @RESULT TABLE(THU INT, DOANHTHU DECIMAL)
                            DECLARE @DOANHTHU_PHIM DECIMAL;
                            DECLARE @DOANHTHU_DOAN DECIMAL;
                            DECLARE @LOOP_BEFORE INT;
                            DECLARE @LOOP_AFTER INT;

                            SELECT @DOANHTHU_PHIM = SUM(TONGTIEN)
                            FROM VE
                            WHERE
	                            DAY(NGAYLAP) = DAY('2022-01-01') AND
	                            MONTH(NGAYLAP) = MONTH('2022-01-01') AND
	                            YEAR(NGAYLAP) = YEAR('2022-01-01')

                            IF(@DOANHTHU_PHIM IS NULL)
	                            BEGIN
		                            SET @DOANHTHU_PHIM = 0
	                            END

                            SELECT @DOANHTHU_DOAN = SUM(TONGTIEN)
                            FROM HOADON
                            WHERE
	                            DAY(NGAYHD) = DAY('2022-01-01') AND
	                            MONTH(NGAYHD) = MONTH('2022-01-01') AND
	                            YEAR(NGAYHD) = YEAR('2022-01-01')

                            IF(@DOANHTHU_DOAN IS NULL)
	                            BEGIN
		                            SET @DOANHTHU_DOAN = 0
	                            END

                            IF(DATEPART(DW, '2022-01-03') = 1)
	                            BEGIN
		                            SET @LOOP_BEFORE = 6
		                            SET @LOOP_AFTER = 0
	                            END
                            ELSE
	                            BEGIN
		                            SET @LOOP_BEFORE = DATEPART(DW, '2022-01-03') - 2;
		                            SET @LOOP_AFTER = 7 - DATEPART(DW, '2022-01-03') + 1;
	                            END
                            DECLARE @NGAYXET DATETIME;
                            DECLARE @DOANHTHUXET DECIMAL;
                            DECLARE @DOANHTHU_PHIM_XET DECIMAL;
                            DECLARE @DOANHTHU_DOAN_XET DECIMAL;

                            SET @NGAYXET = '2022-01-03';
                            WHILE @LOOP_BEFORE != 0 
	                            BEGIN
		                            SET @NGAYXET =  DATEADD(DAY, -1, CAST(@NGAYXET AS DATE));

		                            SELECT @DOANHTHU_PHIM_XET = SUM(TONGTIEN)
		                            FROM VE
		                            WHERE
			                            DAY(NGAYLAP) = DAY(@NGAYXET) AND
			                            MONTH(NGAYLAP) = MONTH(@NGAYXET) AND
			                            YEAR(NGAYLAP) = YEAR(@NGAYXET)

		                            IF(@DOANHTHU_PHIM_XET IS NULL)
			                            BEGIN
				                            SET @DOANHTHU_PHIM_XET = 0
			                            END

		                            SELECT @DOANHTHU_DOAN_XET = SUM(TONGTIEN)
		                            FROM HOADON
		                            WHERE
			                            DAY(NGAYHD) = DAY(@NGAYXET) AND
			                            MONTH(NGAYHD) = MONTH(@NGAYXET) AND
			                            YEAR(NGAYHD) = YEAR(@NGAYXET)

		                            IF(@DOANHTHU_DOAN_XET IS NULL)
			                            BEGIN
				                            SET @DOANHTHU_DOAN_XET = 0
			                            END

		                            SET @DOANHTHUXET = @DOANHTHU_PHIM_XET + @DOANHTHU_DOAN_XET;

		                            INSERT INTO @RESULT(THU, DOANHTHU) 
		                            VALUES (DATEPART(DW, @NGAYXET), @DOANHTHUXET);

		                            SET @LOOP_BEFORE = @LOOP_BEFORE - 1;
	                            END
	
                            INSERT INTO @RESULT(THU, DOANHTHU) 
                            VALUES (DATEPART(DW, '2022-01-03'), @DOANHTHU_PHIM + @DOANHTHU_DOAN);

                            SET @NGAYXET = '2022-01-03';
                            WHILE @LOOP_AFTER != 0 
	                            BEGIN
		                            SET @NGAYXET =  DATEADD(DAY, 1, CAST(@NGAYXET AS DATE));

		                            SELECT @DOANHTHU_PHIM_XET = SUM(TONGTIEN)
		                            FROM VE
		                            WHERE
			                            DAY(NGAYLAP) = DAY(@NGAYXET) AND
			                            MONTH(NGAYLAP) = MONTH(@NGAYXET) AND
			                            YEAR(NGAYLAP) = YEAR(@NGAYXET)

		                            IF(@DOANHTHU_PHIM_XET IS NULL)
			                            BEGIN
				                            SET @DOANHTHU_PHIM_XET = 0
			                            END

		                            SELECT @DOANHTHU_DOAN_XET = SUM(TONGTIEN)
		                            FROM HOADON
		                            WHERE
			                            DAY(NGAYHD) = DAY(@NGAYXET) AND
			                            MONTH(NGAYHD) = MONTH(@NGAYXET) AND
			                            YEAR(NGAYHD) = YEAR(@NGAYXET)

		                            IF(@DOANHTHU_DOAN_XET IS NULL)
			                            BEGIN
				                            SET @DOANHTHU_DOAN_XET = 0
			                            END

		                            SET @DOANHTHUXET = @DOANHTHU_PHIM_XET + @DOANHTHU_DOAN_XET;

		                            INSERT INTO @RESULT(THU, DOANHTHU) 
		                            VALUES (DATEPART(DW, @NGAYXET), @DOANHTHUXET);

		                            SET @LOOP_AFTER = @LOOP_AFTER - 1;
	                            END

                            SELECT * FROM @RESULT ORDER BY THU;";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    int key = reader.GetInt32(0);
                    decimal value = reader.GetDecimal(1);
                    
                    if (key != 1)
                    {
                        table.Rows.Add("Thứ " + key, value);
                    }
                    else
                    {
                        table.Rows.Add("Chủ Nhật", value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return table;
        }

        public DataTable getRevenueDaysOfMonth(DateTime date)
        {
            DataTable table = new DataTable();
            table.Columns.Add("key", typeof(int));
            table.Columns.Add("value", typeof(decimal));
            try
            {
                cnn.Open();
                string query = $@"DECLARE @CACNGAYTRONGTHANG TABLE (NGAY INT, DOANHTHU DECIMAL);
	                            DECLARE @NGAYCUOI INT;
	                            DECLARE @NGAYDAU INT;

	                            SET @NGAYCUOI = DAY(EOMONTH(DATEFROMPARTS({date.Year}, {date.Day}, 1)));
	                            SET @NGAYDAU = 1;
	                            WHILE @NGAYDAU <= @NGAYCUOI
		                            BEGIN
			                            DECLARE @DOANHTHU_PHIM DECIMAL;
			                            DECLARE @DOANHTHU_DOAN DECIMAL;
			                            SELECT @DOANHTHU_PHIM = SUM(TONGTIEN)
			                            FROM VE
			                            WHERE
				                            DAY(NGAYLAP) = @NGAYDAU AND
				                            MONTH(NGAYLAP) = MONTH('{date}') AND
				                            YEAR(NGAYLAP) = YEAR('{date}')

			                            IF(@DOANHTHU_PHIM IS NULL)
				                            BEGIN
					                            SET @DOANHTHU_PHIM = 0
				                            END

			                            SELECT @DOANHTHU_DOAN = SUM(TONGTIEN)
			                            FROM HOADON
			                            WHERE
				                            DAY(NGAYHD) = @NGAYDAU AND
				                            MONTH(NGAYHD) = MONTH('{date}') AND
				                            YEAR(NGAYHD) = YEAR('{date}')

			                            IF(@DOANHTHU_DOAN IS NULL)
				                            BEGIN
					                            SET @DOANHTHU_DOAN = 0
				                            END

			                            INSERT INTO @CACNGAYTRONGTHANG (NGAY, DOANHTHU) 
			                            VALUES (@NGAYDAU, @DOANHTHU_PHIM + @DOANHTHU_DOAN)

			                            SET @NGAYDAU = @NGAYDAU + 1;
		                            END

	                            SELECT NGAY, DOANHTHU FROM @CACNGAYTRONGTHANG;";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    int key = reader.GetInt32(0);
                    decimal value = reader.GetDecimal(1);
                    table.Rows.Add(key, value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return table;
        }
        public DataTable getRevenueMonthsOfYear(DateTime date)
        {
            DataTable table = new DataTable();
            table.Columns.Add("key", typeof(int));
            table.Columns.Add("value", typeof(decimal));
            try
            {
                cnn.Open();
                string query = $@"DECLARE @CACTHANGTRONGNAM TABLE (THANG INT, DOANHTHU DECIMAL);
	                        DECLARE @THANGDAU INT;

	                        SET @THANGDAU = 1;
	                        WHILE @THANGDAU <= 12
		                        BEGIN
			                        DECLARE @DOANHTHU_PHIM DECIMAL;
			                        DECLARE @DOANHTHU_DOAN DECIMAL;
			                        SELECT @DOANHTHU_PHIM = SUM(TONGTIEN)
			                        FROM VE
			                        WHERE
				                        MONTH(NGAYLAP) = @THANGDAU AND
				                        YEAR(NGAYLAP) = YEAR('{date}')

			                        IF(@DOANHTHU_PHIM IS NULL)
				                        BEGIN
					                        SET @DOANHTHU_PHIM = 0
				                        END

			                        SELECT @DOANHTHU_DOAN = SUM(TONGTIEN)
			                        FROM HOADON
			                        WHERE
				                        MONTH(NGAYHD) = @THANGDAU AND
				                        YEAR(NGAYHD) = YEAR('{date}')

			                        IF(@DOANHTHU_DOAN IS NULL)
				                        BEGIN
					                        SET @DOANHTHU_DOAN = 0
				                        END

			                        INSERT INTO @CACTHANGTRONGNAM (THANG, DOANHTHU) 
			                        VALUES (@THANGDAU, @DOANHTHU_PHIM + @DOANHTHU_DOAN)

			                        SET @THANGDAU = @THANGDAU + 1;
		                        END

	                        SELECT THANG, DOANHTHU FROM @CACTHANGTRONGNAM;";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    int key = reader.GetInt32(0);
                    decimal value = reader.GetDecimal(1);
                    table.Rows.Add(key, value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return table;
        }
        public DataTable getRevenueQuartersOfYear(DateTime date)
        {
            DataTable table = new DataTable();
            table.Columns.Add("key", typeof(int));
            table.Columns.Add("value", typeof(decimal));
            try
            {
                cnn.Open();
                string query = $@"DECLARE @CACQUYTRONGNAM TABLE (QUY INT, DOANHTHU DECIMAL);
	                            DECLARE @QUYDAU INT;

	                            SET @QUYDAU = 1;
	                            WHILE @QUYDAU <= 4
		                            BEGIN
			                            DECLARE @DOANHTHU_PHIM DECIMAL;
			                            DECLARE @DOANHTHU_DOAN DECIMAL;
			                            SELECT @DOANHTHU_PHIM = SUM(TONGTIEN)
			                            FROM VE
			                            WHERE
				                            DATEPART(QUARTER,NGAYLAP) = @QUYDAU AND
				                            YEAR(NGAYLAP) = YEAR('{date}')

			                            IF(@DOANHTHU_PHIM IS NULL)
				                            BEGIN
					                            SET @DOANHTHU_PHIM = 0
				                            END

			                            SELECT @DOANHTHU_DOAN = SUM(TONGTIEN)
			                            FROM HOADON
			                            WHERE
				                            DATEPART(QUARTER,NGAYHD) = @QUYDAU AND
				                            YEAR(NGAYHD) = YEAR('{date}')

			                            IF(@DOANHTHU_DOAN IS NULL)
				                            BEGIN
					                            SET @DOANHTHU_DOAN = 0
				                            END

			                            INSERT INTO @CACQUYTRONGNAM (QUY, DOANHTHU) 
			                            VALUES (@QUYDAU, @DOANHTHU_PHIM + @DOANHTHU_DOAN)

			                            SET @QUYDAU = @QUYDAU + 1;
		                            END

	                            SELECT QUY, DOANHTHU FROM @CACQUYTRONGNAM;";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    int key = reader.GetInt32(0);
                    decimal value = reader.GetDecimal(1);
                    table.Rows.Add(key, value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return table;
        }
        public DataTable getRevenueYears(int num, DateTime date)
        {
            DataTable table = new DataTable();
            table.Columns.Add("key", typeof(int));
            table.Columns.Add("value", typeof(decimal));
            try
            {
                cnn.Open();
                string query = $@"DECLARE @CACNAMGANDAY TABLE (NAM INT, DOANHTHU DECIMAL);
	                            DECLARE @QUYDAU INT;

	                            SET @QUYDAU = {date.Year - num};
	                            WHILE @QUYDAU < {date.Year}
		                            BEGIN
			                            DECLARE @DOANHTHU_PHIM DECIMAL;
			                            DECLARE @DOANHTHU_DOAN DECIMAL;
			                            SELECT @DOANHTHU_PHIM = SUM(TONGTIEN)
			                            FROM VE
			                            WHERE
				                            YEAR(NGAYLAP) = YEAR('{date}')

			                            IF(@DOANHTHU_PHIM IS NULL)
				                            BEGIN
					                            SET @DOANHTHU_PHIM = 0
				                            END

			                            SELECT @DOANHTHU_DOAN = SUM(TONGTIEN)
			                            FROM HOADON
			                            WHERE
				                            YEAR(NGAYHD) = YEAR('{date}')

			                            IF(@DOANHTHU_DOAN IS NULL)
				                            BEGIN
					                            SET @DOANHTHU_DOAN = 0
				                            END

			                            INSERT INTO @CACNAMGANDAY (NAM, DOANHTHU) 
			                            VALUES (@QUYDAU, @DOANHTHU_PHIM + @DOANHTHU_DOAN)

			                            SET @QUYDAU = @QUYDAU + 1;
		                            END

	                            SELECT NAM, DOANHTHU FROM @CACNAMGANDAY;";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    int key = reader.GetInt32(0);
                    decimal value = reader.GetDecimal(1);
                    table.Rows.Add(key, value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return table;
        }
        public DataTable getRevenueOfMovie(string keyword)
        {
            DataTable table = new DataTable();
            table.Columns.Add("maphim", typeof(string));
            table.Columns.Add("tenphim", typeof(string));
            table.Columns.Add("doanhthu", typeof(decimal));
            try
            {
                cnn.Open();
                string query = $@"SELECT L.MAPHIM,P.TENPHIM, SUM(V.TONGTIEN) AS DOANHTHU
                            FROM LICHCHIEU L, VE V, PHIM P
                            WHERE L.MALICH = V.MALICH AND L.MAPHIM = P.MAPHIM  AND TENPHIM LIKE N'%{keyword}%'
                            GROUP BY L.MAPHIM, P.TENPHIM";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    string id = reader.GetString(0);
                    string name = reader.GetString(1);
                    decimal revenue = reader.GetDecimal(2);
                    table.Rows.Add(id, name, revenue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return table;
        }
    }
}
