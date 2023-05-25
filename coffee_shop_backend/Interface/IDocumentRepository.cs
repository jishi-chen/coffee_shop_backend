using coffee_shop_backend.Models;
using coffee_shop_backend.ViewModels;

namespace coffee_shop_backend.Interface
{
    public interface IDocumentRepository : IGenericRepository
    {
        IEnumerable<Document> GetAdminList();
        DocumentInfoPage GetInfoPage(string documentId);
        IEnumerable<DocumentField> GetQuestionFieldList(string documentId);
        IEnumerable<DocumentField> GetQuestionFieldList(string documentId, string parentId);
        void InsertDocument(Document document);
        void UpdateDocument(Document document);
        int InsertDocumentField(DocumentField field);
        void UpdateDocumentField(DocumentField field);
        void DeleteFieldOptions(string fieldId);
        void InsertFieldOption(DocumentFieldOption option);
        DocumentField GetDocumentField(string fieldId);
        DocumentField GetDocumentField(string documentId,string parentId, int sort);
        IEnumerable<DocumentFieldOption> GetFieldOption(string fieldId);
        void DeleteField(string fieldId);
        void UpdateFieldSort(IEnumerable<DocumentField> fields);
    }
}
