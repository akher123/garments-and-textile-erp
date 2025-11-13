using System;
using System.Collections.Generic;
using System.Data;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class LineOvertimeHourManager : ILineOvertimeHourManager
    {
        private readonly ILineOvertimeHourRepository _lineOvertimeHourRepository;
        public LineOvertimeHourManager(ILineOvertimeHourRepository lineOvertimeHourRepository)
        {
            _lineOvertimeHourRepository = lineOvertimeHourRepository;
        }
        public List<LineOvertimeHour> GetLineOvertimeHoureByOtDate(DateTime? otDate)
        {
            return _lineOvertimeHourRepository.GetLineOvertimeHoureByOtDate(otDate);
        }

        public bool SendOvertimeHour(Guid? prepairedBy, DateTime? otDate)
        {
            return _lineOvertimeHourRepository.SendOvertimeHour(prepairedBy, otDate);
        }

        public DataTable GetOvertimeHoureByOtDate(DateTime date, bool all, bool garments, bool knitting, bool dyeing)
        {
            DataTable othdt = _lineOvertimeHourRepository.GetOvertimeHoureByOtDate(date, all, garments, knitting, dyeing);
            DataRow row = othdt.NewRow();
            row[2] = "Total";
            row["OTP"] = othdt.Compute("Sum(OTP)", "OTP > 0");
            row["OTH"] = othdt.Compute("Sum(OTH)", "OTH > 0");
            row["AMOUNT"] = othdt.Compute("Sum(AMOUNT)", "AMOUNT > 0");

            othdt.Rows.Add(row);
            return othdt;
        }

        public int SecondApprovedOtHours(long[] lineOvertimeHourIds, Guid? userId)
        {
            int approved = 0;
            foreach (var overtimeHourId in lineOvertimeHourIds)
            {
                var oth = _lineOvertimeHourRepository.FindOne(x => x.LineOvertimeHourId == overtimeHourId);
                oth.SeconSignBy = userId;
                oth.SecondSign = oth.SecondSign == "N" ? "Y" : "N";
                approved += _lineOvertimeHourRepository.Edit(oth);
            }
            return approved;
        }

        public DataTable GetLineWiseEmployeeOTHours(string transactionDate, int departmentLineId)
        {
            DateTime date = DateTime.Parse(transactionDate);
            DataTable othours = _lineOvertimeHourRepository.GetLineWiseEmployeeOTHours(departmentLineId, date);
            DataRow row = othours.NewRow();

            row[1] = "Total";
            row["OTHs"] = othours.Compute("Sum(OTHs)", "OTHs > 0");
            row["OTAmounts"] = othours.Compute("Sum(OTAmounts)", "OTAmounts >= 0");
            row[2] = othours.Compute("COUNT(OTHs)", "OTHs > 0");
            othours.Rows.Add(row);
            return othours;
        }

        public int FirsApprovedOtHours(long[] lineOvertimeHourIds, Guid? userId)
        {
            int approved = 0;
            foreach (var overtimeHourId in lineOvertimeHourIds)
            {
                var oth = _lineOvertimeHourRepository.FindOne(x => x.LineOvertimeHourId == overtimeHourId);
                oth.FirstSignBy = userId;
                oth.FirstSign = oth.FirstSign == "N" ? "Y" : "N";
                approved += _lineOvertimeHourRepository.Edit(oth);
            }
            return approved;
        }
    }
}
