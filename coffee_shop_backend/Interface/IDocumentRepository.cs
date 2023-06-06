using coffee_shop_backend.Models;
using coffee_shop_backend.ViewModels;

namespace coffee_shop_backend.Interface
{
    public interface IDocumentRepository : IGenericRepository
    {
        IEnumerable<Document> GetAdminList(bool? isEnabled);
        Document GetDocument(string documentId);
        IEnumerable<DocumentField> GetFieldList(string documentId);
        IEnumerable<DocumentField> GetFieldList(string documentId, string parentId);
        void InsertDocument(Document document);
        void UpdateDocument(Document document);
        int InsertDocumentField(DocumentField field);
        void UpdateDocumentField(DocumentField field);
        void DeleteFieldOptions(string fieldId);
        void InsertFieldOption(DocumentFieldOption option);
        DocumentField GetDocumentField(string fieldId);
        DocumentField GetDocumentField(string documentId,string parentId, int sort);
        IEnumerable<DocumentFieldOption> GetFieldOption(string fieldId);
        DocumentFieldOption GetFieldOption(string optionId, string fieldId);
        void DeleteField(string fieldId);
        void UpdateFieldSort(IEnumerable<DocumentField> fields);
        DocumentRecord? GetDocumentRecord(string fieldId, string recordId);
        IEnumerable<DocumentRecord> GetDocumentRecord(string recordId);
        IEnumerable<DocumentRecordViewModel> GetDocumentRecordData(string recordId, string documentId);
        void InsertDocumentRecord(DocumentRecord record, string recordId);
    }
}
