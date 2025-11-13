using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class LineRepository : Repository<Line>, ILineRepository
    {
        public LineRepository(SCERPDBContext context)
            : base(context)
        {
        }

      
        public List<Line> GetAllLinesByPaging(int startPage, int pageSize, Line line, out int totalRecords)
        {
            IQueryable<Line> lines;

            try
            {
                lines = Context.Lines
                    .Where(x => x.IsActive&&
                                (((x.Name.Replace(" ", "").ToLower()).Contains(line.Name.Replace(" ", "").ToLower())) ||
                                 string.IsNullOrEmpty(line.Name)));
                totalRecords = lines.Count(); 
                                                    
                switch (line.sortdir)
                {
                    case "DESC":
                        lines = lines
                            .OrderByDescending(r => r.Name)
                            .Skip(startPage*pageSize)
                            .Take(pageSize);
                        break;
                    default:
                        lines = lines
                             .OrderBy(r => r.Name)
                             .Skip(startPage * pageSize)
                             .Take(pageSize);
                        break;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return lines.ToList();
        }

        public Line GetLineById(int lineId)
        {
            Line line = null;
            try
            {
                line =
                    Context.Lines.FirstOrDefault(x => x.IsActive && x.LineId == lineId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return line;
        }
    }
}
