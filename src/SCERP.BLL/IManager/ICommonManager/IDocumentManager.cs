using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.IManager.ICommonManager
{
    public interface IDocumentManager
    {
        List<Document> GetDocumnets(int srcType, string refId);
        int SaveDocument(Document document);
        int EditDocument(Document document);
        Document GetDocumentById(long documentId);
        int DeleteDocument(long documentId);
    }
}
