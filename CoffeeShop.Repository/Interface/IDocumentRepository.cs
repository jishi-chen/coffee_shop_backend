using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using System.Data;

namespace CoffeeShop.Repository.Interface
{
    public interface IDocumentRepository
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
        DocumentField GetDocumentField(string documentId, string parentId, int sort);
        IEnumerable<DocumentFieldOption> GetFieldOption(string fieldId);
        DocumentFieldOption GetFieldOption(string optionId, string fieldId);
        void DeleteField(string fieldId);
        void UpdateFieldSort(IEnumerable<DocumentField> fields);
        DocumentRecordDetail? GetDocumentRecord(string fieldId, string recordId);
        IEnumerable<DocumentRecordDetail> GetDocumentRecord(string recordId);
        IEnumerable<DocumentRecordDetail> GetDocumentRecord();
        IEnumerable<DocumentRecordListViewModel> GetDocumentRecordList(string? documentId);
        IEnumerable<DocumentRecordViewModel> GetDocumentRecordData(string recordId, string documentId);
        int InsertDocumentRecord(DocumentRecord record);
        void InsertDocumentRecordDetail(IEnumerable<DocumentRecordDetail> record);
        void UpdateDocumentRecordDetail(IEnumerable<DocumentRecordDetail> record);
        DataTable GetExportData(string documentId);
    }
}
