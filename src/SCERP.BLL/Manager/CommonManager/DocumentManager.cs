using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.DAL.Repository.CommonRepository;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.Manager.CommonManager
{
    public class DocumentManager : IDocumentManager
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly string _compId;
        public DocumentManager(SCERPDBContext context)
        {
            _documentRepository = new DocumentRepository(context);
            _compId = PortalContext.CurrentUser.CompId;
        }

        public List<Document> GetDocumnets(int srcType, string refId)
        {
            return
                _documentRepository.Filter(x => x.SrcType == srcType && x.RefId == refId && x.CompId == _compId&&x.IsActive).ToList();
        }

        public int SaveDocument(Document document)
        {
            document.CompId = _compId;
            document.IsActive = true;
            return _documentRepository.Save(document);
        }

        public int EditDocument(Document document)
        {
            var docObj = _documentRepository.FindOne(x => x.DocumentId == document.DocumentId && x.CompId == _compId);
            docObj.Name = document.Name;
            docObj.Description = document.Description;
            docObj.Path = document.Path;
            docObj.RefId = document.RefId;
            docObj.SrcType = document.SrcType;
            return _documentRepository.Edit(docObj);
        }

        public Document GetDocumentById(long documentId)
        {
            return _documentRepository.FindOne(x => x.DocumentId == documentId && x.CompId == _compId);
        }

        public int DeleteDocument(long documentId)
        {
            var docObj = _documentRepository.FindOne(x => x.DocumentId == documentId && x.CompId == _compId);
            docObj.IsActive = false;
            return _documentRepository.Edit(docObj);
        }
    }
}
